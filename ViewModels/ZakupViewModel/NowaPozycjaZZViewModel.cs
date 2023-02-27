using Firma.Models.Entities;
using Firma.Models.EntitiesForView;
using Firma.Models.EntitiesForView.ZlecenieZakupu;
using Firma.Models.Validators;
using Firma.ViewModels.Abstract;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;

namespace Firma.ViewModels.ZakupViewModel
{
    public class NowaPozycjaZZViewModel: JedenViewModel<PozycjeZleceniaZakupu>, IDataErrorInfo
    {
        #region Properties

        public List<ComboBoxKeyAndValue> Produkty { get; set; }
        public int IdProduktu
        {
            get
            {
                return Item.IdProduktu;
            }
            set
            {
                if (Item.IdProduktu != value)
                {
                    Item.IdProduktu = value;
                    OnPropertyChanged(() => IdProduktu);
                }
            }
        }
        public decimal Ilosc
        {
            get
            {
                return Item.Ilosc;
            }
            set
            {
                if (Item.Ilosc != value)
                {
                    Item.Ilosc = value;
                    OnPropertyChanged(() => Ilosc);
                }
            }
        }
        public decimal? Cena
        {
            get
            {
                return Item.Cena;
            }
            set
            {
                if (Item.Cena != value)
                {
                    Item.Cena = value;
                    OnPropertyChanged(() => Cena);
                }
            }
        }
        public decimal? Rabat
        {
            get
            {
                return Item.Rabat;
            }
            set
            {
                if (Item.Rabat != value)
                {
                    Item.Rabat = value;
                    OnPropertyChanged(() => Rabat);
                }
            }
        }
        public DateTime? DataRealizacji
        {
            get
            {
                return Item.DataRealizacji;
            }
            set
            {
                if (Item.DataRealizacji != value)
                {
                    Item.DataRealizacji = value;
                    OnPropertyChanged(() => DataRealizacji);
                }
            }
        }

        #endregion

        #region Constructors

        public NowaPozycjaZZViewModel() : base("Pozycja ZZ")
        {
            init();
            IdProduktu = Produkty.FirstOrDefault().Key;
            Ilosc = 1;
            Cena = 0;
            Rabat = 0;
        }

        public NowaPozycjaZZViewModel(PZZForEdit pzz) : base("Edycja pozycji ZZ")
        {
            init();
            IdProduktu = pzz.IdProduktu;
            Ilosc = pzz.Ilosc;
            Cena = pzz.Cena;
            Rabat = pzz.Rabat*100;
            DataRealizacji = pzz.DataRealizacji;
        }

        #endregion

        #region Methods

        private void init()
        {
            Item = new PozycjeZleceniaZakupu()
            {
                CzyAktywny = true,
                Tytul = "pzz",
                DataModyfikacji = DateTime.Now,
                DataUtworzenia = DateTime.Now,
            };
            Produkty = Db.Produkty.Where(item => item.CzyAktywny == true)
                             .Select(item => new ComboBoxKeyAndValue() { Key = item.Id, Value = item.Nazwa })
                             .ToList();
        }

        public override void Save()
        {
            if(IsValid())
            {
                Messenger.Default.Send(Item);
            }
            
        }

        #endregion

        #region Validation

        protected override bool IsValid()
        {
            string wynikWalidacji = string.Empty;
            
            if (this[nameof(Cena)] != string.Empty)
                wynikWalidacji += this[nameof(Cena)] + "\n";
            if (this[nameof(Ilosc)] != string.Empty)
                wynikWalidacji += this[nameof(Ilosc)] + "\n";
            if (this[nameof(Rabat)] != string.Empty)
                wynikWalidacji += this[nameof(Rabat)] + "\n";
            if (this[nameof(DataRealizacji)] != string.Empty)
                wynikWalidacji += this[nameof(DataRealizacji)] + "\n";

            if (wynikWalidacji.Replace("\n",string.Empty) == string.Empty)
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
                    case nameof(Cena):
                        return DecimalValidator.SprawdzCene(Cena ?? -1);
                    case (nameof(Ilosc)):
                        return DecimalValidator.SprawdzIlosc(Ilosc);
                    case nameof(Rabat):
                        return BusinessValidator.SprawdzRabat(Rabat ?? 0);
                    case nameof(DataRealizacji):
                        return DateTimeValidator.SprawdzDateRealizacji(DataRealizacji ?? DateTime.Now);
                    default:
                        return string.Empty;
                }   
            }
        }

        #endregion
    }
}
