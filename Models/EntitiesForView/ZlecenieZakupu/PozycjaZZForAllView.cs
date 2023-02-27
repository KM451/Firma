using System;

namespace Firma.Models.EntitiesForView.ZlecenieZakupu
{
    public class PozycjaZZForAllView
    {
        public int Id { get; set; }
        public int IdProduktu { get; set; }
        public string ProduktKod { get; set; }
        public string ProduktNazwa { get; set; }
        public decimal Ilosc { get; set; }
        public string ProduktJednostkaMiary { get; set; }
        public decimal? Cena { get; set; }
        public decimal? Rabat { get; set; }
        public string ProduktCenaWaluta { get; set; }
        public DateTime? DataRealizacji { get; set; }

        public PozycjaZZForAllView(int id, int idProduktu, string produktKod, string produktNazwa, string produktJednostkaMiary, string produktCenaWaluta, decimal ilosc, decimal? cena, decimal? rabat, DateTime? dataRealizacji)
        {
            Id = id;
            IdProduktu = idProduktu;
            ProduktKod = produktKod;
            ProduktNazwa = produktNazwa;
            ProduktJednostkaMiary = produktJednostkaMiary;
            ProduktCenaWaluta = produktCenaWaluta;
            Ilosc = ilosc;
            Cena = cena;
            Rabat = rabat;
            DataRealizacji = dataRealizacji;
        }
        public PozycjaZZForAllView()
        {
        }
    }
}
