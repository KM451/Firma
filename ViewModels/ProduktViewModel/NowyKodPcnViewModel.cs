using Firma.Models.Entities;
using Firma.ViewModels.Abstract;
using GalaSoft.MvvmLight.Messaging;
using System;

namespace Firma.ViewModels.ProduktViewModel
{
    public class NowyKodPcnViewModel: JedenViewModel<KodyPcn>
    {
        #region Constructor

        public NowyKodPcnViewModel() : base("Kod PCN")
        {
            Item = new KodyPcn()
            {
                CzyAktywny = true,
                DataUtworzenia = DateTime.Now,
                DataModyfikacji = DateTime.Now,
            };
        }

        #endregion

        #region Properties
        public string Tytul
        {
            get
            {
                return Item.Tytul;
            }
            set
            {
                if (Item.Tytul != value)
                {
                    Item.Tytul = value;
                    OnPropertyChanged(() => Tytul);
                }
            }
        }
        public string Notatki
        {
            get
            {
                return Item.Notatki;
            }
            set
            {
                if (Item.Notatki != value)
                {
                    Item.Notatki = value;
                    OnPropertyChanged(() => Notatki);
                }
            }
        }

        #endregion

        #region Method
        public override void Save()
        {
            Db.KodyPcn.AddObject(Item);
            Db.SaveChanges();
            Messenger.Default.Send("odswiez Kod PCN");
        }
        #endregion
    }
}
