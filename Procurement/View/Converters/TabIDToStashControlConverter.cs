using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using Procurement.Controls;
using System.Windows.Controls;

namespace Procurement.View
{
    public class TabIDToStashControlConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            Procurement.ViewModel.ForumExportViewModel.ExportStashInfo item = value as Procurement.ViewModel.ForumExportViewModel.ExportStashInfo;
            Grid g = new Grid();
            g.Children.Add(new StashControl() { TabNumber = item.ID });
            return g;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
