using System;

namespace Firma.Models.EntitiesForView.ZlecenieZakupu
{
    public class ZZForAllView
    {
        #region Properties
        public int IdZleceniaZakupu { get; set; }
        public string ZZKontrahent { get; set; }
        public DateTime DataWystawienia { get; set; }
        public DateTime? DataRealizacji { get; set; }
        public string Status { get; set; }
        public bool? Potwierdzenie { get; set; }
        public string NrPotwierdzenia { get; set; }
        

        #endregion
    }
}
