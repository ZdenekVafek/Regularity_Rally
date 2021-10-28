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
    public class Reporter_MainWindow_Language : INotifyPropertyChanged
    {
        public string Races_MenuItemHeader
        {
            get { return Properties.Resources.RepMW_Events_Races_MenuItemHeader; }
        }
        public string Settings_Language_MenuItemHeader
        {
            get { return Properties.Resources.hs_Settings_Language_MenuItemHeader; }
        }
        public string Logout_MenuItemHeader
        {
            get { return Properties.Resources.hs_Logout_MenuItemHeader; }
        }
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
        public string StateOfRace_label_
        {
            get { return Regularity_Rally.Properties.Resources.mwrs_state_lbl; }
        }
        public string LapsOfRace_label_
        {
            get { return Regularity_Rally.Properties.Resources.hs_LapsOfRace_label_; }
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
        public string name_racer_lbl
        {
            get { return Regularity_Rally.Properties.Resources.mwrs_name_racer_lbl; }
        }
        public string name_team_lbl
        {
            get { return Regularity_Rally.Properties.Resources.mwrs_name_team_lbl; }
        }
        public string referencetime_lbl
        {
            get { return Regularity_Rally.Properties.Resources.mwi_referencetime_lbl; }
        }
        public string TimeKeeping_label_
        {
            get { return Regularity_Rally.Properties.Resources.TK_TimeKeeping_label_; }
        }
        public string PenalizationKeeping_label_
        {
            get { return Regularity_Rally.Properties.Resources.TK_PenalizationKeeping_label_; }
        }
        public string FinalTimeKeeping_label_
        {
            get { return Regularity_Rally.Properties.Resources.TK_FinalTimeKeeping_label_; }
        }
        public string NoteKeeping_label_
        {
            get { return Regularity_Rally.Properties.Resources.TK_NoteKeeping_label_; }
        }
        public string LapKeeping_header
        {
            get { return Regularity_Rally.Properties.Resources.TK_LapKeeping_header; }
        }
        public string Positionkeeper_header
        {
            get { return Regularity_Rally.Properties.Resources.TK_Positionkeeper_header; }
        }
        public string TimeKeeper_header
        {
            get { return Regularity_Rally.Properties.Resources.TK_TimeKeeper_header; }
        }
        public string AddRecord_btn
        {
            get { return Regularity_Rally.Properties.Resources.TK_AddRecord_btn; }
        }
        public string timekeeping_title
        {
            get { return Regularity_Rally.Properties.Resources.TK_timekeeping_title; }
        }
        public string Car_label_
        {
            get { return Regularity_Rally.Properties.Resources.TK_Car_label_; }
        }
        public string Fullname_header
        {
            get { return Regularity_Rally.Properties.Resources.TK_Fullname_header; }
        }
        public string choose_race_btn_
        {
            get { return Regularity_Rally.Properties.Resources.TK_choose_race_btn_; }
        }
        public string detail_racer_btn_
        {
            get { return Regularity_Rally.Properties.Resources.TK_detail_racer_btn_; }
        }
        public string detail_tour_btn_
        {
            get { return Regularity_Rally.Properties.Resources.TK_detail_tour_btn_; }
        }
        public string Paddock_Title
        {
            get { return Regularity_Rally.Properties.Resources.TK_Paddock_Title; }
        }
        public string Rep_Title
        {
            get { return Regularity_Rally.Properties.Resources.TK_Rep_Title; }
        }
        public string Title_head
        {
            get { return Regularity_Rally.Properties.Resources.mwi_Title_head; }
        }
        public string Place_head
        {
            get { return Regularity_Rally.Properties.Resources.mwi_Place_head; }
        }
        public string Date_head
        {
            get { return Regularity_Rally.Properties.Resources.mwi_Date_head; }
        }
        public string Length_head
        {
            get { return Regularity_Rally.Properties.Resources.mwi_Length_head; }
        }
        public string Status_head
        {
            get { return Regularity_Rally.Properties.Resources.mwi_Status_head; }
        }


        public void Reload()
        {
            OnPropertyRaised("Races_MenuItemHeader");
            OnPropertyRaised("Title_head");
            OnPropertyRaised("Place_head");
            OnPropertyRaised("Date_head");
            OnPropertyRaised("Status_head");
            OnPropertyRaised("Length_head");
            OnPropertyRaised("Rep_Title");
            OnPropertyRaised("Paddock_Title");
            OnPropertyRaised("detail_racer_btn_");
            OnPropertyRaised("detail_tour_btn_");
            OnPropertyRaised("referencetime_lbl");
            OnPropertyRaised("name_racer_lbl");
            OnPropertyRaised("name_team_lbl");
            OnPropertyRaised("pos_RacerInRace");
            OnPropertyRaised("time_RacerInRace");
            OnPropertyRaised("pentime_RacerInRace");
            OnPropertyRaised("car_RacerInRace");
            OnPropertyRaised("finaltime_RacerInRace");
            OnPropertyRaised("Settings_Language_MenuItemHeader");
            OnPropertyRaised("Logout_MenuItemHeader");
            OnPropertyRaised("PlaceOfRace_label_");
            OnPropertyRaised("CategoryOfRace_label_");
            OnPropertyRaised("LengthOfRace_label_");
            OnPropertyRaised("StateOfRace_label_");
            OnPropertyRaised("LapsOfRace_label_");
            OnPropertyRaised("TimeKeeping_label_");
            OnPropertyRaised("PenalizationKeeping_label_");
            OnPropertyRaised("FinalTimeKeeping_label_");
            OnPropertyRaised("NoteKeeping_label_");
            OnPropertyRaised("LapKeeping_header");
            OnPropertyRaised("Positionkeeper_header");
            OnPropertyRaised("AddRecord_btn");
            OnPropertyRaised("timekeeping_title");
            OnPropertyRaised("Car_label_");
            OnPropertyRaised("Fullname_header");
        }

        public Reporter_MainWindow_Language()
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
    /// Interaction logic for Reporter_MainWindow.xaml
    /// </summary>
    public partial class Reporter_MainWindow : Window
    {
        public Reporter_MainWindow()
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

            LogOut.Header = MainWindow.GetUser().Nick + " - Logout";
            MainWindow.RegisterLanguageHandler(Set_Language);
            this.Title = (Resources["lang"] as Reporter_MainWindow_Language).Rep_Title;
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(1);
            timer.Tick += time_tick;
            timer.Start();
        }

        void time_tick(object sender, EventArgs e)
        {
            Time_lbl.Content = DateTime.Now.ToString("HH:mm:ss.fff");
        }

        public void Set_Language()
        {
            var language_ = Resources["lang"] as Reporter_MainWindow_Language;
            language_.Reload();
            Dispatcher.BeginInvoke(((Action)(() =>
            {
                this.Title = language_.Rep_Title;
            })));
        }

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

        private void ChooseRace_Menu_Click(object sender, RoutedEventArgs e)
        {
            List<Homescreen.RaceSelectBrief> select_storage = new List<Homescreen.RaceSelectBrief>();

            try
            {
                // load tata to be feeded into selector
                MySqlCommand cmd = Database.Database.CreateCommand(Database.Database.QueryStack["GetRaceDataSelect"]); //GetRaceDataSelect
                MySqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    select_storage.Add(new Homescreen.RaceSelectBrief
                    {
                        ID = rdr.GetInt32("ID_Race"),
                        Title = rdr.GetString("Name"),
                        Date = rdr.GetDateTime("Date").ToShortDateString(),
                        Place = rdr.GetString("Place"),
                        Length = rdr.GetInt32("Length"),
                        Status = rdr.GetString("Status_text"),
                        Laps = rdr.GetString("NumOfLaps")
                    });
                }
                rdr.Close();
            }
            catch
            {
                return;
            }


            // bind data
            RaceList.ItemsSource = null;
            RaceList.ItemsSource = select_storage;

            // disable until selection ocured
            //SelectRace.IsEnabled = false;

            // pop it
            RaceSelector.Visibility = Visibility.Visible;
            Reporter.Visibility = Visibility.Hidden;
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

        private void SelectRace_Click(object sender, RoutedEventArgs e)
        {
            if(SelectedRace != null)
            {
                RaceTitle_RepMW.Text = SelectedRace.Title;
                var language_ = Resources["lang"] as Reporter_MainWindow_Language;
                language_.Reload();
                Dispatcher.BeginInvoke(((Action)(() =>
                {
                    LAPSTitle_RepMW.Text = language_.LapsOfRace_label_;
                })));


                Place_txt_.Text = SelectedRace.Place;
                Lenght_txt_.Text = SelectedRace.Length.ToString();
                State_txt_.Text = SelectedRace.Status;
                Laps_txt_.Text = SelectedRace.Laps;

                CreateRaceTracker(SelectedRace.ID);
                UpdateRacerFinalList();
                UpdateDataList();
                RaceSelector.Visibility = Visibility.Hidden;
                Reporter.Visibility = Visibility.Visible;
            }
        }

        public Homescreen.RaceSelectBrief SelectedRace = null; // new List<Homescreen.RaceSelectBrief>();
        private void RaceList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (RaceList.SelectedItems != null)
            {
                SelectedRace = RaceList.SelectedItem as Homescreen.RaceSelectBrief;
            }
        }

        Race RaceTracker = null;
        public Race.RaceTime CreateRaceTracker(Int32 race_id)
        {
            RaceTracker = new Race(race_id);
            return RaceTracker.GetRaceTimes();
        }

        private void UpdateRacerFinalList()
        {
            if (RaceTracker != null && RaceTracker.LoadLaps())
            {

                //RaceTracker.GetLaps();
                Dispatcher.BeginInvoke(((Action)(() =>
                {
                    RacersInRace.ItemsSource = null;
                    RacersInRace.ItemsSource = RaceTracker.GetSummary();
                })));
            }
        }

        public void UpdateDataList()
        {
            List<Race.RaceLaps> dd = RaceTracker.GetLaps();
            Dispatcher.BeginInvoke(((Action)(() =>
            {
                RacersInRaceLaps.ItemsSource = null;
                RacersInRaceLaps.ItemsSource = dd;
            })));
        }

        private void RacerDetail_Click(object sender, RoutedEventArgs e)
        {
            DetailRacerReporter windowRacer = new DetailRacerReporter(SelectedRace.ID);
            windowRacer.Show();
        }

        private void TournamentView_Click(object sender, RoutedEventArgs e)
        {
            TourDetailReporter windowTour = new TourDetailReporter(SelectedRace.ID);
            windowTour.Show();
        }
    }
}
