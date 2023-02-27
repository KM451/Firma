using Firma.Helpers;
using Firma.Models.Entities;
using Firma.Models.EntitiesForView.Kontrahent;
using Firma.Models.EntitiesForView.ZlecenieKompletacji;
using Firma.Models.EntitiesForView.ZlecenieZakupu;
using Firma.Models.Validators;
using Firma.ViewModels.Abstract;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Firma.ViewModels.KompletacjaViewModel
{
    public class NoweZlecenieKompViewModel : JedenWszystkieViewModel<ZleceniaKompletacji, SkladnikZKForAllView>, IDataErrorInfo
    {
        #region Constructors
        public NoweZlecenieKompViewModel() : base("Nowe ZK", "Składniki ZK", "Dodaj składnik/produkt uboczny ZK")
        {
            Item = new ZleceniaKompletacji();
            init();
            IdStatusuZK = StatusZK.FirstOrDefault().Id;
            IdMontera = Monter.FirstOrDefault().Id;
            Priorytet = PriorytetWartosc.FirstOrDefault();
            OczekiwanaDataRealizacji = DateTime.Today.AddDays(1);
            PotwierdzonaDataRealizacji = DateTime.Today.AddDays(1);
            Ilosc = 1;
            Item.CzyAktywny = true;
            Item.DataUtworzenia = DateTime.Now;
            Item.DataModyfikacji = DateTime.Now;
            Item.Tytul = "zk";
            Db.ZleceniaKompletacji.AddObject(Item);
        }
        public NoweZlecenieKompViewModel(string displayName) : base(displayName, "Składniki ZK", "Dodaj składnik/produkt uboczny ZK")
        {
            init();
        }
        public NoweZlecenieKompViewModel(ZleceniaKompletacji zk) : base("Edycja ZK", "Składniki ZK", "Dodaj składnik/produkt uboczny ZK")
        {
            Item = Db.ZleceniaKompletacji.FirstOrDefault(x => x.Id == zk.Id);
            init();
            IdStatusuZK = zk.IdStatusuZK;
            IdMontera = zk.IdMontera;
            Priorytet = zk.Priorytet;
            OczekiwanaDataRealizacji = zk.OczekiwanaDataRealizacji;
            PotwierdzonaDataRealizacji = zk.PotwierdzonaDataRealizacji;
            Ilosc = zk.Ilosc;

            var kontrahent = Db.Kontrahenci.Where(item => item.Id == zk.IdKontrahenta).FirstOrDefault();
            Kod = kontrahent.Tytul;
            DaneKontrahenta = $"{kontrahent.Nazwa},  NIP:  {kontrahent.Nip}";

            var produkt = Db.Produkty.Where(item => item.Id == zk.IdProduktu).FirstOrDefault();
            ProduktNazwa = produkt.Nazwa;
            ProduktKod = produkt.Tytul;
            ProduktDodatkowaNazwa = produkt.DodatkowaNazwa;
        }


        #endregion

        #region Fields and Properties

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
        public DateTime OczekiwanaDataRealizacji
        {
            get
            {
                return Item.OczekiwanaDataRealizacji;
            }
            set
            {
                if (value != Item.OczekiwanaDataRealizacji)
                {
                    Item.OczekiwanaDataRealizacji = value;
                    OnPropertyChanged(() => OczekiwanaDataRealizacji);
                }
            }
        }
        public DateTime? PotwierdzonaDataRealizacji
        {
            get
            {
                return Item.PotwierdzonaDataRealizacji;
            }
            set
            {
                if (value != Item.PotwierdzonaDataRealizacji)
                {
                    Item.PotwierdzonaDataRealizacji = value;
                    OnPropertyChanged(() => PotwierdzonaDataRealizacji);
                }
            }
        }
        public int IdProduktu
        {
            get
            {
                return Item.IdProduktu;
            }
            set
            {
                if (value != Item.IdProduktu)
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
                if (value != Item.Ilosc)
                {
                    Item.Ilosc = value;
                    OnPropertyChanged(() => Ilosc);
                }
            }
        }
        public int IdMontera
        {
            get
            {
                return Item.IdMontera;
            }
            set
            {
                if (value != Item.IdMontera)
                {
                    Item.IdMontera = value;
                    OnPropertyChanged(() => IdMontera);
                }
            }
        }
        public int Priorytet
        {
            get
            {
                return Item.Priorytet;
            }
            set
            {
                if (value != Item.Priorytet)
                {
                    Item.Priorytet = value;
                    OnPropertyChanged(() => Priorytet);
                }
            }
        }
        public int IdStatusuZK
        {
            get
            {
                return Item.IdStatusuZK;
            }
            set
            {
                if (value != Item.IdStatusuZK)
                {
                    Item.IdStatusuZK = value;
                    OnPropertyChanged(() => IdStatusuZK);
                }
            }
        }
        public TimeSpan? CzasZlecenia
        {
            get
            {
                return Item.CzasZlecenia;
            }
            set
            {
                if (value != Item.CzasZlecenia)
                {
                    Item.CzasZlecenia = value;
                    OnPropertyChanged(() => CzasZlecenia);
                }
            }
        }
        public List<StatusyZK> StatusZK { get; set; }
        public List<Monterzy> Monter { get; set; }
        public List<int> PriorytetWartosc { get; set; }
        public bool SkladnikAkt { get; set; }
        public bool UbocznyAkt { get; set; }

        private string _DaneKontrahenta;
        public string DaneKontrahenta
        {
            get
            {
                return _DaneKontrahenta;
            }
            set
            {
                if (_DaneKontrahenta != value)
                {
                    _DaneKontrahenta = value;
                    OnPropertyChanged(() => DaneKontrahenta);
                }
            }
        }

        private string _Kod;
        public string Kod
        {
            get
            {
                return _Kod;
            }
            set
            {
                if (_Kod != value)
                {
                    _Kod = value;
                    OnPropertyChanged(() => Kod);
                }
            }
        }

        private string _ProduktNazwa;
        public string ProduktNazwa
        {
            get
            {
                return _ProduktNazwa;
            }
            set
            {
                if (_ProduktNazwa != value)
                {
                    _ProduktNazwa = value;
                    OnPropertyChanged(() => ProduktNazwa);
                }
            }
        }

        private string _ProduktKod;
        public string ProduktKod
        {
            get
            {
                return _ProduktKod;
            }
            set
            {
                if (_ProduktKod != value)
                {
                    _ProduktKod = value;
                    OnPropertyChanged(() => ProduktKod);
                }
            }
        }

        private string _ProduktDodatkowaNazwa;
        public string ProduktDodatkowaNazwa
        {
            get
            {
                return _ProduktDodatkowaNazwa;
            }
            set
            {
                if (_ProduktDodatkowaNazwa != value)
                {
                    _ProduktDodatkowaNazwa = value;
                    OnPropertyChanged(() => ProduktDodatkowaNazwa);
                }
            }
        }

        private ObservableCollection<ProdukUbocznyZKForAllView> _WszystkieUboczneList;

        public ObservableCollection<ProdukUbocznyZKForAllView> WszystkieUboczneList
        {
            get => _WszystkieUboczneList;
            set
            {
                if (value != _WszystkieUboczneList)
                {
                    _WszystkieUboczneList = value;
                    OnPropertyChanged(() => WszystkieUboczneList);
                }
            }
        }

        private ProdukUbocznyZKForAllView _SelectedItemUboczny;
        public ProdukUbocznyZKForAllView SelectedItemUboczny
        {
            get
            {
                return _SelectedItemUboczny;
            }
            set
            {
                if (_SelectedItemUboczny != value)
                {
                    _SelectedItemUboczny = value;
                    OnPropertyChanged(() => SelectedItemUboczny);
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

        private ICommand _ShowProduktCommand;
        public ICommand ShowProduktCommand
        {
            get
            {
                if (_ShowProduktCommand == null)
                {
                    _ShowProduktCommand = new BaseCommand(() => ShowProdukt());
                }
                return _ShowProduktCommand;
            }
        }

        private ICommand _CzasPlus;
        public ICommand CzasPlus
        {
            get
            {
                if (_CzasPlus == null)
                {
                    _CzasPlus = new BaseCommand(() => DodajCzas());
                }
                return _CzasPlus;
            }
        }

        private ICommand _CzasMinus;
        public ICommand CzasMinus
        {
            get
            {
                if (_CzasMinus == null)
                {
                    _CzasMinus = new BaseCommand(() => OdejmijCzas());
                }
                return _CzasMinus;
            }
        }

        private ICommand _ShowAddStatusZKCommand;
        public ICommand ShowAddStatusZKCommand
        {
            get
            {
                if (_ShowAddStatusZKCommand == null)
                {
                    _ShowAddStatusZKCommand = new BaseCommand(() => ShowAddStatusZK());
                }
                return _ShowAddStatusZKCommand;
            }
        }

        private ICommand _ShowAddMonterCommand;
        public ICommand ShowAddMonterCommand
        {
            get
            {
                if (_ShowAddMonterCommand == null)
                {
                    _ShowAddMonterCommand = new BaseCommand(() => ShowAddMonter());
                }
                return _ShowAddMonterCommand;
            }
        }



        #endregion

        #region Methods
        /// <summary>
        /// Inicjalizacja klasy w konstruktorze
        /// </summary>
        private void init()
        {
            Messenger.Default.Register<KontrahentForZK>(this, PrzypiszKontrahenta);
            Messenger.Default.Register<ProduktForZK>(this, PrzypiszProdukt);
            Messenger.Default.Register<SkladnikiZleceniaKompletacji>(this, PrzypiszSkladnikZK);
            Messenger.Default.Register<ProduktyUboczneZleceniaKompletacji>(this, PrzypiszPrUbocznyZK);
            Messenger.Default.Register<string>(this, Odswiez);
            StatusZK = Db.StatusyZK.Where(s => s.CzyAktywny).ToList();
            Monter = Db.Monterzy.Where(s => s.CzyAktywny).ToList();
            PriorytetWartosc = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            SkladnikAkt = true;
            UbocznyAkt = false;

            WszystkieList = new ObservableCollection<SkladnikZKForAllView>(Item.SkladnikiZleceniaKompletacji
                .Where(item => item.CzyAktywny)
                .Select(item => new SkladnikZKForAllView()
                {
                    Id = item.Id,
                    IdProduktu = item.IdProduktu,
                    Kod = item.Produkty.Tytul,
                    Nazwa = item.Produkty.Nazwa,
                    Ilosc = item.Ilosc,
                    JednostkaMiary = item.Produkty.JednostkiMiary.Tytul
                }).ToList());

            WszystkieUboczneList = new ObservableCollection<ProdukUbocznyZKForAllView>(Item.ProduktyUboczneZleceniaKompletacji
                .Where(item => item.CzyAktywny)
                .Select(item => new ProdukUbocznyZKForAllView()
                {
                    Id = item.Id,
                    IdProduktu = item.IdProduktu,
                    Kod = item.Produkty.Tytul,
                    Nazwa = item.Produkty.Nazwa,
                    Ilosc = item.Ilosc,
                    JednostkaMiary = item.Produkty.JednostkiMiary.Tytul
                }).ToList());
        }
        /// <summary>
        /// Zmniejsza wartość CzasZlecenia
        /// </summary>
        private void OdejmijCzas()
        {
            if (CzasZlecenia == null)
            {
                CzasZlecenia = TimeSpan.Zero;
            }
            else
            {
                if (CzasZlecenia >= TimeSpan.Parse("00:05:00"))
                    CzasZlecenia -= TimeSpan.Parse("00:05:00");
            }
        }
        /// <summary>
        /// Zwiększa wartość czas zlecenia
        /// </summary>
        private void DodajCzas()
        {
            if (CzasZlecenia == null)
            {
                CzasZlecenia = TimeSpan.Zero;
            }
            else
            {
                CzasZlecenia += TimeSpan.Parse("00:05:00");
            }
        }
        /// <summary>
        /// Wywołanie okno wyboru kontrahenta
        /// </summary>
        private void ShowKontrahent() => Messenger.Default.Send("Kontrahenci ZK Show");
        /// <summary>
        /// Przypisuje dane z messengera do kontrahenta
        /// </summary>
        /// <param name="obj">Dane kontrahenta</param>
        private void PrzypiszKontrahenta(KontrahentForZK obj)
        {
            Kod = obj.Kod;
            DaneKontrahenta = $"{obj.Nazwa},  NIP:  {obj.Nip}";
            IdKontrahenta = obj.IdKontrahenta;
        }
        /// <summary>
        /// Wywoluje okno wyboru produktu
        /// </summary>
        private void ShowProdukt() => Messenger.Default.Send("Produkty ZK Show");
        /// <summary>
        /// Przypisuje dane z messengera do produktu
        /// </summary>
        /// <param name="obj">Dane produktu</param>
        private void PrzypiszProdukt(ProduktForZK obj)
        {
            IdProduktu = obj.IdProduktu;
            ProduktNazwa = obj.Nazwa;
            ProduktKod = obj.Kod;
            ProduktDodatkowaNazwa = obj.DodatkowaNazwa;
        }
        /// <summary>
        /// Przypisuje dane produktu do składnika zlecenia kompletacji 
        /// </summary>
        /// <param name="skladnikZK">Dane produktu do wprowadzenia jako składnik zk</param>
        private void PrzypiszSkladnikZK(SkladnikiZleceniaKompletacji skladnikZK)
        {
            Produkty produkt = Db.Produkty.First(item => item.Id == skladnikZK.IdProduktu);
            JednostkiMiary jednostka = Db.JednostkiMiary.First(item => item.Id == produkt.IdJednostkaMiary);
            if (SelectedItem != null)
            {
                var skladnikEdit = Db.SkladnikiZleceniaKompletacji.FirstOrDefault(s => s.Id == skladnikZK.Id);
                skladnikEdit.IdProduktu = skladnikZK.IdProduktu;
                skladnikEdit.Ilosc = skladnikZK.Ilosc;
                SelectedItem.IdProduktu = skladnikZK.IdProduktu;
                SelectedItem.Kod = produkt.Tytul;
                SelectedItem.Nazwa = produkt.Nazwa;
                SelectedItem.Ilosc = skladnikZK.Ilosc;
                SelectedItem.JednostkaMiary = jednostka.Tytul;
            }
            else
            {
                Item.SkladnikiZleceniaKompletacji.Add(skladnikZK);
                WszystkieList.Add(new SkladnikZKForAllView(skladnikZK.Id, skladnikZK.IdProduktu, produkt.Tytul, produkt.Nazwa, skladnikZK.Ilosc, jednostka.Tytul));
            }
        }
        /// <summary>
        /// Przypisuje dane produktu do produktu ubocznego zlecenia kompletacji 
        /// </summary>
        /// <param name="prUbocznyZK">Dane produktu do wprowadzenia jako produkt uboczny zk</param>
        private void PrzypiszPrUbocznyZK(ProduktyUboczneZleceniaKompletacji prUbocznyZK)
        {
            Produkty produkt = Db.Produkty.First(item => item.Id == prUbocznyZK.IdProduktu);
            JednostkiMiary jednostka = Db.JednostkiMiary.First(item => item.Id == produkt.IdJednostkaMiary);
            if (SelectedItemUboczny != null)
            {
                var ubocznyEdit = Db.ProduktyUboczneZleceniaKompletacji.FirstOrDefault(p => p.Id == prUbocznyZK.Id);
                ubocznyEdit.IdProduktu = prUbocznyZK.IdProduktu;
                ubocznyEdit.Ilosc = prUbocznyZK.Ilosc;
                SelectedItemUboczny.IdProduktu = prUbocznyZK.IdProduktu;
                SelectedItemUboczny.Kod = produkt.Tytul;
                SelectedItemUboczny.Nazwa = produkt.Nazwa;
                SelectedItemUboczny.Ilosc = prUbocznyZK.Ilosc;
                SelectedItemUboczny.JednostkaMiary = jednostka.Tytul;
            }
            else
            {
                Item.ProduktyUboczneZleceniaKompletacji.Add(prUbocznyZK);
                WszystkieUboczneList.Add(new ProdukUbocznyZKForAllView(prUbocznyZK.Id, prUbocznyZK.IdProduktu, produkt.Tytul, produkt.Nazwa, prUbocznyZK.Ilosc, jednostka.Tytul));
            }
        }
        /// <summary>
        /// Odświeżenie listy comboboxów po dodaniu nowych elementów
        /// </summary>
        /// <param name="obj">Teks to wyboru odpowiedniego comboboxa</param>
        private void Odswiez(string obj)
        {
            switch (obj)
            {
                case "odswiez Status ZK":
                    StatusZK = Db.StatusyZK.Where(s => s.CzyAktywny).ToList();
                    break;
                case "odswiez Montera":
                    Monter = Db.Monterzy.Where(s => s.CzyAktywny).ToList();
                    break;
            }
        }
        /// <summary>
        /// Wywołanie okna dodawania nowego statusu zk
        /// </summary>
        private void ShowAddStatusZK() => Messenger.Default.Send("Status ZK Show");
        /// <summary>
        /// Wywołanie okna dodawania nowego montera
        /// </summary>
        private void ShowAddMonter() => Messenger.Default.Send("Monterzy Show");
        /// <summary>
        /// Zapis danych
        /// </summary>
        public override void Save()
        {

            if (WszystkieUboczneList.Count == 0)
            { Item.ProduktyUboczneZleceniaKompletacji = null; }

            Db.SaveChanges();
        }
        /// <summary>
        /// Wywołanie okna dodawania nowego składnika lub produktu ubocznego w zależności od aktywnej zakładki datagrid
        /// </summary>
        protected override void ShowAddView()
        {
            if (SkladnikAkt)
            {
                SelectedItem = null;
                Messenger.Default.Send("Składniki zlecenia kompletacji Add");
            }

            if (UbocznyAkt)
            {
                SelectedItemUboczny = null;
                Messenger.Default.Send("Produkty uboczne zlecenia kompletacji Add");
            }

        }

        protected override void ShowEditView()
        {
            if (SkladnikAkt)
            {
                if (SelectedItem != null)
                {
                    Messenger.Default.Send(new EditSklZKForAllView()
                    {
                        Id = SelectedItem.Id,
                        IdProduktu = SelectedItem.IdProduktu,
                        Ilosc = SelectedItem.Ilosc
                    });
                }
            }

            if (UbocznyAkt)
            {
                if (SelectedItemUboczny != null)
                {
                    Messenger.Default.Send(new EditProdUbZKForAllView()
                    {
                        Id = SelectedItemUboczny.Id,
                        IdProduktu = SelectedItemUboczny.IdProduktu,
                        Ilosc = SelectedItemUboczny.Ilosc
                    });
                }
            }
        }

        /// <summary>
        /// Usuwanie wybranego składnika lub produktu ubocznego zlecenia kompletacji
        /// </summary>
        protected override void Delete()
        {
            if (SkladnikAkt)
            {
                if (SelectedItem != null)
                {

                    if (MessageBox.Show("Czy napewno chcesz usunąć wybrany składnik zlecenia kompletacji?", "Usuwanie", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {
                        Db.SkladnikiZleceniaKompletacji.First(item => item.Id == SelectedItem.Id).CzyAktywny = false;
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

            if (UbocznyAkt)
            {
                if (SelectedItemUboczny != null)
                {

                    if (MessageBox.Show("Czy napewno chcesz usunąć wybrany produkt uboczny zlecenia zakupu?", "Usuwanie", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {
                        Db.ProduktyUboczneZleceniaKompletacji.First(item => item.Id == SelectedItemUboczny.Id).CzyAktywny = false;
                        WszystkieUboczneList.Remove(SelectedItemUboczny);
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
        }


        #endregion

        #region Validation

        protected override bool IsValid()
        {
            string wynikWalidacji = string.Empty;

            if (this[nameof(OczekiwanaDataRealizacji)] != string.Empty)
                wynikWalidacji += this[nameof(OczekiwanaDataRealizacji)] + "\n";
            if (this[nameof(PotwierdzonaDataRealizacji)] != string.Empty)
                wynikWalidacji += this[nameof(PotwierdzonaDataRealizacji)] + "\n";
            if (this[nameof(Ilosc)] != string.Empty)
                wynikWalidacji += this[nameof(Ilosc)] + "\n";
            if (this[nameof(DaneKontrahenta)] != string.Empty)
                wynikWalidacji += this[nameof(DaneKontrahenta)] + "\n";
            if (this[nameof(ProduktNazwa)] != string.Empty)
                wynikWalidacji += this[nameof(ProduktNazwa)] + "\n";
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
                    case nameof(OczekiwanaDataRealizacji):
                        return DateTimeValidator.SprawdzOczekiwanaDataRealizacji(OczekiwanaDataRealizacji);
                    case nameof(PotwierdzonaDataRealizacji):
                        return DateTimeValidator.SprawdzPotwierdzonaDataRealizacji(PotwierdzonaDataRealizacji ?? DateTime.Now);
                    case (nameof(Ilosc)):
                        return DecimalValidator.SprawdzIlosc(Ilosc);
                    case nameof(DaneKontrahenta):
                        return StringValidator.SprawdzDaneKontrahenta(DaneKontrahenta);
                    case nameof(ProduktNazwa):
                        return StringValidator.SprawdzNazwe(ProduktNazwa);
                    case nameof(WszystkieList):
                        return DataGridValidator.SprawdzSkladniki(WszystkieList);

                    default:
                        return string.Empty;
                }
            }
        }

        #endregion
    }
}
