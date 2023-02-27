using Firma.Helpers;
using Firma.Models.Entities;
using System.Windows;
using System.Windows.Input;


namespace Firma.ViewModels.Abstract
{
    //z tej klasy bedą dziedziczyły wszystkie zakładki
    public abstract class JedenViewModel<T> : WorkspaceViewModel
    {
        #region Fields
        //To jst nasza baza danych
        public FirmaDBEntities Db { get; set; }
        //To jest nasz dodawany towar
        public T Item { get; set; }
        #endregion

        #region Constructor
        public JedenViewModel(string displayName)
        {
            base.DisplayName = displayName;
            Db = new FirmaDBEntities();
        }
        #endregion

        #region Command
        //to jest komenda ktora zostanie podpieta (zbindowana) z przyciskiem zapisz i zamknij. komednda ta wywoła funkcje save and close
        private BaseCommand _SaveAndCloseCommand;
        public ICommand SaveAndCloseCommand
        {
            get
            {
                if (_SaveAndCloseCommand == null)
                {
                    _SaveAndCloseCommand = new BaseCommand(() => saveAndClose());
                }
                return _SaveAndCloseCommand;
            }
        }
        #endregion

        #region Save
        protected virtual bool IsValid() => true;
        public abstract void Save();

        private void saveAndClose()
        {
            if (IsValid())
            {
                Save();
                base.OnRequestClose();
                MessageBox.Show("Zapisano pomyślnie", "Sukces");
            }
        }
        #endregion
    }
}
