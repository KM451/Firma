using Firma.Helpers;
using Firma.Models.Entities;
using Firma.Models.EntitiesForView.Kontrahent;
using Firma.Models.EntitiesForView.ZlecenieZakupu;
using Firma.Models.Validators;
using Firma.ViewModels.Abstract;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Firma.ViewModels.ZakupViewModel
{
    public class NoweZlecenieZakupuViewModel : JedenWszystkieViewModel<ZleceniaZakupu, PozycjaZZForAllView>, IDataErrorInfo
    {
        #region Constructors
        public NoweZlecenieZakupuViewModel() : base("Nowe ZZ", "Pozycje Zlecenia Zakupu", "Dodaj Pozycje Zlecenia Zakupu")
        {
            Item = new ZleceniaZakupu();
            init();
            IdStatusuZZ = StatusZZ.FirstOrDefault().Id;
            IdTypuTransakcji = TypTransakcji.FirstOrDefault().Id;
            IdSposobuDostawy = SposobDostawy.FirstOrDefault().Id;
            IdRodzajuTransportu = RodzajTransportu.FirstOrDefault().Id;
            IdSposobuPlatnosci = SposobPlatnosci.FirstOrDefault().Id;

            Item.Tytul = "zz";
            Item.CzyAktywny = true;
            Item.DataUtworzenia = DateTime.Now;
            Item.DataModyfikacji = DateTime.Now;
            Db.ZleceniaZakupu.AddObject(Item);
        }


        public NoweZlecenieZakupuViewModel(ZleceniaZakupu zz) : base("Edycja ZZ", "Pozycje Zlecenia Zakupu", "Dodaj Pozycje Zlecenia Zakupu")
        {
            Item = Db.ZleceniaZakupu.FirstOrDefault(x => x.Id == zz.Id);
            init();
            var kontrahent = Db.Kontrahenci.Where(item => item.Id == zz.IdKontrahenta).FirstOrDefault();
            DaneDostawcy = $"{kontrahent.Tytul},  {kontrahent.Nazwa},  NIP:  {kontrahent.Nip}";
            var adres = Db.Adresy.Where(a => a.IdKontrahenta == zz.IdKontrahenta && a.Siedziba).FirstOrDefault();
            AdresDostawcy = "ul. " + adres.Ulica + " " + adres.NrDomu.Trim()
                                                            + (adres.NrLokalu == null ? ", " : "/" + adres.NrLokalu.Trim() + ", ")
                                                            + adres.KodPoczowy.Trim() + " " + adres.Miejscowosc + ", " + adres.Kraj.Trim();

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
        public bool CzyAktywny
        {
            get
            {
                return Item.CzyAktywny;
            }
            set
            {
                if (value != Item.CzyAktywny)
                {
                    Item.CzyAktywny = value;
                    OnPropertyChanged(() => CzyAktywny);
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
        public DateTime DataUtworzenia
        {
            get
            {
                return Item.DataUtworzenia;
            }
            set
            {
                if (value != Item.DataUtworzenia)
                {
                    Item.DataUtworzenia = value;
                    OnPropertyChanged(() => DataUtworzenia);
                }
            }
        }
        public DateTime DataModyfikacji
        {
            get
            {
                return Item.DataModyfikacji;
            }
            set
            {
                if (value != Item.DataModyfikacji)
                {
                    Item.DataModyfikacji = value;
                    OnPropertyChanged(() => DataModyfikacji);
                }
            }
        }
        public string NrOferty
        {
            get
            {
                return Item.NrOferty;
            }
            set
            {
                if (value != Item.NrOferty)
                {
                    Item.NrOferty = value;
                    OnPropertyChanged(() => NrOferty);
                }
            }
        }
        public bool Potwierdzone
        {
            get
            {
                return Item.Potwierdzone;
            }
            set
            {
                if (value != Item.Potwierdzone)
                {
                    Item.Potwierdzone = value;
                    OnPropertyChanged(() => Potwierdzone);
                }
            }
        }
        public string NrPotwierdzenia
        {
            get
            {
                return Item.NrPotwierdzenia;
            }
            set
            {
                if (value != Item.NrPotwierdzenia)
                {
                    Item.NrPotwierdzenia = value;
                    OnPropertyChanged(() => NrPotwierdzenia);
                }
            }
        }
        public int IdKontrahenta
        {
            get
            {
                return Item.IdKontrahenta;
            }
            set
            {
                if (value != Item.IdKontrahenta)
                {
                    Item.IdKontrahenta = value;
                    OnPropertyChanged(() => IdKontrahenta);
                }
            }
        }
        public int IdStatusuZZ
        {
            get
            {
                return Item.IdStatusuZZ;
            }
            set
            {
                if (value != Item.IdStatusuZZ)
                {
                    Item.IdStatusuZZ = value;
                    OnPropertyChanged(() => IdStatusuZZ);
                }
            }
        }
        public int IdTypuTransakcji
        {
            get
            {
                return Item.IdTypuTransakcji;
            }
            set
            {
                if (value != Item.IdTypuTransakcji)
                {
                    Item.IdTypuTransakcji = value;
                    OnPropertyChanged(() => IdTypuTransakcji);
                }
            }
        }
        public int IdSposobuDostawy
        {
            get
            {
                return Item.IdSposobuDostawy;
            }
            set
            {
                if (value != Item.IdSposobuDostawy)
                {
                    Item.IdSposobuDostawy = value;
                    OnPropertyChanged(() => IdSposobuDostawy);
                }
            }
        }
        public int IdRodzajuTransportu
        {
            get
            {
                return Item.IdRodzajuTransportu;
            }
            set
            {
                if (value != Item.IdRodzajuTransportu)
                {
                    Item.IdRodzajuTransportu = value;
                    OnPropertyChanged(() => IdRodzajuTransportu);
                }
            }
        }
        public int IdSposobuPlatnosci
        {
            get
            {
                return Item.IdSposobuPlatnosci;
            }
            set
            {
                if (value != Item.IdSposobuPlatnosci)
                {
                    Item.IdSposobuPlatnosci = value;
                    OnPropertyChanged(() => IdSposobuPlatnosci);
                }
            }
        }
        public List<StatusyZZ> StatusZZ { get; set; }
        public List<TypyTransakcji> TypTransakcji { get; set; }
        public List<SposobyDostawy> SposobDostawy { get; set; }
        public List<RodzajeTransportu> RodzajTransportu { get; set; }
        public List<SposobyPlatnosci> SposobPlatnosci { get; set; }
        public string AdresDostawcy { get; set; }

        private string _DaneDostawcy;
        public string DaneDostawcy
        {
            get
            {
                return _DaneDostawcy;
            }
            set
            {
                if (_DaneDostawcy != value)
                {
                    _DaneDostawcy = value;
                    OnPropertyChanged(() => DaneDostawcy);
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

        private ICommand _ShowAddRodzajTransportuCommand;
        public ICommand ShowAddRodzajTransportuCommand
        {
            get
            {
                if (_ShowAddRodzajTransportuCommand == null)
                {
                    _ShowAddRodzajTransportuCommand = new BaseCommand(() => ShowRodzajTransportu());
                }
                return _ShowAddRodzajTransportuCommand;
            }
        }

        private ICommand _ShowAddSposobDostawyCommand;
        public ICommand ShowAddSposobDostawyCommand
        {
            get
            {
                if (_ShowAddSposobDostawyCommand == null)
                {
                    _ShowAddSposobDostawyCommand = new BaseCommand(() => ShowSposobDostawy());
                }
                return _ShowAddSposobDostawyCommand;
            }
        }

        private ICommand _ShowAddSposobPlatnosciCommand;
        public ICommand ShowAddSposobPlatnosciCommand
        {
            get
            {
                if (_ShowAddSposobPlatnosciCommand == null)
                {
                    _ShowAddSposobPlatnosciCommand = new BaseCommand(() => ShowSposobPlatnosci());
                }
                return _ShowAddSposobPlatnosciCommand;
            }
        }

        private ICommand _ShowAddStatusZZCommand;
        public ICommand ShowAddStatusZZCommand
        {
            get
            {
                if (_ShowAddStatusZZCommand == null)
                {
                    _ShowAddStatusZZCommand = new BaseCommand(() => ShowStatusZZ());
                }
                return _ShowAddStatusZZCommand;
            }
        }

        private ICommand _ShowAddTypTransakcjiCommand;
        public ICommand ShowAddTypTransakcjiCommand
        {
            get
            {
                if (_ShowAddTypTransakcjiCommand == null)
                {
                    _ShowAddTypTransakcjiCommand = new BaseCommand(() => ShowTypTransakcji());
                }
                return _ShowAddTypTransakcjiCommand;
            }
        }

        #endregion

        #region Methods

        private void Odswiez(string obj)
        {
            switch (obj)
            {
                case "odswiez TypTransakcji":
                    TypTransakcji = Db.TypyTransakcji.Where(t => t.CzyAktywny).ToList();
                    break;
                case "odswiez SposobPlatnosci":
                    SposobPlatnosci = Db.SposobyPlatnosci.Where(s => s.CzyAktywny).ToList();
                    break;
                case "odswiez SposobDostawy":
                    SposobDostawy = Db.SposobyDostawy.Where(s => s.CzyAktywny).ToList();
                    break;
                case "odswiez StatusZZ":
                    StatusZZ = Db.StatusyZZ.Where(s => s.CzyAktywny).ToList();
                    break;
                case "odswiez RodzajTransportu":
                    RodzajTransportu = Db.RodzajeTransportu.Where(r => r.CzyAktywny).ToList();
                    break;
            }
        }
        private void init()
        {
            Messenger.Default.Register<DostawcaForZZ>(this, PrzypiszDostawce);
            Messenger.Default.Register<PozycjeZleceniaZakupu>(this, PrzypiszPozycjeZZ);
            Messenger.Default.Register<string>(this, Odswiez);

            WszystkieList = new ObservableCollection<PozycjaZZForAllView>(Item.PozycjeZleceniaZakupu
                .Where(item => item.CzyAktywny == true)
                .Select(item => new PozycjaZZForAllView()
                {
                    Id = item.Id,
                    IdProduktu = item.IdProduktu,
                    ProduktKod = item.Produkty.Tytul,
                    ProduktNazwa = item.Produkty.Nazwa,
                    ProduktJednostkaMiary = item.Produkty.JednostkiMiary.Tytul,
                    Cena = item.Cena,
                    ProduktCenaWaluta = item.Produkty.Ceny.Where(c => c.IdTypuCeny == 1).Select(c => c.Waluta).FirstOrDefault(),
                    Ilosc = item.Ilosc,
                    Rabat = item.Rabat / 100,
                    DataRealizacji = item.DataRealizacji,
                }).ToList());

            StatusZZ = Db.StatusyZZ.Where(s => s.CzyAktywny).ToList();
            TypTransakcji = Db.TypyTransakcji.Where(t => t.CzyAktywny).ToList();
            SposobDostawy = Db.SposobyDostawy.Where(s => s.CzyAktywny).ToList();
            RodzajTransportu = Db.RodzajeTransportu.Where(r => r.CzyAktywny).ToList();
            SposobPlatnosci = Db.SposobyPlatnosci.Where(s => s.CzyAktywny).ToList();


        }


        private void PrzypiszPozycjeZZ(PozycjeZleceniaZakupu pozycjaZZ)
        {
            Produkty produkt = Db.Produkty.First(item => item.Id == pozycjaZZ.IdProduktu);
            JednostkiMiary jednostka = Db.JednostkiMiary.First(item => item.Id == produkt.IdJednostkaMiary);
            Ceny cena = Db.Ceny.FirstOrDefault(item => item.IdProduktu == produkt.Id && item.IdTypuCeny == 1);

            if (SelectedItem != null)
            {
                var pozycjaEdit = Db.PozycjeZleceniaZakupu.FirstOrDefault(c => c.Id == SelectedItem.Id);
                pozycjaEdit.IdProduktu = pozycjaZZ.IdProduktu;
                pozycjaEdit.Ilosc = pozycjaZZ.Ilosc;
                pozycjaEdit.Cena = pozycjaZZ.Cena;
                pozycjaEdit.Rabat = pozycjaZZ.Rabat;
                pozycjaEdit.DataRealizacji = pozycjaZZ.DataRealizacji;

                SelectedItem.IdProduktu = pozycjaZZ.IdProduktu;
                SelectedItem.ProduktKod = produkt.Tytul;
                SelectedItem.ProduktNazwa = produkt.Nazwa;
                SelectedItem.ProduktJednostkaMiary = jednostka.Tytul;
                SelectedItem.Cena = pozycjaZZ.Cena;
                SelectedItem.ProduktCenaWaluta = cena.Waluta;
                SelectedItem.Ilosc = pozycjaZZ.Ilosc;
                SelectedItem.Rabat = pozycjaZZ.Rabat / 100;
                SelectedItem.DataRealizacji = pozycjaZZ.DataRealizacji;
            }
            else
            {
                pozycjaZZ.Notatki = Db.Ceny.Where(c => c.IdTypuCeny == 1).Select(c => c.Waluta).FirstOrDefault();
                Item.PozycjeZleceniaZakupu.Add(pozycjaZZ);
                WszystkieList.Add(new PozycjaZZForAllView(
                                                            pozycjaZZ.Id,
                                                            pozycjaZZ.IdProduktu,
                                                            produkt.Tytul,
                                                            produkt.Nazwa,
                                                            jednostka.Tytul,
                                                            cena.Waluta,
                                                            pozycjaZZ.Ilosc,
                                                            pozycjaZZ.Cena,
                                                            pozycjaZZ.Rabat / 100,
                                                            pozycjaZZ.DataRealizacji));
            }
        }

        private void ShowKontrahent() => Messenger.Default.Send("Dostawcy ZZ Show");
        private void ShowRodzajTransportu() => Messenger.Default.Send("RodzajTransportu ZZ Show");
        private void ShowSposobDostawy() => Messenger.Default.Send("SposobDostawy ZZ Show");
        private void ShowSposobPlatnosci() => Messenger.Default.Send("SposobPlatnosci ZZ Show");
        private void ShowStatusZZ() => Messenger.Default.Send("StatusZZ ZZ Show");
        private void ShowTypTransakcji() => Messenger.Default.Send("TypTransakcji ZZ Show");
        protected override void ShowAddView()
        {
            SelectedItem = null;
            Messenger.Default.Send(WszystkieDisplayName + " Add");
        }

        private void PrzypiszDostawce(DostawcaForZZ obj)
        {
            DaneDostawcy = $"{obj.Kod},  {obj.Nazwa},  NIP:  {obj.Nip}";
            AdresDostawcy = $"{obj.KontrachentAdres}";
            IdKontrahenta = obj.IdKontrahenta;
        }

        public override void Save()
        {
            if (IsValid())
            {
                Db.SaveChanges();
            }
        }

        protected override void ShowEditView()
        {
            if (SelectedItem != null)
            {
                Messenger.Default.Send(new PZZForEdit()
                {
                    IdProduktu = SelectedItem.IdProduktu,
                    Ilosc = SelectedItem.Ilosc,
                    Cena = SelectedItem.Cena,
                    Rabat = SelectedItem.Rabat,
                    DataRealizacji = SelectedItem.DataRealizacji,

                });
            }
        }

        protected override void Delete()
        {
            if (SelectedItem != null)
            {

                if (MessageBox.Show("Czy napewno chcesz usunąć wybraną pozycję zlecenia zakupu?", "Usuwanie", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    Db.PozycjeZleceniaZakupu.First(item => item.Id == SelectedItem.Id).CzyAktywny = false;
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
            string wynikWalidacji = string.Empty;

            if (this[nameof(DaneDostawcy)] != string.Empty)
                wynikWalidacji += this[nameof(DaneDostawcy)] + "\n";
            if (this[nameof(WszystkieList)] != string.Empty)
                wynikWalidacji += this[nameof(WszystkieList)] + "\n";

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
                    case nameof(DaneDostawcy):
                        return StringValidator.SprawdzDaneDostawcy(DaneDostawcy);
                    case nameof(WszystkieList):
                        return DataGridValidator.SprawdzDataGrid(WszystkieList);
                    default:
                        return string.Empty;
                }
            }
        }

        #endregion
    }
}
