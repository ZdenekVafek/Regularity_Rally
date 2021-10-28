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
    public class Add_New_Racers_Language : INotifyPropertyChanged
    {
        public string First_nameRacer_head
        {
            get { return Properties.Resources.racer_First_nameRacer_head; }
        }
        public string Last_nameRacer_head
        {
            get { return Properties.Resources.racer_Last_nameRacer_head; }
        }
        public string Short_nameRacer_head
        {
            get { return Properties.Resources.racer_Short_nameRacer_head; }
        }
        public string GenderRacer_head
        {
            get { return Properties.Resources.racer_GenderRacer_head; }
        }
        public string NationalityRacer_head
        {
            get { return Properties.Resources.racer_NationalityRacer_head; }
        }
        public string AddressRacer_head
        {
            get { return Properties.Resources.racer_AddressRacer_head; }
        }
        public string TelRacer_head
        {
            get { return Properties.Resources.racer_TelRacer_head; }
        }
        public string MailRacer_head
        {
            get { return Properties.Resources.racer_MailRacer_head; }
        }
        public string TeamRacer_head
        {
            get { return Properties.Resources.racer_TeamRacer_head; }
        }
        public string BornRacer_head
        {
            get { return Properties.Resources.racer_BornRacer_head; }
        }
        public string save_racer_btn_
        {
            get { return Properties.Resources.racer_save_racer_btn_; }
        }
        public string delete_racer_btn_
        {
            get { return Properties.Resources.racer_delete_racer_btn_; }
        }
        public string cancel_racer_btn_
        {
            get { return Properties.Resources.racer_cancel_racer_btn_; }
        }
        public string Racer_Title
        {
            get { return Properties.Resources.racer_Racer_Title; }
        }


        public void Reload()
        {
            OnPropertyRaised("First_nameRacer_head");
            OnPropertyRaised("Last_nameRacer_head");
            OnPropertyRaised("Short_nameRacer_head");
            OnPropertyRaised("GenderRacer_head");
            OnPropertyRaised("NationalityRacer_head");
            OnPropertyRaised("AddressRacer_head");
            OnPropertyRaised("TelRacer_head");
            OnPropertyRaised("MailRacer_head");
            OnPropertyRaised("TeamRacer_head");
            OnPropertyRaised("BornRacer_head");
            OnPropertyRaised("save_racer_btn_");
            OnPropertyRaised("delete_racer_btn_");
            OnPropertyRaised("Racer_Title");
        }

        public Add_New_Racers_Language()
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


    public enum RacerOperationType
    {
        Add,
        Update
    }

    /// <summary>
    /// Interaction logic for Add_new_racer.xaml
    /// </summary>
    public partial class Add_new_racer : Window
    {
        public EventHandler SetLang;
        private RacerOperationType CurrentOperation = RacerOperationType.Add;
        Racers parent = null;
        RacerView data = null;
        User LogUser = MainWindow.GetUser();

        public Add_new_racer(Racers parent, RacerOperationType operation, object data)
        {
            InitializeComponent();
            this.Title = (Resources["lang"] as Add_New_Racers_Language).Racer_Title;
            MainWindow.RegisterLanguageHandler(Set_Language);

            if ((LogUser.Permission) == UserPermission.View) { ActionSave.Visibility = Visibility.Hidden; }
            if ((LogUser.Permission) < UserPermission.All ) { ActionDelete.Visibility = Visibility.Hidden; }

            // reister operations 
            this.CurrentOperation = operation;
            this.parent = parent;
            this.data = data as RacerView;
            SetOperation(operation, this.data);
            FillTeamRacerCombo();
        }

        public List<TeamView> ComboTeamSelect = new List<TeamView>();
        private void FillTeamRacerCombo()
        {
            List<TeamView> _items = new List<TeamView>();
            MySqlDataReader rdr = null;
            try
            {
                // load data base on filter
                MySqlCommand command = Database.Database.CreateCommand(Database.Database.QueryStack["GetTeamData"]);
                rdr = command.ExecuteReader();

                while (rdr.Read())
                {
                    _items.Add(new TeamView()
                    {
                        ID = rdr.GetInt32("ID_Team"),
                        Team_name = rdr.GetString("Team_name"),
                        Short_name = rdr.GetString("Short_name"),
                        E_mail = rdr.GetString("E_mail"),

                    });
                }
                rdr.Close();

                Console.WriteLine("test");
                foreach (TeamView field in _items)
                {
                    Console.WriteLine(field.ID);
                }

                ComboTeamSelect.Clear();
                ComboTeamSelect = _items;
                Dispatcher.BeginInvoke(((Action)(() =>
                {
                    // rebind data value
                    TeamRacerCombo.ItemsSource = null;
                    TeamRacerCombo.ItemsSource = ComboTeamSelect;
                })));
            }
            catch
            {
                if (rdr != null && !rdr.IsClosed)
                    rdr.Close();
            }
        }

        private void SetOperation(RacerOperationType operation, RacerView data)
        {
            switch (operation)
            {
                case RacerOperationType.Add:
                    break;
                case RacerOperationType.Update:
                    Dispatcher.BeginInvoke(((Action)(() =>
                    {
                        FirstName_txt.Text = data.First_name;
                        LastNameRacer_txt.Text = data.Last_name;
                        InitialsRacer_txt.Text = data.Short_name;
                        GenderRacer_txt.Text = data.Gender;
                        NationalityRacer_txt.Text = data.Nationality;
                        AdressRacer_txt.Text = data.Address;
                        TelRacer_txt.Text = data.Tel;
                        EmailRace_txt.Text = data.Mail;
                        BirthdayRacer.Text = data.Born; 
                        TeamRacerCombo.SelectedIndex = data.TeamID-1;    
                    })));
                    break;
            }
            try
            {
                this.Show();
            }
            catch { }


        }

        protected override void OnClosed(EventArgs e)
        {
            // unregister
            MainWindow.UnRegisterLanguageHandler(Set_Language);
            base.OnClosed(e);
        }

        public void Set_Language()
        {
            var language_ = Resources["lang"] as Add_New_Racers_Language;
            language_.Reload();
            Dispatcher.BeginInvoke(((Action)(() =>
            {
                this.Title = language_.Racer_Title;
            })));
        }

        private void CarotSet(object sender, RoutedEventArgs e)
        {
            if (EditingCommands.MoveToLineEnd.CanExecute(null, sender as IInputElement))
            {
                EditingCommands.MoveToLineEnd.Execute(null, sender as IInputElement);
            }
        }

        private void ActionSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int RacerID = (CurrentOperation == RacerOperationType.Add) ? 0 : data.ID;
                string _firstName = MySqlHelper.EscapeString(FirstName_txt.Text);
                string _lastName = MySqlHelper.EscapeString(LastNameRacer_txt.Text);
                string _shortName = MySqlHelper.EscapeString(InitialsRacer_txt.Text);
                string _bornRacer = MySqlHelper.EscapeString(BirthdayRacer.Text);
                string _gender = MySqlHelper.EscapeString(GenderRacer_txt.Text);
                string _nationality = MySqlHelper.EscapeString(NationalityRacer_txt.Text);
                string _address = MySqlHelper.EscapeString(AdressRacer_txt.Text);
                string _tel = MySqlHelper.EscapeString(TelRacer_txt.Text);
                string _mail = MySqlHelper.EscapeString(EmailRace_txt.Text);
                int _TeamID = TeamRacerCombo.SelectedIndex+1;
                

                DateTime ParseTime = DateTime.ParseExact(_bornRacer, "dd.MM.yyyy", null);
                _bornRacer=ParseTime.ToString("yyyy-MM-dd");

                MySqlCommand user_cmd = (RacerID == 0) ?
                    Database.Database.CreateCommand(string.Format(Database.Database.QueryStack["AddRacerData"],
                    _firstName,
                    _lastName,
                    _shortName,
                    _bornRacer,
                    _gender,
                    _nationality,
                    _address,
                    _tel,
                    _mail,
                    _TeamID
                    ))
                    :   //L
                    Database.Database.CreateCommand(string.Format(Database.Database.QueryStack["RepRacerData"],
                    RacerID,
                    _firstName,
                    _lastName,
                    _shortName,
                    _bornRacer,
                    _gender,
                    _nationality,
                    _address,
                    _tel,
                    _mail,
                    _TeamID
                    ));

                int retcc = user_cmd.ExecuteNonQuery();

                parent.Refresh();
                this.Close();
            }
            catch
            {
                // call parent first, that will pass comant over dispatcher soo im not needed anymore
                parent.Refresh();
                this.Close();
            }
        }

        private void ActionDelete_Click(object sender, RoutedEventArgs e)
        {
            //delete record from database
            if (MessageBox.Show("Delete Record?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Warning) != MessageBoxResult.No)
            {
                try
                {
                    MySqlCommand command = Database.Database.CreateCommand(
                        string.Format(Database.Database.QueryStack["RemRacerData"],
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

        private void ActionCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
