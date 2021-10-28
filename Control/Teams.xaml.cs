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
    public class Teams_Language : INotifyPropertyChanged
    {

        public string teamname_head
        {
            get { return Properties.Resources.team_teamname_head; }
        }
        public string shortname_head
        {
            get { return Properties.Resources.team_shortname_head; }
        }
        public string email_head
        {
            get { return Properties.Resources.team_email_head; }
        }
        public string addteam_btn_
        {
            get { return Properties.Resources.team_addteam_btn_; }
        }
        public string closeteam_btn_
        {
            get { return Properties.Resources.team_closeteam_btn_; }
        }
        public string teams_title
        {
            get { return Properties.Resources.team_teams_title; }
        }
        public string print_btn_
        {
            get { return Properties.Resources.print_btn_; }
        }

        public void Reload()
        {
            OnPropertyRaised("teamname_head");
            OnPropertyRaised("print_btn_");
            OnPropertyRaised("shortname_head");
            OnPropertyRaised("email_head");
            OnPropertyRaised("addteam_btn_");
            OnPropertyRaised("closeteam_btn_");
            OnPropertyRaised("teams_title");
        }

        public Teams_Language()
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

    public class TeamView
    {
        public int ID { get; set; }
        public string Team_name { get; set; }
        public string Short_name { get; set; }
        public string E_mail { get; set; }

        public bool Compare(TeamView _ref)
        {
            if (_ref == null)
                return false;
            return (_ref.Team_name == this.Team_name && _ref.Short_name == this.Short_name && _ref.E_mail == this.E_mail);
        }
    }

    /// <summary>
    /// Interaction logic for Teams.xaml
    /// </summary>
    public partial class Teams : Window
    {
        public EventHandler SetLang;
        private static List<TeamView> TeamItems = new List<TeamView>();
        private Add_new_team UpdateWindow = null;
        User LogUser = MainWindow.GetUser();

        public Teams()
        {
            InitializeComponent();
            this.Title = (Resources["lang"] as Teams_Language).teams_title;
            MainWindow.RegisterLanguageHandler(SetLanguage);

            if ((LogUser.Permission) == UserPermission.View) { AddTeam_btn.Visibility = Visibility.Hidden; }
            UpdateTeamList(string.Empty);
        }

        protected override void OnClosed(EventArgs e)
        {
            CloseTeam_btn_Click(null, null);
            MainWindow.UnRegisterLanguageHandler(SetLanguage);
            base.OnClosed(e);
        }

        public void SetLanguage()
        {
            var language_ = Resources["lang"] as Teams_Language;
            language_.Reload();
            Dispatcher.BeginInvoke(((Action)(() =>
            {
                this.Title = language_.teams_title;
            })));
        }

        public void Refresh()
        {
            Dispatcher.BeginInvoke(((Action)(() =>
            {
                UpdateTeamList(string.Empty);
            })));
        }

        private void OnTeamDialogUpdate(object sender, EventArgs args)
        {
            UpdateTeamList(string.Empty);
        }

        public static EventHandler TeamDialogUpdate;
        public static List<EventHandler> TeamSelectionUpdateNotify = new List<EventHandler>();
        public static TeamView LastTeamSelectedItem = null;

        private void TeamList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (this.UpdateWindow != null)
                this.UpdateWindow.Close();

            // opened on selected index, just alovate object aand push data operation
            if ((LogUser.Permission) != UserPermission.View)
            {
                this.UpdateWindow = null;
                this.UpdateWindow = new Add_new_team(this, TeamOperationType.Update, LastTeamSelectedItem);
            }
        }

        private void TeamList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TeamList.SelectedItem != null)
                LastTeamSelectedItem = TeamList.SelectedItem as TeamView;
        }

        private void UpdateTeamList(string filter)
        {
            Dispatcher.BeginInvoke(((Action)(() =>
            {
                action_search.Visibility = Visibility.Visible;
            })));

            TeamList.ItemsSource = TeamItems;
            List<TeamView> _items = new List<TeamView>();
            int SelectedIndex = 0;
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

                TeamItems.Clear();
                TeamItems = _items;
                Dispatcher.BeginInvoke(((Action)(() =>
                {
                    // rebind data value
                    TeamList.ItemsSource = null;
                    TeamList.ItemsSource = TeamItems;
                    TeamList.SelectedIndex = SelectedIndex;
                })));
            }
            catch
            {
                if (rdr != null && !rdr.IsClosed)
                    rdr.Close();
            }

        }

        private void AddTeam_btn_Click(object sender, RoutedEventArgs e)
        {
            if (this.UpdateWindow != null)
                this.UpdateWindow.Close();

            this.UpdateWindow = null;
            this.UpdateWindow = new Add_new_team(this, TeamOperationType.Add, null);

        }

        private void CloseTeam_btn_Click(object sender, RoutedEventArgs e)
        {
            // free opened windows
            if (this.UpdateWindow != null)
            {
                this.UpdateWindow.Close();
                this.UpdateWindow = null;
            }

            // close this
            this.Close();

        }

        private void Print_btn_Click(object sender, RoutedEventArgs e)
        {
            PrintManager print = new PrintManager();
            print.Print(PrintManager.PrintType.Teams);
        }
    }
}
