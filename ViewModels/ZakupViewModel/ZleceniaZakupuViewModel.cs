using Firma.Models.Entities;
using Firma.Models.EntitiesForView.ZlecenieZakupu;
using Firma.ViewModels.Abstract;
using Firma.ViewResources;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;

namespace Firma.ViewModels.ZakupViewModel
{
    public class ZleceniaZakupuViewModel : WszystkieViewModel<ZZForAllView>
    {
        #region Constructor
        public ZleceniaZakupuViewModel() : base(BaseResources.ZleceniaZakupu) 
        {
            FilterField = string.Empty;
            OrderByField = nameof(ZZForAllView.IdZleceniaZakupu);
            OrderBy();
        }
        #endregion

        #region Methods
        public override void Load()
        {
            AllList =
            FirmaDBEntities.ZleceniaZakupu.Where(zz => zz.CzyAktywny)
            .Join(FirmaDBEntities.PozycjeZleceniaZakupu.Where(pz => pz.CzyAktywny)
            .GroupBy(pz => pz.IdZleceniaZakupu, pz => pz.DataRealizacji,
            (id, date) => new { Idzz = id, MaxD = date.Max() }),
            zz => zz.Id, dm => dm.Idzz, (zz, dm) => new ZZForAllView
            {
                IdZleceniaZakupu = zz.Id,
                ZZKontrahent = zz.Kontrahenci.Nazwa,
                DataWystawienia = zz.DataUtworzenia,
                DataRealizacji = dm.MaxD,
                Status = zz.StatusyZZ.Tytul,
                Potwierdzenie = zz.Potwierdzone,
                NrPotwierdzenia = zz.NrPotwierdzenia,
            }).ToList();
            Filter();
        }

        /// <summary>
        /// Usuwanie zlecenia zakupu
        /// </summary>
        protected override void Delete()
        {
            if (SelectedItem != null)
            {

                if (MessageBox.Show("Czy napewno chcesz usunąć wybrane zlecenie zakupu?", "Usuwanie", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    FirmaDBEntities.ZleceniaZakupu.First(item => item.Id == SelectedItem.IdZleceniaZakupu).CzyAktywny = false;
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

        protected override void Open()
        {
            if(SelectedItem != null)
            Messenger.Default.Send(FirmaDBEntities.ZleceniaZakupu.Where(zz => zz.Id == SelectedItem.IdZleceniaZakupu).FirstOrDefault());
        }

        protected override void ShowAddView() => Messenger.Default.Send("Nowe ZZ AddShow");

        #region OrderBy

        protected override void OrderBy()
        {
            switch (OrderByField)
            {
                case nameof(ZZForAllView.IdZleceniaZakupu):
                    List = new ObservableCollection<ZZForAllView>(OrderDescending ?
                        List.OrderByDescending(item => item.IdZleceniaZakupu) : List.OrderBy(item => item.IdZleceniaZakupu));
                    break;
                case nameof(ZZForAllView.ZZKontrahent):
                    List = new ObservableCollection<ZZForAllView>(OrderDescending ?
                        List.OrderByDescending(item => item.ZZKontrahent) : List.OrderBy(item => item.ZZKontrahent));
                    break;
                case nameof(ZZForAllView.DataWystawienia):
                    List = new ObservableCollection<ZZForAllView>(OrderDescending ?
                        List.OrderByDescending(item => item.DataWystawienia) : List.OrderBy(item => item.DataWystawienia));
                    break;
                case nameof(ZZForAllView.DataRealizacji):
                    List = new ObservableCollection<ZZForAllView>(OrderDescending ?
                        List.OrderByDescending(item => item.DataRealizacji) : List.OrderBy(item => item.DataRealizacji));
                    break;
                case nameof(ZZForAllView.Status):
                    List = new ObservableCollection<ZZForAllView>(OrderDescending ?
                        List.OrderByDescending(item => item.Status) : List.OrderBy(item => item.Status));
                    break;
                case nameof(ZZForAllView.Potwierdzenie):
                    List = new ObservableCollection<ZZForAllView>(OrderDescending ?
                        List.OrderByDescending(item => item.Potwierdzenie) : List.OrderBy(item => item.Potwierdzenie));
                    break;
                case nameof(ZZForAllView.NrPotwierdzenia):
                    List = new ObservableCollection<ZZForAllView>(OrderDescending ?
                        List.OrderByDescending(item => item.NrPotwierdzenia) : List.OrderBy(item => item.NrPotwierdzenia));
                    break;
            }
        }

        protected override ObservableCollection<KeyValuePair<string, string>> GetListOfItemsOrderBy()
        {
            return new ObservableCollection<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>(nameof(ZZForAllView.IdZleceniaZakupu), "ID"),
                new KeyValuePair<string, string>(nameof(ZZForAllView.ZZKontrahent), "Kontrahent"),
                new KeyValuePair<string, string>(nameof(ZZForAllView.DataWystawienia), "Data wystawienia"),
                new KeyValuePair<string, string>(nameof(ZZForAllView.DataRealizacji), "Data realizacji"),
                new KeyValuePair<string, string>(nameof(ZZForAllView.Status), "Status"),
                new KeyValuePair<string, string>(nameof(ZZForAllView.Potwierdzenie), "Czy potwierdzone"),
                new KeyValuePair<string, string>(nameof(ZZForAllView.NrPotwierdzenia), "Nr potwierdzenia")
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
                    case nameof(ZZForAllView.IdZleceniaZakupu):
                        List = new ObservableCollection<ZZForAllView>(AllList.Where(item => item.IdZleceniaZakupu.ToString().Contains(SearchPhrase)));
                        break;
                    case nameof(ZZForAllView.ZZKontrahent):
                        List = new ObservableCollection<ZZForAllView>(AllList.Where(item => item.ZZKontrahent.Contains(SearchPhrase)));
                        break;
                    case nameof(ZZForAllView.DataWystawienia):
                        List = new ObservableCollection<ZZForAllView>(AllList.Where(item => item.DataWystawienia.ToString().Contains(SearchPhrase)));
                        break;
                    case nameof(ZZForAllView.DataRealizacji):
                        List = new ObservableCollection<ZZForAllView>(AllList.Where(item => item.DataRealizacji.ToString().Contains(SearchPhrase)));
                        break;
                    case nameof(ZZForAllView.Status):
                        List = new ObservableCollection<ZZForAllView>(AllList.Where(item => item.Status.Contains(SearchPhrase)));
                        break;
                    case nameof(ZZForAllView.NrPotwierdzenia):
                        List = new ObservableCollection<ZZForAllView>(AllList.Where(item => item.NrPotwierdzenia?.Contains(SearchPhrase)?? false));
                        break;
                    default:
                        List = new ObservableCollection<ZZForAllView>(AllList);
                        break;
                }
            }
            else
            {
                List = new ObservableCollection<ZZForAllView>(AllList);
            }
            OrderBy();
        }

        protected override ObservableCollection<KeyValuePair<string, string>> GetListOfItemsFilter()
        {
            return new ObservableCollection<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>(string.Empty, "brak fitrowania"),
                new KeyValuePair<string, string>(nameof(ZZForAllView.IdZleceniaZakupu), "ID"),
                new KeyValuePair<string, string>(nameof(ZZForAllView.ZZKontrahent), "Kontrahent"),
                new KeyValuePair<string, string>(nameof(ZZForAllView.DataWystawienia), "Data wystawienia"),
                new KeyValuePair<string, string>(nameof(ZZForAllView.DataRealizacji), "Data realizacji"),
                new KeyValuePair<string, string>(nameof(ZZForAllView.Status), "Status"),
                new KeyValuePair<string, string>(nameof(ZZForAllView.NrPotwierdzenia), "Nr potwierdzenia")
            };
        }

        #endregion

        #endregion
    }
}
