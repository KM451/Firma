using Firma.ViewResources;

namespace Firma.Models.Validators
{
    public class StringValidator: Validator
    {
        /// <summary>
        /// Sprawdza czy Nip składa się z 10, wyłącznie cyfr.
        /// </summary>
        /// <param name="nip">Nip do sprawdzenia</param>
        /// <returns>Pusty tekst lub tekst błędu</returns>
        public static string SprawdzNip(string nip)
        {
            if (nip.Length != 10 && nip.Length != 0)
                return BaseResources.InvalidNipDlugosc;
            else
            {
                foreach (var znak in nip)
                {
                    if (!char.IsDigit(znak))
                        return BaseResources.InvalidNipZnak;
                }
            }
            return string.Empty;
        }

        /// <summary>
        /// Sprawdza Właściwy format kodu pocztowego (2 cyfry, myślnik, 3 cyfry).
        /// </summary>
        /// <param name="kod"></param>
        /// <returns>Pusty tekst lub tekst błędu</returns>
        public static string SprawdzKodPocztowy(string kod)
        {
            if (kod.Length != 6)
                return BaseResources.InvalidKod;
            else
            {
                if (char.IsDigit(kod[0])&&
                    char.IsDigit(kod[1]) &&
                    char.IsDigit(kod[3]) &&
                    char.IsDigit(kod[4]) &&
                    char.IsDigit(kod[5]) &&
                    kod[2].ToString() == "-" )
                    return string.Empty;
                else return BaseResources.InvalidKod;
            }

        }
        /// <summary>
        /// Sprawdza czy kod produktu nie jest pusty lub null.
        /// </summary>
        /// <param name="kod">Kod do sprawdzenia</param>
        /// <returns>Pusty tekst lub tekst błędu</returns>
        public static string SprawdzKod(string kod)
        {
            if (kod is null || kod==string.Empty)
                return BaseResources.InvalidKodNull;
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Sprawdza czy nazwa produktu nie jest pusta lub null.
        /// </summary>
        /// <param name="nazwa">Nazwa do sprawdzenia</param>
        /// <returns>Pusty tekst lub tekst błędu</returns>
        public static string SprawdzNazwe(string nazwa)
        {
            if (nazwa is null || nazwa == string.Empty)
                return BaseResources.InvalidNazwaNull;
            else
            {
                return string.Empty;
            }
        }
        /// <summary>
        /// Sprawdza czy kraj pochodzenia produktu nie jest pusty lub null.
        /// </summary>
        /// <param name="kraj">Kraj pochodzenia do sprawdzenia</param>
        /// <returns>Pusty tekst lub tekst błędu</returns>
        public static string SprawdzKraj(string kraj)
        {
            if (kraj is null || kraj == string.Empty)
                return BaseResources.InvalidKrajNull;
            else
            {
                return string.Empty;
            }
        }
        /// <summary>
        /// Sprawdza czy pole dane dostawcy nie jest puste lub null.
        /// </summary>
        /// <param name="dd">dane do sprawdzenia</param>
        /// <returns>Pusty tekst lub tekst błędu</returns>
        public static string SprawdzDaneDostawcy(string dd)
        {
            if (dd is null || dd == string.Empty)
                return BaseResources.InvalidDaneDostawcy;
            else
            {
                return string.Empty;
            }
        }
        /// <summary>
        /// Sprawdza czy pole dane kontrahenta nie jest puste lub null.
        /// </summary>
        /// <param name="dd">dane do sprawdzenia</param>
        /// <returns>Pusty tekst lub tekst błędu</returns>
        public static string SprawdzDaneKontrahenta(string dd)
        {
            if (dd is null || dd == string.Empty)
                return BaseResources.BrakDanychKontrahenta;
            else
            {
                return string.Empty;
            }
        }
    }
}
