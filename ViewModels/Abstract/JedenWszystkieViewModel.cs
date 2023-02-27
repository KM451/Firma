using Firma.Helpers;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Firma.ViewModels.Abstract
{
    /// <summary>
    /// ViewModel realizujący połączenie jeden-do-wielu, np. Jedna faktura ma wiele pozycji faktury.
    /// </summary>
    /// <typeparam name="JedenType">np. Zlecenie Zakupu, Zlecenie Kompletacji...</typeparam>
    /// <typeparam name="WszystkieType">np. Pozycje Zlecenia Zakupu, Pozycje Zlecenia Kompletacji...</typeparam>
    public abstract class JedenWszystkieViewModel<JedenType, WszystkieType> : JedenViewModel<JedenType> where WszystkieType : class
    {
        #region Fields and Properties
        private ObservableCollection<WszystkieType> _WszystkieList;
        public ObservableCollection<WszystkieType> WszystkieList
        {
            get => _WszystkieList;
            set
            {
                if (value != _WszystkieList)
                {
                    _WszystkieList = value;
                    OnPropertyChanged(() => WszystkieList);
                }
            }
        }

        public string WszystkieDisplayName { get; set; }
        public string ShowAddViewButtonContent { get; set; }

        private WszystkieType _SelectedItem;
        public WszystkieType SelectedItem
        {
            get
            {
                return _SelectedItem;
            }
            set
            {
                if (_SelectedItem != value)
                {
                    _SelectedItem = value;
                    OnPropertyChanged(() => SelectedItem);
                }
            }
        }

        #endregion

        #region Commands

        private ICommand _ShowAddViewCommand;
        public ICommand ShowAddViewCommand
        {
            get
            {
                if (_ShowAddViewCommand == null)
                {
                    _ShowAddViewCommand = new BaseCommand(() => ShowAddView());
                }
                return _ShowAddViewCommand;
            }
        }

        private ICommand _ShowEditViewCommand;
        public ICommand ShowEditViewCommand
        {
            get
            {
                if (_ShowEditViewCommand == null)
                {
                    _ShowEditViewCommand = new BaseCommand(() => ShowEditView());
                }
                return _ShowEditViewCommand;
            }
        }


        private ICommand _DeleteCommand;
        public ICommand DeleteCommand
        {
            get
            {
                if (_DeleteCommand == null)
                {
                    _DeleteCommand = new BaseCommand(() => Delete());
                }
                return _DeleteCommand;
            }
        }



        #endregion

        #region Constructor
        public JedenWszystkieViewModel(string displayName, string wszystkieDisplayName, string showAddViewButtonContent) : base(displayName)
        {
            WszystkieDisplayName = wszystkieDisplayName;
            ShowAddViewButtonContent = showAddViewButtonContent;
        }
        #endregion

        #region Method
        protected abstract void ShowAddView();

        protected abstract void ShowEditView();

        protected abstract void Delete();


        #endregion
    }
}
