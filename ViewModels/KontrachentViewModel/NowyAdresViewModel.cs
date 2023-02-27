using Firma.Models.Entities;
using Firma.Models.Validators;
using Firma.ViewModels.Abstract;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.ComponentModel;
using System.Windows;

namespace Firma.ViewModels.KontrachentViewModel
{
    public class NowyAdresViewModel : JedenViewModel<Adresy>, IDataErrorInfo
    {
        #region Properties

        public string Ulica
        {
            get
            {
                return Item.Ulica;
            }
            set
            {
                if (Item.Ulica != value)
                {
                    Item.Ulica = value[0].ToString().ToUpper() + value.Remove(0, 1);
                    OnPropertyChanged(() => Ulica);
                }
            }
        }
        public string Miejscowosc
        {
            get
            {
                return Item.Miejscowosc;
            }
            set
            {
                if (Item.Miejscowosc != value)
                {
                    Item.Miejscowosc = value[0].ToString().ToUpper() + value.Remove(0, 1); ;
                    OnPropertyChanged(() => Miejscowosc);
                }
            }
        }
        public string NrDomu
        {
            get
            {
                return Item.NrDomu;
            }
            set
            {
                if (Item.NrDomu != value)
                {
                    Item.NrDomu = value;
                    OnPropertyChanged(() => NrDomu);
                }
            }
        }
        public string NrLokalu
        {
            get
            {
                return Item.NrLokalu;
            }
            set
            {
                if (Item.NrLokalu != value)
                {
                    Item.NrLokalu = value;
                    OnPropertyChanged(() => NrLokalu);
                }
            }
        }
        public string KodPoczowy
        {
            get
            {
                return Item.KodPoczowy;
            }
            set
            {

                if (Item.KodPoczowy != value)
                {
                    Item.KodPoczowy = value;
                    OnPropertyChanged(() => KodPoczowy);
                }
            }
        }
        public string Poczta
        {
            get
            {
                return Item.Poczta;
            }
            set
            {
                if (Item.Poczta != value)
                {
                    Item.Poczta = value[0].ToString().ToUpper() + value.Remove(0, 1); ;
                    OnPropertyChanged(() => Poczta);
                }
            }
        }
        public string Kraj
        {
            get
            {
                return Item.Kraj;
            }
            set
            {
                if (Item.Kraj != value)
                {
                    Item.Kraj = value[0].ToString().ToUpper() + value.Remove(0, 1);
                    OnPropertyChanged(() => Kraj);
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
                if (Item.Notatki != value)
                {
                    Item.Notatki = value;
                    OnPropertyChanged(() => Notatki);
                }
            }
        }
        public bool Siedziba
        {
            get
            {
                return Item.Siedziba;
            }
            set
            {
                if (Item.Siedziba != value)
                {
                    Item.Siedziba = value;
                    OnPropertyChanged(() => Siedziba);
                }
            }
        }
        public bool Wysylkowy
        {
            get
            {
                return Item.Wysylkowy;
            }
            set
            {
                if (Item.Wysylkowy != value)
                {
                    Item.Wysylkowy = value;
                    OnPropertyChanged(() => Wysylkowy);
                }
            }
        }

        #endregion

        #region Constructor
        public NowyAdresViewModel() : base("Adresy")
        {
            Item = new Adresy()
            {
                Tytul = "adres",
                CzyAktywny = true,
                DataUtworzenia = DateTime.Now,
                DataModyfikacji = DateTime.Now,
            };
        }
        #endregion

        #region Method
        public override void Save()
        {
            Messenger.Default.Send(Item);
        }
        #endregion

        #region Validation

        protected override bool IsValid()
        {
            string wynikWalidacji = string.Empty;

            if (this[nameof(KodPoczowy)] != string.Empty)
                wynikWalidacji += this[nameof(KodPoczowy)] + "\n";


            if (wynikWalidacji.Replace("\n", string.Empty) == string.Empty)
            {
                return true;
            }
            else
            {
                MessageBox.Show("Podano nieprawidłowe dane.\n" + wynikWalidacji + "\nPopraw podane pozycje przed zapisem!", "Błąd");
                return false;
            }
        }

        public string Error => string.Empty;

        public string this[string columnName]
        {
            get
            {
                switch (columnName)
                {
                    case nameof(KodPoczowy):
                        return StringValidator.SprawdzKodPocztowy(KodPoczowy ?? string.Empty);

                    default:
                        return string.Empty;
                }
            }
        }

        #endregion
    }
}
