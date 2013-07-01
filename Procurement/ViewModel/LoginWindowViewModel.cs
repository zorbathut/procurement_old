using System;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using POEApi.Model;
using POEApi.Model.Events;
using Procurement.View;
using System.Security;
using POEApi.Infrastructure;
using POEApi.Infrastructure.Events;

namespace Procurement.ViewModel
{
    public class LoginWindowViewModel : INotifyPropertyChanged
    {
        private static bool authOffLine;       
        
        private UserControl view;
        private Brush brush;
        private static RichTextBox statusBox;
        public event LoginCompleted OnLoginCompleted;
        public delegate void LoginCompleted();
        private bool usePasswordBoxPassword;

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string property)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(property));
        }

        private string email;
        public string Email 
        {
            get { return email; }
            set
            {
                if (value != email)
                {
                    email = value;
                    OnPropertyChanged("Email");
                }
            }
        }

        public LoginWindowViewModel(UserControl view)
        {
            this.view = view;

            Email = Settings.UserSettings["AccountLogin"];
            this.usePasswordBoxPassword = string.IsNullOrEmpty(Settings.UserSettings["AccountPassword"]);

            if (!this.usePasswordBoxPassword)
                (this.view as LoginView).txtPassword.Password = string.Empty.PadLeft(8); //For the visuals

            (this.view as LoginView).txtPassword.PasswordChanged += new System.Windows.RoutedEventHandler(txtPassword_PasswordChanged);

            statusBox = (this.view as LoginView).StatusBox;
            brush = statusBox.Foreground;
            statusBox.AppendText(ApplicationState.Version + " Initialized.\r");

            ApplicationState.Model.Authenticating += new POEModel.AuthenticateEventHandler(model_Authenticating);
            ApplicationState.Model.StashLoading += new POEModel.StashLoadEventHandler(model_StashLoading);
            ApplicationState.Model.ImageLoading += new POEModel.ImageLoadEventHandler(model_ImageLoading);
            ApplicationState.Model.Throttled += new ThottledEventHandler(model_Throttled);
        }

        void txtPassword_PasswordChanged(object sender, System.Windows.RoutedEventArgs e)
        {
            this.usePasswordBoxPassword = true;
        }

        public void Login(bool offline)
        {
            authOffLine = offline;
            toggleControls();

            Task.Factory.StartNew(() =>
            {
                SecureString password = usePasswordBoxPassword ? (this.view as LoginView).txtPassword.SecurePassword : Settings.UserSettings["AccountPassword"].Decrypt();
                ApplicationState.Model.Authenticate(Email, password, authOffLine);
                saveSettings(password);

                if (!authOffLine)
                    ApplicationState.Model.ForceRefresh();

                updateView("Loading characters...");
                var chars = ApplicationState.Model.GetCharacters();
                updateView("[OK]");

                bool downloadOnlyMyLeagues = false;
                downloadOnlyMyLeagues = (Settings.UserSettings.ContainsKey("DownloadOnlyMyLeagues") && 
                                         bool.TryParse(Settings.UserSettings["DownloadOnlyMyLeagues"], out downloadOnlyMyLeagues) && 
                                         downloadOnlyMyLeagues &&
                                         Settings.Lists.ContainsKey("MyLeagues") &&
                                         Settings.Lists["MyLeagues"].Count > 0
                                         );

                foreach (var character in chars)
                {
                    if (character.League == "Void")
                        continue;

                    if (downloadOnlyMyLeagues && !Settings.Lists["MyLeagues"].Contains(character.League))
                        continue;

                    ApplicationState.Characters.Add(character);
                    loadCharacterInventory(character);
                    loadStash(character);
                }

                if (downloadOnlyMyLeagues && ApplicationState.Characters.Count == 0)
                    throw new Exception("No characters found in the leagues specified. Check spelling or try setting DownloadOnlyMyLeagues to false in settings");

                ApplicationState.SetDefaults();

                updateView("\nDone!");
                OnLoginCompleted();
            }).ContinueWith((t) => { Logger.Log(t.Exception.InnerException.ToString()); updateView("ERROR: " + t.Exception.InnerException.Message); }, TaskContinuationOptions.OnlyOnFaulted);
        }

        private void saveSettings(SecureString password)
        {
            if (!usePasswordBoxPassword)
                return;
         
            Settings.UserSettings["AccountLogin"] = Email;
            Settings.UserSettings["AccountPassword"] = password.Encrypt();
            Settings.Save();
        }

        private void toggleControls()
        {
            var view = (this.view as LoginView);
            view.LoginButton.IsEnabled = !view.LoginButton.IsEnabled;
            view.OfflineButton.IsEnabled = !view.OfflineButton.IsEnabled;
            view.txtLogin.IsEnabled = !view.txtLogin.IsEnabled;
            view.txtPassword.IsEnabled = !view.txtPassword.IsEnabled;
        }

        private void loadStash(Character character)
        {
            if (ApplicationState.Leagues.Contains(character.League))
                return;

            ApplicationState.CurrentLeague = character.League;
            ApplicationState.Stash[character.League] = ApplicationState.Model.GetStash(character.League);
            ApplicationState.Model.GetImages(ApplicationState.Stash[character.League]);
            ApplicationState.Leagues.Add(character.League);
        }

        private void loadCharacterInventory(Character character)
        {
            updateView(string.Format("Loading {0}'s inventory...", character.Name));
            var inventory = ApplicationState.Model.GetInventory(character.Name);
            var inv = inventory.Where(i => i.inventoryId != "MainInventory");
            updateView("[OK]");

            ApplicationState.Model.GetImages(inventory);
        }

        private void updateView(string message)
        {
            if (statusBox.Dispatcher.CheckAccess())
            {
                Run text = new Run(message);

                if (message.Contains("OK"))
                {
                    text.Foreground = Brushes.White;
                    text.Text = "[";
                    ((Paragraph)statusBox.Document.Blocks.LastBlock).Inlines.Add(text);

                    text = new Run();
                    text.Foreground = Brushes.Green;
                    text.Text = "OK";
                    ((Paragraph)statusBox.Document.Blocks.LastBlock).Inlines.Add(text);

                    text = new Run();
                    text.Foreground = Brushes.White;
                    text.Text = "]";

                    ((Paragraph)statusBox.Document.Blocks.LastBlock).Inlines.Add(text);

                }
                else if (message.Contains("ERROR"))
                {
                    text.Foreground = Brushes.White;
                    text.Text = "\r\r[";
                    ((Paragraph)statusBox.Document.Blocks.LastBlock).Inlines.Add(text);

                    text = new Run();
                    text.Foreground = Brushes.Red;
                    text.Text = "NOT OK";
                    ((Paragraph)statusBox.Document.Blocks.LastBlock).Inlines.Add(text);

                    text = new Run();
                    text.Foreground = Brushes.White;
                    text.Text = "]";
                    ((Paragraph)statusBox.Document.Blocks.LastBlock).Inlines.Add(text);

                    text = new Run(message + "\r\r");
                    text.Foreground = Brushes.White;
                    ((Paragraph)statusBox.Document.Blocks.LastBlock).Inlines.Add(text);

                    toggleControls();
                }
                else
                {
                    text.Foreground = brush;
                    text.Text = "\r" + getPaddedString(text.Text);
                    ((Paragraph)statusBox.Document.Blocks.LastBlock).Inlines.Add(text);
                }

                statusBox.ScrollToEnd();
            }
            else
            {
                statusBox.Dispatcher.Invoke((Action)delegate()
                {
                    updateView(message);
                });
            }
        }

        void model_StashLoading(POEModel sender, StashLoadedEventArgs e)
        {
            update("Loading " + ApplicationState.CurrentLeague + " Stash Tab " + (e.StashID + 1) + "...", e);
        }

        void model_ImageLoading(POEModel sender, ImageLoadedEventArgs e)
        {
            update("Loading Image For " + e.URL, e);
        }

        void model_Authenticating(POEModel sender, AuthenticateEventArgs e)
        {
            update("Authenticating " + e.Email, e);
        }

        void model_Throttled(object sender, ThottledEventArgs e)
        {
            if (e.WaitTime.TotalSeconds > 4)
                update(string.Format("GGG Server request limit hit, throttling activated. Please wait {0} seconds", e.WaitTime.Seconds), new POEEventArgs(POEEventState.BeforeEvent));
        }

        private void update(string message, POEEventArgs e)
        {
            if (e.State == POEEventState.BeforeEvent)
            {
                updateView(message);
                return;
            }

            updateView("[OK]");
        }

        private string getPaddedString(string text)
        {
            return text.PadRight(90, ' ');
        }
    }
}