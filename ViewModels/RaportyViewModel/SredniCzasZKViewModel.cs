using Firma.Models.BusinessLogic;
using System;

namespace Firma.ViewModels.RaportyViewModel
{
    public class SredniCzasZKViewModel : DataBaseViewModel
    {
        #region Constructor
        public SredniCzasZKViewModel(): base("Raport ZK")
        {
            _DataOd = DateTime.Today;
            _DataDo = DateTime.Today;
        }
        #endregion

        #region Fields and Properties

        private DateTime _DataOd;
        public DateTime DataOd
        {
            get { return _DataOd; }
            set
            {
                if (_DataOd != value)
                {
                    _DataOd = value;
                    OnPropertyChanged(() => DataOd);
                }
            }
        }

        private DateTime _DataDo;
        public DateTime DataDo
        {
            get { return _DataDo; }
            set
            {
                if (_DataDo != value)
                {
                    _DataDo = value;
                    OnPropertyChanged(() => DataDo);
                }
            }
        }

        private double _SredniCzas;

        public double SredniCzas
        {
            get { return _SredniCzas; }
            set
            {
                if (_SredniCzas != value)
                {
                    _SredniCzas = value;
                    OnPropertyChanged(() => SredniCzas);
                }
            }
        }
        #endregion

        #region Method
        protected override void ObliczClick()
        {
            SredniCzas = new SredniCzasZK(FirmaEntities).SredniCzasRealizacjiZK(DataOd, DataDo);
        }
        #endregion

    }
}
