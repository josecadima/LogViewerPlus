using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;

namespace LogViewerPlus.Converters
{
    public class BoolToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return 0;

            bool result;
            if (!Boolean.TryParse(value.ToString(), out result))
                return "/Images/button_blank_green_14986.ico";

            if (result)
                return "/Images/button_blank_green_14986.ico";

            return "/Images/button_blank_gray_14985.ico";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
