using Firma.Models.Entities;
using Firma.Models.EntitiesForView.Kontrahent;
using Firma.Models.Validators;
using Firma.ViewModels.Abstract;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;

namespace Firma.ViewModels.KontrachentViewModel
{
    public class NowyKontrahentViewModel : JedenWszystkieViewModel<Kontrahenci, Adresy>, IDataErrorInfo
    {
        #region Constructors
        public NowyKontrahentViewModel() : base("Nowy Kontrahent", "Adresy kontrahenta", "Dodaj adres")
        {
            init();
        }

        public NowyKontrahentViewModel(string text1, string text2, string text3) : base(text1, text2, text3)
        {
            init();
        }

        #endregion

        #region Properties
        public string Tytul
        {
            get
            {
                return Item.Tytul;
            }
            set
            {
                if (value != Item.Tytul)
                {
                    Item.Tytul = value;
                    OnPropertyChanged(() => Tytul);
                }
            }
        }
        public bool Dostawca
        {
            get
            {
                return Item.Dostawca;
            }
            set
            {
                if (value != Item.Dostawca)
                {
                    Item.Dostawca = value;
                    OnPropertyChanged(() => Dostawca);
                }
            }
        }
        public bool Odbiorca
        {
            get
            {
                return Item.Odbiorca;
            }
            set
            {
                if (value != Item.Odbiorca)
                {
                    Item.Odbiorca = value;
                    OnPropertyChanged(() => Odbiorca);
                }
            }
        }
        public string Notatki
        {
            get
            {
                return Item.Notatki;
            }
            set
            {
                if (value != Item.Notatki)
                {
                    Item.Notatki = value;
                    OnPropertyChanged(() => Notatki);
                }
            }
        }
        public string Nazwa
        {
            get
            {
                return Item.Nazwa;
            }
            set
            {
                if (value != Item.Nazwa)
                {
                    Item.Nazwa = value;
                    OnPropertyChanged(() => Nazwa);
                }
            }
        }
        public string Nip
        {
            get
            {
                return Item.Nip;
            }
            set
            {
                if (value != Item.Nip)
                {
                    Item.Nip = value;
                    OnPropertyChanged(() => Nip);
                }
            }
        }

        #endregion

        #region Methods

        private void init()
        {
            Item = new Kontrahenci();
            Messenger.Default.Register<Adresy>(this, PrzypiszAdres);
            WszystkieList = new ObservableCollection<Adresy>(Item.Adresy
                .Where(item => item.CzyAktywny).ToList());
        }
        public override void Save()
        {
            Item.DataUtworzenia = DateTime.Now;
            Item.DataModyfikacji = DateTime.Now;
            Item.CzyAktywny = true;
            Db.Kontrahenci.AddObject(Item);
            Db.SaveChanges();
        }

        private void PrzypiszAdres(Adresy adres)
        {
            if (SelectedItem != null)
            {
                SelectedItem.Ulica = adres.Ulica;
                SelectedItem.Miejscowosc = adres.Miejscowosc;
                SelectedItem.NrDomu = adres.NrDomu;
                SelectedItem.NrLokalu = adres.NrLokalu;
                SelectedItem.KodPoczowy = adres.KodPoczowy;
                SelectedItem.Poczta = adres.Poczta;
                SelectedItem.Kraj = adres.Kraj;
                SelectedItem.Notatki = adres.Notatki;
                SelectedItem.Siedziba = adres.Siedziba;
                SelectedItem.Wysylkowy = adres.Wysylkowy;
            }
            else
            {
                Item.Adresy.Add(adres);
                WszystkieList.Add(adres);
            }
        }

        protected override void ShowAddView()
        {
            SelectedItem = null; //dzięki temu metoda PrzypiszAdres rozpoznaje czy dodajemy nowy adres czy edytujemy już podany
            Messenger.Default.Send(WszystkieDisplayName + " Add");
        }

        protected override void ShowEditView()
        {
            if (SelectedItem != null)
            {
                Messenger.Default.Send(new AdresForEdit()
                {
                    Id = SelectedItem.Id,
                    Ulica = SelectedItem.Ulica,
                    Miejscowosc = SelectedItem.Miejscowosc,
                    NrDomu = SelectedItem.NrDomu,
                    NrLokalu = SelectedItem.NrLokalu,
                    KodPoczowy = SelectedItem.KodPoczowy,
                    Poczta = SelectedItem.Poczta,
                    Kraj = SelectedItem.Kraj,
                    Notatki = SelectedItem.Notatki,
                    Siedziba = SelectedItem.Siedziba,
                    Wysylkowy = SelectedItem.Wysylkowy
                });
            }
        }

        protected override void Delete()
        {
            if (SelectedItem != null)
            {

                if (MessageBox.Show("Czy napewno chcesz usunąć wybrany adres?", "Usuwanie", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    Db.Adresy.First(item => item.Id == SelectedItem.Id).CzyAktywny = false;
                    WszystkieList.Remove(SelectedItem);
                    try
                    {
                        Db.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"Wystapił błąd podczas usuwania!\n{ex}\n{ex.Message}");
                    }
                }
            }
            else MessageBox.Show("Nie wybrano żadnej pozycji do usunięcia", "Usuwanie");
        }

        #endregion

        #region Validation
        protected override bool IsValid()
        {
            if (this[nameof(Nip)] == string.Empty)
                return true;
            else
            {
                MessageBox.Show("Podano nieprawidłowe dane.\n" + this[nameof(Nip)]
                    + "\nPopraw podane pozycje przed zapisem!", "Błąd");
                return false;
            }

        }

        public string Error => string.Empty;
        public string this[string columnName]
        {
            get
            {
                if (columnName == nameof(Nip))
                    return StringValidator.SprawdzNip(Nip ?? string.Empty);
                else
                    return string.Empty;
            }
        }
        #endregion
    }
}
