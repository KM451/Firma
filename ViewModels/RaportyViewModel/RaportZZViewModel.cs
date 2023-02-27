using Firma.Helpers;
using Firma.Models.BusinessLogic;
using Firma.Models.EntitiesForView.Kontrahent;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Windows.Input;

namespace Firma.ViewModels.RaportyViewModel
{
    public class RaportZZViewModel : DataBaseViewModel
    {
        #region Constructor
        public RaportZZViewModel() : base("Raport ZZ")
        {
            Messenger.Default.Register<KontrahentForRaportZZ>(this, PrzypiszKontrahenta);
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

        private int _IdKontrahenta;
        public int IdKontrahenta
        {
            get { return _IdKontrahenta; }
            set
            {
                if (_IdKontrahenta != value)
                {
                    _IdKontrahenta = value;
                    OnPropertyChanged(() => IdKontrahenta);
                }
            }
        }

        private string _Kod;
        public string Kod
        {
            get { return _Kod; }
            set
            {
                if (_Kod != value)
                {
                    _Kod = value;
                    OnPropertyChanged(() => Kod);
                }
            }
        }

        private string _DaneKontrahenta;

        public string DaneKontrahenta
        {
            get { return _DaneKontrahenta; }
            set
            {
                if (_DaneKontrahenta != value)
                {
                    _DaneKontrahenta = value;
                    OnPropertyChanged(() => DaneKontrahenta);
                }
            }
        }

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

        private string _WartoscZlecen;

        public string WartoscZlecen
        {
            get { return _WartoscZlecen; }
            set
            {
                if (_WartoscZlecen != value)
                {
                    _WartoscZlecen = value;
                    OnPropertyChanged(() => WartoscZlecen);
                }
            }
        }

        private string _WalutaZlecen;

        public string WalutaZlecen
        {
            get { return _WalutaZlecen; }
            set
            {
                if (_WalutaZlecen != value)
                {
                    _WalutaZlecen = value;
                    OnPropertyChanged(() => WalutaZlecen);
                }
            }
        }

        #endregion

        #region Commands
        private ICommand _ShowKontrahentCommand;
        public ICommand ShowKontrahentCommand
        {
            get
            {
                if (_ShowKontrahentCommand == null)
                {
                    _ShowKontrahentCommand = new BaseCommand(() => ShowKontrahent());
                }
                return _ShowKontrahentCommand;
            }
        }

        #endregion

        #region Methods

        protected override void ObliczClick()
        {
            IloscZlecen = new ZestawienieZZ(FirmaEntities).IloscZzKontrahent(IdKontrahenta, DataOd, DataDo);
            WartoscZlecen = new ZestawienieZZ(FirmaEntities).WartoscZZKontrahent(IdKontrahenta, DataOd, DataDo);
        }

        private void ShowKontrahent() => Messenger.Default.Send("Kontrahenci Raport ZZ Show");

        private void PrzypiszKontrahenta(KontrahentForRaportZZ obj)
        {
            Kod = obj.Kod;
            DaneKontrahenta = $"{obj.Nazwa},  NIP:  {obj.Nip}";
            IdKontrahenta = obj.IdKontrahenta;
        }

        #endregion

    }
}
