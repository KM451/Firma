using Firma.Models.Entities;

namespace Firma.Models.BusinessLogic
{
    public class DatabaseClass
    {
        #region Filds
        protected FirmaDBEntities firmaEntities;
        #endregion

        #region Constructor
        public DatabaseClass(FirmaDBEntities firmaEntities)
        {
            this.firmaEntities = firmaEntities;
        }
        #endregion
    }
}
