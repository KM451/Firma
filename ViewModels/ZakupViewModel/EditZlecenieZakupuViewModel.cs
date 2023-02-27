using Firma.Models.Entities;
using Firma.Models.EntitiesForView.ZlecenieZakupu;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace Firma.ViewModels.ZakupViewModel
{
    public class EditZlecenieZakupuViewModel: NoweZlecenieZakupuViewModel
    {
        #region Constructor
        public EditZlecenieZakupuViewModel(ZleceniaZakupu zz): base("Dane szczegłowe ZZ")
        {
            Item = Db.ZleceniaZakupu.FirstOrDefault(x=>x.Id == zz.Id);
            WszystkieList = new ObservableCollection<PozycjaZZForAllView>(Item.PozycjeZleceniaZakupu
                .Where(item => item.CzyAktywny == true)
                .Select(item => new PozycjaZZForAllView()
                {
                    Id= item.Id,
                    ProduktKod = item.Produkty.Tytul,
                    ProduktNazwa = item.Produkty.Nazwa,
                    ProduktJednostkaMiary = item.Produkty.JednostkiMiary.Tytul,
                    Cena = item.Cena,
                    ProduktCenaWaluta = item.Produkty.Ceny.Where(c => c.IdTypuCeny == 1).Select(c => c.Waluta).FirstOrDefault(),
                    Ilosc = item.Ilosc,
                    Rabat = item.Rabat / 100,
                    DataRealizacji = item.DataRealizacji,
                }).ToList());
            PrzypiszDostawce(zz.IdKontrahenta);
        }

        #endregion

        #region Methods

        private void PrzypiszDostawce(int id)
        {
            var kontrahent = Db.Kontrahenci.Where(item => item.Id == id).FirstOrDefault();
            DaneDostawcy = $"{kontrahent.Tytul},  {kontrahent.Nazwa},  NIP:  {kontrahent.Nip}";
            var adres = Db.Adresy.Where(a => a.IdKontrahenta == id && a.Siedziba).FirstOrDefault();
            AdresDostawcy = "ul. " + adres.Ulica + " " + adres.NrDomu.Trim()
                                                            + (adres.NrLokalu == null ? ", " : "/" + adres.NrLokalu.Trim() + ", ")
                                                            + adres.KodPoczowy.Trim() + " " + adres.Miejscowosc + ", " + adres.Kraj.Trim();
        }

        public override void Save() => Db.SaveChanges();

        #endregion


    }
}
