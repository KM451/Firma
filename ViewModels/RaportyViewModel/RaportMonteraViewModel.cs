using Firma.Models.BusinessLogic;
using Firma.Models.EntitiesForView;
using System;
using System.Collections.Generic;

namespace Firma.ViewModels.RaportyViewModel
{
    public class RaportMonteraViewModel : DataBaseViewModel
    {
        #region Constructor

        public RaportMonteraViewModel() : base("Raport Montera")
        {
            Data = DateTime.Today;
            IloscZlecen = 0;
            CzasZlecen = TimeSpan.Zero;
        }
        #endregion

        #region Fields and Properties
        private DateTime _Data;
        public DateTime Data
        {
            get { return _Data; }
            set
            {
                if (_Data != value)
                {
                    _Data = value;
                    OnPropertyChanged(() => Data);
                }
            }
        }

        private int _IdMontera;

        public int IdMontera
        {
            get { return _IdMontera; }
            set
            {
                if (_IdMontera != value)
                {
                    _IdMontera = value;
                    OnPropertyChanged(() => IdMontera);
                }
            }
        }

        public List<ComboBoxKeyAndValue> MonterComboBoxItems { get { return new MonterB(FirmaEntities).GetMonterComboBoxItems(); } }

        private int? _IloscZlecen;

        public int? IloscZlecen
        {
            get { return _IloscZlecen; }
            set
            {
                if (_IloscZlecen != value)
                {
                    _IloscZlecen = value;
                    OnPropertyChanged(() => IloscZlecen);
                }
            }
        }

        private TimeSpan _CzasZlecen;

        public TimeSpan CzasZlecen
        {
            get { return _CzasZlecen; }
            set
            {
                if (_CzasZlecen != value)
                {
                    _CzasZlecen = value;
                    OnPropertyChanged(() => CzasZlecen);
                }
            }
        }

        #endregion

        #region Method

        protected override void ObliczClick()
        {
            IloscZlecen = new OblozenieB(FirmaEntities).IloscZlecenMonter(IdMontera, Data);
            CzasZlecen = new OblozenieB(FirmaEntities).CzasZlecenMonter(IdMontera, Data);
        }

        #endregion
    }
}
