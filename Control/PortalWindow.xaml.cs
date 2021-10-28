using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.ComponentModel;
using Regularity_Rally.Control;
using System.Threading.Tasks;

namespace Regularity_Rally.Control
{
    #region Language
    public class PortalWindow_Language : INotifyPropertyChanged
    {
        public string connectdbs_label_
        {
            get { return Properties.Resources.pw_connectdbs_label_; }
        }
        public string password_label_
        {
            get { return Properties.Resources.pw_password_label_; }
        }
        public string connectdbs_btn_
        {
            get { return Properties.Resources.pw_connectdbs_btn_; }
        }

        public string port_label_
        {
            get { return Properties.Resources.pw_port_label_; }
        }

        public string keep_label_
        {
            get { return Properties.Resources.pw_keep_label_; }
        }

        public string serverdb_label_
        {
            get { return Properties.Resources.pw_serverdb_label_; }
        }

        public string user_label_
        {
            get { return Properties.Resources.pw_user_label_; }
        }

        public PortalWindow_Language()
        {

        }

        public void Reload()
        {
            OnPropertyRaised("connectdbs_label_");
            OnPropertyRaised("password_label_");
            OnPropertyRaised("connectdbs_btn_");
            OnPropertyRaised("port_label_");
            OnPropertyRaised("keep_label_");
            OnPropertyRaised("serverdb_label_");
            OnPropertyRaised("user_label_");

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
    /// Interaction logic for PortalWindow.xaml
    /// </summary>
    public partial class PortalWindow : Window
    {
        public EventHandler SetLang;

        public PortalWindow()
        {
            InitializeComponent();
            SetLang += new EventHandler(Set_Language);

            loadDefault();

            try
            {
                this.Show();
            }
            catch { }
        }

        public void loadDefault()
        {

            _server_name.Text = Database.Database.instance.Server;
            _server_port.Text = Database.Database.instance.Server_port;
            _server_user.Text = Database.Database.instance.Server_user;
            _server_pass.Password = Database.Database.instance.Server_password;
            _server_database.Text = Database.Database.instance.Database_Name;

        }

        protected override void OnClosing(CancelEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }

        public void Set_Language(object sender, EventArgs args)
        {
            var language_ = Resources["lang"] as PortalWindow_Language;
            language_.Reload();
        }

        bool penging = false;
        private void ConnAction_Click(object sender, RoutedEventArgs e)
        {
            // validate all inputs heav value
            if (_server_name.Text == string.Empty)
                return;
            if (_server_port.Text == string.Empty)
                return;
            if (_server_user.Text == string.Empty)
                return;
            if (_server_pass.Password == string.Empty)
                return;
            if (_server_database.Text == string.Empty)
                return;

            if (penging)
                return;

            bool keep_ = _remember.IsChecked ?? true;

            // dont allow user to resend request again, also may it look we doing somtink
            StateOfAction(false);

            penging = true;

            // block of data that will be passed as resources to another thread
            string server_name_ = _server_name.Text;
            string server_port_ = _server_port.Text;
            string server_user_ = _server_user.Text;
            string server_pass_ = _server_pass.Password;
            string server_database_ = _server_database.Text;

            // perform async, copz resources to another memorz block then pass them to task, cant ead property from ui cos its on another thread ... 
            Task task = new Task(() => Database.Database.instance.OpenConnection(server_name_, server_user_, server_database_, server_port_, server_pass_, keep_, new EventHandler(OnResponce)));
            task.Start();
        }

        private void OnResponce(object sender, EventArgs args)
        {
            penging = false;

            // in any case allow user to send again
            Dispatcher.BeginInvoke(((Action)(() =>
            {
                StateOfAction(true);
            })));

            bool _r = (bool)sender;
            if (_r)
            {
                Dispatcher.BeginInvoke(((Action)(() =>
                {
                    this.Hide();
                })));
            }
        }

        private void StateOfAction(bool _s)
        {
            ConnAction.IsEnabled = _s;
            if (_s)
            {
                ui_action_stat.Visibility = Visibility.Hidden;
                ui_action_text.Visibility = Visibility.Visible;
            }
            else
            {
                ui_action_stat.Visibility = Visibility.Visible;
                ui_action_text.Visibility = Visibility.Hidden;
            }
        }
    }
}
