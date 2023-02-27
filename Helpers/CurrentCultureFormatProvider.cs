using System;
using System.Globalization;

namespace Firma.Helpers
{
    public class CurrentCultureFormatProvider : IFormatProvider
    {
        public Object GetFormat(Type formatType)
        {
            Console.WriteLine("Requesting an object of type {0}",
                              formatType.Name);
            if (formatType == typeof(NumberFormatInfo))
                return NumberFormatInfo.CurrentInfo;
            else if (formatType == typeof(DateTimeFormatInfo))
                return DateTimeFormatInfo.CurrentInfo;
            else
                return null;
        }
    }
}
