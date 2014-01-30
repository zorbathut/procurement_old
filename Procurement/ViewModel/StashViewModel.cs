using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using POEApi.Model;
using Procurement.Controls;
using Procurement.View;
using Procurement.ViewModel.Filters;
using System;
using System.IO;

namespace Procurement.ViewModel
{
    public class StashViewModel : INotifyPropertyChanged
    {
        private class WhatsInTheBox
        {
            public int Index { get; set; }
            public TabItem TabItem { get; set; }
            public StashControl Stash { get; set; }
            public WhatsInTheBox(int index, TabItem tabItem, StashControl stash)
            {
                this.Index = index;
                this.TabItem = tabItem;
                this.Stash = stash;
            }           
        }

        private List<WhatsInTheBox> tabsAndContent;
        private StashView stashView;
        private List<IFilter> categoryFilter;
        private TabItem selectedTab { get; set; }
        private ResourceDictionary expressionDark;
        private OrbType configuredOrbType;
        private bool currencyDistributionUsesCount;
        private static Dictionary<string, CroppedBitmap> imageCache = new Dictionary<string, CroppedBitmap>();

        private string filter;

        public string Filter
        {
            get { return filter; }
            set 
            { 
                filter = value;
                processFilter();
            }
        }

        private void processFilter()
        {
            List<IFilter> allfilters = getUserFilter(filter);
            allfilters.AddRange(categoryFilter);

            foreach (var item in tabsAndContent)
            {
                item.Stash.SetValue(StashControl.FilterProperty, allfilters);
                item.Stash.ForceUpdate();
                if (item.Stash.FilterResults == 0)
                {
                    item.TabItem.Visibility = Visibility.Collapsed;
                    (item.TabItem.Content as UIElement).Visibility = Visibility.Collapsed;
                }
                else
                {
                    item.TabItem.Visibility = Visibility.Visible;
                    (item.TabItem.Content as UIElement).Visibility = Visibility.Visible;
                }
            }
            var first = tabsAndContent.Find(w => w.TabItem.Visibility == Visibility.Visible);
            if (first != null)
                first.TabItem.IsSelected = true;
        }

        public void SetCategoryFilter(string category, bool? isChecked)
        {
            if (!isChecked.Value)
            {
                var filtersBeGone = CategoryManager.GetCategory(category).Select(f => f.GetType()).ToList();
                categoryFilter.RemoveAll(f => filtersBeGone.Contains(f.GetType()));
                processFilter();
                return;
            }

            categoryFilter.AddRange(CategoryManager.GetCategory(category));
            processFilter();
        }

        public ICommand GetTabs { get; set; }

        public Dictionary<string, string> AvailableCategories { get; private set; }

        public List<string> Leagues
        {
            get { return ApplicationState.Leagues; }
        }

        public string CurrentLeague
        {
            get { return ApplicationState.CurrentLeague; }
        }

        public string Total
        {
            get { return "Total " + configuredOrbType.ToString() + " in Orbs : " + ApplicationState.Stash[ApplicationState.CurrentLeague].GetTotal(configuredOrbType).ToString(); }
        }

        public Dictionary<OrbType, double> TotalDistibution
        {
            get 
            {
                if (currencyDistributionUsesCount)
                    return ApplicationState.Stash[ApplicationState.CurrentLeague].GetTotalCurrencyCount();

                return ApplicationState.Stash[ApplicationState.CurrentLeague].GetTotalCurrencyDistribution(configuredOrbType); 
            }
        }

        public List<string> AvailableItems { get; private set; }


        public StashViewModel(StashView stashView)
        {
            this.stashView = stashView;
            categoryFilter = new List<IFilter>();
            AvailableCategories = CategoryManager.GetAvailableCategories();
            tabsAndContent = new List<WhatsInTheBox>();
            stashView.Loaded += new System.Windows.RoutedEventHandler(stashView_Loaded);
            GetTabs = new DelegateCommand(GetTabList);
            ApplicationState.LeagueChanged += new PropertyChangedEventHandler(ApplicationState_LeagueChanged);
            stashView.tabControl.SelectionChanged += new SelectionChangedEventHandler(tabControl_SelectionChanged);
            getAvailableItems();
            expressionDark = Application.LoadComponent(new Uri("/Procurement;component/Controls/ExpressionDark.xaml", UriKind.RelativeOrAbsolute)) as ResourceDictionary;

            configuredOrbType = OrbType.GemCutterPrism;
            string currencyDistributionMetric = Settings.UserSettings["CurrencyDistributionMetric"];
            if (currencyDistributionMetric.ToLower() == "count")
                currencyDistributionUsesCount = true;
            else
                configuredOrbType = (OrbType)Enum.Parse(typeof(OrbType), currencyDistributionMetric);
        }

        private void getAvailableItems()
        {
            AvailableItems = ApplicationState.Stash[ApplicationState.CurrentLeague].Get<Item>().SelectMany(i => getSearchTerms(i)).Distinct().ToList();
        }
        private IEnumerable<string> getSearchTerms(Item item)
        {
            yield return item.TypeLine;
            if (!string.IsNullOrEmpty(item.Name))
                yield return item.Name;

            Gear gear = item as Gear;
            if (gear != null)
                yield return gear.GearType.ToString();
        }

        void tabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (selectedTab != null)
                unselectPreviousTab(selectedTab);

            var item = stashView.tabControl.SelectedItem as TabItem;
            selectedTab = item;
            Image i = item.Header as Image;
            CroppedBitmap bm = (CroppedBitmap)i.Source;
            Tab tab = (Tab)i.Tag;
            item.Header = getImage(tab, true);
        }

        private void unselectPreviousTab(TabItem selectedTab)
        {
            Image i = selectedTab.Header as Image;
            Tab tab = i.Tag as Tab;
            selectedTab.Header = getImage(tab, false);
        }

        void ApplicationState_LeagueChanged(object sender, PropertyChangedEventArgs e)
        {
            getAvailableItems();
            stashView.tabControl.SelectionChanged -= new SelectionChangedEventHandler(tabControl_SelectionChanged);
            stashView.tabControl.Items.Clear();
            stashView.tabControl.SelectionChanged += new SelectionChangedEventHandler(tabControl_SelectionChanged);
            stashView_Loaded(sender, null);
            raisePropertyChanged("AvailableItems");
            raisePropertyChanged("Total");
            raisePropertyChanged("TotalDistibution");
        }

        public void GetTabList(object o)
        {
            Button selector = o as Button;
            ScrollViewer scrollViewer = selector.TemplatedParent as ScrollViewer;
            TabControl tabControl = scrollViewer.TemplatedParent as TabControl;

            selector.ContextMenu = getContextMenu(selector, tabControl);
            selector.ContextMenu.IsOpen = true;
        }

        private ContextMenu getContextMenu(Button target, TabControl tabControl)
        {
            ContextMenu menu = new ContextMenu();
            menu.PlacementTarget = target;
            menu.Resources = expressionDark;

            foreach (TabItem item in tabControl.Items)
            {
                MenuItem menuItem = new MenuItem();
                menuItem.Tag = item;
                menuItem.Header = item.Tag.ToString(); //((item.Header as TextBlock).Inlines.FirstInline as Run).Text;
                menuItem.Click += (o, e) => { closeAndSelect(menu, menuItem); };
                menu.Items.Add(menuItem);
            }

            return menu;
        }

        private void closeAndSelect(ContextMenu menu, MenuItem menuItem)
        {
            menu.IsOpen = false;
            TabItem newCurrent = menuItem.Tag as TabItem;
            newCurrent.BringIntoView();
            newCurrent.IsSelected = true;
        }

        void stashView_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            var filter = string.Empty;

            for (int i = 1; i <= ApplicationState.Stash[ApplicationState.CurrentLeague].NumberOfTabs; i++)
            {
                TabItem item = new TabItem();

                item.Header = getImage(ApplicationState.Stash[ApplicationState.CurrentLeague].Tabs[i - 1], false);
                item.Tag = ApplicationState.Stash[ApplicationState.CurrentLeague].Tabs[i - 1].Name;
                item.HorizontalAlignment = HorizontalAlignment.Left;
                item.VerticalAlignment = VerticalAlignment.Top;
                item.Background = Brushes.Transparent;
                item.BorderBrush = Brushes.Transparent;
                StashControl itemStash = new StashControl();

                itemStash.SetValue(StashControl.FilterProperty, getUserFilter(filter));
                item.Content = itemStash;
                itemStash.TabNumber = ApplicationState.Stash[ApplicationState.CurrentLeague].Tabs[i - 1].i;

                if (!ApplicationState.Model.Offline)
                {
                    ContextMenu contextMenu = new ContextMenu();
                    MenuItem refresh = new MenuItem() { Header = "Refresh" };
                    refresh.Tag = itemStash;
                    refresh.Click += new RoutedEventHandler(refresh_Click);
                    contextMenu.Items.Add(refresh);
                    item.ContextMenu = contextMenu;
                }

                stashView.tabControl.Items.Add(item);
                tabsAndContent.Add(new WhatsInTheBox(i - 1, item, itemStash));
            }

            stashView.Loaded -= new System.Windows.RoutedEventHandler(stashView_Loaded);
        }

        private static List<IFilter> getUserFilter(string filter)
        {
            if (string.IsNullOrEmpty(filter))
                return new List<IFilter>();

            UserSearchFilter searchCriteria = new UserSearchFilter(filter);
            return new List<IFilter>() { searchCriteria };
        }

        void refresh_Click(object sender, RoutedEventArgs e)
        {
            MenuItem source = sender as MenuItem;
            StashControl stash = source.Tag as StashControl;
            stash.RefreshTab();
        }

        public static Image getImage(Tab tab, bool mouseOver)
        {
            List<System.Drawing.Bitmap> images = new List<System.Drawing.Bitmap>();
            System.Drawing.Bitmap finalImage = null;

            Image img = new Image();
            int offset = mouseOver ? 26 : 0;

            string key = tab.srcL + tab.srcC + tab.srcR + tab.Name + mouseOver.ToString();

            if (!imageCache.ContainsKey(key))
            {
                try
                {
                    int width = 0;
                    int height = 0;
                    int count = 0;
                    float middleWidth = 0;
                    foreach (Stream stream in ApplicationState.Model.GetImage(tab))
                    {
                        System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(stream);

                        if (count == 1)
                        {
                            using (System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(new System.Drawing.Bitmap(200, 200)))
                            {
                                System.Drawing.SizeF measured = g.MeasureString(tab.Name, System.Drawing.SystemFonts.DefaultFont);
                                width += (int)measured.Width;
                                middleWidth = measured.Width;
                            }
                        }
                        else
                        {
                            width += bitmap.Width;
                        }
                        height = bitmap.Height > height ? bitmap.Height : height;
                        images.Add(bitmap);
                        count++;
                    }

                    finalImage = new System.Drawing.Bitmap(width, height);
                    using (System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(finalImage))
                    {
                        //set background color
                        g.Clear(System.Drawing.Color.Black);

                        //go through each image and draw it on the final image
                        int woffset = 0;
                        count = 0;
                        foreach (System.Drawing.Bitmap image in images)
                        {
                            int iwidth = image.Width;
                            if (count == 1)
                                iwidth = (int)middleWidth;
                            g.DrawImage(image, new System.Drawing.Rectangle(woffset, 0, iwidth, image.Height));
                            woffset += iwidth;
                            if (count == 1)
                                woffset -= 1;
                            count++;
                        }
                        g.DrawString(tab.Name, System.Drawing.SystemFonts.DefaultFont, System.Drawing.Brushes.Yellow, 10, 6);
                        g.DrawString(tab.Name, System.Drawing.SystemFonts.DefaultFont, System.Drawing.Brushes.Yellow, 10, 32);
                    }

                    using (MemoryStream stream = new MemoryStream())
                    {
                        finalImage.Save(stream, System.Drawing.Imaging.ImageFormat.Bmp);
                        stream.Position = 0;
                        var bitmap = new BitmapImage();
                        bitmap.BeginInit();
                        bitmap.StreamSource = stream;
                        bitmap.CacheOption = BitmapCacheOption.OnLoad;
                        bitmap.EndInit();

                        imageCache.Add(key, new CroppedBitmap(bitmap, new Int32Rect(0, offset, (int)bitmap.Width, 26)));
                    }
                }
                catch (Exception ex)
                {
                    if (finalImage != null)
                        finalImage.Dispose();

                    throw ex;
                }
                finally
                {
                    foreach (System.Drawing.Bitmap image in images)
                        image.Dispose();
                }
            }

            img.Source = imageCache[key];
            img.Tag = tab;

            return img;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void raisePropertyChanged(string property)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(property));
        }
    }
}