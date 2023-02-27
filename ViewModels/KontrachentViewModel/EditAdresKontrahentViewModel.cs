using Firma.Models.Entities;
using Firma.Models.EntitiesForView.Kontrahent;
using GalaSoft.MvvmLight.Messaging;
using System.Linq;

namespace Firma.ViewModels.KontrachentViewModel
{
    public class EditAdresKontrahentViewModel : NowyAdresViewModel
    {
        #region Constructor
        public EditAdresKontrahentViewModel(AdresForEdit adres)
        {
            Ulica = adres.Ulica;
            Miejscowosc = adres.Miejscowosc;
            NrDomu = adres.NrDomu;
            NrLokalu = adres.NrLokalu;
            KodPoczowy = adres.KodPoczowy.Trim();
            Poczta = adres.Poczta;
            Kraj = adres.Kraj;
            Notatki = adres.Notatki;
            Siedziba = adres.Siedziba;
            Wysylkowy = adres.Wysylkowy;
        }
        #endregion

        #region Save

        public override void Save()
        {
            var editAdres = new Adresy()
            {       
                Ulica = Ulica,
                Miejscowosc = Miejscowosc,
                NrDomu = NrDomu,
                NrLokalu = NrLokalu,
                KodPoczowy = KodPoczowy,
                Poczta = Poczta,
                Kraj = Kraj,
                Notatki = Notatki,
                Siedziba = Siedziba,
                Wysylkowy = Wysylkowy,
            };
            Messenger.Default.Send(editAdres);
        }
        #endregion
    }
}
