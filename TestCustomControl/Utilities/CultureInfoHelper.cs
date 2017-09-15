using System.Globalization;
using Windows.Globalization.DateTimeFormatting;
using Windows.System;

namespace TestCustomControl.Utilities
{
    public static class CultureInfoHelper
    {
        private const VirtualKey decimalPoint = (VirtualKey)190;
        private const VirtualKey comma = (VirtualKey)188;

        public static string DecimalSeparator
        {
            get => GetCurrentCulture().NumberFormat.NumberDecimalSeparator;
        }



        /// <summary>
        /// Retrieve correct CultureInfo object using a technique by Pedro Lamas
        /// <see cref="https://www.pedrolamas.com/2015/11/02/cultureinfo-changes-in-uwp/"/>
        /// </summary>
        /// <returns>CultureInfo object for the user's Regional Settings</returns>
        public static CultureInfo GetCurrentCulture()
        {
            var cultureName = new DateTimeFormatter("longdate", new[] { "US" }).ResolvedLanguage;

            return new CultureInfo(cultureName);
        }


        public static bool IsKeyDecimalSeparator(VirtualKey key)
        {
            bool isDecimalSeparator = false;
            if ((key == comma && DecimalSeparator[0] == ',') || (key == (VirtualKey)190 && DecimalSeparator[0] == '.') ||
                (key == VirtualKey.Space && DecimalSeparator[0] == ' '))
            {
                isDecimalSeparator = true;
            }

            return isDecimalSeparator;
        }
    }
}
