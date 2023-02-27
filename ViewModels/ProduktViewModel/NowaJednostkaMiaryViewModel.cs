using Firma.Models.Entities;
using Firma.ViewModels.Abstract;
using GalaSoft.MvvmLight.Messaging;
using System;

namespace Firma.ViewModels.ProduktViewModel
{
    public class NowaJednostkaMiaryViewModel: JedenViewModel<JednostkiMiary>
    {
        #region Constructor

        public NowaJednostkaMiaryViewModel() : base("Jednostka Miary")
        {
            Item = new JednostkiMiary()
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
            Db.JednostkiMiary.AddObject(Item);
            Db.SaveChanges();
            Messenger.Default.Send("odswiez Jednostke Miary");
        }
        #endregion
    }
}
