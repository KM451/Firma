using Firma.Models.EntitiesForView.ZlecenieKompletacji;
using Firma.Models.EntitiesForView.ZlecenieZakupu;
using Firma.ViewResources;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Firma.Models.Validators
{
    public class DataGridValidator: Validator
    {
        /// <summary>
        /// Sprawdza czy data grid zawiera dane.
        /// </summary>
        /// <param name="dg">dane do sprawdzenia</param>
        /// <returns>Pusty tekst lub tekst błędu</returns>
        public static string SprawdzDataGrid(ObservableCollection<PozycjaZZForAllView> dg)
        {
            if (dg.Count()==0)
                return BaseResources.InvalidDataGrid;
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Sprawdza czy data grid zawiera dane.
        /// </summary>
        /// <param name="dg">dane do sprawdzenia</param>
        /// <returns>Pusty tekst lub tekst błędu</returns>
        public static string SprawdzSkladniki(ObservableCollection<SkladnikZKForAllView> dg)
        {
            if (dg.Count() == 0)
                return BaseResources.SkladnikiMissing;
            else
            {
                return string.Empty;
            }
        }
    }
}
