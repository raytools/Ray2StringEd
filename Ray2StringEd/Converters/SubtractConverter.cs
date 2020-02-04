using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace Ray2StringEd.Converters
{
    public class SubtractConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length <= 0) return 0;

            int total = 0;

            if (values[0] is int first)
            {
                total = first;

                foreach (object value in values.Skip(1))
                {
                    if (value is int valueInt)
                        total -= valueInt;
                }
            }

            return total;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}