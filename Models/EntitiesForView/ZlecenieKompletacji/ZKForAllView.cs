using System;

namespace Firma.Models.EntitiesForView.ZlecenieKompletacji
{
    public class ZKForAllView
    {
        #region Properties
        public int IdZleceniaKompletacji { get; set; }
        public string ProduktKod { get; set; }
        public string ProduktNazwa { get; set; }
        public decimal Ilosc { get; set; }
        public string ProduktKontrahent { get; set; }
        public DateTime DataWystawienia { get; set; }
        public DateTime? PotwierdzonaDataRealizacji { get; set; }
        public string ProduktMonter { get; set; }
        public int? Priorytet { get; set; }
        public string Status { get; set; }

        #endregion
    }
}
