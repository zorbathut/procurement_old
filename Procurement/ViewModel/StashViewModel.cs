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
        private TabItem selectedTab { get; set; }

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
            foreach (var item in tabsAndContent)
            {
                item.Stash.SetValue(StashControl.FilterProperty, filter);
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

        public ICommand GetTabs { get; set; }

        public List<string> Leagues
        {
            get { return ApplicationState.Leagues; }
        }

        public string CurrentLeague
        {
            get { return ApplicationState.CurrentLeague; }
        }

        public string TotalGCP
        {
            get { return "Total GCP in Orbs : " + ApplicationState.Stash[ApplicationState.CurrentLeague].GetTotalGCP().ToString(); }
        }

        public Dictionary<OrbType, float> TotalGCPDistibution
        {
            get { return ApplicationState.Stash[ApplicationState.CurrentLeague].GetTotalGCPDistribution(); }
        }

        public List<string> AvailableItems { get; private set; }


        public StashViewModel(StashView stashView)
        {
            this.stashView = stashView;
            tabsAndContent = new List<WhatsInTheBox>();
            stashView.Loaded += new System.Windows.RoutedEventHandler(stashView_Loaded);
            GetTabs = new DelegateCommand(GetTabList);
            ApplicationState.LeagueChanged += new PropertyChangedEventHandler(ApplicationState_LeagueChanged);
            stashView.tabControl.SelectionChanged += new SelectionChangedEventHandler(tabControl_SelectionChanged);
            getAvailableItems();
            
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
            raisePropertyChanged("AvailableItems");
            raisePropertyChanged("TotalGCP");
            raisePropertyChanged("TotalGCPDistibution");
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

                itemStash.SetValue(StashControl.FilterProperty, filter);

                item.Content = itemStash;
                itemStash.TabNumber = i - 1;

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

        void refresh_Click(object sender, RoutedEventArgs e)
        {
            MenuItem source = sender as MenuItem;
            StashControl stash = source.Tag as StashControl;
            stash.RefreshTab();
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

        public event PropertyChangedEventHandler PropertyChanged;

        private void raisePropertyChanged(string property)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(property));
        }
    }
}