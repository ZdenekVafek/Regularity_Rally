using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel;
using MySql.Data.MySqlClient;
namespace Regularity_Rally.Control
{
    #region Language
    public class User_Language : INotifyPropertyChanged
    {

        public string login_head
        {
            get { return Properties.Resources.user_login_head; }
        }
        public string name_head
        {
            get { return Properties.Resources.user_name_head; }
        }
        public string lastname_head
        {
            get { return Properties.Resources.user_lastname_head; }
        }
        public string email_head
        {
            get { return Properties.Resources.user_email_head; }
        }
        public string role_head
        {
            get { return Properties.Resources.user_role_head; }
        }
        public string active_head
        {
            get { return Properties.Resources.user_active; }
        }
        public string adduser_btn_
        {
            get { return Properties.Resources.user_adduser_btn_; }
        }
        public string closeuser_btn_
        {
            get { return Properties.Resources.user_closeuser_btn_; }
        }
        public string users_title
        {
            get { return Properties.Resources.user_users_title; }
        }
        public string print_btn_
        {
            get { return Properties.Resources.print_btn_; }
        }

        public void Reload()
        {
            OnPropertyRaised("login_head");
            OnPropertyRaised("print_btn_");
            OnPropertyRaised("name_head");
            OnPropertyRaised("lastname_head");
            OnPropertyRaised("email_head");
            OnPropertyRaised("role_head");
            OnPropertyRaised("adduser_btn_");
            OnPropertyRaised("closeuser_btn_");
            OnPropertyRaised("users_title");
        }

        public User_Language()
        {

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


    public class UserView
    {
        public uint ID { get; set; }
        public string Login { get; set; }
        public bool Active { get; set; }
        public string Name { get; set; }
        public string Lastname { get; set; }
        public string E_mail { get; set; }
        public string Permisson { get; set; }
        public string Role { get; set; }

        public UserView(uint id, string login, bool active, string name, string lastname, string email, string perm, string role, User user = null)
        {
            this.ID = id;
            this.Login = login;
            this.Active = active;
            this.Name = name;
            this.Lastname = lastname;
            this.E_mail = email;
            this.Permisson = perm;
            this.Role = role;

            data = user;
        }

        public User data;   

    }

    /// <summary>
    /// Interaction logic for Users.xaml
    /// </summary>
    public partial class Users : Window
    {
        //public EventHandler SetLang;
        private List<UserView> UsersItems = new List<UserView>();
        private Add_new_user UpdateWindow = null;

        public Users()
        {
            InitializeComponent();
            this.Title = (Resources["lang"] as User_Language).users_title;
            MainWindow.RegisterLanguageHandler(SetLanguage);
            UpdateUserList(string.Empty);
        }

        protected override void OnClosed(EventArgs e)
        {
            CloseUser_btn_Click(null, null);
            MainWindow.UnRegisterLanguageHandler(SetLanguage);
            base.OnClosed(e);
        }

        public void SetLanguage()
        {
            var language_ = Resources["lang"] as User_Language;
            language_.Reload();
            Dispatcher.BeginInvoke(((Action)(() =>
            {
                this.Title = language_.users_title;
            })));
        }

        public void Refresh()
        {
            Dispatcher.BeginInvoke(((Action)(() =>
            {
                UpdateUserList(string.Empty);
            })));
        }

        private void UserList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (UserList.SelectedItem != null)
            {
                UserView _t = LastUserSelectedItem;
                LastUserSelectedItem = UserList.SelectedItem as UserView;
            }
        }

        private UserView LastUserSelectedItem = null;

        private void UpdateUserList(string filter)
        {
            Dispatcher.BeginInvoke(((Action)(() =>
            {
                action_search.Visibility = Visibility.Visible;
            })));

            //UserList.ItemsSource = UsersItems;
            List<UserView> _items = new List<UserView>();
            int SelectedIndex = 0;
            MySqlDataReader rdr = null;
            try
            {
                // load data base on filter
                List<User> userdata = User.GetUserCollection();
                foreach(User user in userdata)
                {
                    _items.Add(user.ToUserView());
                }

                UsersItems.Clear();
                UsersItems = _items;
                Dispatcher.BeginInvoke(((Action)(() =>
                {
                    // rebind data value
                    UserList.ItemsSource = null;
                    UserList.ItemsSource = UsersItems;
                    UserList.SelectedIndex = SelectedIndex;
                })));
            }
            catch
            {
                if (rdr != null && !rdr.IsClosed)
                    rdr.Close();
            }
        }

        private void UserList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (this.UpdateWindow != null)
                this.UpdateWindow.Close();

            // opened on selected index, just alovate object aand push data operation
            this.UpdateWindow = null;
            this.UpdateWindow = new Add_new_user(this, UserOperationType.Update, LastUserSelectedItem.data);
        }

        private void AddUser_btn_Click(object sender, RoutedEventArgs e)
        {
            if (this.UpdateWindow != null)
                this.UpdateWindow.Close();

            this.UpdateWindow = null;
            this.UpdateWindow = new Add_new_user(this, UserOperationType.Add, null);
        }

        private void CloseUser_btn_Click(object sender, RoutedEventArgs e)
        {
            // free opened windows
            if (this.UpdateWindow != null)
            {
                this.UpdateWindow.Close();
                this.UpdateWindow = null;
            }

            this.Close();
        }

        private void Print_btn_Click(object sender, RoutedEventArgs e)
        {
            PrintManager print = new PrintManager();
            print.Print(PrintManager.PrintType.Users);
        }
    }
}
