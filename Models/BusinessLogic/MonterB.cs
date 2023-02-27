using Firma.Models.Entities;
using Firma.Models.EntitiesForView;
using System.Collections.Generic;
using System.Linq;

namespace Firma.Models.BusinessLogic
{
    public class MonterB : DatabaseClass
    {
        public MonterB(FirmaDBEntities firmaEntities) : base(firmaEntities)
        { }
        public List<ComboBoxKeyAndValue> GetMonterComboBoxItems()
        {
            return firmaEntities.Monterzy.Where(m => m.CzyAktywny).Select(m => new ComboBoxKeyAndValue()
            {
                Key = m.Id,
                Value = m.Tytul
            }).ToList();
        }
    }
}
