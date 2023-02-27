using Firma.Helpers;
using Firma.Models.Entities;
using Firma.Models.EntitiesForView;
using Firma.Models.EntitiesForView.Kontrahent;
using Firma.Models.EntitiesForView.Produkt;
using Firma.ViewModels.Abstract;
using Firma.ViewResources;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Firma.ViewModels.ProduktViewModel
{
    public class WszystkieProduktyViewModel : WszystkieViewModel<ProduktForAllView>
    {
        #region Constructor
        public WszystkieProduktyViewModel() : base(BaseResources.ListaProduktow)
        {
            WidocznoscKolumnaProducent = "Visible";
            WidocznoscKolumnaJM = "Visible";
            WidocznoscKolumnaMarka = "Visible";
            WidocznoscKolumnaDostawca = "Visible";
            WidocznoscKolumnaTyp = "Visible";
            WidocznoscKolumnaCena = "Visible";
            WidocznoscKolumnVat = "Visible";
            SelectedTypCeny = FirmaDBEntities.TypyCen.Where(x => x.CzyAktywny).Select(x => x.Id).FirstOrDefault();
            TypyCen = FirmaDBEntities.TypyCen.Where(x => x.CzyAktywny).Select(x => new ComboBoxKeyAndValue()
            {
                Key = x.Id,
                Value = x.Tytul
            }).ToList();
            FilterField = string.Empty;
            OrderByField = nameof(ProduktForAllView.IdProduktu);
            OrderBy();
        }
        #endregion

        #region Fields and Properties

        private int _SelectedTypCeny;
        public int SelectedTypCeny
        {
            get
            {
                return _SelectedTypCeny;
            }
            set
            {
                if (_SelectedTypCeny != value)
                {
                    _SelectedTypCeny = value;
                    OnPropertyChanged(() => SelectedItem);
                    Load();
                }
            }
        }
        private bool _SelectedBrutto;
        public bool SelectedBrutto
        {
            get { return _SelectedBrutto; }
            set
            {
                _SelectedBrutto = value;
                OnPropertyChanged(() => SelectedBrutto);
                Load();
            }
        }
        public string GdzieWyslac { get; set; }
        public List<ComboBoxKeyAndValue> TypyCen { get; set; }

        #endregion

        #region Method

        public override void Load()
        {
            AllList =
                    FirmaDBEntities.Produkty.Where(p => p.CzyAktywny).Select(p => new ProduktForAllView
                    {
                        IdProduktu = p.Id,
                        Kod = p.Tytul,
                        Nazwa = p.Nazwa,
                        DodatkowaNazwa = p.DodatkowaNazwa,
                        Marka = p.Marka,
                        JednostkaMiary = p.JednostkiMiary.Tytul,
                        Producent = p.Producent,
                        DostawcaNazwa = p.Kontrahenci.Nazwa,
                        VatSprzedazy = p.StawkaVatSprzedazy,
                        VatZakupu = p.StawkaVatZakupu,
                        Typ = p.TypProduktu,
                        Waluta = p.Ceny.Where(x => x.IdTypuCeny == SelectedTypCeny).Select(x => x.Waluta).FirstOrDefault(),
                        Cena = p.Ceny.Where(x => x.IdTypuCeny == SelectedTypCeny)
                        .Select(x => SelectedBrutto ? x.Wartosc + x.Wartosc * (x.TypyCen.Notatki == "zakup" ? x.Produkty.StawkaVatZakupu : x.Produkty.StawkaVatSprzedazy) / 100 : x.Wartosc
                        ).FirstOrDefault()
                    }).ToList();
            Filter();
        }

        protected override void Open()
        {
            if (CzyModyfikowac)
            {
                if (SelectedItem != null)
                    Messenger.Default.Send(FirmaDBEntities.Produkty.Where(p => p.Id == SelectedItem.IdProduktu).FirstOrDefault());
            }
            else
            {
                if (GdzieWyslac == "ZK")
                {
                    if (SelectedItem != null)
                    {
                        Messenger.Default.Send(new ProduktForZK()
                        {
                            IdProduktu = SelectedItem.IdProduktu,
                            Nazwa = SelectedItem.Nazwa,
                            Kod = SelectedItem.Kod,
                            DodatkowaNazwa = SelectedItem.DodatkowaNazwa,
                        });
                        OnRequestClose();
                    }
                }
            }
        }

        protected override void ShowAddView() => Messenger.Default.Send("Nowy produkt AddShow");

        protected override void Delete()
        {
            if (SelectedItem != null)
            {

                if (MessageBox.Show("Czy napewno chcesz usunąć wybrany produkt?", "Usuwanie", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    FirmaDBEntities.Produkty.First(item => item.Id == SelectedItem.IdProduktu).CzyAktywny = false;
                    List.Remove(SelectedItem);
                    try
                    {
                        FirmaDBEntities.SaveChanges();
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

        #region UkrywanieKolumn

        private string _WidocznoscKolumnaJM;
        private string _WidocznoscKolumnaProducent;
        private string _WidocznoscKolumnaMarka;
        private string _WidocznoscKolumnaDostawca;
        private string _WidocznoscKolumnaTyp;
        private string _WidocznoscKolumnaCena;
        private string _WidocznoscKolumnVat;

        public string WidocznoscKolumnaProducent
        {
            get
            {
                return _WidocznoscKolumnaProducent;
            }
            set
            {
                if (_WidocznoscKolumnaProducent != value)
                {
                    _WidocznoscKolumnaProducent = value;
                    OnPropertyChanged(() => WidocznoscKolumnaProducent);
                }
            }
        }
        public string WidocznoscKolumnaJM
        {
            get
            {
                return _WidocznoscKolumnaJM;
            }
            set
            {
                if (_WidocznoscKolumnaJM != value)
                {
                    _WidocznoscKolumnaJM = value;
                    OnPropertyChanged(() => WidocznoscKolumnaJM);
                }
            }
        }
        public string WidocznoscKolumnaMarka
        {
            get
            {
                return _WidocznoscKolumnaMarka;
            }
            set
            {
                if (_WidocznoscKolumnaMarka != value)
                {
                    _WidocznoscKolumnaMarka = value;
                    OnPropertyChanged(() => WidocznoscKolumnaMarka);
                }
            }
        }
        public string WidocznoscKolumnaDostawca
        {
            get
            {
                return _WidocznoscKolumnaDostawca;
            }
            set
            {
                if (_WidocznoscKolumnaDostawca != value)
                {
                    _WidocznoscKolumnaDostawca = value;
                    OnPropertyChanged(() => WidocznoscKolumnaDostawca);
                }
            }
        }
        public string WidocznoscKolumnaTyp
        {
            get
            {
                return _WidocznoscKolumnaTyp;
            }
            set
            {
                if (_WidocznoscKolumnaTyp != value)
                {
                    _WidocznoscKolumnaTyp = value;
                    OnPropertyChanged(() => WidocznoscKolumnaTyp);
                }
            }
        }
        public string WidocznoscKolumnaCena
        {
            get
            {
                return _WidocznoscKolumnaCena;
            }
            set
            {
                if (_WidocznoscKolumnaCena != value)
                {
                    _WidocznoscKolumnaCena = value;
                    OnPropertyChanged(() => WidocznoscKolumnaCena);
                }
            }
        }
        public string WidocznoscKolumnVat
        {
            get
            {
                return _WidocznoscKolumnVat;
            }
            set
            {
                if (_WidocznoscKolumnVat != value)
                {
                    _WidocznoscKolumnVat = value;
                    OnPropertyChanged(() => WidocznoscKolumnVat);
                }
            }
        }


        public ICommand ZmienWidocznoscKolumnyProducentCommand { get { return new BaseCommand(() => ZmienWidocznoscKolumnyProducent()); } }
        public ICommand ZmienWidocznoscKolumnyJMCommand { get { return new BaseCommand(() => ZmienWidocznoscKolumnyJM()); } }
        public ICommand ZmienWidocznoscKolumnyMarkaCommand { get { return new BaseCommand(() => ZmienWidocznoscKolumnyMarka()); } }
        public ICommand ZmienWidocznoscKolumnyDostawcaCommand { get { return new BaseCommand(() => ZmienWidocznoscKolumnyDostawca()); } }
        public ICommand ZmienWidocznoscKolumnyTypCommand { get { return new BaseCommand(() => ZmienWidocznoscKolumnyTyp()); } }
        public ICommand ZmienWidocznoscKolumnyCenaCommand { get { return new BaseCommand(() => ZmienWidocznoscKolumnyCena()); } }
        public ICommand ZmienWidocznoscKolumnVatCommand { get { return new BaseCommand(() => ZmienWidocznoscKolumnVat()); } }

        private void ZmienWidocznoscKolumnyProducent()
        {
            WidocznoscKolumnaProducent = WidocznoscKolumnaProducent == "Visible" ? "Collapsed" : "Visible";
            if (WidocznoscKolumnaProducent == "Visible")
            {
                ListOfItemsOrderBy.Add(new KeyValuePair<string, string>(nameof(ProduktForAllView.Producent), "Producent"));
                ListOfItemsFilter.Add(new KeyValuePair<string, string>(nameof(ProduktForAllView.Producent), "Producent"));
            }
            else
            {
                if (OrderByField == nameof(ProduktForAllView.Producent))
                    OrderByField = nameof(ProduktForAllView.IdProduktu);
                ListOfItemsOrderBy.Remove(new KeyValuePair<string, string>(nameof(ProduktForAllView.Producent), "Producent"));
                if (FilterField == nameof(ProduktForAllView.Producent))
                {
                    FilterField = string.Empty;
                    Filter();
                }
                ListOfItemsFilter.Remove(new KeyValuePair<string, string>(nameof(ProduktForAllView.Producent), "Producent"));
            }
        }

        private void ZmienWidocznoscKolumnyJM()
        {
            WidocznoscKolumnaJM = WidocznoscKolumnaJM == "Visible" ? "Collapsed" : "Visible";
            if (WidocznoscKolumnaJM == "Visible")
            {
                ListOfItemsOrderBy.Add(new KeyValuePair<string, string>(nameof(ProduktForAllView.JednostkaMiary), "Jednostka miary"));
                ListOfItemsFilter.Add(new KeyValuePair<string, string>(nameof(ProduktForAllView.JednostkaMiary), "Jednostka miary"));
            }
            else
            {
                if (OrderByField == nameof(ProduktForAllView.JednostkaMiary))
                    OrderByField = nameof(ProduktForAllView.IdProduktu);
                ListOfItemsOrderBy.Remove(new KeyValuePair<string, string>(nameof(ProduktForAllView.JednostkaMiary), "Jednostka miary"));
                if (FilterField == nameof(ProduktForAllView.JednostkaMiary))
                {
                    FilterField = string.Empty;
                    Filter();
                }
                ListOfItemsFilter.Remove(new KeyValuePair<string, string>(nameof(ProduktForAllView.JednostkaMiary), "Jednostka miary"));
            }
        }

        private void ZmienWidocznoscKolumnyMarka()
        {
            WidocznoscKolumnaMarka = WidocznoscKolumnaMarka == "Visible" ? "Collapsed" : "Visible";
            if (WidocznoscKolumnaMarka == "Visible")
            {
                ListOfItemsOrderBy.Add(new KeyValuePair<string, string>(nameof(ProduktForAllView.Marka), "Marka"));
                ListOfItemsFilter.Add(new KeyValuePair<string, string>(nameof(ProduktForAllView.Marka), "Marka"));
            }
            else
            {
                if (OrderByField == nameof(ProduktForAllView.Marka))
                    OrderByField = nameof(ProduktForAllView.IdProduktu);
                ListOfItemsOrderBy.Remove(new KeyValuePair<string, string>(nameof(ProduktForAllView.Marka), "Marka"));
                if (FilterField == nameof(ProduktForAllView.Marka))
                {
                    FilterField = string.Empty;
                    Filter();
                }
                ListOfItemsFilter.Remove(new KeyValuePair<string, string>(nameof(ProduktForAllView.Marka), "Marka"));
            }
        }

        private void ZmienWidocznoscKolumnyDostawca()
        {
            WidocznoscKolumnaDostawca = WidocznoscKolumnaDostawca == "Visible" ? "Collapsed" : "Visible";
            if (WidocznoscKolumnaDostawca == "Visible")
            {
                ListOfItemsOrderBy.Add(new KeyValuePair<string, string>(nameof(ProduktForAllView.DostawcaNazwa), "Dostawca"));
                ListOfItemsFilter.Add(new KeyValuePair<string, string>(nameof(ProduktForAllView.DostawcaNazwa), "Dostawca"));
            }
            else
            {
                if (OrderByField == nameof(ProduktForAllView.DostawcaNazwa))
                    OrderByField = nameof(ProduktForAllView.IdProduktu);
                ListOfItemsOrderBy.Remove(new KeyValuePair<string, string>(nameof(ProduktForAllView.DostawcaNazwa), "Dostawca"));
                if (FilterField == nameof(ProduktForAllView.DostawcaNazwa))
                {
                    FilterField = string.Empty;
                    Filter();
                }
                ListOfItemsFilter.Remove(new KeyValuePair<string, string>(nameof(ProduktForAllView.DostawcaNazwa), "Dostawca"));
            }
        }

        private void ZmienWidocznoscKolumnyTyp()
        {
            WidocznoscKolumnaTyp = WidocznoscKolumnaTyp == "Visible" ? "Collapsed" : "Visible";
            if (WidocznoscKolumnaTyp == "Visible")
            {
                ListOfItemsOrderBy.Add(new KeyValuePair<string, string>(nameof(ProduktForAllView.Typ), "Typ"));
                ListOfItemsFilter.Add(new KeyValuePair<string, string>(nameof(ProduktForAllView.Typ), "Typ"));
            }
            else
            {
                if (OrderByField == nameof(ProduktForAllView.Typ))
                    OrderByField = nameof(ProduktForAllView.IdProduktu);
                ListOfItemsOrderBy.Remove(new KeyValuePair<string, string>(nameof(ProduktForAllView.Typ), "Typ"));
                if (FilterField == nameof(ProduktForAllView.Typ))
                {
                    FilterField = string.Empty;
                    Filter();
                }
                ListOfItemsFilter.Remove(new KeyValuePair<string, string>(nameof(ProduktForAllView.Typ), "Typ"));
            }
        }

        private void ZmienWidocznoscKolumnyCena()
        {
            WidocznoscKolumnaCena = WidocznoscKolumnaCena == "Visible" ? "Collapsed" : "Visible";
            if (WidocznoscKolumnaCena == "Visible")
            {
                ListOfItemsOrderBy.Add(new KeyValuePair<string, string>(nameof(ProduktForAllView.Cena), "Cena"));
                ListOfItemsFilter.Add(new KeyValuePair<string, string>(nameof(ProduktForAllView.Cena), "Cena"));
                ListOfItemsOrderBy.Add(new KeyValuePair<string, string>(nameof(ProduktForAllView.Waluta), "Waluta"));
                ListOfItemsFilter.Add(new KeyValuePair<string, string>(nameof(ProduktForAllView.Waluta), "Waluta"));
            }
            else
            {
                if (OrderByField == nameof(ProduktForAllView.Cena) || OrderByField == nameof(ProduktForAllView.Waluta))
                    OrderByField = nameof(ProduktForAllView.IdProduktu);
                ListOfItemsOrderBy.Remove(new KeyValuePair<string, string>(nameof(ProduktForAllView.Cena), "Cena"));
                ListOfItemsOrderBy.Remove(new KeyValuePair<string, string>(nameof(ProduktForAllView.Waluta), "Waluta"));
                if (FilterField == nameof(ProduktForAllView.Cena) || FilterField == nameof(ProduktForAllView.Waluta))
                {
                    FilterField = string.Empty;
                    Filter();
                }
                ListOfItemsFilter.Remove(new KeyValuePair<string, string>(nameof(ProduktForAllView.Cena), "Cena"));
                ListOfItemsFilter.Remove(new KeyValuePair<string, string>(nameof(ProduktForAllView.Waluta), "Waluta"));
            }
        }

        private void ZmienWidocznoscKolumnVat()
        {
            WidocznoscKolumnVat = WidocznoscKolumnVat == "Visible" ? "Collapsed" : "Visible";
            if (WidocznoscKolumnVat == "Visible")
            {
                ListOfItemsOrderBy.Add(new KeyValuePair<string, string>(nameof(ProduktForAllView.VatZakupu), "Stawka Vat zakupu"));
                ListOfItemsFilter.Add(new KeyValuePair<string, string>(nameof(ProduktForAllView.VatZakupu), "Stawka Vat zakupu"));
                ListOfItemsOrderBy.Add(new KeyValuePair<string, string>(nameof(ProduktForAllView.VatSprzedazy), "Stawka Vat sprzedazy"));
                ListOfItemsFilter.Add(new KeyValuePair<string, string>(nameof(ProduktForAllView.VatSprzedazy), "Stawka Vat sprzedazy"));
            }
            else
            {
                if (OrderByField == nameof(ProduktForAllView.VatZakupu) || OrderByField == nameof(ProduktForAllView.VatSprzedazy))
                    OrderByField = nameof(ProduktForAllView.IdProduktu);
                ListOfItemsOrderBy.Remove(new KeyValuePair<string, string>(nameof(ProduktForAllView.VatZakupu), "Stawka Vat zakupu"));
                ListOfItemsOrderBy.Remove(new KeyValuePair<string, string>(nameof(ProduktForAllView.VatSprzedazy), "Stawka Vat sprzedazy"));
                if (FilterField == nameof(ProduktForAllView.VatZakupu) || FilterField == nameof(ProduktForAllView.VatSprzedazy))
                {
                    FilterField = string.Empty;
                    Filter();
                }
                ListOfItemsFilter.Remove(new KeyValuePair<string, string>(nameof(ProduktForAllView.VatZakupu), "Stawka Vat zakupu"));
                ListOfItemsFilter.Remove(new KeyValuePair<string, string>(nameof(ProduktForAllView.VatSprzedazy), "Stawka Vat sprzedazy"));
            }
        }

        #endregion

        #region OrderBy


        protected override void OrderBy()
        {
            switch (OrderByField)
            {
                case nameof(ProduktForAllView.IdProduktu):
                    List = new ObservableCollection<ProduktForAllView>(OrderDescending ?
                        List.OrderByDescending(item => item.IdProduktu) : List.OrderBy(item => item.IdProduktu));
                    break;
                case nameof(ProduktForAllView.Kod):
                    List = new ObservableCollection<ProduktForAllView>(OrderDescending ?
                        List.OrderByDescending(item => item.Kod) : List.OrderBy(item => item.Kod));
                    break;
                case nameof(ProduktForAllView.Nazwa):
                    List = new ObservableCollection<ProduktForAllView>(OrderDescending ?
                        List.OrderByDescending(item => item.Nazwa) : List.OrderBy(item => item.Nazwa));
                    break;
                case nameof(ProduktForAllView.DodatkowaNazwa):
                    List = new ObservableCollection<ProduktForAllView>(OrderDescending ?
                        List.OrderByDescending(item => item.DodatkowaNazwa) : List.OrderBy(item => item.DodatkowaNazwa));
                    break;
                case nameof(ProduktForAllView.Marka):
                    List = new ObservableCollection<ProduktForAllView>(OrderDescending ?
                        List.OrderByDescending(item => item.Marka) : List.OrderBy(item => item.Marka));
                    break;
                case nameof(ProduktForAllView.JednostkaMiary):
                    List = new ObservableCollection<ProduktForAllView>(OrderDescending ?
                        List.OrderByDescending(item => item.JednostkaMiary) : List.OrderBy(item => item.JednostkaMiary));
                    break;
                case nameof(ProduktForAllView.Producent):
                    List = new ObservableCollection<ProduktForAllView>(OrderDescending ?
                        List.OrderByDescending(item => item.Producent) : List.OrderBy(item => item.Producent));
                    break;
                case nameof(ProduktForAllView.DostawcaNazwa):
                    List = new ObservableCollection<ProduktForAllView>(OrderDescending ?
                        List.OrderByDescending(item => item.DostawcaNazwa) : List.OrderBy(item => item.DostawcaNazwa));
                    break;
                case nameof(ProduktForAllView.Typ):
                    List = new ObservableCollection<ProduktForAllView>(OrderDescending ?
                        List.OrderByDescending(item => item.Typ) : List.OrderBy(item => item.Typ));
                    break;
                case nameof(ProduktForAllView.Cena):
                    List = new ObservableCollection<ProduktForAllView>(OrderDescending ?
                        List.OrderByDescending(item => item.Cena) : List.OrderBy(item => item.Cena));
                    break;
                case nameof(ProduktForAllView.Waluta):
                    List = new ObservableCollection<ProduktForAllView>(OrderDescending ?
                        List.OrderByDescending(item => item.Waluta) : List.OrderBy(item => item.Waluta));
                    break;
                case nameof(ProduktForAllView.VatZakupu):
                    List = new ObservableCollection<ProduktForAllView>(OrderDescending ?
                        List.OrderByDescending(item => item.VatZakupu) : List.OrderBy(item => item.VatZakupu));
                    break;
                case nameof(ProduktForAllView.VatSprzedazy):
                    List = new ObservableCollection<ProduktForAllView>(OrderDescending ?
                        List.OrderByDescending(item => item.VatSprzedazy) : List.OrderBy(item => item.VatSprzedazy));
                    break;
            }
        }

        protected override ObservableCollection<KeyValuePair<string, string>> GetListOfItemsOrderBy()
        {
            return new ObservableCollection<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>(nameof(ProduktForAllView.IdProduktu), "ID"),
                new KeyValuePair<string, string>(nameof(ProduktForAllView.Kod), "Kod"),
                new KeyValuePair<string, string>(nameof(ProduktForAllView.Nazwa), "Nazwa"),
                new KeyValuePair<string, string>(nameof(ProduktForAllView.Cena), "Cena"),
                new KeyValuePair<string, string>(nameof(ProduktForAllView.Waluta), "Waluta"),
                new KeyValuePair<string, string>(nameof(ProduktForAllView.JednostkaMiary), "Jednostka miary"),
                new KeyValuePair<string, string>(nameof(ProduktForAllView.Producent), "Producent"),
                new KeyValuePair<string, string>(nameof(ProduktForAllView.Marka), "Marka"),
                new KeyValuePair<string, string>(nameof(ProduktForAllView.DostawcaNazwa), "Dostawca"),
                new KeyValuePair<string, string>(nameof(ProduktForAllView.Typ), "Typ"),
                new KeyValuePair<string, string>(nameof(ProduktForAllView.VatZakupu), "Stawka Vat zakupu"),
                new KeyValuePair<string, string>(nameof(ProduktForAllView.VatSprzedazy), "Stawka Vat sprzedazy")
            };
        }


        #endregion

        #region Filter

        protected override void Filter()
        {
            if (!string.IsNullOrEmpty(SearchPhrase))
            {
                switch (FilterField)
                {
                    case nameof(ProduktForAllView.IdProduktu):
                        List = new ObservableCollection<ProduktForAllView>(AllList.Where(item => item.IdProduktu.ToString().Contains(SearchPhrase)));
                        break;
                    case nameof(ProduktForAllView.Kod):
                        List = new ObservableCollection<ProduktForAllView>(AllList.Where(item => item.Kod.Contains(SearchPhrase)));
                        break;
                    case nameof(ProduktForAllView.Nazwa):
                        List = new ObservableCollection<ProduktForAllView>(AllList.Where(item => item.Nazwa.Contains(SearchPhrase)));
                        break;
                    case nameof(ProduktForAllView.DodatkowaNazwa):
                        List = new ObservableCollection<ProduktForAllView>(AllList.Where(item => item.DodatkowaNazwa.Contains(SearchPhrase)));
                        break;
                    case nameof(ProduktForAllView.Marka):
                        List = new ObservableCollection<ProduktForAllView>(AllList.Where(item => item.Marka?.Contains(SearchPhrase) ?? false));
                        break;
                    case nameof(ProduktForAllView.JednostkaMiary):
                        List = new ObservableCollection<ProduktForAllView>(AllList.Where(item => item.JednostkaMiary.Contains(SearchPhrase)));
                        break;
                    case nameof(ProduktForAllView.Producent):
                        List = new ObservableCollection<ProduktForAllView>(AllList.Where(item => item.Producent?.Contains(SearchPhrase) ?? false));
                        break;
                    case nameof(ProduktForAllView.DostawcaNazwa):
                        List = new ObservableCollection<ProduktForAllView>(AllList.Where(item => item.DostawcaNazwa?.Contains(SearchPhrase) ?? false));
                        break;
                    case nameof(ProduktForAllView.Typ):
                        List = new ObservableCollection<ProduktForAllView>(AllList.Where(item => item.Typ.Contains(SearchPhrase)));
                        break;
                    case nameof(ProduktForAllView.Cena):
                        List = new ObservableCollection<ProduktForAllView>(AllList.Where(item => item.Cena.ToString()?.Contains(SearchPhrase) ?? false));
                        break;
                    case nameof(ProduktForAllView.Waluta):
                        List = new ObservableCollection<ProduktForAllView>(AllList.Where(item => item.Waluta?.Contains(SearchPhrase) ?? false));
                        break;
                    case nameof(ProduktForAllView.VatZakupu):
                        List = new ObservableCollection<ProduktForAllView>(AllList.Where(item => item.VatZakupu.ToString().Contains(SearchPhrase)));
                        break;
                    case nameof(ProduktForAllView.VatSprzedazy):
                        List = new ObservableCollection<ProduktForAllView>(AllList.Where(item => item.VatSprzedazy.ToString().Contains(SearchPhrase)));
                        break;

                    default:
                        List = new ObservableCollection<ProduktForAllView>(AllList);
                        break;
                }
            }
            else
            {
                List = new ObservableCollection<ProduktForAllView>(AllList);
            }
            OrderBy();
        }

        protected override ObservableCollection<KeyValuePair<string, string>> GetListOfItemsFilter()
        {
            return new ObservableCollection<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>(string.Empty, "brak fitrowania"),
                new KeyValuePair<string, string>(nameof(ProduktForAllView.Kod), "Kod"),
                new KeyValuePair<string, string>(nameof(ProduktForAllView.Nazwa), "Nazwa"),
                new KeyValuePair<string, string>(nameof(ProduktForAllView.Cena), "Cena"),
                new KeyValuePair<string, string>(nameof(ProduktForAllView.Waluta), "Waluta"),
                new KeyValuePair<string, string>(nameof(ProduktForAllView.JednostkaMiary), "Jednostka miary"),
                new KeyValuePair<string, string>(nameof(ProduktForAllView.Producent), "Producent"),
                new KeyValuePair<string, string>(nameof(ProduktForAllView.Marka), "Marka"),
                new KeyValuePair<string, string>(nameof(ProduktForAllView.DostawcaNazwa), "Dostawca"),
                new KeyValuePair<string, string>(nameof(ProduktForAllView.Typ), "Typ"),
                new KeyValuePair<string, string>(nameof(ProduktForAllView.VatZakupu), "Stawka Vat zakupu"),
                new KeyValuePair<string, string>(nameof(ProduktForAllView.VatSprzedazy), "Stawka Vat sprzedazy")
            };
        }

        #endregion

        
    }
}
