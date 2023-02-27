using Firma.ViewResources;
using System;

namespace Firma.Models.Validators
{
    public class DateTimeValidator
    {
        /// <summary>
        /// Sprawdza czy data nie jest przeszła oraz czy nie przypada w sobotę lub w niedzielę.
        /// </summary>
        /// <param name="data">Data do sprawdzenia</param>
        /// <returns>Teks błędu lub pusty tekst</returns>
        public static string SprawdzDateRealizacji(DateTime data)
        {
            if (data < DateTime.Today)
                return BaseResources.InvalidData;
            else
            {
                if (data.DayOfWeek == DayOfWeek.Saturday || data.DayOfWeek == DayOfWeek.Sunday)
                {
                    return BaseResources.InvalidDataDay;
                }
            }
            return string.Empty;
        }
        /// <summary>
        /// Sprawdza czy Oczekiwana data nie jest przeszła oraz czy nie przypada w sobotę lub w niedzielę.
        /// </summary>
        /// <param name="data">Data do sprawdzenia</param>
        /// <returns>Teks błędu lub pusty tekst</returns>
        public static string SprawdzOczekiwanaDataRealizacji(DateTime data)
        {
            if (data < DateTime.Today)
                return BaseResources.InvalidDataOczekiwana;
            else
            {
                if (data.DayOfWeek == DayOfWeek.Saturday || data.DayOfWeek == DayOfWeek.Sunday)
                {
                    return BaseResources.InvalidDataDay;
                }
            }
            return string.Empty;
        }
        /// <summary>
        /// Sprawdza czy Potwierdzona data nie jest przeszła oraz czy nie przypada w sobotę lub w niedzielę.
        /// </summary>
        /// <param name="data">Data do sprawdzenia</param>
        /// <returns>Teks błędu lub pusty tekst</returns>
        public static string SprawdzPotwierdzonaDataRealizacji(DateTime data)
        {
            if(data < DateTime.Today)
                return BaseResources.InvalidDataPotwierdzona;
            else
            {
                if (data.DayOfWeek == DayOfWeek.Saturday || data.DayOfWeek == DayOfWeek.Sunday)
                {
                    return BaseResources.InvalidDataDay;
                }
            }
            return string.Empty;
        }
    }
}
