using Firma.Models.EntitiesForView.Kontrahent;
using Firma.ViewResources;
using GalaSoft.MvvmLight.Messaging;
using System.Linq;

namespace Firma.ViewModels.KontrachentViewModel
{
    public class WszyscyDostawcyViewModel : WszyscyKontrahenciViewModel
    {
        #region Constructor
        public WszyscyDostawcyViewModel() : base(BaseResources.ListaDostawcow)
        {
        }
        #endregion

        #region Methods
        public override void Load()
        {
            AllList =
                    FirmaDBEntities.Kontrahenci.Where(k => k.CzyAktywny && k.Dostawca).Join(FirmaDBEntities.Adresy.Where(a => a.Siedziba), k => k.Id, a => a.IdKontrahenta,
                    (k, a) => new KontrahentForAllView
                    {
                        IdKontrahenta = k.Id,
                        Kod = k.Tytul,
                        Nazwa = k.Nazwa,
                        Nip = k.Nip,
                        KontrachentAdres = "ul. " + a.Ulica + " " + a.NrDomu.Trim()
                                                            + (a.NrLokalu == null ? ", " : "/" + a.NrLokalu.Trim() + ", ")
                                                            + a.KodPoczowy.Trim() + " " + a.Miejscowosc + ", " + a.Kraj.Trim()
                    }).ToList();
            Filter();
        }

        /// <summary>
        /// Wysyła dane wybranego dostawcy do wybranego viewmodelu
        /// </summary>
        protected override void Open()
        {
            if (SelectedItem != null)
            {
                if (GdzieWyslac == "Produkty")
                    Messenger.Default.Send(SelectedItem);
                if (GdzieWyslac == "ZZ")
                    Messenger.Default.Send(new DostawcaForZZ()
                    {
                        IdKontrahenta = SelectedItem.IdKontrahenta,
                        Nazwa = SelectedItem.Nazwa,
                        Kod = SelectedItem.Kod,
                        Nip = SelectedItem.Nip,
                        KontrachentAdres = SelectedItem.KontrachentAdres
                    });
                if (GdzieWyslac == "RaportZZ")
                    Messenger.Default.Send(new KontrahentForRaportZZ()
                    {
                        IdKontrahenta = SelectedItem.IdKontrahenta,
                        Nazwa = SelectedItem.Nazwa,
                        Kod = SelectedItem.Kod,
                        Nip = SelectedItem.Nip,
                    });
                OnRequestClose();
            }
        }

        #endregion

    }
}
