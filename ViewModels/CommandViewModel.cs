using System;
using System.Windows.Input;

namespace Firma.ViewModels
{
    //to jest klasa która jest po to żeby tworzuć komendy w menu z lewej strony
    public class CommandViewModel : BaseViewModel
    {
        #region Właściwości
        public string DisplayName { get; set; }//to jest nazwa przycisku w menu z lewej strony
        public ICommand Command { get; set; }//kazdy przycisk zawiera komendę, która wywołuje funkcję, która otwiera zakładkę
        #endregion

        #region Konstruktor
        public CommandViewModel(string displayName, ICommand command)
        {
            if (command == null)
                throw new ArgumentNullException("Command");
            DisplayName = displayName;
            Command = command;
        }
        #endregion
    }
}
