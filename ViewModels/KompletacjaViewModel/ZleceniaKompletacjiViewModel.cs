using Firma.Models.Entities;
using Firma.Models.EntitiesForView.ZlecenieKompletacji;
using Firma.ViewModels.Abstract;
using Firma.ViewResources;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;

namespace Firma.ViewModels.KompletacjaViewModel
{
    public class ZleceniaKompletacjiViewModel : WszystkieViewModel<ZKForAllView>
    {
        #region Constructor
        public ZleceniaKompletacjiViewModel() : base(BaseResources.ZleceniaKompletacji)
        {
            FilterField = string.Empty;
            OrderByField = nameof(ZKForAllView.IdZleceniaKompletacji);
            OrderBy();
        }

        #endregion

        #region Methods
        public override void Load()
        {
            AllList =
            FirmaDBEntities.ZleceniaKompletacji.Where(zk => zk.CzyAktywny).Select(zk => new ZKForAllView
            {
                IdZleceniaKompletacji = zk.Id,
                ProduktKod = zk.Produkty.Tytul,
                ProduktNazwa = zk.Produkty.Nazwa,
                Ilosc = zk.Ilosc,
                ProduktKontrahent = zk.Kontrahenci.Nazwa,
                DataWystawienia = zk.DataUtworzenia,
                PotwierdzonaDataRealizacji = zk.PotwierdzonaDataRealizacji,
                ProduktMonter = zk.Monterzy.Tytul,
                Priorytet = zk.Priorytet,
                Status = zk.StatusyZK.Tytul
            }).ToList();
            Filter();
        }

        /// <summary>
        /// Usuwanie zlecenia kompletacji
        /// </summary>
        protected override void Delete()
        {
            if (SelectedItem != null)
            {

                if (MessageBox.Show("Czy napewno chcesz usunąć wybrane zlecenie kompletacji?", "Usuwanie", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    FirmaDBEntities.ZleceniaKompletacji.First(item => item.Id == SelectedItem.IdZleceniaKompletacji).CzyAktywny = false;
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
        /// Wywołanie okna szczegółów/edycji zlecenia kompletacji
        /// </summary>
        protected override void Open()
        {
            if(SelectedItem!= null)
            Messenger.Default.Send(FirmaDBEntities.ZleceniaKompletacji.FirstOrDefault(zk => zk.Id == SelectedItem.IdZleceniaKompletacji));
        }

        /// <summary>
        /// Wywołanie okna dodawania nowego zlecenia kompletacji
        /// </summary>
        protected override void ShowAddView() => Messenger.Default.Send("Nowe ZK AddShow");

        #region OrderBy
        protected override void OrderBy()
        {
            switch (OrderByField)
            {
                case nameof(ZKForAllView.IdZleceniaKompletacji):
                    List = new ObservableCollection<ZKForAllView>(OrderDescending ?
                        List.OrderByDescending(item => item.IdZleceniaKompletacji) : List.OrderBy(item => item.IdZleceniaKompletacji));
                    break;
                case nameof(ZKForAllView.ProduktKod):
                    List = new ObservableCollection<ZKForAllView>(OrderDescending ?
                        List.OrderByDescending(item => item.ProduktKod) : List.OrderBy(item => item.ProduktKod));
                    break;
                case nameof(ZKForAllView.ProduktNazwa):
                    List = new ObservableCollection<ZKForAllView>(OrderDescending ?
                        List.OrderByDescending(item => item.ProduktNazwa) : List.OrderBy(item => item.ProduktNazwa));
                    break;
                case nameof(ZKForAllView.Ilosc):
                    List = new ObservableCollection<ZKForAllView>(OrderDescending ?
                        List.OrderByDescending(item => item.Ilosc) : List.OrderBy(item => item.Ilosc));
                    break;
                case nameof(ZKForAllView.ProduktKontrahent):
                    List = new ObservableCollection<ZKForAllView>(OrderDescending ?
                        List.OrderByDescending(item => item.ProduktKontrahent) : List.OrderBy(item => item.ProduktKontrahent));
                    break;
                case nameof(ZKForAllView.DataWystawienia):
                    List = new ObservableCollection<ZKForAllView>(OrderDescending ?
                        List.OrderByDescending(item => item.DataWystawienia) : List.OrderBy(item => item.DataWystawienia));
                    break;
                case nameof(ZKForAllView.PotwierdzonaDataRealizacji):
                    List = new ObservableCollection<ZKForAllView>(OrderDescending ?
                        List.OrderByDescending(item => item.PotwierdzonaDataRealizacji) : List.OrderBy(item => item.PotwierdzonaDataRealizacji));
                    break;
                case nameof(ZKForAllView.ProduktMonter):
                    List = new ObservableCollection<ZKForAllView>(OrderDescending ?
                        List.OrderByDescending(item => item.ProduktMonter) : List.OrderBy(item => item.ProduktMonter));
                    break;
                case nameof(ZKForAllView.Priorytet):
                    List = new ObservableCollection<ZKForAllView>(OrderDescending ?
                        List.OrderByDescending(item => item.Priorytet) : List.OrderBy(item => item.Priorytet));
                    break;
                case nameof(ZKForAllView.Status):
                    List = new ObservableCollection<ZKForAllView>(OrderDescending ?
                        List.OrderByDescending(item => item.Status) : List.OrderBy(item => item.Status));
                    break;
            }
        }

        protected override ObservableCollection<KeyValuePair<string, string>> GetListOfItemsOrderBy()
        {
            return new ObservableCollection<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>(nameof(ZKForAllView.IdZleceniaKompletacji), "ID"),
                new KeyValuePair<string, string>(nameof(ZKForAllView.ProduktKod), "Kod"),
                new KeyValuePair<string, string>(nameof(ZKForAllView.ProduktNazwa), "Produkt"),
                new KeyValuePair<string, string>(nameof(ZKForAllView.Ilosc), "Ilość"),
                new KeyValuePair<string, string>(nameof(ZKForAllView.ProduktKontrahent), "Kontrahent"),
                new KeyValuePair<string, string>(nameof(ZKForAllView.DataWystawienia), "Data wystawienia"),
                new KeyValuePair<string, string>(nameof(ZKForAllView.PotwierdzonaDataRealizacji), "Data potwierdzona"),
                new KeyValuePair<string, string>(nameof(ZKForAllView.ProduktMonter), "Montażysta"),
                new KeyValuePair<string, string>(nameof(ZKForAllView.Priorytet), "Priorytet"),
                new KeyValuePair<string, string>(nameof(ZKForAllView.Status), "Status")
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
                    case nameof(ZKForAllView.IdZleceniaKompletacji):
                        List = new ObservableCollection<ZKForAllView>(AllList.Where(item => item.IdZleceniaKompletacji.ToString().Contains(SearchPhrase)));
                        break;
                    case nameof(ZKForAllView.ProduktKod):
                        List = new ObservableCollection<ZKForAllView>(AllList.Where(item => item.ProduktKod.Contains(SearchPhrase)));
                        break;
                    case nameof(ZKForAllView.ProduktNazwa):
                        List = new ObservableCollection<ZKForAllView>(AllList.Where(item => item.ProduktNazwa.Contains(SearchPhrase)));
                        break;
                    case nameof(ZKForAllView.Ilosc):
                        List = new ObservableCollection<ZKForAllView>(AllList.Where(item => item.Ilosc.ToString().Contains(SearchPhrase)));
                        break;
                    case nameof(ZKForAllView.ProduktKontrahent):
                        List = new ObservableCollection<ZKForAllView>(AllList.Where(item => item.ProduktKontrahent.Contains(SearchPhrase)));
                        break;
                    case nameof(ZKForAllView.DataWystawienia):
                        List = new ObservableCollection<ZKForAllView>(AllList.Where(item => item.DataWystawienia.ToString().Contains(SearchPhrase)));
                        break;
                    case nameof(ZKForAllView.PotwierdzonaDataRealizacji):
                        List = new ObservableCollection<ZKForAllView>(AllList.Where(item => item.PotwierdzonaDataRealizacji?.ToString().Contains(SearchPhrase) ?? false));
                        break;
                    case nameof(ZKForAllView.ProduktMonter):
                        List = new ObservableCollection<ZKForAllView>(AllList.Where(item => item.ProduktMonter.Contains(SearchPhrase)));
                        break;
                    case nameof(ZKForAllView.Priorytet):
                        List = new ObservableCollection<ZKForAllView>(AllList.Where(item => item.Priorytet?.ToString().Contains(SearchPhrase) ?? false));
                        break;
                    case nameof(ZKForAllView.Status):
                        List = new ObservableCollection<ZKForAllView>(AllList.Where(item => item.Status.Contains(SearchPhrase)));
                        break;

                    default:
                        List = new ObservableCollection<ZKForAllView>(AllList);
                        break;
                }
            }
            else
            {
                List = new ObservableCollection<ZKForAllView>(AllList);
            }
            OrderBy();
        }

        protected override ObservableCollection<KeyValuePair<string, string>> GetListOfItemsFilter()
        {
            return new ObservableCollection<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>(string.Empty, "brak fitrowania"),
                new KeyValuePair<string, string>(nameof(ZKForAllView.IdZleceniaKompletacji), "ID"),
                new KeyValuePair<string, string>(nameof(ZKForAllView.ProduktKod), "Kod"),
                new KeyValuePair<string, string>(nameof(ZKForAllView.ProduktNazwa), "Produkt"),
                new KeyValuePair<string, string>(nameof(ZKForAllView.Ilosc), "Ilość"),
                new KeyValuePair<string, string>(nameof(ZKForAllView.ProduktKontrahent), "Kontrahent"),
                new KeyValuePair<string, string>(nameof(ZKForAllView.DataWystawienia), "Data wystawienia"),
                new KeyValuePair<string, string>(nameof(ZKForAllView.PotwierdzonaDataRealizacji), "Data potwierdzona"),
                new KeyValuePair<string, string>(nameof(ZKForAllView.ProduktMonter), "Montażysta"),
                new KeyValuePair<string, string>(nameof(ZKForAllView.Priorytet), "Priorytet"),
                new KeyValuePair<string, string>(nameof(ZKForAllView.Status), "Status")
            };
        }


        #endregion

        #endregion
    }
}
