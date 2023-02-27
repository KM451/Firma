using Firma.Models.Entities;
using Firma.ViewModels.Abstract;
using GalaSoft.MvvmLight.Messaging;
using System;

namespace Firma.ViewModels.KompletacjaViewModel
{
    public class NowyMonterViewModel: JedenViewModel<Monterzy>
    {
        #region Constructor

        public NowyMonterViewModel() : base("Monterzy")
        {
            Item = new Monterzy()
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

        #region Methods

        public override void Save()
        {
            Db.Monterzy.AddObject(Item);
            Db.SaveChanges();
            Messenger.Default.Send("odswiez Montera");
        }
        #endregion
    }
}
