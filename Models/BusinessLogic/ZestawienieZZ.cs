using Firma.Helpers;
using Firma.Models.Entities;
using System;
using System.Linq;

namespace Firma.Models.BusinessLogic
{
    public class ZestawienieZZ : DatabaseClass
    {
        public ZestawienieZZ(FirmaDBEntities firmaEntities) : base(firmaEntities)
        {
        }

        /// <summary>
        /// Podaje ilość zleceń zakupu wystawionych do danego kontrahenta w podanym przedziale czasowym
        /// </summary>
        /// <param name="idKontrahenta">Id kontrahenta</param>
        /// <param name="data_od">Data początkowa okresu sprawdzania</param>
        /// <param name="data_do">Data końcowa okresu sprawdzania</param>
        /// <returns>Wartość całkowitą ilości dni</returns>
        public int? IloscZzKontrahent(int idKontrahenta, DateTime data_od, DateTime data_do) =>
                            firmaEntities.ZleceniaZakupu.Where(zz => zz.CzyAktywny == true &&
                                                          zz.IdKontrahenta == idKontrahenta &&
                                                          zz.DataUtworzenia >= data_od &&
                                                          zz.DataUtworzenia <= data_do).Count();

        /// <summary>
        /// Podaje wartość zleceń zakupu wystawionych do danego kontrahenta w zadanym przedziale czasowym
        /// </summary>
        /// <param name="idKontrahenta">Id kontrahenta</param>
        /// <param name="data_od">Data początkowa okresu sprawdzania</param>
        /// <param name="data_do">Data końcowa okresu sprawdzania</param>
        /// <returns>Tekst zawierający wartość zamówień oraz walutę</returns>
        public string WartoscZZKontrahent(int idKontrahenta, DateTime data_od, DateTime data_do)
        {
            var item = firmaEntities.ZleceniaZakupu
                .Where(zz => zz.CzyAktywny == true &&
                             zz.IdKontrahenta == idKontrahenta &&
                             zz.DataUtworzenia >= data_od &&
                             zz.DataUtworzenia <= data_do).ToList();

            var wartosc = item.Select(zz => zz.PozycjeZleceniaZakupu
                                             .Where(pzz => pzz.CzyAktywny)
                                             .Select(pzz => (pzz.Cena == null ? 0 : pzz.Cena)
                                                         * pzz.Ilosc 
                                                         * (100 - (pzz.Rabat == null ? 0 : pzz.Rabat)) / 100)
                                             .Sum())
                              .Sum();

            var waluta = item.Select(zz => zz.PozycjeZleceniaZakupu
                             .Where(pzz => pzz.CzyAktywny)
                             .Select(pzz => pzz.Notatki).FirstOrDefault())
                .FirstOrDefault();

            return wartosc?.ToString("C2", new CurrentCultureFormatProvider()) ?? 0 + " " + waluta;
        }
    }

}
