using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using Procurement.Controls;
using System.Windows.Controls;
using Procurement.ViewModel.Filters;
using POEApi.Model;

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

    public class TabIDToStashControlFiltered : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            Item item = value as Item;
            int inventoryId = int.Parse(item.inventoryId.Replace("Stash", "")) - 1;
            Grid g = new Grid();
            Label l = new Label() { Content = item.inventoryId, FontSize = 16 };
            StashControl control = new StashControl() { TabNumber = inventoryId };

            control.SetValue(StashControl.FilterProperty, new List<IFilter>() { new ItemFilter(item) });
            control.ForceUpdate();
            g.Children.Add(l);
            g.Children.Add(control);
            return g;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}