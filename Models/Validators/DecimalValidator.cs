using Firma.ViewResources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Firma.Models.Validators
{
    public class DecimalValidator: Validator
    {
        /// <summary>
        /// Sprawdza czy cena jest dodatnia
        /// </summary>
        /// <param name="cena">Cena do sprawdzenia</param>
        /// <returns>Teks błędu lub pusty tekst</returns>
        public static string SprawdzCene(decimal cena)
        {
            return cena >= 0.00m ? string.Empty : BaseResources.InvalidCena;
        }
        /// <summary>
        /// Sprawdza czy ilość jest większa od 0
        /// </summary>
        /// <param name="ilosc">Ilość do sprawdzenia</param>
        /// <returns>Teks błędu lub pusty tekst</returns>
        public static string SprawdzIlosc(decimal ilosc)
        {
            return ilosc > 0.00m ? string.Empty : BaseResources.InvalidIlosc;
        }
    }
}
