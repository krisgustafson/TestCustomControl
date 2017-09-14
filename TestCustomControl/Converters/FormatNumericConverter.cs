using System;
using System.Globalization;
using TestCustomControl.Utilities;
using Windows.UI.Xaml.Data;


namespace TestCustomControl.Converters
{
    public class FormatNumericConverter : IValueConverter
    {
        /// <summary>
        /// Format a double or int to a string, as appropriate for the culture
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value != null && value is string)
            {
                // Get double precision (default is applied, if 'precision' was not supplied)
                var precision = GetPrecision(parameter);

                // format string with precision mask
                // NOTE: code in method GetNumberFromString() coerces the value to be 0.0 if negative
                var maskedDouble = FormatValueWithSpecifiedPrecision(precision, GetNumberFromString((string)value));

                return maskedDouble;
            }

            return value;
        }


        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            double stringToDouble;
            bool conversionSucceeded = double.TryParse((string)value, out stringToDouble);
            return conversionSucceeded ? stringToDouble : value;
        }


        #region private methods for Convert

        private static uint GetPrecision(object parameter)
        {
            if (parameter == null)
            {
                return 3;
            }

            uint precision;
            if (uint.TryParse((string)parameter, out precision) == false)
            {
                throw new ArgumentException(nameof(parameter),
                    $"DoubleMaskConverter.cs - Convert() - ConverterParameter in XAML must be a positive integer, or zero. parameter = '{parameter}'");
            }

            return precision;
        }

        private static double GetNumberFromString(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return 0.0;
            }

            double val;
            if (double.TryParse(value, out val) == false)
            {
                throw new ArgumentException(nameof(value),
                    $"DoubleMaskConverter.cs - Convert() - 'value' from XAML must be convertible to a double. parameter = '{value}'");
            }

            // Error correction here!
            val = (val < 0.0) ? 0.0 : val;

            return val;
        }

        private static string FormatValueWithSpecifiedPrecision(uint precision, double val)
        {
            CultureInfo culture = CultureInfoHelper.GetCurrentCulture();
            NumberFormatInfo numberFormat = culture.NumberFormat;

            // Format the double to a string
            if (culture != null)
            {
                return string.Format(culture, "{0:N" + precision + "}", val);
            }
            else
            {
                return string.Format("{0:N" + precision + "}", val);
            }
        }

        #endregion
    }
}
