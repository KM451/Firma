using Firma.Models.Entities;
using Firma.Models.EntitiesForView.Kontrahent;
using Firma.ViewModels.Abstract;
using Firma.ViewResources;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;

namespace Firma.ViewModels.KontrachentViewModel
{
    public class WszyscyKontrahenciViewModel : WszystkieViewModel<KontrahentForAllView>
    {
        #region Constructors
        public WszyscyKontrahenciViewModel() : base(BaseResources.ListaKontrahentow) => init();
        public WszyscyKontrahenciViewModel(string dispalyName) : base(dispalyName) => init();

        #endregion

        #region Properties
        public string GdzieWyslac { get; set; }

        #endregion

        #region Methods
        public override void Load()
        {
            AllList =
                    FirmaDBEntities.Kontrahenci.Where(k => k.CzyAktywny)
                    .Join(FirmaDBEntities.Adresy.Where(a => a.Siedziba), k => k.Id, a => a.IdKontrahenta,
                    (k, a) => new KontrahentForAllView
                    {
                        IdKontrahenta = k.Id,
                        Kod = k.Tytul,
                        Nazwa = k.Nazwa,
                        Nip = k.Nip,
                        KontrachentAdres = "ul. " + a.Ulica + " " + a.NrDomu.Trim()
                                                            + (a.NrLokalu == null ? ", " : "/" + a.NrLokalu.Trim() + ", ")
                                                            + a.KodPoczowy.Trim() + " " + a.Miejscowosc + ", " + a.Kraj.Trim()
                    }).ToList();
            Filter();
        }

        /// <summary>
        /// Wysyła dane wybranego kontrahenta do wybranego viewmodelu
        /// </summary>
        protected override void Open()
        {
            if (CzyModyfikowac)
            {
                if (SelectedItem != null)
                    Messenger.Default.Send(FirmaDBEntities.Kontrahenci.Where(k => k.Id == SelectedItem.IdKontrahenta).FirstOrDefault());
            }
            else
            {
                if (SelectedItem != null)
                {
                    if (GdzieWyslac == "ZK")
                        Messenger.Default.Send(new KontrahentForZK()
                        {
                            IdKontrahenta = SelectedItem.IdKontrahenta,
                            Nazwa = SelectedItem.Nazwa,
                            Kod = SelectedItem.Kod,
                            Nip = SelectedItem.Nip,
                        });
                    OnRequestClose();
                }
            }
        }

        /// <summary>
        /// Usuwanie kontrahenta
        /// </summary>
        protected override void Delete()
        {
            if (SelectedItem != null)
            {

                if (MessageBox.Show("Czy napewno chcesz usunąć wybranego kontrahenta?", "Usuwanie", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    FirmaDBEntities.Kontrahenci.First(item => item.Id == SelectedItem.IdKontrahenta).CzyAktywny = false;
                    List.Remove(SelectedItem);
                    try
                    {
                        FirmaDBEntities.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"Wystapił błąd podczas usuwania!\n{ex}\n{ex.Message}");
                    }
                }
            }
            else MessageBox.Show("Nie wybrano żadnej pozycji do usunięcia", "Usuwanie");
        }


        /// <summary>
        /// Inicjalizuje wybrane elementy (do użytku w konstruktorze)
        /// </summary>
        private void init()
        {
            FilterField = string.Empty;
            OrderByField = nameof(KontrahentForAllView.IdKontrahenta);
            OrderBy();
        }
        /// <summary>
        /// Wywołuje okno dodawania nowego kontrahenta
        /// </summary>
        protected override void ShowAddView() => Messenger.Default.Send("Nowy Kontrahent AddShow");

        #region OrderBy

        protected override void OrderBy()
        {
            switch (OrderByField)
            {
                case nameof(KontrahentForAllView.IdKontrahenta):
                    List = new ObservableCollection<KontrahentForAllView>(OrderDescending ?
                        List.OrderByDescending(item => item.IdKontrahenta) :
                        List.OrderBy(item => item.IdKontrahenta));
                    break;
                case nameof(KontrahentForAllView.Kod):
                    List = new ObservableCollection<KontrahentForAllView>(OrderDescending ?
                        List.OrderByDescending(item => item.Kod) :
                        List.OrderBy(item => item.Kod));
                    break;
                case nameof(KontrahentForAllView.Nazwa):
                    List = new ObservableCollection<KontrahentForAllView>(OrderDescending ?
                        List.OrderByDescending(item => item.Nazwa) :
                        List.OrderBy(item => item.Nazwa));
                    break;
                case nameof(KontrahentForAllView.Nip):
                    List = new ObservableCollection<KontrahentForAllView>(OrderDescending ?
                        List.OrderByDescending(item => item.Nip) :
                        List.OrderBy(item => item.Nip));
                    break;
                case nameof(KontrahentForAllView.KontrachentAdres):
                    List = new ObservableCollection<KontrahentForAllView>(OrderDescending ?
                        List.OrderByDescending(item => item.KontrachentAdres) :
                        List.OrderBy(item => item.KontrachentAdres));
                    break;
            }
        }

        protected override ObservableCollection<KeyValuePair<string, string>> GetListOfItemsOrderBy()
        {
            return new ObservableCollection<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>(nameof(KontrahentForAllView.IdKontrahenta), "ID"),
                new KeyValuePair<string, string>(nameof(KontrahentForAllView.Kod), "Kod"),
                new KeyValuePair<string, string>(nameof(KontrahentForAllView.Nazwa), "Nazwa"),
                new KeyValuePair<string, string>(nameof(KontrahentForAllView.Nip), "NIP"),
                new KeyValuePair<string, string>(nameof(KontrahentForAllView.KontrachentAdres), "Adres")
            };
        }

        #endregion

        #region Filter

        protected override void Filter()
        {
            if (!string.IsNullOrEmpty(SearchPhrase))
            {
                switch (FilterField)
                {
                    case nameof(KontrahentForAllView.IdKontrahenta):
                        List = new ObservableCollection<KontrahentForAllView>(AllList.Where(item => item.IdKontrahenta.ToString().Contains(SearchPhrase)));
                        break;
                    case nameof(KontrahentForAllView.Kod):
                        List = new ObservableCollection<KontrahentForAllView>(AllList.Where(item => item.Kod.Contains(SearchPhrase)));
                        break;
                    case nameof(KontrahentForAllView.Nazwa):
                        List = new ObservableCollection<KontrahentForAllView>(AllList.Where(item => item.Nazwa.Contains(SearchPhrase)));
                        break;
                    case nameof(KontrahentForAllView.Nip):
                        List = new ObservableCollection<KontrahentForAllView>(AllList.Where(item => item.Nip.Contains(SearchPhrase)));
                        break;
                    case nameof(KontrahentForAllView.KontrachentAdres):
                        List = new ObservableCollection<KontrahentForAllView>(AllList.Where(item => item.KontrachentAdres.Contains(SearchPhrase)));
                        break;
                    default:
                        List = new ObservableCollection<KontrahentForAllView>(AllList);
                        break;
                }
            }
            else
            {
                List = new ObservableCollection<KontrahentForAllView>(AllList);
            }
            OrderBy();
        }

        protected override ObservableCollection<KeyValuePair<string, string>> GetListOfItemsFilter()
        {
            return new ObservableCollection<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>(string.Empty, "brak fitrowania"),
                new KeyValuePair<string, string>(nameof(KontrahentForAllView.IdKontrahenta), "ID"),
                new KeyValuePair<string, string>(nameof(KontrahentForAllView.Kod), "Kod"),
                new KeyValuePair<string, string>(nameof(KontrahentForAllView.Nazwa), "Nazwa"),
                new KeyValuePair<string, string>(nameof(KontrahentForAllView.Nip), "NIP"),
                new KeyValuePair<string, string>(nameof(KontrahentForAllView.KontrachentAdres), "Adres")
            };
        }

        #endregion


        #endregion
    }
}
