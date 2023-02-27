using System;

namespace Firma.Models.EntitiesForView.Produkt
{
    public class ProduktForAllView
    {
        #region Properties
        public int IdProduktu { get; set; }
        public string Kod { get; set; }
        public string Nazwa { get; set; }
        public string DodatkowaNazwa { get; set; }
        public string Marka { get; set; }
        public string JednostkaMiary { get; set; }
        public string Producent { get; set; }
        public string DostawcaNazwa { get; set; }
        public string Typ { get; set; }
        public decimal Cena { get; set;}
        public string Waluta { get; set; }
        public decimal VatZakupu { get; set; }
        public decimal VatSprzedazy { get; set; }

        #endregion
    }
}
