using Firma.Models.Entities;
using System;
using System.Linq;

namespace Firma.Models.BusinessLogic
{
    public class SredniCzasZK: DatabaseClass
    {
        public SredniCzasZK(FirmaDBEntities firmaEntities) :base(firmaEntities)
        {
        }

        /// <summary>
        /// Podaje średni czas realizacji zleceń kompletacji utworzonych w zadanym przedziale czasowym.
        /// </summary>
        /// <param name="data_od">Data początkowa okresu sprawdzania</param>
        /// <param name="data_do">Data końcowa okresu sprawdzania</param>
        /// <returns>Wartość średnią ilości dni</returns>
        public double SredniCzasRealizacjiZK(DateTime data_od, DateTime data_do)
        {
            var lista = firmaEntities.ZleceniaKompletacji.Where(zk => zk.CzyAktywny &&
                                                           zk.PotwierdzonaDataRealizacji != null &&
                                                           zk.DataUtworzenia >= data_od &&
                                                           zk.DataUtworzenia <= data_do).ToList();
            return lista.Select(zk => (zk.PotwierdzonaDataRealizacji - zk.DataUtworzenia)?.TotalDays)?.Average() ?? 0;
            
        }
    }
}