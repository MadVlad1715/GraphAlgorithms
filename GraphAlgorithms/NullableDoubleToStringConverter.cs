using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace GraphAlgorithms
{
    internal class NullableDoubleToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double? doubleValue = (double?)value;
            return doubleValue != null ? doubleValue.ToString() : "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string strValue = (value as string).Trim();

            if (strValue == String.Empty) return null;

            if (double.TryParse(strValue, out double doubleVal))
                return doubleVal;
            else
                return DependencyProperty.UnsetValue;
        }
    }
}