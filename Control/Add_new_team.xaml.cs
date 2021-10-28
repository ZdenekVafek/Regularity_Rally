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
    public class Add_New_Team_Language : INotifyPropertyChanged
    {
        public string name_team_label_
        {
            get { return Regularity_Rally.Properties.Resources.ancar_name_team_label_; }
        }
        public string short_team_label_
        {
            get { return Regularity_Rally.Properties.Resources.ancar_short_team_label_; }
        }
        public string email_team_label_
        {
            get { return Regularity_Rally.Properties.Resources.ancar_email_team_label_; }
        }
        public string save_team_btn_
        {
            get { return Regularity_Rally.Properties.Resources.ancar_save_team_btn_; }
        }
        public string delete_team_btn_
        {
            get { return Regularity_Rally.Properties.Resources.ancar_delete_team_btn_; }
        }
        public string team_title
        {
            get { return Regularity_Rally.Properties.Resources.ancar_team_title; }
        }
        public string First_nameRacer_head
        {
            get { return Properties.Resources.racer_First_nameRacer_head; }
        }
        public string Last_nameRacer_head
        {
            get { return Properties.Resources.racer_Last_nameRacer_head; }
        }
        public string NationalityRacer_head
        {
            get { return Properties.Resources.racer_NationalityRacer_head; }
        }
        public string TelRacer_head
        {
            get { return Properties.Resources.racer_TelRacer_head; }
        }
        public string MailRacer_head
        {
            get { return Properties.Resources.racer_MailRacer_head; }
        }
        public string close_team_btn_
        {
            get { return Properties.Resources.racer_closeracers_btn_; }
        }

        public Add_New_Team_Language()
        {

        }

        public void Reload()
        {
            OnPropertyRaised("name_team_label_");
            OnPropertyRaised("close_team_btn_");
            OnPropertyRaised("short_team_label_");
            OnPropertyRaised("email_team_label_");
            OnPropertyRaised("save_team_btn_");
            OnPropertyRaised("delete_team_btn_");
            OnPropertyRaised("team_title");
            OnPropertyRaised("First_nameRacer_head");
            OnPropertyRaised("Last_nameRacer_head");
            OnPropertyRaised("NationalityRacer_head");
            OnPropertyRaised("TelRacer_head");
            OnPropertyRaised("MailRacer_head");
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


    public enum TeamOperationType
    {
        Add,
        Update
    }

    public partial class Add_new_team : Window
    {
        //public EventHandler SetLang;
        private TeamOperationType CurrentOperation = TeamOperationType.Add;
        Teams parent = null;
        TeamView data = null;
        User LogUser = MainWindow.GetUser();

        public Add_new_team(Teams parent, TeamOperationType operation, object data)
        {
            InitializeComponent();
            this.Title = (Resources["lang"] as Add_New_Team_Language).team_title;

            MainWindow.RegisterLanguageHandler(Set_Language);

            if ((LogUser.Permission) < UserPermission.All) { ActionDelete.Visibility = Visibility.Hidden; }
            if ((LogUser.Permission) < UserPermission.Update) { save_team_btn.Visibility = Visibility.Hidden; }
            //refister operations
            this.CurrentOperation = operation;
            this.parent = parent;
            this.data = data as TeamView;
            SetOperation(operation, this.data);
        }

        protected override void OnClosed(EventArgs e)
        {
            MainWindow.UnRegisterLanguageHandler(Set_Language);
            base.OnClosed(e);
        }

        public void Set_Language()
        {
            var language_ = Resources["lang"] as Add_New_Team_Language;
            language_.Reload();
            Dispatcher.BeginInvoke(((Action)(() =>
            {
                this.Title = language_.team_title;
            })));
        }

        private void SetOperation(TeamOperationType operation, TeamView data)
        {
            switch (operation)
            {
                case TeamOperationType.Add:

                    break;
                case TeamOperationType.Update:

                    Dispatcher.BeginInvoke(((Action)(() =>
                    {
                        name_team_txt_.Text = data.Team_name;
                        short_team_txt_.Text = data.Short_name;
                        email_team_txt_.Text = data.E_mail;
                    })));
                    UpdateRacersInTeamList();
                    break;
            }
            // try bring window in fron in case hasn't been invoked yet
            try
            {
                this.Show();
            }
            catch { }
        }

        private static List<RacerView> RacerInTeamItems = new List<RacerView>();
        public void UpdateRacersInTeamList()
        {
            //RacersInTeamList
            RacersInTeamList.ItemsSource = RacerInTeamItems;
            List<RacerView> _items = new List<RacerView>();
            int SelectedIndex = 0;
            MySqlDataReader rdr = null;
            try
            {
                // load data base on filter
                MySqlCommand command = Database.Database.CreateCommand(string.Format(Database.Database.QueryStack["GetRacerInTeamData"], data.ID));
                rdr = command.ExecuteReader();

                while (rdr.Read())
                {
                    _items.Add(new RacerView()
                    {
                        ID = rdr.GetInt32("ID_Racer"),
                        First_name = rdr.GetString("First_name"),
                        Last_name = rdr.GetString("Last_name"),
                        Short_name = rdr.GetString("Short_name"),
                        Gender = rdr.GetString("Gender"),
                        Born = rdr.GetString("Born"),
                        Nationality = rdr.GetString("Nationality"),
                        Address = rdr.GetString("Address"),
                        Tel = rdr.GetString("Telephone"),
                        Mail = rdr.GetString("Email"),
                        TeamID = rdr.GetInt32("FK_Team")
                    });
                }
                rdr.Close();

                RacerInTeamItems.Clear();
                RacerInTeamItems = _items;
                Dispatcher.BeginInvoke(((Action)(() =>
                {
                    // rebind data value
                    RacersInTeamList.ItemsSource = null;
                    RacersInTeamList.ItemsSource = RacerInTeamItems;
                    RacersInTeamList.SelectedIndex = SelectedIndex;
                })));
            }
            catch
            {
                if (rdr != null && !rdr.IsClosed)
                    rdr.Close();
            }
        }

        private void Save_team_btn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int TeamID = (CurrentOperation == TeamOperationType.Add) ? 0 : data.ID;
                string _teamname = MySqlHelper.EscapeString(name_team_txt_.Text);
                string _shortteamname = MySqlHelper.EscapeString(short_team_txt_.Text);
                string _emailteam = MySqlHelper.EscapeString(email_team_txt_.Text);

                MySqlCommand team_cmd = (TeamID == 0) ?
                    Database.Database.CreateCommand(string.Format(Database.Database.QueryStack["AddTeamData"],
                    _teamname,
                    _shortteamname,
                    _emailteam))
                    :
                    Database.Database.CreateCommand(string.Format(Database.Database.QueryStack["RepTeamData"],
                    TeamID,
                    _teamname,
                    _shortteamname,
                    _emailteam));

                int retcc = team_cmd.ExecuteNonQuery();
            }
            catch { }
            parent.Refresh();
            this.Close();
        }

        private void ActionDelete_Click(object sender, RoutedEventArgs e)
        {
            //delete record from databse
            if (MessageBox.Show("Delete Record?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Warning) != MessageBoxResult.No)
            {
                try
                {
                    MySqlCommand command = Database.Database.CreateCommand(
                        string.Format(Database.Database.QueryStack["RemTeamData"],
                        data.ID));
                    int RowEffected = command.ExecuteNonQuery();
                }
                catch { }
                parent.Refresh();
                this.Close();
            }
        }

        private void CarotSet(object sender, RoutedEventArgs e)
        {
            if (EditingCommands.MoveToLineEnd.CanExecute(null, sender as IInputElement))
            {
                EditingCommands.MoveToLineEnd.Execute(null, sender as IInputElement);
            }
        }

        private void ActionClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}