using Firma.Helpers;
using Firma.Models.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Firma.ViewModels.Abstract
{
    public abstract class WszystkieViewModel<T> : WorkspaceViewModel where T : class
    {
        #region Fields and Properties
        
        private readonly FirmaDBEntities firmaDBEntities;
        public FirmaDBEntities FirmaDBEntities
        {
            get { return firmaDBEntities; }
        }

        private ObservableCollection<T> _List;
        public ObservableCollection<T> List
        {
            get
            {
                if (_List == null)
                    Load();
                return _List;
            }
            set
            {
                _List = value;
                OnPropertyChanged(() => List);
            }
        }

        public List<T> AllList { get; set; }

        private T _SelectedItem;
        public T SelectedItem
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

        private ObservableCollection<KeyValuePair<string, string>> _ListOfItemsOrderBy;
        public ObservableCollection<KeyValuePair<string, string>> ListOfItemsOrderBy 
        {
            get { return _ListOfItemsOrderBy; }
            set 
            { 
                _ListOfItemsOrderBy = value;
                OnPropertyChanged(() => ListOfItemsOrderBy);
            }
        }

        private ObservableCollection<KeyValuePair<string, string>> _ListOfItemsFilter;
        public ObservableCollection<KeyValuePair<string, string>> ListOfItemsFilter
        {
            get { return _ListOfItemsFilter; }
            set
            {
                _ListOfItemsFilter = value;
                OnPropertyChanged(() => ListOfItemsFilter);
            }
        }

        private bool _OrderDescending;
        public bool OrderDescending
        {
            get { return _OrderDescending; }
            set 
            {
                if(_OrderDescending != value)
                _OrderDescending = value; 
                OnPropertyChanged(() => OrderDescending);
                OrderBy();
            }
        }

        private string _SearchPhrase;
        public string SearchPhrase
        {
            get { return _SearchPhrase; }
            set
            {
                if (_SearchPhrase != value)
                    _SearchPhrase = value;
                OnPropertyChanged(() => SearchPhrase);
            }
        }

        private string _OrderByField;
        public string OrderByField
        {
            get { return _OrderByField; }
            set
            {
                if (_OrderByField != value)
                    _OrderByField = value;
                OnPropertyChanged(() => OrderByField);
                OrderBy();
            }
        }

        private string _FilterField;
        public string FilterField
        {
            get { return _FilterField; }
            set
            {
                if (_FilterField != value)
                    _FilterField = value;
                OnPropertyChanged(() => FilterField);
            }
        }
        public bool CzyModyfikowac { get; set; }

        #endregion

        #region Constructor
        public WszystkieViewModel(string displayName)
        {
            base.DisplayName = displayName;
            this.firmaDBEntities = new FirmaDBEntities();
            CzyModyfikowac = true;
            ListOfItemsFilter = GetListOfItemsFilter();
            ListOfItemsOrderBy = GetListOfItemsOrderBy();

        }
        #endregion

        #region Methods

        protected abstract void Filter();
        protected abstract void OrderBy();
        protected abstract void ShowAddView();
        public abstract void Load();
        protected abstract void Open();
        protected abstract void Delete();
        protected abstract ObservableCollection<KeyValuePair<string, string>> GetListOfItemsOrderBy();
        protected abstract ObservableCollection<KeyValuePair<string, string>> GetListOfItemsFilter();

        #endregion

        #region Commands

        private BaseCommand _OpenCommand;
        public ICommand OpenCommand
        {
            get
            {
                if (_OpenCommand == null)
                {
                    _OpenCommand = new BaseCommand(() => Open());
                }
                return _OpenCommand;
            }
        }

        private BaseCommand _DeleteCommand;
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

        private BaseCommand _LoadCommand;
        public ICommand LoadCommand
        {
            get
            {
                if (_LoadCommand == null)
                {
                    _LoadCommand = new BaseCommand(() => Load());
                }
                return _LoadCommand;

            }
        }

        private BaseCommand _ShowAddViewCommand;
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

        private BaseCommand _RefreshViewCommand;
        public ICommand RefreshViewCommand
        {
            get
            {
                if (_RefreshViewCommand == null)
                {
                    _RefreshViewCommand = new BaseCommand(() => Load());
                }
                return _RefreshViewCommand;

            }
        }

        private BaseCommand _FilterCommand;
        public ICommand FilterCommand
        {
            get
            {
                if (_FilterCommand == null)
                {
                    _FilterCommand = new BaseCommand(() => Filter());
                }
                return _FilterCommand;

            }
        }

        #endregion

    }
}
