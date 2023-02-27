using Firma.Models.Entities;
using System;
using System.Linq;

namespace Firma.Models.BusinessLogic
{
    public class OblozenieB : DatabaseClass
    {
        public OblozenieB(FirmaDBEntities firmaEntities) : base(firmaEntities)
        {
        }
        /// <summary>
        /// Oblicza ilość zleceń kompletacji przydzielonych do wybranego montażysty w wybranym dniu
        /// </summary>
        /// <param name="idMontera">Id montażysty</param>
        /// <param name="data">Dzień, w którym sprawdzana jest ilość zleceń</param>
        /// <returns>Liczbę całkowitą równą ilości zleceń</returns>
        public int? IloscZlecenMonter(int idMontera, DateTime data)
        {
            return firmaEntities.ZleceniaKompletacji.Where(zk => zk.IdMontera == idMontera && zk.PotwierdzonaDataRealizacji == data).Count();
        }
        /// <summary>
        /// Podaje łączny czas realizacji zleceń kompletacji przypisanych do danego montazysty w danym dniu
        /// </summary>
        /// <param name="idMontera">Id montazysty</param>
        /// <param name="data">Dzień, w którym obliczny jest łączny czas</param>
        /// <returns>Łączny czas w formacie TimeSpan</returns>
        public TimeSpan CzasZlecenMonter(int idMontera, DateTime data)
        {
            var czas = firmaEntities.ZleceniaKompletacji.Where(zk => zk.IdMontera == idMontera && zk.PotwierdzonaDataRealizacji == data).Select(zk => zk.CzasZlecenia ?? TimeSpan.Zero).ToList();
            TimeSpan czasSuma = TimeSpan.Zero;
            foreach (var item in czas)
            {
                czasSuma += item;
            }
            return czasSuma;
        }

    }
}
