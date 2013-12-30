using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using POEApi.Model;
using Procurement.ViewModel;

namespace Procurement.Controls
{
    public partial class ItemDisplay : UserControl
    {
        private static List<Popup> annoyed = new List<Popup>();
        private static ResourceDictionary expressionDark;

        private TextBlock textblock;

        public ItemDisplay()
        {
            InitializeComponent();
            expressionDark = expressionDark ?? Application.LoadComponent(new Uri("/Procurement;component/Controls/ExpressionDark.xaml", UriKind.RelativeOrAbsolute)) as ResourceDictionary;

            this.Loaded += new RoutedEventHandler(ItemDisplay_Loaded);
        }

        public static void ClosePopups()
        {
            closeOthersButNot(new Popup());
        }

        void ItemDisplay_Loaded(object sender, RoutedEventArgs e)
        {
            ItemDisplayViewModel vm = this.DataContext as ItemDisplayViewModel;
            Image i = vm.getImage();

            UIElement socket = vm.getSocket();

            this.MainGrid.Children.Add(i);

            if (socket != null)
                doSocketOnHover(socket, i);

            i.ContextMenu = getContextMenu();

            this.Height = i.Height;
            this.Width = i.Width;
            this.Loaded -= new RoutedEventHandler(ItemDisplay_Loaded);

            resyncText();
        }

        private void resyncText()
        {
            ItemDisplayViewModel vm = this.DataContext as ItemDisplayViewModel;
            Item item = vm.Item;

            if ((item is Currency))
                return;

            MenuItem setBuyout = new MenuItem();
            string buyoutValue = string.Empty;

            if (Settings.Buyouts.ContainsKey(item.UniqueIDHash))
                buyoutValue = Settings.Buyouts[item.UniqueIDHash];

            if (textblock != null)
                this.MainGrid.Children.Remove(textblock);

            textblock = new TextBlock();
            textblock.Text = buyoutValue;
            textblock.IsHitTestVisible = false;
            textblock.Margin = new Thickness(1, 1, 0, 0);
            this.MainGrid.Children.Add(textblock);
        }

        private void doSocketAlwaysOver(UIElement socket)
        {
            this.MainGrid.Children.Add(socket);
        }

        private void doSocketOnHover(UIElement socket, Image i)
        {
            NonTopMostPopup popup = new NonTopMostPopup();
            popup.PopupAnimation = PopupAnimation.Fade;
            popup.StaysOpen = true;
            popup.Child = socket;
            popup.Placement = PlacementMode.Center;
            popup.PlacementTarget = i;
            popup.AllowsTransparency = true;
            i.MouseEnter += (o, ev) =>
            {
                closeOthersButNot(popup);
                popup.IsOpen = true;
            };

            i.MouseLeave += (o, ev) =>
            {
                Rect rect = System.Windows.Media.VisualTreeHelper.GetDescendantBounds(i);
                if (!rect.Contains(ev.GetPosition(o as IInputElement)))
                    popup.IsOpen = false;
            };

            this.MainGrid.Children.Add(popup);
            annoyed.Add(popup);
        }

        private ContextMenu getContextMenu()
        {
            ItemDisplayViewModel vm = this.DataContext as ItemDisplayViewModel;
            Item item = vm.Item;

            ContextMenu menu = new ContextMenu();
            menu.Resources = expressionDark;

            if (!(item is Currency))
            {
                MenuItem setBuyout = new MenuItem();
                string buyoutValue = string.Empty;
                if (Settings.Buyouts.ContainsKey(item.UniqueIDHash))
                    buyoutValue = Settings.Buyouts[item.UniqueIDHash];

                var buyoutView = new SetBuyoutView();
                buyoutView.BuyoutValue.Text = buyoutValue;

                setBuyout.Header = buyoutView;
                setBuyout.Click += new RoutedEventHandler(setBuyout_Click);
                menu.Items.Add(setBuyout);
            }

            return menu;
        }

        void setBuyout_Click(object sender, RoutedEventArgs e)
        {
            ItemDisplayViewModel vm = this.DataContext as ItemDisplayViewModel;
            Item item = vm.Item;

            string buyoutValue = ((sender as MenuItem).Header as SetBuyoutView).BuyoutValue.Text;

            Settings.Buyouts[item.UniqueIDHash] = buyoutValue;

            if (buyoutValue == string.Empty)
                Settings.Buyouts.Remove(item.UniqueIDHash);

            Settings.Save();

            resyncText();
        }

        public static void closeOthersButNot(Popup current)
        {
            List<Popup> others = annoyed.Where(p => p.IsOpen && !object.ReferenceEquals(current, p)).ToList();
            Task.Factory.StartNew(() => others.ToList().ForEach(p => p.Dispatcher.Invoke((Action)(() => { p.IsOpen = false; }))));
        }
    }
}
