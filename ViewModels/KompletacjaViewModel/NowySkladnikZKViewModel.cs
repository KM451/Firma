using Firma.Models.Entities;
using Firma.Models.EntitiesForView;
using Firma.Models.EntitiesForView.ZlecenieKompletacji;
using Firma.Models.Validators;
using Firma.ViewModels.Abstract;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;

namespace Firma.ViewModels.KompletacjaViewModel
{
    public class NowySkladnikZKViewModel : JedenViewModel<SkladnikiZleceniaKompletacji>, IDataErrorInfo
    {
        #region Fields and Properties

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


        #endregion

        #region Constructors
        public NowySkladnikZKViewModel() : base("Składnik ZK")
        {
            Item = new SkladnikiZleceniaKompletacji()
            {
                CzyAktywny = true,
                Tytul = "pz",
                DataUtworzenia = DateTime.Now,
                DataModyfikacji = DateTime.Now,
            };
            init();
            IdProduktu = Produkty.FirstOrDefault().Key;
            Ilosc = 1;
        }

        public NowySkladnikZKViewModel(EditSklZKForAllView szk) : base("Edycja składnika ZK")
        {
            Item = Db.SkladnikiZleceniaKompletacji.FirstOrDefault(s => s.Id == szk.Id);
            init();
            IdProduktu = szk.IdProduktu;
            Ilosc = szk.Ilosc;
        }

        #endregion

        #region Methods

        private void init() => Produkty = Db.Produkty.Where(item => item.CzyAktywny == true)
                                         .Select(item => new ComboBoxKeyAndValue() { Key = item.Id, Value = item.Nazwa })
                                         .ToList();

        public override void Save()
        {
            if(IsValid())
                Messenger.Default.Send(Item);
        }


        #endregion

        #region Validation
        protected override bool IsValid()
        {
            if (this[nameof(Ilosc)] == string.Empty)
                return true;
            else
            {
                MessageBox.Show("Podano nieprawidłowe dane.\n" + this[nameof(Ilosc)] 
                    + "\nPopraw podane pozycje przed zapisem!", "Błąd");
                return false;
            }

        }
        public string Error => string.Empty;
        public string this[string columnName]
        {
            get
            {
                if (columnName == nameof(Ilosc))
                    return DecimalValidator.SprawdzIlosc(Ilosc);
                else
                    return string.Empty;
            }
        }
        #endregion
    }
}
