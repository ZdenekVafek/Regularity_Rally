using System;
using System.Timers;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.ComponentModel;
using System.Windows.Threading;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows.Documents;

namespace Regularity_Rally.Control
{
    #region Language
    public class AddNewTournament_Language : INotifyPropertyChanged
    {
        public string add_new_btn_
        {
            get { return Properties.Resources.mwts_add_new_btn; }
        }
        public string print_
        {
            get { return Properties.Resources.mwrs_print; }
        }
        public string title_label_
        {
            get { return Properties.Resources.mwts_title_label; }
        }
        public string settour_lbl_
        {
            get { return Properties.Resources.mwts_settour_lbl_; }
        }
        public string choosetour_btn_
        {
            get { return Properties.Resources.mwts_choosetour_btn_; }
        }
        public string nametour_lbl_
        {
            get { return Properties.Resources.mwts_nametour_lbl_; }
        }
        public string shortname_lbl_
        {
            get { return Properties.Resources.mwts_shortname_lbl_; }
        }
        public string cattour_lbl_
        {
            get { return Properties.Resources.mwts_cattour_lbl_; }
        }
        public string numofracestour_lbl_
        {
            get { return Properties.Resources.mwts_numofracestour_lbl_; }
        }
        public string numofracerstour_lbl_
        {
            get { return Properties.Resources.mwts_numofracerstour_lbl_; }
        }
        public string yeartour_lbl_
        {
            get { return Properties.Resources.mwts_yeartour_lbl_; }
        }
        public string racestour_lbl_
        {
            get { return Properties.Resources.mwts_racestour_lbl_; }
        }
        public string racerpostour_lbl_
        {
            get { return Properties.Resources.mwts_racerpostour_lbl_; }
        }
        public string save_lbl_
        {
            get { return Properties.Resources.mwts_save_lbl_; }
        }
        public string cancel_lbl_
        {
            get { return Properties.Resources.mwts_cancel_lbl_; }
        }
        public string add_lbl_
        {
            get { return Properties.Resources.mwts_add_lbl_; }
        }
        public string remove_lbl_
        {
            get { return Properties.Resources.mwts_remove_lbl_; }
        }
        public string addnewrace_lbl_
        {
            get { return Properties.Resources.mwts_addnewrace_lbl_; }
        }
        public string delete_lbl_
        {
            get { return Properties.Resources.mwts_delete_lbl_; }
        }
        public string races_lbl_
        {
            get { return Properties.Resources.mwts_races_lbl_; }
        }
        public string choosetour_lbl_
        {
            get { return Properties.Resources.mwts_choosetour_lbl_; }
        }
        public string cancel_lbl
        {
            get { return Properties.Resources.mwts_cancel_lbl; }
        }
        public string nametour_header
        {
            get { return Properties.Resources.mwts_nametour_header; }
        }
        public string datetour_header
        {
            get { return Properties.Resources.mwts_datetour_header; }
        }
        public string placetour_header
        {
            get { return Properties.Resources.mwts_placetour_header; }
        }
        public string lenghttour_header
        {
            get { return Properties.Resources.mwts_lenghttour_header; }
        }
        public string add_race_btn_
        {
            get { return Properties.Resources.mwts_add_race_btn_; }
        }
        public string delete_race_btn_
        {
            get { return Properties.Resources.mwts_delete_race_btn_; }
        }
        public string tour_title
        {
            get { return Properties.Resources.mwts_tour_title; }
        }
        public string cat_race_label_
        {
            get { return Properties.Resources.mwts_cat_race_label_; }
        }
        public string timetour_header
        {
            get { return Properties.Resources.mwts_timetour_header; }
        }
        public string numoflapstour_header
        {
            get { return Properties.Resources.mwts_numoflapstour_header; }
        }

        public AddNewTournament_Language()
        {

        }

        public void Reload()
        {

            OnPropertyRaised("add_new_btn_");
            OnPropertyRaised("numoflapstour_header");
            OnPropertyRaised("timetour_header");
            OnPropertyRaised("cat_race_label_");
            OnPropertyRaised("print_");
            OnPropertyRaised("title_label_");
            OnPropertyRaised("choosetour_lbl_");
            OnPropertyRaised("settour_lbl_");
            OnPropertyRaised("choosetour_btn_");
            OnPropertyRaised("nametour_lbl_");
            OnPropertyRaised("shortname_lbl_");
            OnPropertyRaised("cattour_lbl_");
            OnPropertyRaised("numofracestour_lbl_");
            OnPropertyRaised("numofracerstour_lbl_");
            OnPropertyRaised("yeartour_lbl_");
            OnPropertyRaised("racestour_lbl_");
            OnPropertyRaised("racerpostour_lbl_");
            OnPropertyRaised("save_lbl_");
            OnPropertyRaised("cancel_lbl_");
            OnPropertyRaised("add_lbl_");
            OnPropertyRaised("remove_lbl_");
            OnPropertyRaised("addnewrace_lbl_");
            OnPropertyRaised("delete_lbl_");
            OnPropertyRaised("races_lbl_");
            OnPropertyRaised("cancel_lbl");
            OnPropertyRaised("mwts_nametour_header");
            OnPropertyRaised("mwts_datetour_header");
            OnPropertyRaised("mwts_placetour_header");
            OnPropertyRaised("mwts_lenghttour_header");
            OnPropertyRaised("mwts_add_race_btn_");
            OnPropertyRaised("mwts_delete_race_btn_");
            OnPropertyRaised("tour_title");
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

    public class RacesInTourData
    {
        public int ID { get; set; }
        public int FK_RaceStatus { get; set; }
        public int FK_RaceNote { get; set; }
        public int FK_Category { get; set; }
        public int FK_Meas_company { get; set; }
        public int FK_Tour { get; set; }
        public string RaceName { get; set; }
        public string Date { get; set; }
        public string Place { get; set; }
        public string Time { get; set; }
        public string StartTime { get; set; }
        public int Length { get; set; }
        public int NumOfLaps { get; set; }
        public string StatusText { get; set; }
    }

    public class CheckRaceTourID
    {
        public int ID { get; set; }
    }

    public enum TournamentOperationType
    {
        Add,
        Update
    }

    public partial class Add_new_tournament : Window
    {
        private TournamentOperationType CurrentOperation = TournamentOperationType.Add;
        Tournaments parent = null;
        TournamentsView data = null;
        User LogUser = MainWindow.GetUser();

        public Add_new_tournament(Tournaments parent, TournamentOperationType operation, object data)
        {
            InitializeComponent();
            this.Title = (Resources["lang"] as AddNewTournament_Language).tour_title;
            MainWindow.RegisterLanguageHandler(SetLanguage);
            if ((LogUser.Permission) < UserPermission.Update) { ActionSave.Visibility = Visibility.Hidden; }
            if ((LogUser.Permission) < UserPermission.Update) { AddRace.Visibility = Visibility.Hidden; }
            if ((LogUser.Permission) < UserPermission.Update) { RemoveRace.Visibility = Visibility.Hidden; }
            if ((LogUser.Permission) < UserPermission.All) { ActionDelete.Visibility = Visibility.Hidden; }
            this.CurrentOperation = operation;
            this.parent = parent;
            this.data = data as TournamentsView;
            FillCatRaceCombo();
            SetOperation(operation, this.data);
        }

        public List<CatView> ComboRaceCategory = new List<CatView>();
        private void FillCatRaceCombo()
        {
            List<CatView> _items = new List<CatView>();
            MySqlDataReader rdr = null;
            try
            {
                MySqlCommand command = Database.Database.CreateCommand(Database.Database.QueryStack["GetRaceCategoryANR"]);
                rdr = command.ExecuteReader();
                while (rdr.Read())
                {
                    _items.Add(new CatView()
                    {
                        ID = rdr.GetInt32("ID_Category"),
                        Name = rdr.GetString("Name")
                    });
                }
                ComboRaceCategory.Clear();
                ComboRaceCategory = _items;
                Dispatcher.BeginInvoke(((Action)(() =>
                {
                    CatRaceCombo.ItemsSource = null;
                    CatRaceCombo.ItemsSource = ComboRaceCategory;
                })));
                rdr.Close();
            }
            catch { }
        }

        private static List<RacesInTourData> RaceToTourItems = new List<RacesInTourData>();
        public void UpdateRacesListToTour(int _id)
        {
            RacesToTourList.ItemsSource = RaceToTourItems;
            List<RacesInTourData> _items = new List<RacesInTourData>();
            MySqlDataReader rdra = null;
            try
            {
                MySqlCommand cmd = Database.Database.CreateCommand(string.Format(Database.Database.QueryStack["GetRacesToTourANT"], _id, data.ID));
                rdra = cmd.ExecuteReader();
                while (rdra.Read())
                {
                    _items.Add(new RacesInTourData
                    {
                        ID = rdra.GetInt32("ID_Race"),
                        RaceName = rdra.GetString("Name"),
                        Date = rdra.GetDateTime("Date").ToShortDateString(),
                        Place = rdra.GetString("Place"),
                        Length = rdra.GetInt32("Length"),
                        Time = rdra.GetString("Time"), //.ToString(),
                        NumOfLaps = rdra.GetInt32("NumOfLaps")
                        //FK_Category = rdra.GetInt32("FK_Category"),
                        //FK_Meas_company = rdra.GetInt32("FK_Meas_company"),
                        //FK_RaceNote = rdra.GetInt32("FK_Race_Note"),
                        //FK_RaceStatus = rdra.GetInt32("FK_Race_Status"),
                        //FK_Tour = rdra.GetInt32("FK_Tour"),
                        //StartTime = rdra.GetString("Start_Time")
                    });
                }
                rdra.Close();

                RaceToTourItems.Clear();
                RaceToTourItems = _items;
                Dispatcher.BeginInvoke(((Action)(() =>
                {
                    RacesToTourList.ItemsSource = null;
                    RacesToTourList.ItemsSource = RaceToTourItems;
                })));
            }
            catch
            {
                if (rdra != null && !rdra.IsClosed)
                    rdra.Close();
            }
        }

        private static List<RacesInTourData> RacerInRaceItems = new List<RacesInTourData>();
        public void UpdateRacesListInTour(int _id)
        {
            RacesInTourList.ItemsSource = RacerInRaceItems;
            List<RacesInTourData> _items = new List<RacesInTourData>();
            MySqlDataReader rdra = null;
            try
            {
                MySqlCommand cmd = Database.Database.CreateCommand(string.Format(Database.Database.QueryStack["GetRacesInTourANT"], _id));
                rdra = cmd.ExecuteReader();

                while (rdra.Read())
                {
                    _items.Add(new RacesInTourData
                    {
                        ID = rdra.GetInt32("ID_Race"),
                        RaceName = rdra.GetString("Name"),
                        Date = rdra.GetString("Date"),
                        Length = rdra.GetInt32("Length"),
                        Place = rdra.GetString("Place"),
                        Time = rdra.GetString("Time"), //.ToString(),
                        NumOfLaps = rdra.GetInt32("NumOfLaps")
                    }) ;
                }
                rdra.Close();

                RacerInRaceItems.Clear();
                RacerInRaceItems = _items;
                Dispatcher.BeginInvoke(((Action)(() =>
                {
                    RacesInTourList.ItemsSource = null;
                    RacesInTourList.ItemsSource = RacerInRaceItems;
                })));
            }
            catch
            {
                if (rdra != null && !rdra.IsClosed)
                    rdra.Close();
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            MainWindow.UnRegisterLanguageHandler(SetLanguage);
            base.OnClosed(e);
        }

        public void SetLanguage()
        {
            var language_ = Resources["lang"] as AddNewTournament_Language;
            language_.Reload();
            Dispatcher.BeginInvoke(((Action)(() =>
            {
                this.Title = language_.tour_title;
            })));
        }

        private void SetOperation(TournamentOperationType operation, TournamentsView data)
        {
            switch (operation)
            {
                case TournamentOperationType.Add:

                    break;
                case TournamentOperationType.Update:

                    Dispatcher.BeginInvoke(((Action)(() =>
                    {
                        NameOfTour.Text = data.Name;
                        ShortName.Text = data.Name_Short;
                        Year.Text = data.Year.ToString();
                        CatRaceCombo.SelectedIndex = data.CategoryID-1;
                        //Console.WriteLine(this.data.ID);
                        //Console.WriteLine(this.data.CategoryID);
                        UpdateRacesListInTour(this.data.ID);
                        UpdateRacesListToTour(data.CategoryID);
                    })));

                    break;
            }
            try
            {
                this.Show();
            }
            catch { }
        }

        private void ActionSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int _ID = (CurrentOperation == TournamentOperationType.Add) ? 0 : data.ID;
                string _teamtour = MySqlHelper.EscapeString(NameOfTour.Text);
                string _shortnametour = MySqlHelper.EscapeString(ShortName.Text);
                string _yeartour = MySqlHelper.EscapeString(Year.Text);
                int categoryID = CatRaceCombo.SelectedIndex+1;
                int LastInseredID = 0;

                MySqlCommand cmd = (_ID == 0) ?
                    Database.Database.CreateCommand(string.Format(Database.Database.QueryStack["AddNewTour"],
                    _teamtour,
                    _shortnametour,
                    _yeartour,
                    categoryID))
                    :
                    Database.Database.CreateCommand(string.Format(Database.Database.QueryStack["RepNewTour"],
                    _ID,
                    categoryID,
                    _teamtour,
                    _shortnametour,
                    _yeartour
                    ));

                int retcc = cmd.ExecuteNonQuery();
                LastInseredID = (int)cmd.LastInsertedId;

                parent.Refresh();
            }
            catch { }
        }

        private void ActionDelete_Click(object sender, RoutedEventArgs e)
        {
            //delete record from databse
            if (MessageBox.Show("Delete Record?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Warning) != MessageBoxResult.No)
            {
                try
                {
                    MySqlCommand check_RacesInTour = Database.Database.CreateCommand(string.Format(Database.Database.QueryStack["CheckExistRIT"], data.ID));
                    int rows_RacesInTour = (int)Convert.ToInt32(check_RacesInTour.ExecuteScalar());
                    int rwtRacesInTour = check_RacesInTour.ExecuteNonQuery();
                    if (rows_RacesInTour > 0)
                    {
                        MySqlCommand cmd_RacesInTour = Database.Database.CreateCommand(string.Format(Database.Database.QueryStack["RemRacesRIT"], data.ID));
                        int rowRemRacerCarANR = cmd_RacesInTour.ExecuteNonQuery();
                    }
                }
                catch { }


                try
                {
                    MySqlCommand command = Database.Database.CreateCommand(
                        string.Format(Database.Database.QueryStack["RemNewTour"],
                        data.ID));
                    int RowEffected = command.ExecuteNonQuery();
                }
                catch { }
                parent.Refresh();
                this.Close();
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

        public static RacesInTourData LastSelectedRaceInTour = null;
        private void RacesInTourList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (RacesInTourList.SelectedItem != null)
            {
                LastSelectedRaceInTour = RacesInTourList.SelectedItem as RacesInTourData;
            }
        }

        private void AddRace_Click(object sender, RoutedEventArgs e)
        {
            RaceSelectionToTournament.Visibility = Visibility.Visible;
        }

        private void RemoveRace_Click(object sender, RoutedEventArgs e)
        {
            int _RaceID = LastSelectedRaceInTour.ID;
            int _TourID = data.ID;

            try
            {
                MySqlCommand cmd = Database.Database.CreateCommand(string.Format(Database.Database.QueryStack["RemRaceTourID"], _RaceID, _TourID));
                int retcc = cmd.ExecuteNonQuery();
            }
            catch { }
            UpdateRacesListInTour(data.ID);
        }

        public static RacesInTourData LastSelectedRaceToTour = null;
        private void RacesToTourList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (RacesToTourList.SelectedItem != null)
            {
                RacesInTourData _t = LastSelectedRaceToTour;
                LastSelectedRaceToTour = RacesToTourList.SelectedItem as RacesInTourData;
            }
        }

        public List<CheckRaceTourID> CheckRacerTourIDList = new List<CheckRaceTourID>();
        private void AddRaceToTour_Click(object sender, RoutedEventArgs e)
        {
            int _RaceID = LastSelectedRaceToTour.ID;
            int _TourID = data.ID;

            try
            {
                    MySqlCommand cmd = Database.Database.CreateCommand(string.Format(Database.Database.QueryStack["AddRaceToTourANT"], _RaceID, _TourID));
                    int retcc = cmd.ExecuteNonQuery();
            }
            catch { }

            UpdateRacesListInTour(data.ID);
            RaceSelectionToTournament.Visibility = Visibility.Hidden;
        }

        private void CloseRaceToTour_Click(object sender, RoutedEventArgs e)
        {
            RaceSelectionToTournament.Visibility = Visibility.Hidden;
        }
    }
}
