using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using MySql.Data.MySqlClient;
using System.Data.SqlClient;
using System.Windows.Threading;

namespace Regularity_Rally.Control
{
    #region Language
    public class Homescreen_Language : INotifyPropertyChanged
    {
        public string PlaceOfRace_label_
        {
            get { return Regularity_Rally.Properties.Resources.mwrs_place_lbl; }
        }
        public string CategoryOfRace_label_
        {
            get { return Regularity_Rally.Properties.Resources.mwrs_cat_lbl; }
        }
        public string LengthOfRace_label_
        {
            get { return Regularity_Rally.Properties.Resources.mwrs_length_lbl; }
        }
        public string MeasComInRace
        {
            get { return Regularity_Rally.Properties.Resources.mwrs_timekeepers_lbl; }
        }
        public string StateOfRace_label_
        {
            get { return Regularity_Rally.Properties.Resources.mwrs_state_lbl; }
        }
        public string name_racer_lbl
        {
            get { return Regularity_Rally.Properties.Resources.mwrs_name_racer_lbl; }
        }
        public string name_team_lbl
        {
            get { return Regularity_Rally.Properties.Resources.mwrs_name_team_lbl; }
        }
        public string nametour_lbl_
        {
            get { return Regularity_Rally.Properties.Resources.mwts_nametour_lbl_; }
        }
        public string shortname_lbl_
        {
            get { return Regularity_Rally.Properties.Resources.mwts_shortname_lbl_; }
        }
        public string cattour_lbl_
        {
            get { return Regularity_Rally.Properties.Resources.mwts_cattour_lbl_; }
        }
        public string numofracestour_lbl_
        {
            get { return Regularity_Rally.Properties.Resources.mwts_numofracestour_lbl_; }
        }
        public string numofracerstour_lbl_
        {
            get { return Regularity_Rally.Properties.Resources.mwts_numofracerstour_lbl_; }
        }
        public string yeartour_lbl_
        {
            get { return Regularity_Rally.Properties.Resources.mwts_yeartour_lbl_; }
        }
        public string name_race_lbl
        {
            get { return Regularity_Rally.Properties.Resources.mwts_name_race_lbl; }
        }
        public string date_race_lbl
        {
            get { return Regularity_Rally.Properties.Resources.mwts_date_race_lbl; }
        }
        public string place_race_lbl
        {
            get { return Regularity_Rally.Properties.Resources.mwts_place_race_lbl; }
        }
        public string state_race_lbl
        {
            get { return Regularity_Rally.Properties.Resources.mwts_state_race_lbl; }
        }
        public string racestour_lbl_
        {
            get { return Regularity_Rally.Properties.Resources.mwts_racestour_lbl_; }
        }
        public string racerpostour_lbl_
        {
            get { return Regularity_Rally.Properties.Resources.mwts_racerpostour_lbl_; }
        }
        public string pos_tour_lbl
        {
            get { return Regularity_Rally.Properties.Resources.mwts_pos_tour_lbl; }
        }
        public string nameracer_tour_lbl
        {
            get { return Regularity_Rally.Properties.Resources.mwts_nameracer_tour_lbl; }
        }
        public string team_tour_lbl
        {
            get { return Regularity_Rally.Properties.Resources.mwts_team_tour_lbl; }
        }
        public string score_tour_lbl
        {
            get { return Regularity_Rally.Properties.Resources.mwts_score_tour_lbl; }
        }
        public string pos_RacerInRace
        {
            get { return Regularity_Rally.Properties.Resources.mwrs_pos_RacerInRace; }
        }
        public string time_RacerInRace
        {
            get { return Regularity_Rally.Properties.Resources.mwrs_time_RacerInRace; }
        }
        public string pentime_RacerInRace
        {
            get { return Regularity_Rally.Properties.Resources.mwrs_pentime_RacerInRace; }
        }
        public string car_RacerInRace
        {
            get { return Regularity_Rally.Properties.Resources.mwrs_car_RacerInRace; }
        }
        public string finaltime_RacerInRace
        {
            get { return Regularity_Rally.Properties.Resources.mwrs_finaltime_RacerInRace; }
        }

        public string Title_head
        {
            get { return Properties.Resources.mwi_Title_head; }
        }
        public string Place_head
        {
            get { return Properties.Resources.mwi_Place_head; }
        }
        public string Date_head
        {
            get { return Properties.Resources.mwi_Date_head; }
        }
        public string Length_head
        {
            get { return Properties.Resources.mwi_Length_head; }
        }
        public string Mdate_head
        {
            get { return Properties.Resources.mwi_Mdate_head; }
        }
        public string Status_head
        {
            get { return Properties.Resources.mwi_Status_head; }
        }
        public string Meas_head
        {
            get { return Properties.Resources.mwi_Meas_head; }
        }
        public string choose_race_btn_
        {
            get { return Properties.Resources.mwi_choose_race_btn_; }
        }
        public string referencetime_lbl
        {
            get { return Properties.Resources.mwi_referencetime_lbl; }
        }
        public string racers_lbl
        {
            get { return Properties.Resources.mwi_racers_lbl; }
        }
        public string Timekeeping_btn
        {
            get { return Properties.Resources.mwi_Timekeeping_btn; }
        }
        public string Name_head
        {
            get { return Properties.Resources.t_Name_head; }
        }
        public string ShortName_head
        {
            get { return Properties.Resources.t_ShortName_head; }
        }
        public string Year_head
        {
            get { return Properties.Resources.t_Year_head; }
        }
        public string Category_head
        {
            get { return Properties.Resources.t_Category_head; }
        }
        public string Events_MenuItemHeader
        {
            get { return Properties.Resources.hs_Events_MenuItemHeader; }
        }
        public string Events_Races_MenuItemHeader
        {
            get { return Properties.Resources.hs_Events_Races_MenuItemHeader; }
        }
        public string Events_Tournaments_MenuItemHeader
        {
            get { return Properties.Resources.hs_Events_Tournaments_MenuItemHeader; }
        }
        public string Items_MenuItemHeader
        {
            get { return Properties.Resources.hs_Items_MenuItemHeader; }
        }
        public string Items_Cars_MenuItemHeader
        {
            get { return Properties.Resources.hs_Items_Cars_MenuItemHeader; }
        }
        public string Items_Category_MenuItemHeader
        {
            get { return Properties.Resources.hs_Items_Category_MenuItemHeader; }
        }
        public string Items__Measurerbrands_MenuItemHeader
        {
            get { return Properties.Resources.hs_Items__Measurerbrands_MenuItemHeader; }
        }
        public string Items_Racers_MenuItemHeader
        {
            get { return Properties.Resources.hs_Items_Racers_MenuItemHeader; }
        }
        public string Items_Races_MenuItemHeader
        {
            get { return Properties.Resources.hs_Items_Races_MenuItemHeader; }
        }
        public string Items_Teams_MenuItemHeader
        {
            get { return Properties.Resources.hs_Items_Teams_MenuItemHeader; }
        }
        public string Items_Tournaments_MenuItemHeader
        {
            get { return Properties.Resources.hs_Items_Tournaments_MenuItemHeader; }
        }
        public string Items_Users_MenuItemHeader
        {
            get { return Properties.Resources.hs_Items_Users_MenuItemHeader; }
        }
        public string Settings_MenuItemHeader
        {
            get { return Properties.Resources.hs_Settings_MenuItemHeader; }
        }
        public string Settings_Device_MenuItemHeader
        {
            get { return Properties.Resources.hs_Settings_Device_MenuItemHeader; }
        }
        public string Logout_MenuItemHeader
        {
            get { return Properties.Resources.hs_Logout_MenuItemHeader; }
        }
        public string Settings_Language_MenuItemHeader
        {
            get { return Properties.Resources.hs_Settings_Language_MenuItemHeader; }
        }
        public string LapsOfRace_label_
        {
            get { return Properties.Resources.hs_LapsOfRace_label_; }
        }
        public string Add_ons_MenuItemHeader
        {
            get { return Regularity_Rally.Properties.Resources.hs_Add_ons_MenuItemHeader; }
        }
        public string Add_ons_Reporter_MenuItemHeader
        {
            get { return Regularity_Rally.Properties.Resources.hs_Add_ons_Reporter_MenuItemHeader; }
        }
        public string Add_ons_Paddock_MenuItemHeader
        {
            get { return Regularity_Rally.Properties.Resources.hs_Add_ons_Paddock_MenuItemHeader; }
        }



        public void Reload()
        {
            OnPropertyRaised("PlaceOfRace_label_");
            OnPropertyRaised("Add_ons_Paddock_MenuItemHeader");
            OnPropertyRaised("Add_ons_Reporter_MenuItemHeader");
            OnPropertyRaised("Add-Add_ons_MenuItemHeader");
            OnPropertyRaised("LapsOfRace_label_");
            OnPropertyRaised("Logout_MenuItemHeader");
            OnPropertyRaised("Settings_Language_MenuItemHeader");
            OnPropertyRaised("Settings_Device_MenuItemHeader");
            OnPropertyRaised("Settings_MenuItemHeader");
            OnPropertyRaised("Items_Users_MenuItemHeader");
            OnPropertyRaised("Items_Tournaments_MenuItemHeader");
            OnPropertyRaised("Items_Teams_MenuItemHeader");
            OnPropertyRaised("Items_Races_MenuItemHeader");
            OnPropertyRaised("Items_Racers_MenuItemHeader");
            OnPropertyRaised("Items__Measurerbrands_MenuItemHeader");
            OnPropertyRaised("Items_Category_MenuItemHeader");
            OnPropertyRaised("Items_Cars_MenuItemHeader");
            OnPropertyRaised("Items_MenuItemHeader");
            OnPropertyRaised("Events_MenuItemHeader");
            OnPropertyRaised("Events_Tournaments_MenuItemHeader");
            OnPropertyRaised("Events_Races_MenuItemHeader");
            OnPropertyRaised("nametour_lbl_");
            OnPropertyRaised("Category_head");
            OnPropertyRaised("Name_head");
            OnPropertyRaised("ShortName_head");
            OnPropertyRaised("Year_head");
            OnPropertyRaised("CategoryOfRace_label_");
            OnPropertyRaised("LengthOfRace_label_");
            OnPropertyRaised("MeasComInRace");
            OnPropertyRaised("StateOfRace_label_");
            OnPropertyRaised("name_racer_lbl");
            OnPropertyRaised("name_team_lbl");
            OnPropertyRaised("nametour_lbl_");
            OnPropertyRaised("shortname_lbl_");
            OnPropertyRaised("cattour_lbl_");
            OnPropertyRaised("numofracestour_lbl_");
            OnPropertyRaised("numofracerstour_lbl_");
            OnPropertyRaised("yeartour_lbl_");
            OnPropertyRaised("name_race_lbl");
            OnPropertyRaised("date_race_lbl");
            OnPropertyRaised("place_race_lbl");
            OnPropertyRaised("state_race_lbl");
            OnPropertyRaised("racestour_lbl_");
            OnPropertyRaised("racerpostour_lbl_");
            OnPropertyRaised("pos_tour_lbl");
            OnPropertyRaised("nameracer_tour_lbl");
            OnPropertyRaised("team_tour_lbl");
            OnPropertyRaised("score_tour_lbl");
            OnPropertyRaised("pos_RacerInRace");
            OnPropertyRaised("time_RacerInRace");
            OnPropertyRaised("pentime_RacerInRace");
            OnPropertyRaised("car_RacerInRace");
            OnPropertyRaised("finaltime_RacerInRace");
            OnPropertyRaised("Title_head");
            OnPropertyRaised("Place_head");
            OnPropertyRaised("Date_head");
            OnPropertyRaised("Length_head");
            OnPropertyRaised("Mdate_head");
            OnPropertyRaised("Status_head");
            OnPropertyRaised("Meas_head");
            OnPropertyRaised("choose_race_btn_");
            OnPropertyRaised("referencetime_lbl");
            OnPropertyRaised("racers_lbl");
            OnPropertyRaised("nametour_lbl_");
            OnPropertyRaised("Timekeeping_btn");
        }

        public Homescreen_Language()
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
    /// Interaction logic for Homescreen.xaml
    /// </summary>
    public partial class Homescreen : UserControl
    {

        //User LogUser = MainWindow.GetUser();
        public Homescreen()
        {
            InitializeComponent();

            LangCZ.IsChecked = false;
            LangEN.IsChecked = false;
            LangDE.IsChecked = false;

            // determinate language selected, mark menu object
            switch (Settings.GetValue("language", "unk"))
            {
                case "cs-CZ":
                    LangCZ.IsChecked = true;
                    break;
                case "":
                case "en-US":
                    LangEN.IsChecked = true;
                    break;
                case "de-DE":
                    LangDE.IsChecked = true;
                    break;
                default:
                    break;
            }

            //if(LogUser.)


            if ((int)(MainWindow.GetUser().Permission) == 4) { UserMenuItem.Visibility = Visibility.Hidden; }
            //UserMenuItem.Visibility = Visibility.Hidden;
            LogOut.Header = MainWindow.GetUser().Nick + " - Logout";
            MainWindow.RegisterLanguageHandler(Set_Language);

            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(1);
            timer.Tick += time_tick;
            timer.Start();
        }

        public void Set_Language()
        {
            var language_ = Resources["lang"] as Homescreen_Language;
            language_.Reload();
            Dispatcher.BeginInvoke(((Action)(() =>
            {
                //this.Title = language_.car_title;
            })));
        }

        #region UI_STUFFS

        private void Category_Menu_Click(object sender, RoutedEventArgs e)
        {
            Category CategoryWindow = new Category();
            CategoryWindow.Show();
        }

        private void Cars_Menu_Click(object sender, RoutedEventArgs e)
        {
            Cars CarsWindow = new Cars();
            CarsWindow.Show();
        }

        private void Brands_Menu_Click(object sender, RoutedEventArgs e)
        {
            Measurer_Brands BrandsWindow = new Measurer_Brands();
            BrandsWindow.Show();
        }

        private void Racer_Menu_Click(object sender, RoutedEventArgs e)
        {
            Racers RacerWindow = new Racers();
            RacerWindow.Show();
        }

        private void RacesItems_Menu_Click(object sender, RoutedEventArgs e)
        {
            Races RacesWindow = new Races();
            RacesWindow.Show();
        }

        private void Team_Menu_Click(object sender, RoutedEventArgs e)
        {
            Teams TeamWindow = new Teams();
            TeamWindow.Show();
        }

        private void Tournament_Item_Click(object sender, RoutedEventArgs e)
        {
            Tournaments TournamentWindow = new Tournaments();
            TournamentWindow.Show();
        }

        private void User_Menu_Click(object sender, RoutedEventArgs e)
        { 
            Users UserWindow = new Users();
            UserWindow.Show();
        }

        // once only used for data storing
        public class RaceSelectBrief
        {
            public Int32 ID { get; set; }
            public string Title { get; set; }
            public string Date { get; set; }
            public string Place { get; set; }
            public Int32 Length { get; set; }
            public string Status { get; set; }
            public string Category { get; set; }
            public string Laps { get; set; }
        };

        public void DisplayRaceTime(TimeSpan time)
        {
            //DateTime.Now.TimeOfDay - time;
        }

        private void Races_Menu_Click(object sender, RoutedEventArgs e)
        {
            // make sure overrendered areas are blocked
            tournaments_module.Visibility = Visibility.Hidden;
            race_module.Visibility = Visibility.Hidden;

            List<RaceSelectBrief> select_storage = new List<RaceSelectBrief>();

            try
            { 
                // load tata to be feeded into selector
                MySqlCommand cmd = Database.Database.CreateCommand(Database.Database.QueryStack["GetRaceDataSelect"]);
                MySqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    select_storage.Add(new RaceSelectBrief {
                        ID = rdr.GetInt32("ID_Race"),
                        Title = rdr.GetString("Name"),
                        Date = rdr.GetDateTime("Date").ToShortDateString(),
                        Place = rdr.GetString("Place"),
                        Length = rdr.GetInt32("Length"),
                        Status = rdr.GetString("Status_text")
                    });
                }
                rdr.Close();
            } catch {
                // promp error, app will remain silent
                return;
            }


            // bind data
            RaceList.ItemsSource = null;
            RaceList.ItemsSource = select_storage;

            // disable until selection ocured
            SelectRace.IsEnabled = false;

            // pop it
            RaceSelector.Visibility = Visibility.Visible;
        }

        private void RaceList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // determinate null state
            if (RaceList.SelectedValue == null)
            {
                SelectRace.IsEnabled = false;
                return;
            }

            SelectRace.IsEnabled = true;
        }

        private void SelectRace_Click(object sender, RoutedEventArgs e)
        {
            RaceSelector.Visibility = Visibility.Hidden;

            // contact proccesor to track race from now on
            (Application.Current.MainWindow as MainWindow).RequestOperation(
                new MainWindow.ProccesorComand {
                    operation = MainWindow.ProccesorOperations.UpdateRace,
                    args = (RaceList.SelectedValue as RaceSelectBrief).ID.ToString()
                }
            );
            //race_module.Visibility = Visibility.Visible;
        }

        private void Race_module_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            // not displayd, stop track data update
            if ((bool)e.NewValue == false) {
                (Application.Current.MainWindow as MainWindow).RequestOperation(
                    new MainWindow.ProccesorComand
                    {
                        operation = MainWindow.ProccesorOperations.ClearRace
                    }
                    );
            }
        }

        private static List<TournamentsView> TournamentsItems = new List<TournamentsView>();
        private void Tournaments_Menu_Click(object sender, RoutedEventArgs e)
        {
            // Load Tournaments for select one
            //tournaments_module.Visibility = Visibility.Hidden;
            //race_module.Visibility = Visibility.Hidden;

            // load data ofc

            TourSelector.Visibility = Visibility.Visible;

            /////////////////////////////////////////////////////////////////////////////////////
            ///
            TourList.ItemsSource = TournamentsItems;
            List<TournamentsView> _items = new List<TournamentsView>();
            MySqlCommand command;
            MySqlDataReader rdr = null;
            try
            {
                // load data base on filter
                command = Database.Database.CreateCommand(Database.Database.QueryStack["GetRacesInTourT"]);
                rdr = command.ExecuteReader();

                while (rdr.Read())
                {
                    _items.Add(new TournamentsView()
                    {
                        ID = rdr.GetInt32("ID_Tournament"),
                        Name = rdr.GetString("Tournament_name"),
                        Name_Short = rdr.GetString("Tournamen_shortname"),
                        Year = rdr.GetInt32("Year"),
                        CategoryID = rdr.GetInt32("FK_Category"),
                        CategoryName = rdr.GetString("Name")

                    });
                }
                rdr.Close();

                TournamentsItems.Clear();
                TournamentsItems = _items;
                Dispatcher.BeginInvoke(((Action)(() =>
                {
                    // rebind data value
                    TourList.ItemsSource = null;
                    TourList.ItemsSource = TournamentsItems;
                })));
            }
            catch
            {
                if (rdr != null && !rdr.IsClosed)
                    rdr.Close();
            }
            // pop it
            //TourSelector.Visibility = Visibility.Visible;

            // make sure overrendered areas are blocked
            tournaments_module.Visibility = Visibility.Hidden;
            race_module.Visibility = Visibility.Hidden;

        }


        private void LangCZ_Click(object sender, RoutedEventArgs e)
        {
            LangCZ.IsChecked = true;
            LangEN.IsChecked = false;
            LangDE.IsChecked = false;

            MainWindow.UpdateLanguageGlobal("cs-CZ");
        }

        private void LangEN_Click(object sender, RoutedEventArgs e)
        {
            LangCZ.IsChecked = false;
            LangEN.IsChecked = true;
            LangDE.IsChecked = false;

            MainWindow.UpdateLanguageGlobal("en-US");
        }

        private void LangDE_Click(object sender, RoutedEventArgs e)
        {
            LangCZ.IsChecked = false;
            LangEN.IsChecked = false;
            LangDE.IsChecked = true;

            MainWindow.UpdateLanguageGlobal("de-DE");
        }

        private void CarotSet(object sender, RoutedEventArgs e)
        {
            if (EditingCommands.MoveToLineEnd.CanExecute(null, sender as IInputElement))
            {
                EditingCommands.MoveToLineEnd.Execute(null, sender as IInputElement);
            }
        }

        #endregion

        private void LogOut_Click(object sender, RoutedEventArgs e)
        {
            // in case we logout, clear main module user hodler
            MainWindow.GetUser() = null;

            Regularity_Rally.Control.MainWindow hanlder_object = Application.Current.MainWindow as MainWindow;
            (hanlder_object).Dispatcher.BeginInvoke(((Action)(() =>
            {
                hanlder_object.SetStage(ApplicationStage.Login);
            })));
        }

        private void ChangeState_btn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void SaveRace_btn_Click(object sender, RoutedEventArgs e)
        {

        }

        #region DataDisplay

        Race RaceTracker = null;
        public Race.RaceTime CreateRaceTracker(Int32 race_id)
        {
            RaceTracker = new Race(race_id);
            return RaceTracker.GetRaceTimes();
        }

       
        void time_tick(object sender, EventArgs e)
        {
            Time_lbl.Content = DateTime.Now.ToString("HH:mm:ss.fff");
        }

        // this fnc in meant to be called from main proccesing loot that is responcible for data refresh
        // internal condition to determiante data state after valies is refresed has to be presented
        // method is dispoces from another thread ... again
        RaceSelectBrief RaceData = new RaceSelectBrief();
        public void RefreshRaceData(Int32 race_id)
        {
            // select race sumary, can use brief as befor
            RaceSelectBrief race = new RaceSelectBrief();
            try
            {
                
                // load tata to be feeded into selector
                MySqlCommand cmd = Database.Database.CreateCommand(string.Format(Database.Database.QueryStack["GetRaceDataCond"], race_id));
                MySqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    race.ID = rdr.GetInt32("ID_Race");
                    race.Title = rdr.GetString("Name");
                    race.Date = rdr.GetDateTime("Date").ToShortDateString();
                    race.Place = rdr.GetString("Place");
                    race.Length = rdr.GetInt32("Length");
                    race.Status = rdr.GetString("Status_text");
                    race.Category = rdr.GetString("Name");
                    race.Laps = rdr.GetString("NumOfLaps");
                }
                rdr.Close();
            }
            catch
            {
                // data load terminated, continue to records
            }
            finally
            {
                Dispatcher.BeginInvoke(((Action)(() =>
                {
                    Race_RaceName.Text = race.Title;
                    Place_txt_.Text = race.Place;
                    Lenght_txt_.Text = race.Length.ToString();
                    State_txt_.Text = race.Status;
                    Category_txt_.Text = race.Category;
                    //if(race.Status != "Připraveno" || race.Status != "Probíhá") { TimekeepingLapData.Visibility = Visibility.Hidden; }
                    RaceData = race;
                    Laps_txt_.Text = race.Laps;
                })));
            }

            if (RaceTracker != null && RaceTracker.LoadLaps())
            {
                
                //RaceTracker.GetLaps();
                Dispatcher.BeginInvoke(((Action)(() =>
                {
                    RacersInRace.ItemsSource = null;
                    RacersInRace.ItemsSource = RaceTracker.GetSummary();
                })));
            }

            // send ui update to local thread
            Dispatcher.BeginInvoke(((Action)(() =>
            {
                race_module.Visibility = Visibility.Visible;
            })));

            if (TimekeepingWindow != null)
            {
                TimekeepingWindow.UpdateDataList(ref RaceTracker);
            }
        }

        Timekeeping TimekeepingWindow = null;
        private void TimekeepingLapData_Click(object sender, RoutedEventArgs e)
        {
            
            
            if (TimekeepingWindow == null)
            {
                TimekeepingWindow = new Timekeeping(ref RaceTracker);
            }
            try
            {
                TimekeepingWindow.Show();
            }
            catch
            {
                TimekeepingWindow = new Timekeeping(ref RaceTracker);
                TimekeepingWindow.Show();
            }
        }


        // holder
        DeviceForm deviceocnfig = null;
        private void DeviceForm_Click(object sender, RoutedEventArgs e)
        {
            //if()
            if (deviceocnfig != null)
            {
                deviceocnfig.Close();
                deviceocnfig = null;
            }
            deviceocnfig = new DeviceForm();
            deviceocnfig.Show();
        }


        #endregion

        public static TournamentsView LastSelectedTournament = null;
        private void TourList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TourList.SelectedItem != null)
            {
                LastSelectedTournament = TourList.SelectedItem as TournamentsView;
                //Console.WriteLine(LastTournamentSelectedItem.ID);
            }
        }

        private void SelectTour_Click(object sender, RoutedEventArgs e)
        {
            TourSelector.Visibility = Visibility.Hidden;
            UpdateTournamentModule(LastSelectedTournament.ID);
            NameOfTourInput.Text = LastSelectedTournament.Name;
            ShortNameInput.Text = LastSelectedTournament.Name_Short;
            CategoryInput.Text = LastSelectedTournament.CategoryName;
            YearInput.Text = LastSelectedTournament.Year.ToString();

            
            tournaments_module.Visibility = Visibility.Visible;
        }

        Tournament TClass = null;
        private static List<RacesInTourData> RacerInRaceItems = new List<RacesInTourData>();
        private void UpdateTournamentModule(int _id)
        {
            RacesInTour.ItemsSource = RacerInRaceItems;

            TClass = new Tournament(_id);
            List<RacesInTourData> _items = new List<RacesInTourData>();
            MySqlDataReader rdra = null;
            try
            {
                
                MySqlCommand cmd = Database.Database.CreateCommand(string.Format(Database.Database.QueryStack["RacesInTourHomeS"], _id));
                rdra = cmd.ExecuteReader();

                while (rdra.Read())
                {
                    _items.Add(new RacesInTourData
                    {
                        ID = rdra.GetInt32("ID_Race"),
                        RaceName = rdra.GetString("Name"),
                        Date = rdra.GetDateTime("Date").ToShortDateString(),
                        Length = rdra.GetInt32("Length"),
                        Place = rdra.GetString("Place"),
                        Time = rdra.GetString("Time"),
                        NumOfLaps = rdra.GetInt32("NumOfLaps"),
                        StatusText = rdra.GetString("Status_text")
                    });
                }
                rdra.Close();

                RacerInRaceItems.Clear();
                RacerInRaceItems = _items;
                Dispatcher.BeginInvoke(((Action)(() =>
                {
                    RacesInTour.ItemsSource = null;
                    RacesInTour.ItemsSource = RacerInRaceItems;

                })));
                Dispatcher.BeginInvoke(((Action)(() =>
                {
                    RacersInTour.ItemsSource = null;
                    RacersInTour.ItemsSource = TClass.GetPositionInTour(); 
                })));
            }
            catch
            {
                if (rdra != null && !rdra.IsClosed)
                    rdra.Close();
            }
        }


        private void Reporter_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            Reporter_MainWindow RepWindow = new Reporter_MainWindow();
            RepWindow.Show();
        }

        private void Paddock_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            Paddock PadWindow = new Paddock();
            PadWindow.Show();
        }
    }
}
