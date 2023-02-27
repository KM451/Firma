using Firma.Models.Entities;
using Firma.Models.EntitiesForView.ZlecenieKompletacji;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace Firma.ViewModels.KompletacjaViewModel
{

    public class EditZlecenieKompViewModel : NoweZlecenieKompViewModel
    {
        #region Constructor
        public EditZlecenieKompViewModel(ZleceniaKompletacji zk) : base("Dane szczegółowe ZK")
        {
            Item = zk;

            WszystkieList = new ObservableCollection<SkladnikZKForAllView>(Item.SkladnikiZleceniaKompletacji
                .Where(item => item.CzyAktywny)
                .Select(item => new SkladnikZKForAllView()
                {
                    Kod = item.Produkty.Tytul,
                    Nazwa = item.Produkty.Nazwa,
                    Ilosc = item.Ilosc,
                    JednostkaMiary = item.Produkty.JednostkiMiary.Tytul
                }).ToList());

            WszystkieUboczneList = new ObservableCollection<ProdukUbocznyZKForAllView>(Item.ProduktyUboczneZleceniaKompletacji
                .Where(item => item.CzyAktywny)
                .Select(item => new ProdukUbocznyZKForAllView()
                {
                    Kod = item.Produkty.Tytul,
                    Nazwa = item.Produkty.Nazwa,
                    Ilosc = item.Ilosc,
                    JednostkaMiary = item.Produkty.JednostkiMiary.Tytul
                }).ToList());
            PrzypiszKontrahenta(zk.IdKontrahenta);
            PrzypiszProdukt(zk.IdProduktu);
        }

        #endregion

        #region Methods


        private void PrzypiszKontrahenta(int id)
        {
            var kontrahent = Db.Kontrahenci.Where(item => item.Id == id).FirstOrDefault();
            Kod = kontrahent.Tytul;
            DaneKontrahenta = $"{kontrahent.Nazwa},  NIP:  {kontrahent.Nip}";
        }

        private void PrzypiszProdukt(int id)
        {
            var produkt = Db.Produkty.Where(item => item.Id == id).FirstOrDefault();
            ProduktNazwa = produkt.Nazwa;
            ProduktKod = produkt.Tytul;
            ProduktDodatkowaNazwa = produkt.DodatkowaNazwa;
        }

        public override void Save()
        {
            Item.CzyAktywny = true;
            //Item.DataUtworzenia = DateTime.Now;
            Item.DataModyfikacji = DateTime.Now;
            Item.Tytul = "zk";
            //Db.SaveChanges();
        }

        #endregion

        #region Validation

        protected override bool IsValid()
        {
            string wynikWalidacji = string.Empty;

            if (this[nameof(Ilosc)] != string.Empty)
                wynikWalidacji += this[nameof(Ilosc)] + "\n";

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
        #endregion
    }
}
