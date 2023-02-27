using Firma.ViewResources;

namespace Firma.Models.Validators
{
    public class BusinessValidator: Validator
    {
        /// <summary>
        /// Sprawdza czy rabat jest z przedziału 0-100.
        /// </summary>
        /// <param name="rabat">Rabat do sprawdzenia</param>
        /// <returns>Teks błędu lub pusty tekst</returns>
        public static string SprawdzRabat(decimal rabat)
        {
            return rabat >= 0.00m && rabat <= 100.00m ? string.Empty : BaseResources.InvalidRabat;
        }
       

    }
}
