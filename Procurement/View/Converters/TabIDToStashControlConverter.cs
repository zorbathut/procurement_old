using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using Procurement.Controls;
using System.Windows.Controls;
using Procurement.ViewModel.Filters;
using POEApi.Model;
using System.Windows.Media.Imaging;
using System.Windows;

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
            if (value == null)
                return null;

            Item item = value as Item;
            int inventoryId = int.Parse(item.inventoryId.Replace("Stash", "")) - 1;
            Grid g = new Grid();

            StashControl control = new StashControl() { TabNumber = inventoryId };
            Tab tab = ApplicationState.Stash[ApplicationState.CurrentLeague].Tabs.Find(t => t.i == inventoryId);
            Image tabImage = getImage(tab, true);
            

            control.SetValue(StashControl.FilterProperty, new List<IFilter>() { new ItemFilter(item) });
            control.ForceUpdate();
            RowDefinition imageRow = new RowDefinition();
            imageRow.Height = new GridLength(26);
            g.RowDefinitions.Add(imageRow);
            g.RowDefinitions.Add(new RowDefinition());
            tabImage.SetValue(Grid.RowProperty, 0);
            control.SetValue(Grid.RowProperty, 1);
            g.Children.Add(tabImage);
            g.Children.Add(control);

            return g;
        }

        private Image getImage(Tab tab, bool mouseOver)
        {
            Image img = new Image();
            int offset = mouseOver ? 26 : 0;

            using (var stream = ApplicationState.Model.GetImage(tab))
            {
                var bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.StreamSource = stream;
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.EndInit();

                var croppedImage = new CroppedBitmap(bitmap, new Int32Rect(0, offset, (int)bitmap.Width, 26));
                img.Source = croppedImage;
            }
            img.Tag = tab;

            return img;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}