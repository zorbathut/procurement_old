using System;
using System.Windows.Data;

namespace Procurement.View
{
    public class VisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            System.Windows.Visibility hidden = System.Windows.Visibility.Hidden;
            if (parameter != null && parameter.ToString() == "CollapseWhenFalse")
                hidden = System.Windows.Visibility.Collapsed;

            bool val = (bool)value;
            return val ? System.Windows.Visibility.Visible : hidden;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
