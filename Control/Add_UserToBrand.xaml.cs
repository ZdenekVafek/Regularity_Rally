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
    public class Add_UserToBrand_Language : INotifyPropertyChanged
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
            get { return Properties.Resources.autb_users_title; }
        }

        public Add_UserToBrand_Language()
        {

        }

        public void Reload()
        {
            OnPropertyRaised("login_head");
            OnPropertyRaised("name_head");
            OnPropertyRaised("lastname_head");
            OnPropertyRaised("email_head");
            OnPropertyRaised("role_head");
            OnPropertyRaised("adduser_btn_");
            OnPropertyRaised("closeuser_btn_");
            OnPropertyRaised("users_title");
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
    /// <summary>
    /// Interaction logic for Add_UserToBrand.xaml
    /// </summary>
    public partial class Add_UserToBrand : Window
    {
        private List<UserToBrand> UsersItems = new List<UserToBrand>();
        Add_new_Meascom parent = null;
        int MeasIDdata = 0;


        public Add_UserToBrand(Add_new_Meascom parent, int data)
        {
            InitializeComponent();
            this.Title = (Resources["lang"] as Add_UserToBrand_Language).users_title;
            MainWindow.RegisterLanguageHandler(SetLanguage);
            this.parent = parent;
            MeasIDdata = data;
            UpdateUserList();
            Console.WriteLine(MeasIDdata);
        }

        public void SetLanguage()
        {
            var language_ = Resources["lang"] as Add_UserToBrand_Language;
            language_.Reload();
            Dispatcher.BeginInvoke(((Action)(() =>
            {
                this.Title = language_.users_title;
            })));
        }

        private void UpdateUserList()
        {
            UserList.ItemsSource = UsersItems;
            List<UserToBrand> _items = new List<UserToBrand>();
            int SelectedIndex = 0;
            MySqlDataReader rdr = null;
            try
            {
                // load data base on filter
                MySqlCommand command = Database.Database.CreateCommand(Database.Database.QueryStack["GetUserToBrand"]);
                rdr = command.ExecuteReader();

                while (rdr.Read())
                {
                    _items.Add(new UserToBrand()
                    {
                        ID = rdr.GetInt32("ID_User"),
                        Login = rdr.GetString("Login"),
                        Name = rdr.GetString("Name"),
                        Lastname = rdr.GetString("Lastname"),
                        E_mail = rdr.GetString("E_mail"),
                        Role = rdr.GetInt32("Role").ToString() 
                    });
                }
                rdr.Close();

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

        private void CloseSelectUser_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        public UserToBrand LastUserToBrandSelectedItem = null;
        private void UserList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (UserList.SelectedItem != null)
            {
                LastUserToBrandSelectedItem = UserList.SelectedItem as UserToBrand;
                Console.WriteLine(LastUserToBrandSelectedItem.ID);
            }
        }

        private void AddUser_btn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int _ID = 0;
                int _ID_User = LastUserToBrandSelectedItem.ID;
                int _ID_Meas = MeasIDdata;


                MySqlCommand check_ExistUserInBrand = Database.Database.CreateCommand(string.Format(Database.Database.QueryStack["CheckUserToBrand"], _ID_User, _ID_Meas));
                Int32 rows_count = Convert.ToInt32(check_ExistUserInBrand.ExecuteScalar());
                if (rows_count > 0) { MessageBox.Show("Already exist!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning); }
                else
                {
                    MySqlCommand autb_cmd = (_ID == 0) ?
                            Database.Database.CreateCommand(string.Format(Database.Database.QueryStack["AddUserToBrand"],
                            _ID_User,
                            _ID_Meas
                            ))
                            :   //L
                            Database.Database.CreateCommand(string.Format(Database.Database.QueryStack["RepUserToBrand"],
                            _ID,
                            _ID_User,
                            _ID_Meas
                            ));

                    int retcc = autb_cmd.ExecuteNonQuery();
                }
            }
            catch { }
            parent.Refresh();
            this.Close();
        }
    }
}
