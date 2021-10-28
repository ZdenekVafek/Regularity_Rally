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
    public class Add_New_User_Language : INotifyPropertyChanged
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
        public string active_head
        {
            get { return Properties.Resources.user_active_head; }
        }
        public string email_head
        {
            get { return Properties.Resources.user_email_head; }
        }
        public string perm_user_label
        {
            get { return Properties.Resources.user_role_head; }
        }

        public string newpass_user_label    // remove me or rename
        {
            get { return Properties.Resources.user_newpass_user_label; }
        }
        public string closeuser_btn_
        {
            get { return Properties.Resources.user_closeuser_btn_; }
        }
        public string save_user_btn
        {
            get { return Properties.Resources.user_save_user_btn; }
        }
        public string user_title
        {
            get { return Properties.Resources.user_user_title; }
        }
        public string password_head
        {
            get { return "password"; }
        }
        public string password_re_head
        {
            get { return "password znovu"; }
        }
        public void Reload()
        {
            OnPropertyRaised("login_head");
            OnPropertyRaised("name_head");
            OnPropertyRaised("lastname_head");
            OnPropertyRaised("email_head");
            OnPropertyRaised("perm_user_label");
            OnPropertyRaised("active_head");
            OnPropertyRaised("newpass_user_label");
            OnPropertyRaised("save_user_btn");
            OnPropertyRaised("closeuser_btn_");
            OnPropertyRaised("user_title");
        }

        public Add_New_User_Language()
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
    /// <summary>
    /// Interaction logic for Add_new_user.xaml
    /// </summary>
    /// 

    public enum UserOperationType
    {
        Add,
        Update
    }

    public partial class Add_new_user : Window
    {
        private UserOperationType CurrentOperation = UserOperationType.Add;
        Users parent = null;
        User data = null;           // holder to direct user object

        public Add_new_user(Users parent, UserOperationType operation, object data)
        {
            InitializeComponent();
            this.Title = (Resources["lang"] as Add_New_User_Language).user_title; 
            MainWindow.RegisterLanguageHandler(SetLanguage);

            // bind enum to combo
            role_combo.ItemsSource = Enum.GetValues(typeof(UserRole));
            perm_combo.ItemsSource = Enum.GetValues(typeof(UserPermission));

            // operations
            this.CurrentOperation = operation;
            this.parent = parent;
            this.data = data as User;
            SetOperation(operation, this.data);
        }

        protected override void OnClosed(EventArgs e)
        {
            MainWindow.UnRegisterLanguageHandler(SetLanguage);

            base.OnClosed(e);
        }

        public void SetLanguage()
        {
            var language_ = Resources["lang"] as Add_New_User_Language;
            language_.Reload();
            Dispatcher.BeginInvoke(((Action)(() =>
            {
                this.Title = language_.user_title;
            })));
        }

        private void SetOperation(UserOperationType operation, User data)
        {
            Dispatcher.BeginInvoke(((Action)(() =>
            {
                switch (operation)
                {
                    case UserOperationType.Add:
                        // clear
                        LoginUser_txt.Text = string.Empty;
                        FirstNameUser_txt.Text = string.Empty;
                        LastNameUser_txt.Text = string.Empty;
                        Email_txt.Text = string.Empty;

                        role_combo.SelectedItem = null;
                        perm_combo.SelectedItem = null;
                        break;
                    case UserOperationType.Update:
                        UserView view = data.ToUserView(false);
                        LoginUser_txt.Text = view.Login;
                        FirstNameUser_txt.Text = view.Name;
                        LastNameUser_txt.Text = view.Lastname;
                        Email_txt.Text = view.E_mail;
                        role_combo.SelectedItem = data.Role;
                        perm_combo.SelectedItem = data.Permission;
                        break;
                }
                // try bring window in fron in case hasn't been invoked yet
                try
                {
                    this.Show();
                }
                catch { }
            })));
        }


        private void ActionSave_Click(object sender, RoutedEventArgs e)
        {
            if (this.CurrentOperation == UserOperationType.Update)
            {
                // allow user objest hanlde all operations
                Dictionary<string, string> updates = new Dictionary<string, string>();
                UserView view = data.ToUserView(false);

                if (view.Login != LoginUser_txt.Text && LoginUser_txt.Text != string.Empty)
                    updates.Add("Login", LoginUser_txt.Text);

                if (view.Name != FirstNameUser_txt.Text && FirstNameUser_txt.Text != string.Empty)
                    updates.Add("Name", FirstNameUser_txt.Text);

                if (view.Lastname != LastNameUser_txt.Text && LastNameUser_txt.Text != string.Empty)
                    updates.Add("Lastname", LastNameUser_txt.Text);

                if (view.E_mail != Email_txt.Text && Email_txt.Text != string.Empty)
                    updates.Add("E_mail", Email_txt.Text);

                // password
                if ((PassUser_txt.Password != string.Empty && PassUserRe_txt.Password != string.Empty) && (PassUser_txt.Password == PassUserRe_txt.Password))
                {
                    // create needed string
                    string salt = Regularity_Rally.Password.CreateRS();
                    string salted_pass = Regularity_Rally.Password.CretePasswordHash(PassUser_txt.Password, salt);

                    updates.Add("Salt", salt);
                    updates.Add("Password", salted_pass);
                }

                // role
                UserRole _role = UserRole.None;
                if (role_combo.SelectedItem != null && (UserRole)role_combo.SelectedItem != UserRole.None && Enum.TryParse<UserRole>(view.Role, out _role))
                {
                    if (_role != (UserRole)role_combo.SelectedItem)
                        updates.Add("Role", ((Int32)(UserRole)role_combo.SelectedItem).ToString());
                }

                // Permisson
                UserPermission perm = UserPermission.None;
                if (perm_combo.SelectedItem != null && (UserPermission)perm_combo.SelectedItem != UserPermission.None && Enum.TryParse<UserPermission>(view.Permisson, out perm))
                {
                    if (perm != (UserPermission)perm_combo.SelectedItem)
                        updates.Add("Permisson", ((Int32)(UserPermission)perm_combo.SelectedItem).ToString());
                }

                try
                {
                    if (!User.UpdateUser(data, updates)) { Console.WriteLine("Error with additing user!"); }
                }
                catch (InvalidOperationException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            if (this.CurrentOperation == UserOperationType.Add)
            {
                if ((PassUser_txt.Password != string.Empty && PassUserRe_txt.Password != string.Empty) && (PassUser_txt.Password != PassUserRe_txt.Password))
                {
                    // report prom about password 
                    Console.WriteLine("Error password not match");
                    return;
                }

                if (User.CreateUser(LoginUser_txt.Text, PassUser_txt.Password, FirstNameUser_txt.Text, LastNameUser_txt.Text, Email_txt.Text, true, (UserRole)role_combo.SelectedItem, (Int32)(UserPermission)perm_combo.SelectedItem) == null)
                {
                    // report user fail
                    Console.WriteLine("failure creating user");
                    parent.Refresh();
                    return;
                }
            }

            parent.Refresh();
            this.Close();
        }

        private void ActionCancel_Click(object sender, RoutedEventArgs e)
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
    }
}
