using Firma.Models.Entities;
using System.Collections.ObjectModel;
using System.Linq;

namespace Firma.ViewModels.KontrachentViewModel
{
    public class EditKontrahentViewModel: NowyKontrahentViewModel
    {
        #region Constructor
        public EditKontrahentViewModel(Kontrahenci kontrahent) : base("Kontrahent", "Adresy kontrahenta", "Dodaj adres") 
        {
            Item = Db.Kontrahenci.FirstOrDefault(x=>x.Id == kontrahent.Id);
            WszystkieList = new ObservableCollection<Adresy>(Item.Adresy
                .Where(item => item.CzyAktywny).ToList());
        }

        #endregion

        #region Save
        public override void Save() => Db.SaveChanges();
        #endregion

    }
}
