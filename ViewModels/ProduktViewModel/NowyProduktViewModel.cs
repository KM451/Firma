using Firma.Helpers;
using Firma.Models.Entities;
using Firma.Models.EntitiesForView;
using Firma.Models.EntitiesForView.Kontrahent;
using Firma.Models.EntitiesForView.Produkt;
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

namespace Firma.ViewModels.ProduktViewModel
{
    public class NowyProduktViewModel : JedenViewModel<Produkty>, IDataErrorInfo
    {
        #region Constructor
        public NowyProduktViewModel() : base("Produkt")
        {
            Item = new Produkty();
            JednostkaMiary = Db.JednostkiMiary.Where(item => item.CzyAktywny).ToList();
            IdJednostkaMiary = JednostkaMiary.FirstOrDefault().Id;
            KodPcn = Db.KodyPcn.Where(item => item.CzyAktywny).ToList();
            IdKoduPcn = KodPcn.FirstOrDefault()?.Id;
            TpProdukt = true;
            CzyAktywny = true;

            StawkiProcentoweVAT = new List<ComboBoxKeyAndValueDecimal>{new ComboBoxKeyAndValueDecimal(0,"0,00%"), new ComboBoxKeyAndValueDecimal(8.00m,"8,00%"),
                                                                new ComboBoxKeyAndValueDecimal(22.00m,"22,00%"), new ComboBoxKeyAndValueDecimal(23.00m,"23,00%")};
            StawkiProcentoweClo = new List<ComboBoxKeyAndValueDecimal>{new ComboBoxKeyAndValueDecimal(0,"0,00%"), new ComboBoxKeyAndValueDecimal(8.00m,"8,00%"),
                                                                new ComboBoxKeyAndValueDecimal(22.00m,"22,00%"), new ComboBoxKeyAndValueDecimal(23.00m,"23,00%")};
            StawkiProcentoweAkcyza = new List<ComboBoxKeyAndValueDecimal>{ new ComboBoxKeyAndValueDecimal(0,"0,00%"), new ComboBoxKeyAndValueDecimal(3.30m,"3,30%"),
                                                                    new ComboBoxKeyAndValueDecimal(6.50m,"6,50%"), new ComboBoxKeyAndValueDecimal(12.00m,"12,00%")};

            Messenger.Default.Register<KontrahentForAllView>(this, PrzypiszDostawce);
            Messenger.Default.Register<string>(this, Odswiez);

            CenyProduktu = new ObservableCollection<CenaForAllView>(Db.TypyCen.Where(x => x.CzyAktywny)
                                                                    .GroupJoin(Db.Ceny.Where(y => y.CzyAktywny && y.IdProduktu == Item.Id),
                                                                    typCeny => typCeny.Id,
                                                                    cena => cena.IdTypuCeny,
                                                                    (x, y) => new { typCeny = x, cena = y })
                                                                    .SelectMany(
                                                                    xy => xy.cena.DefaultIfEmpty(),
                                                                    (x, y) => new { typCeny = x.typCeny, cena = y })
                                                                    .Select(s => new CenaForAllView()
                                                                    {
                                                                        Id = s.typCeny.Id,
                                                                        Nazwa = s.typCeny.Tytul,
                                                                        SpZak = s.typCeny.Notatki,
                                                                        Wartosc = s.cena.Wartosc == null ? 0 : s.cena.Wartosc,
                                                                        Waluta = s.cena.Waluta == null ? "-" : s.cena.Waluta
                                                                    }).ToList());

            WalutaList = new List<string> { "-", "PLN", "EURO" };

        }

        #endregion

        #region Fields and Properties

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
        public string DodatkowaNazwa
        {
            get
            {
                return Item.DodatkowaNazwa;
            }
            set
            {
                if (value != Item.DodatkowaNazwa)
                {
                    Item.DodatkowaNazwa = value;
                    OnPropertyChanged(() => DodatkowaNazwa);
                }
            }
        }
        public int IdJednostkaMiary
        {
            get
            {
                return Item.IdJednostkaMiary;
            }
            set
            {
                if (value != Item.IdJednostkaMiary)
                {
                    Item.IdJednostkaMiary = value;
                    OnPropertyChanged(() => IdJednostkaMiary);
                }
            }
        }
        public string Symbol
        {
            get
            {
                return Item.Symbol;
            }
            set
            {
                if (value != Item.Symbol)
                {
                    Item.Symbol = value;
                    OnPropertyChanged(() => Symbol);
                }
            }
        }
        public string SWWPKWiU
        {
            get
            {
                return Item.SWWPKWiU;
            }
            set
            {
                if (value != Item.SWWPKWiU)
                {
                    Item.SWWPKWiU = value;
                    OnPropertyChanged(() => SWWPKWiU);
                }
            }
        }
        public decimal StawkaVatZakupu
        {
            get
            {
                return Item.StawkaVatZakupu;
            }
            set
            {
                if (value != Item.StawkaVatZakupu)
                {
                    Item.StawkaVatZakupu = value;
                    OnPropertyChanged(() => StawkaVatZakupu);
                }
            }
        }
        public decimal StawkaVatSprzedazy
        {
            get
            {
                return Item.StawkaVatSprzedazy;
            }
            set
            {
                if (value != Item.StawkaVatSprzedazy)
                {
                    Item.StawkaVatSprzedazy = value;
                    OnPropertyChanged(() => StawkaVatSprzedazy);
                }
            }
        }
        public int? IdDostawcy
        {
            get
            {
                return Item.IdDostawcy;
            }
            set
            {
                if (value != Item.IdDostawcy)
                {
                    Item.IdDostawcy = value;
                    OnPropertyChanged(() => IdDostawcy);
                }
            }
        }
        public string Producent
        {
            get
            {
                return Item.Producent;
            }
            set
            {
                if (value != Item.Producent)
                {
                    Item.Producent = value;
                    OnPropertyChanged(() => Producent);
                }
            }
        }
        public string Marka
        {
            get
            {
                return Item.Marka;
            }
            set
            {
                if (value != Item.Marka)
                {
                    Item.Marka = value;
                    OnPropertyChanged(() => Marka);
                }
            }
        }
        public decimal? StawkaCla
        {
            get
            {
                return Item.StawkaCla;
            }
            set
            {
                if (value != Item.StawkaCla)
                {
                    Item.StawkaCla = value;
                    OnPropertyChanged(() => StawkaCla);
                }
            }
        }
        public decimal? StawkaAkcyzy
        {
            get
            {
                return Item.StawkaAkcyzy;
            }
            set
            {
                if (value != Item.StawkaAkcyzy)
                {
                    Item.StawkaAkcyzy = value;
                    OnPropertyChanged(() => StawkaAkcyzy);
                }
            }
        }
        public int? IdKoduPcn
        {
            get
            {
                return Item.IdKoduPcn;
            }
            set
            {
                if (value != Item.IdKoduPcn)
                {
                    Item.IdKoduPcn = value;
                    OnPropertyChanged(() => IdKoduPcn);
                }
            }
        }
        public string KrajPochodzenia
        {
            get
            {
                return Item.KrajPochodzenia;
            }
            set
            {
                if (value != Item.KrajPochodzenia)
                {
                    Item.KrajPochodzenia = value;
                    OnPropertyChanged(() => KrajPochodzenia);
                }
            }
        }
        public IEnumerable<string> WalutaList { get; set; }
        public List<JednostkiMiary> JednostkaMiary { get; set; }
        public List<KodyPcn> KodPcn { get; set; }
        public List<ComboBoxKeyAndValueDecimal> StawkiProcentoweVAT { get; set; }
        public List<ComboBoxKeyAndValueDecimal> StawkiProcentoweClo { get; set; }
        public List<ComboBoxKeyAndValueDecimal> StawkiProcentoweAkcyza { get; set; }
        private ObservableCollection<CenaForAllView> _CenyProduktu;
        public ObservableCollection<CenaForAllView> CenyProduktu
        {
            get
            {
                return _CenyProduktu;
            }
            set
            {
                if (_CenyProduktu != value)
                {
                    _CenyProduktu = value;
                    OnPropertyChanged(() => CenyProduktu);
                }
            }
        }
        public CenaForAllView SelectedCena { get; set; }
        private string _NowyRodzajCeny;
        public string NowyRodzajCeny
        {
            get
            {
                return _NowyRodzajCeny;
            }
            set
            {
                if (_NowyRodzajCeny != value)
                {
                    _NowyRodzajCeny = value;
                    OnPropertyChanged(() => NowyRodzajCeny);
                }
            }
        }
        private bool _SelectedZakup;
        public bool SelectedZakup
        {
            get { return _SelectedZakup; }
            set
            {
                _SelectedZakup = value;
                OnPropertyChanged(() => SelectedZakup);
            }
        }

        //obsługa radiobuttonow
        private bool _TpTowar;
        private bool _TpProdukt;
        private bool _TpKoszt;
        private bool _TpUsluga;
        public bool TpTowar
        {
            get { return _TpTowar; }
            set
            {
                _TpTowar = value;
                OnPropertyChanged(() => TpTowar);
            }
        }
        public bool TpProdukt
        {
            get { return _TpProdukt; }
            set
            {
                _TpProdukt = value;
                OnPropertyChanged(() => TpProdukt);
            }
        }
        public bool TpKoszt
        {
            get { return _TpKoszt; }
            set
            {
                _TpKoszt = value;
                OnPropertyChanged(() => TpKoszt);
            }
        }
        public bool TpUsluga
        {
            get { return _TpUsluga; }
            set
            {
                _TpUsluga = value;
                OnPropertyChanged(() => TpUsluga);
            }
        }

        private string _TypProduktu;
        public string TypProduktu
        {
            get
            {
                return _TypProduktu;
            }
            set
            {
                _TypProduktu = value;
                OnPropertyChanged(() => TypProduktu);
            }
        }

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

        private ICommand _ShowDostawcyCommand;
        public ICommand ShowDostawcyCommand
        {
            get
            {
                if (_ShowDostawcyCommand == null)
                {
                    _ShowDostawcyCommand = new BaseCommand(() => ShowDostawcy());
                }
                return _ShowDostawcyCommand;
            }
        }

        private BaseCommand _SaveTypCenyCommand;
        public ICommand SaveTypCenyCommand
        {
            get
            {
                if (_SaveTypCenyCommand == null)
                {
                    _SaveTypCenyCommand = new BaseCommand(() => saveTypCeny());
                }
                return _SaveTypCenyCommand;
            }
        }

        private BaseCommand _DeleteTypCenyCommand;
        public ICommand DeleteTypCenyCommand
        {
            get
            {
                if (_DeleteTypCenyCommand == null)
                {
                    _DeleteTypCenyCommand = new BaseCommand(() => deleteTypCeny());
                }
                return _DeleteTypCenyCommand;
            }
        }

        private ICommand _ShowAddJednostkaCommand;
        public ICommand ShowAddJednostkaCommand
        {
            get
            {
                if (_ShowAddJednostkaCommand == null)
                {
                    _ShowAddJednostkaCommand = new BaseCommand(() => ShowAddJednostka());
                }
                return _ShowAddJednostkaCommand;
            }
        }

        private ICommand _ShowAddKodPcnCommand;
        public ICommand ShowAddKodPcnCommand
        {
            get
            {
                if (_ShowAddKodPcnCommand == null)
                {
                    _ShowAddKodPcnCommand = new BaseCommand(() => ShowKodPcn());
                }
                return _ShowAddKodPcnCommand;
            }
        }

        #endregion

        #region Methods

        private void ShowAddJednostka() => Messenger.Default.Send("Jednostka Miary Show");
        private void ShowKodPcn() => Messenger.Default.Send("Kod Pcn Show");

        /// <summary>
        /// Odświeża listę pozycji w wybranym comboboxie
        /// </summary>
        /// <param name="obj">Tekst identyfikujący combobox</param>
        private void Odswiez(string obj)
        {
            switch (obj)
            {
                case "odswiez Jednostke Miary":
                    JednostkaMiary = Db.JednostkiMiary.Where(item => item.CzyAktywny).ToList();
                    break;
                case "odswiez Kod PCN":
                    KodPcn = Db.KodyPcn.Where(item => item.CzyAktywny).ToList();
                    break;
            }
        }
        private void PrzypiszDostawce(KontrahentForAllView obj)
        {
            DaneDostawcy = $"{obj.Nazwa}, NIP: {obj.Nip}";
            IdDostawcy = obj.IdKontrahenta;
        }
        private void ShowDostawcy() => Messenger.Default.Send("Dostawcy Show");
        /// <summary>
        /// Zwraca typ produktu na podstawie stanu radiobutonów
        /// </summary>
        /// <param name="t">Stan radiobutona Towar</param>
        /// <param name="p">Stan radiobutona Produkt</param>
        /// <param name="k">Stan radiobutona Koszt</param>
        /// <param name="u">Stan radiobutona Usługa</param>
        /// <returns>Tekst z nazwą typu produktu</returns>
        protected string typProduktu(bool t, bool p, bool k, bool u) => string.Concat(t ? "towar" : "", p ? "produkt" : "", k ? "koszt" : "", u ? "usługa" : "");
        private void saveTypCeny()
        {
            if (NowyRodzajCeny != null)
            {
                Db.TypyCen.AddObject(new TypyCen()
                {
                    Tytul = NowyRodzajCeny,
                    Notatki = SelectedZakup ? "zakup" : "sprzedaż",
                    CzyAktywny = true,
                    DataUtworzenia = DateTime.Now,
                    DataModyfikacji = DateTime.Now
                });
                Db.SaveChanges();
                CenyProduktu.Add(new CenaForAllView()
                {
                    Id = Db.TypyCen.Where(x => x.Tytul == NowyRodzajCeny).Select(x => x.Id).FirstOrDefault(),
                    Nazwa = NowyRodzajCeny,
                    SpZak = SelectedZakup ? "zakup" : "sprzedaż",
                    Wartosc = 0,
                    Waluta = "-"
                });

                NowyRodzajCeny = null;
            }
        }
        private void deleteTypCeny()
        {
            if (SelectedCena != null)
            {

                if (MessageBox.Show("Czy napewno chcesz usunąć wybraną rodzaj ceny?", "Usuwanie", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    Db.TypyCen.First(item => item.Id == SelectedCena.Id).CzyAktywny = false;
                    CenyProduktu.Remove(SelectedCena);
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




        public override void Save()
        {
            if (IsValid())
            {
                TypProduktu = typProduktu(TpTowar, TpProdukt, TpKoszt, TpUsluga);
                Item.TypProduktu = TypProduktu;
                Item.DataUtworzenia = DateTime.Now;
                Item.DataModyfikacji = DateTime.Now;
                Db.Produkty.AddObject(Item);
                Db.SaveChanges();
                foreach (var c in CenyProduktu)
                {
                    if (c.Wartosc != 0)
                    {
                        Db.Ceny.AddObject(new Ceny()
                        {
                            Tytul = "c",
                            CzyAktywny = true,
                            DataUtworzenia = DateTime.Now,
                            DataModyfikacji = DateTime.Now,
                            IdProduktu = Item.Id,
                            IdTypuCeny = c.Id,
                            Wartosc = c.Wartosc,
                            Waluta = c.Waluta,
                        });
                    }
                }
                Db.SaveChanges();
            }
        }

        #endregion

        #region Validation
        protected override bool IsValid()
        {
            string wynikWalidacji = string.Empty;

            if (this[nameof(Tytul)] != string.Empty)
                wynikWalidacji += this[nameof(Tytul)] + "\n";
            if (this[nameof(Nazwa)] != string.Empty)
                wynikWalidacji += this[nameof(Nazwa)] + "\n";
            if (this[nameof(KrajPochodzenia)] != string.Empty)
                wynikWalidacji += this[nameof(KrajPochodzenia)] + "\n";

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
                    case nameof(Tytul):
                        return StringValidator.SprawdzKod(Tytul);
                    case (nameof(Nazwa)):
                        return StringValidator.SprawdzNazwe(Nazwa);
                    case nameof(KrajPochodzenia):
                        return StringValidator.SprawdzKraj(KrajPochodzenia);
                    default:
                        return string.Empty;
                }
            }
        }

        #endregion
    }
}
