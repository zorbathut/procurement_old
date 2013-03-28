using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using POEApi.Model;
using Procurement.View;

namespace Procurement.ViewModel
{
    public class ScreenController
    {
        private static MainWindow mainView;
        private static Dictionary<string, IView> screens = new Dictionary<string, IView>();

        public double HeaderHeight { get; set; }
        public double FooterHeight { get; set; }
        public bool ButtonsVisible{ get; set; }
        public bool FullMode { get; set; }

        public DelegateCommand MenuButtonCommand { get; set; }

        private const string STASH_VIEW = "StashView";

        public ScreenController(MainWindow layout)
        {
            FullMode = !bool.Parse(Settings.UserSettings["CompactMode"]);
            if (FullMode)
            {
                HeaderHeight = 169;
                FooterHeight = 138;
            }

            MenuButtonCommand = new DelegateCommand(execute);
            mainView = layout;
            initLogin();
        }

        private void execute(object obj)
        {
            loadView(screens[obj.ToString()]);
        }

        private void initScreens()
        {
            Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal,
            new Action(() =>
            {
                screens.Add(STASH_VIEW, new StashView());
                screens.Add("Inventory", new InventoryView());
                screens.Add("Trading", new TradingView());
                screens.Add("Settings", new SettingsView());
                screens.Add("Recipes", new RecipeView());
            }));
        }

        private void initLogin()
        {
            var loginView = new LoginView();
            var loginVM = (loginView.DataContext as LoginWindowViewModel);
            loginVM.OnLoginCompleted += new LoginWindowViewModel.LoginCompleted(loginCompleted);
            loadView(loginView);
        }

        void loginCompleted()
        {
            initScreens();
            loadView(screens.First().Value);
            showMenuButtons();
        }

        private static void showMenuButtons()
        {
            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
            new Action(() =>
            {
                mainView.Buttons.Visibility = Visibility.Visible;
            }));
        }

        private void loadView(IView view)
        {
            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal, 
                new Action(() => 
                {
                     mainView.MainRegion.Children.Clear();
                     mainView.MainRegion.Children.Add(view as UserControl);
                }));
        }
    }
}