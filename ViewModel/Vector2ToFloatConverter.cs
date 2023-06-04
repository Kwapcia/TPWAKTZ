using System;
using System.Globalization;
using System.Windows.Data;
using System.Numerics;

namespace ViewModel
{
    public class Vector2ToFloatConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Vector2 vector && parameter is string axis)
            {
                switch (axis.ToUpper())
                {
                    case "X":
                        return vector.X;
                    case "Y":
                        return vector.Y;
                    default:
                        throw new InvalidOperationException($"Unsupported conversion axis: {axis}");
                }
            }

            throw new InvalidOperationException($"Unsupported conversion type: {value?.GetType()}");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException($"Conversion back is not supported");
        }
    }
}
