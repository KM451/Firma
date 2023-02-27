using Firma.Helpers;
using System;
using System.Windows.Input;

namespace Firma.ViewModels
{
    //to jest klasa z ktorej bedą dziedziczyć wszystkie ViewModele zakładek
    public class WorkspaceViewModel : BaseViewModel
    {
        #region Pola i Komendy
        //Każda zakładka ma minimum nazwę i zamknij
        public string DisplayName { get; set; }//w tym properties będzie przechowywana nazwa zakładki
        private BaseCommand _CloseCommand;//to jest komenda do obsługi zamykania okna (pole do tej komendy)
        public ICommand CloseCommand
        {
            get
            {
                if (_CloseCommand == null)
                    _CloseCommand = new BaseCommand(() => this.OnRequestClose());//ta komenda wywoła funkcje zamykającą zakładkę
                return _CloseCommand;
            }
        }

        #endregion

        #region RequestClose [event] 
        public event EventHandler RequestClose;  //metoda zamykająca zkładkę skopiowana z netu
        public void OnRequestClose()
        {
            EventHandler handler = this.RequestClose;
            if (handler != null)
                handler(this, EventArgs.Empty);
        }
        #endregion

        /// <summary>
        /// Autowłaściwość zwracająca dzisiejszą datę dla kontrolek DatePicker
        /// </summary>
        public DateTime Dzisiejsza_data { get; set; }
        public WorkspaceViewModel() : base()
        {
            Dzisiejsza_data = DateTime.Now;
        }

    }
}
