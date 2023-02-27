namespace Firma.Models.EntitiesForView.ZlecenieKompletacji
{
    public class SkladnikZKForAllView
    {
        public int Id { get; set; }
        public int IdProduktu { get; set; }
        public string Kod { get; set; }
        public string Nazwa { get; set; }
        public decimal Ilosc { get; set; }
        public string JednostkaMiary { get; set; }

        public SkladnikZKForAllView(int id, int idProduktu, string kod, string nazwa, decimal ilosc, string jednostkaMiary)
        {
            Id = id;
            IdProduktu = idProduktu;
            Kod = kod;
            Nazwa = nazwa;
            Ilosc = ilosc;
            JednostkaMiary = jednostkaMiary;
        }
        public SkladnikZKForAllView()
        {
        }
    }
}
