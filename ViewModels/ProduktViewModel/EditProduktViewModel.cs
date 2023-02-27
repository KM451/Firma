using Firma.Models.Entities;
using Firma.Models.EntitiesForView.Produkt;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace Firma.ViewModels.ProduktViewModel
{
    public class EditProduktViewModel : NowyProduktViewModel
    {
        #region Constructor

        public EditProduktViewModel(Produkty produkt)
        {
            Item = produkt;
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
            PrzypiszDostawce(produkt.IdDostawcy ?? 0);
            StawkaVatZakupu = produkt.StawkaVatZakupu;
            StawkaVatSprzedazy = produkt.StawkaVatSprzedazy;
            StawkaCla = produkt.StawkaCla;
            StawkaAkcyzy = produkt.StawkaAkcyzy;
            TpTowar = produkt.TypProduktu == "towar" ? true : false;
            TpProdukt = produkt.TypProduktu == "produkt" ? true : false;
            TpKoszt = produkt.TypProduktu == "koszt" ? true : false;
            TpUsluga = produkt.TypProduktu == "usługa" ? true : false;
        }

        #endregion

        #region Methods

        private void PrzypiszDostawce(int id)
        {
            var dostawca = Db.Kontrahenci.Where(d => d.Id == id).FirstOrDefault();
            DaneDostawcy = dostawca == null ? string.Empty : $"{dostawca?.Nazwa ?? string.Empty}, NIP: {dostawca?.Nip ?? string.Empty}";
        }

        public override void Save()
        {
            if (IsValid())
            {
                var oldProdukt = Db.Produkty.Where(p => p.Id == Item.Id).FirstOrDefault();
                TypProduktu = typProduktu(TpTowar, TpProdukt, TpKoszt, TpUsluga);
                Item.TypProduktu = TypProduktu;


                oldProdukt.Tytul = Item.Tytul;
                oldProdukt.DataModyfikacji = DateTime.Now;
                oldProdukt.Nazwa = Item.Nazwa;
                oldProdukt.IdJednostkaMiary = Item.IdJednostkaMiary;
                oldProdukt.TypProduktu = TypProduktu;
                oldProdukt.StawkaCla = Item.StawkaCla;
                oldProdukt.Marka = Item.Marka;
                oldProdukt.StawkaVatZakupu = Item.StawkaVatZakupu;
                oldProdukt.StawkaVatSprzedazy = Item.StawkaVatSprzedazy;
                oldProdukt.DodatkowaNazwa = Item.DodatkowaNazwa;
                oldProdukt.IdDostawcy = Item.IdDostawcy;
                oldProdukt.IdKoduPcn = Item.IdKoduPcn;
                oldProdukt.Symbol = Item.Symbol;
                oldProdukt.SWWPKWiU = Item.SWWPKWiU;
                oldProdukt.Producent = Item.Producent;
                oldProdukt.KrajPochodzenia = Item.KrajPochodzenia;
                var oldCena = Db.Ceny.Where(c => c.IdProduktu == Item.Id).ToList();
                foreach (var item in oldCena)
                {
                    item.Wartosc = CenyProduktu.Where(tc => tc.Id == item.IdTypuCeny).Select(tc => tc?.Wartosc ?? 0).FirstOrDefault();
                    item.Waluta = CenyProduktu.Where(tc => tc.Id == item.IdTypuCeny).Select(tc => tc?.Waluta ?? "-").FirstOrDefault();

                }
                foreach (var item in CenyProduktu)
                {
                    if (oldCena.Exists(x => x.IdTypuCeny == item.Id))
                    {
                    }
                    else
                    {
                        Db.Ceny.AddObject(new Ceny()
                        {
                            Tytul = "c",
                            CzyAktywny = true,
                            DataUtworzenia = DateTime.Today,
                            DataModyfikacji = DateTime.Today,
                            IdProduktu = oldProdukt.Id,
                            IdTypuCeny = item.Id,
                            Wartosc = item.Wartosc,
                            Waluta = item.Waluta
                        });
                    }
                }

                Db.SaveChanges();
            }
        }
        #endregion
    }
}
