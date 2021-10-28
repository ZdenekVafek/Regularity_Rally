using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.ComponentModel;
using Regularity_Rally.Control;
using System.Threading.Tasks;
using System.Windows.Documents;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Collections.Generic;

namespace Regularity_Rally.Control
{
    #region Language
    public class Add_New_Meascom_Language : INotifyPropertyChanged
    {
        public string name_com_label_
        {
            get { return Properties.Resources.anm_name_com_label; }
        }
        public string address_com_label_
        {
            get { return Properties.Resources.anm_Address_com_label_; }
        }
        public string tel_com_label_
        {
            get { return Properties.Resources.anm_tel_com_label_; }
        }
        public string email_com_label_
        {
            get { return Properties.Resources.anm_email_com_label; }
        }
        public string web_com_label_
        {
            get { return Properties.Resources.anm_web_com_label; }
        }
        public string save_com_btn_
        {
            get { return Properties.Resources.anm_save_com_btn; }
        }
        public string delete_com_btn_
        {
            get { return Properties.Resources.anm_delete_com_btn; }
        }
        public string cancel_com_btn_
        {
            get { return Properties.Resources.adc_cancel_com_btn_; }
        }
        public string brands_title
        {
            get { return Properties.Resources.adc_brands_title; }
        }
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
        public string AddUserToBrand
        {
            get { return Properties.Resources.user_AddUserToBrand; }
        }
        public string RemUserToBrand
        {
            get { return Properties.Resources.user_RemUserToBrand; }
        }

        public Add_New_Meascom_Language()
        {

        }

        public void Reload()
        {
            OnPropertyRaised("name_com_label_");
            OnPropertyRaised("RemUserToBrand");
            OnPropertyRaised("address_com_label_");
            OnPropertyRaised("tel_com_label_");
            OnPropertyRaised("email_com_label_");
            OnPropertyRaised("web_com_label_");
            OnPropertyRaised("save_com_btn_");
            OnPropertyRaised("delete_com_btn_");
            OnPropertyRaised("cancel_com_btn_");
            OnPropertyRaised("brands_title");
            OnPropertyRaised("login_head");
            OnPropertyRaised("name_head");
            OnPropertyRaised("lastname_head");
            OnPropertyRaised("email_head");
            OnPropertyRaised("role_head");
            OnPropertyRaised("AddUserToBrand");
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

    public class UserToBrand
    {
        public int ID { get; set; }
        public string Login { get; set; }
        public string Name { get; set; }
        public string Lastname { get; set; }
        public string E_mail { get; set; }
        public string Role { get; set; }
    }

    public class BrandUser
    {
        public int ID { get; set; }
        public int ID_User { get; set; }
        public int ID_Meas { get; set; }
    }

    public enum MeasurementOperationType
    {
        Add,
        Update
    }
    /// <summary>
    /// Interaction logic for Add_new_Meascom.xaml
    /// </summary>
    public partial class Add_new_Meascom : Window
    {
        //public EventHandler SetLang;
        private MeasurementOperationType CurrentOperation = MeasurementOperationType.Add;
        Measurer_Brands parent = null;
        BrandView data = null;
        User LogUser = MainWindow.GetUser();


        public Add_new_Meascom(Measurer_Brands parent, MeasurementOperationType operation, object data)
        {
            InitializeComponent();
            this.Title = (Resources["lang"] as Add_New_Meascom_Language).brands_title;
            MainWindow.RegisterLanguageHandler(Set_Language);
            if ((LogUser.Permission) <= UserPermission.Update) { ActionDelete.Visibility = Visibility.Hidden; }
            if ((LogUser.Permission) <= UserPermission.View) { AddUserToBrand.Visibility = Visibility.Hidden; }
            if ((LogUser.Permission) <= UserPermission.View) { DeleteUserFromBrand.Visibility = Visibility.Hidden; }

            //register operations
            this.CurrentOperation = operation;
            this.parent = parent;
            this.data = data as BrandView;
            SetOperation(operation, this.data);
        }

        private void SetOperation(MeasurementOperationType operation, BrandView data)
        {
            switch (operation)
            {
                case MeasurementOperationType.Add:

                    break;
                case MeasurementOperationType.Update:

                    Dispatcher.BeginInvoke(((Action)(() =>
                    {
                        CompanyName.Text = data.Company;
                        Addresss.Text = data.Address;
                        TelNumber.Text = data.Telephone;
                        Email.Text = data.Email;
                        WebAddress.Text = data.Web;
                    })));
                    UpdateUsersInBrand();

                    break;
            }
            // try bring window in fron in case hasn't been invoked yet
            try
            {
                this.Show();
            }
            catch { }
        }

        public void Set_Language()
        {
            var language_ = Resources["lang"] as Add_New_Meascom_Language;
            language_.Reload();
            Dispatcher.BeginInvoke(((Action)(() =>
            {
                this.Title = language_.brands_title;
            })));
        }

        protected override void OnClosed(EventArgs e)
        {
            MainWindow.UnRegisterLanguageHandler(Set_Language);
            base.OnClosed(e);
        }

        private void ActionSave_Click(object sender, RoutedEventArgs e)
        {
            SaveData();
        }

        private void SaveData()
        {
            try
            {
                int _ID = (CurrentOperation == MeasurementOperationType.Add) ? 0 : data.ID;
                string _Company = MySqlHelper.EscapeString(CompanyName.Text);
                string _Address = MySqlHelper.EscapeString(Addresss.Text);
                string _Telephone = MySqlHelper.EscapeString(TelNumber.Text);
                string _Email = MySqlHelper.EscapeString(Email.Text);
                string _Web = MySqlHelper.EscapeString(WebAddress.Text);

                MySqlCommand brand_cmd = (_ID == 0) ?
                    Database.Database.CreateCommand(string.Format(Database.Database.QueryStack["AddMeasCorpData"],
                    _Company,
                    _Address,
                    _Telephone,
                    _Email,
                    _Web))
                    :
                    Database.Database.CreateCommand(string.Format(Database.Database.QueryStack["RepMeasCorpData"],
                    _ID,
                    _Company,
                    _Address,
                    _Telephone,
                    _Email,
                    _Web));
                int retcc = brand_cmd.ExecuteNonQuery();
            }
            catch { }
            parent.Refresh();
            this.Close();
        }

        public void Refresh()
        {
            Dispatcher.BeginInvoke(((Action)(() =>
            {
                UpdateUsersInBrand();
            })));
        }

        private void ActionDelete_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Delete Record?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Warning) != MessageBoxResult.No)
            {
                //CheckExisUsersBrand
                MySqlCommand check_ExistUserInBrand = Database.Database.CreateCommand(string.Format(Database.Database.QueryStack["CheckExisUsersBrand"], data.ID));
                Int32 rows_count = Convert.ToInt32(check_ExistUserInBrand.ExecuteScalar());
                if (rows_count > 0)
                {
                    try
                    {                            
                        MySqlCommand command = Database.Database.CreateCommand(
                         string.Format(Database.Database.QueryStack["RemAllUserBrand"],
                         data.ID
                         ));
                        int RowEffected = command.ExecuteNonQuery();
                    }
                    catch { }
                }

                try
                {
                    MySqlCommand command = Database.Database.CreateCommand(
                        string.Format(Database.Database.QueryStack["RemMeasComData"],
                        data.ID
                        ));
                    int RowEffected = command.ExecuteNonQuery();
                }
                catch { }
                // update summoner
                parent.Refresh();
                this.Close();
            }
        }

        private List<UserToBrand> UserToBrandItems = new List<UserToBrand>();
        private void UpdateUsersInBrand()
        {
            //GetUsersInBrand

            UsersInBrand.ItemsSource = UserToBrandItems;
            List<UserToBrand> _items = new List<UserToBrand>();
            int SelectedIndex = 0;
            MySqlDataReader rdr = null;
            try
            {
                // load data base on filter
                MySqlCommand command = Database.Database.CreateCommand(string.Format(Database.Database.QueryStack["GetUsersInBrand"], data.ID));
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


                //UserRole();
                UserToBrandItems.Clear();
                UserToBrandItems = _items;
                Dispatcher.BeginInvoke(((Action)(() =>
                {
                    // rebind data value
                    UsersInBrand.ItemsSource = null;
                    UsersInBrand.ItemsSource = UserToBrandItems;
                    UsersInBrand.SelectedIndex = SelectedIndex;
                })));
            }
            catch
            {
                if (rdr != null && !rdr.IsClosed)
                    rdr.Close();
            }

        }

        private void ActionClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void CarotSet(object sender, RoutedEventArgs e)
        {
            if (EditingCommands.MoveToLineEnd.CanExecute(null, sender as IInputElement))
            {
                EditingCommands.MoveToLineEnd.Execute(null, sender as IInputElement);
            }
        }

        private void AddUserToBrand_Click(object sender, RoutedEventArgs e)
        {
            Add_UserToBrand AddUserToBrandDataWindow = new Add_UserToBrand(this, this.data.ID);
            AddUserToBrandDataWindow.Show();
        }

        private void DeleteUserFromBrand_Click(object sender, RoutedEventArgs e)
        {
            //RemUsersFromBrand
            if (MessageBox.Show("Delete Record?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Warning) != MessageBoxResult.No)
            {

                try
                {
                    MySqlCommand command = Database.Database.CreateCommand(
                        string.Format(Database.Database.QueryStack["RemUsersFromBrand"],
                        LastUserSelectedItem.ID,
                        data.ID
                        ));
                    int RowEffected = command.ExecuteNonQuery();
                }
                catch { }
                Refresh();
                parent.Refresh();
            }
        }

        public UserToBrand LastUserSelectedItem = null;
        private void UsersInBrand_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (UsersInBrand.SelectedItem != null)
            {
                LastUserSelectedItem = UsersInBrand.SelectedItem as UserToBrand;
            }
        }
    }
}
