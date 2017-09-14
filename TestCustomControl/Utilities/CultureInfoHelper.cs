using System.Globalization;
using Windows.Globalization.DateTimeFormatting;



namespace TestCustomControl.Utilities
{
    public static class CultureInfoHelper
    {
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
    }
}
