using System;
using System.Windows;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.ComponentModel;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace Regularity_Rally.Control
{
    #region Language
    public class LoginWindowLanguage : INotifyPropertyChanged
    {
        public string connectdbs_btn_
        {
            get { return Properties.Resources.lw_connectdbs_btn; }
        }
        public string login_btn_
        {
            get { return Properties.Resources.lw_login_btn; }
        }
        public string login_label_
        {
            get { return Properties.Resources.lw_login_label; }
        }
        public string password_label_
        {
            get { return Properties.Resources.lw_password_label; }
        }

        public LoginWindowLanguage()
        {

        }

        public void Reload()
        {
            OnPropertyRaised("connectdbs_btn_");
            OnPropertyRaised("login_btn_");
            OnPropertyRaised("login_label_");
            OnPropertyRaised("password_label_");
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyRaised(string propertyname)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyname));
            }
        }
    }

    #endregion

    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : UserControl
    {
        private PortalWindow PortalWin = null;

        public LoginWindow()
        {
            InitializeComponent();

            // language
            MainWindow.RegisterLanguageHandler(SetLanguage);

            // que notification
            Regularity_Rally.Database.Database.RegisterConnectionNotify(new EventHandler(OnDBConnect));

            // limit interaction of login
            UpdateUI();
        }

        public void SetLanguage()
        {
            var language_ = Resources["lang"] as LoginWindowLanguage;
            language_.Reload();
        }

        public void OnDBConnect(object sender, EventArgs args)
        {
            // value is reversed cos we refering to state of connection as needed not established
            Dispatcher.BeginInvoke(((Action)(() =>
            {
                UpdateUI();
            })));
        }

        // force close database window that owns modul
        public void Close(IAsyncResult result)
        {
            Dispatcher.BeginInvoke(((Action)(() =>
            {
                AuthPending = false;

                // clean up
                if (PortalWin != null)
                {
                    PortalWin.Close();
                    PortalWin = null;
                }

                user_name.Text = string.Empty;
                user_pass.Password = string.Empty;
                ErrorReport.Visibility = Visibility.Hidden;

            })));
        }

        public void Hold(IAsyncResult async)
        {
            AuthPending = true;
        }

        /// <summary>
        /// used to allow enable/dissable login system based on database requirements
        /// </summary>
        private void UpdateUI(bool Report = false)
        {
            AuthPending = false;
            bool is_connected = Regularity_Rally.Database.Database.isConnect();
            Login_action.IsEnabled = is_connected;  // enable login button

            // not working !!!
            DoubleAnimation animation;
            if (is_connected)
            {
                animation = new DoubleAnimation
                {
                    From = 0,
                    To = 1,
                    BeginTime = TimeSpan.FromSeconds(0),
                    Duration = TimeSpan.FromMilliseconds(500),
                    AutoReverse = true,
                    RepeatBehavior = RepeatBehavior.Forever
                };
            }
            else
            {
                animation = new DoubleAnimation
                {
                    To = 0,
                    BeginTime = TimeSpan.FromSeconds(0),
                    Duration = TimeSpan.FromMilliseconds(100),
                    FillBehavior = FillBehavior.HoldEnd
                };
            }

            if (Report)
            {
                Dispatcher.BeginInvoke(((Action)(() =>
                {
                    ErrorReport.Visibility = Visibility.Visible;
                })));
            }
            else
            {
                Dispatcher.BeginInvoke(((Action)(() =>
                {
                    ErrorReport.Visibility = Visibility.Hidden;
                })));
            }

            // set animation on effect
            ConnectFlash.BeginAnimation(OpacityProperty, animation);
        }

        private void Connect_database_action_Click(object sender, RoutedEventArgs e)
        {
            if (this.PortalWin == null)
                this.PortalWin = new PortalWindow();
            else
                this.PortalWin.Show();
        }
        
        private bool AuthPending = false;
        private void Login_action_Click(object sender, RoutedEventArgs e)
        {
            if (user_name.Text == string.Empty)
                return;
            if (user_pass.Password == string.Empty)
                return;

            if (AuthPending)
                return;

            // prevent memory loss
            string n = user_name.Text, p = user_pass.Password;

            var pending = new AsyncCallback(Hold);
            pending.BeginInvoke(null, null, null);

            // pass login to task
            Dispatcher.BeginInvoke(((Action)(() =>
            {
                User user = User.LogIn(n, p);
                if (user != null)
                {
                    Regularity_Rally.Control.MainWindow hanlder_object = Application.Current.MainWindow as MainWindow;
                    hanlder_object.Dispatcher.BeginInvoke(((Action)(() =>
                    {
                        hanlder_object.Login(user, new AsyncCallback(Close));
                    })));

                    AuthPending = false;
                }
                else
                    UpdateUI(true);
            })));
        }

        private void user_pass_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && Regularity_Rally.Database.Database.isConnect())
            {
                Login_action_Click(null, null);
            }
        }
        // load function
        private void user_name_Loaded(object sender, RoutedEventArgs e)
        {
            user_name.Focus();
        }

        private void User_name_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                user_pass.Focus();
            }
        }

        private void Bd_MouseLeave(object sender, MouseEventArgs e)
        {
            Mouse.OverrideCursor = null;
        }

        private void Bd_MouseEnter(object sender, MouseEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.IBeam;
        }
    }
}