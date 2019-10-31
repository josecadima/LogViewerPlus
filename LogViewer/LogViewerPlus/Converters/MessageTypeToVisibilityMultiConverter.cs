using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace LogViewerPlus.Converters
{
    public class MessageTypeToVisibilityMultiConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            bool isVisible = true;

            if (values == null || values.Count() < 6)
                isVisible = true;
            else if (values[0].ToString() == "Error")
                isVisible = Boolean.Parse(values[1].ToString());
            else if (values[0].ToString() == "Warning")
                isVisible = Boolean.Parse(values[2].ToString());
            else if (values[0].ToString() == "Debug")
                isVisible = Boolean.Parse(values[3].ToString());
            else if (values[0].ToString() == "Info")
                isVisible = Boolean.Parse(values[4].ToString());
            else if (values[0].ToString() == "None")
                isVisible = Boolean.Parse(values[5].ToString());

            if (isVisible)
                return Visibility.Visible;

            return Visibility.Collapsed;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
