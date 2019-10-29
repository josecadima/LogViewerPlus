using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;
using System.Drawing;


namespace LogViewerPlus.Converters
{
    public class MessageTypeToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            SolidColorBrush color;

            if (value == null)
                color = new SolidColorBrush(Colors.Black);
            else if (value.ToString() == "Error")
                color = new SolidColorBrush(Colors.Red);
            else if (value.ToString() == "Warning")
                color = new SolidColorBrush(Colors.Green);
            else if (value.ToString() == "Debug")
                color = new SolidColorBrush(Colors.Blue);
            else if (value.ToString() == "Info")
                color = new SolidColorBrush(Colors.LightBlue);
            else
                color = new SolidColorBrush(Colors.Black);

            return color;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
