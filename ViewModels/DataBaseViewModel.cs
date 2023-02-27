using Firma.Helpers;
using Firma.Models.Entities;
using System.Windows.Input;

namespace Firma.ViewModels
{
    public class DataBaseViewModel : WorkspaceViewModel
    {
        #region Fields and Properties
        
        private readonly FirmaDBEntities firmaEntities;
        public FirmaDBEntities FirmaEntities
        {
            get { return firmaEntities; }
        }
        #endregion

        #region Constructor

        public DataBaseViewModel(string displayName)
        {
            base.DisplayName = displayName;
            this.firmaEntities= new FirmaDBEntities();
        }

        #endregion

        #region Commands

        private BaseCommand _ObliczCommand;
        public ICommand ObliczCommand
        {
            get
            {
                if (_ObliczCommand == null)
                {
                    _ObliczCommand = new BaseCommand(() => ObliczClick());
                }
                return _ObliczCommand;
            }
        }

        #endregion

        #region Methods
        protected virtual void ObliczClick() { }


        #endregion
    }
}
