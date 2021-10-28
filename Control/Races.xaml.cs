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


namespace Regularity_Rally.Control
{
    #region Language
    public class Races_Language : INotifyPropertyChanged
    {
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
        public string AddRace_
        {
            get { return Properties.Resources.mwi_AddRace_; }
        }
        public string CloseRace_
        {
            get { return Properties.Resources.mwi_CloseRace_; }
        }
        public string race_title
        {
            get { return Properties.Resources.mwi_race_title; }
        }
        public string NumOfLaps_head
        {
            get { return Properties.Resources.race_laps_race_label__; }
        }
        public string print_btn_
        {
            get { return Properties.Resources.print_btn_; }
        }

        public Races_Language()
        {

        }

        public void Reload()
        {
            OnPropertyRaised("Title_head");
            OnPropertyRaised("print_btn_");
            OnPropertyRaised("Place_head");
            OnPropertyRaised("Date_head");
            OnPropertyRaised("Length_head");
            OnPropertyRaised("Mdate_head");
            OnPropertyRaised("Status_head");
            OnPropertyRaised("Meas_head");
            OnPropertyRaised("race_title");
            OnPropertyRaised("NumOfLaps_head");
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
    /// Interaction logic for Races.xaml
    /// </summary>
    /// 

    public class RacesView
    {
        public int ID { get; set; }
        public int StatusID { get; set; }
        public int RaceNote { get; set; }
        public int Category { get; set; }
        public int RacerCar { get; set; }
        public string Status { get; set; }
        public int MeasID { get; set; }
        public string Meas { get; set; }
        public string Title { get; set; }
        public string Place { get; set; }
        public string Date { get; set; }
        public float Length { get; set; }
        public string Mdate { get; set; }
        public int NumOfLaps { get; set; }
        public string Time { get; set; }
        public string StartTime { get; set; }
        public string TourID { get; set; }

        public string Identifier
        {
            get
            {
                return string.Format("{0}", this.Title);
            }
        }

        public bool Compare(RacesView _ref)
        {
            if (_ref == null)
                return false;

            return (_ref.Title == this.Title && _ref.Place == this.Title && _ref.Date == this.Date && _ref.Length == this.Length && _ref.Mdate == this.Mdate);
        }

    }


    public partial class Races : Window
    {
        private static List<RacesView> RacesItems = new List<RacesView>();
        private Add_new_race UpdateWindow = null;
        public static RacesView LastRacesSelectedItem = null;
        User LogUser = MainWindow.GetUser();

        public Races()
        {

            InitializeComponent();
            this.Title = (Resources["lang"] as Races_Language).race_title;
            MainWindow.RegisterLanguageHandler(SetLanguage);
            UpdateRacesList(string.Empty);

            if ((LogUser.Permission) < UserPermission.Update) { AddRace_btn.Visibility = Visibility.Hidden; }
        }

        protected override void OnClosed(EventArgs e)
        {
            Close_btn_Click(null, null);
            MainWindow.UnRegisterLanguageHandler(SetLanguage);
            base.OnClosed(e);
        }

        public void SetLanguage()
        {
            var language_ = Resources["lang"] as Races_Language;
            language_.Reload();
            Dispatcher.BeginInvoke(((Action)(() =>
            {
                this.Title = language_.race_title;
            })));
        }

        public void Refresh()
        {
            Dispatcher.BeginInvoke(((Action)(() =>
            {
                UpdateRacesList(string.Empty);
            })));
        }

        public List<EventHandler> RacesSelectionUpdateNotify = new List<EventHandler>();

        private void RacesList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(RaceList.SelectedItem != null)
            {
                RacesView _t = LastRacesSelectedItem;
                LastRacesSelectedItem = RaceList.SelectedItem as RacesView;
                if (_t != null)
                {
                    if (LastRacesSelectedItem.ID != _t.ID)
                    {
                        foreach (EventHandler _event in RacesSelectionUpdateNotify)
                        {
                            if (_event != null)
                                _event.BeginInvoke(null, null, null, null);
                        }
                    }
                }
            }
        }

        private void UpdateRacesList(string filter)
        {
            // set procesing state
            Dispatcher.BeginInvoke(((Action)(() =>
            {
                //action_search.Visibility = Visibility.Visible;
            })));

            RaceList.ItemsSource = RacesItems;
            List<RacesView> _items = new List<RacesView>();
            //int SelectedIndex = 0;
            //List<RacesView> select_storage = new List<RacesView>();
            MySqlDataReader rdr = null;
            //Int32 HelpID = 0;

            try
            {
                // load tata to be feeded into selector
                MySqlCommand cmd = Database.Database.CreateCommand(Database.Database.QueryStack["GetRaceDataSelect"]);
                rdr = cmd.ExecuteReader();
                //string TourHelp = "None";
                //int colIndex;

                while (rdr.Read())
                {
                    RacesView DataToList = new RacesView();

                    DataToList.ID = rdr.GetInt32("ID_Race");
                    //DataToList.TourID = (!rdr.IsDBNull(rdr.GetOrdinal("FK_Tour"))) ? rdr.GetString("FK_Tour") : "NULL";

                    DataToList.StatusID = rdr.GetInt32("FK_Race_Status");
                    DataToList.RaceNote = rdr.GetInt32("FK_Race_Note");
                    DataToList.Category = rdr.GetInt32("FK_Category");
                    DataToList.MeasID = rdr.GetInt32("FK_Meas_company");
                    DataToList.Title = rdr.GetString("Name");
                    DataToList.Date = rdr.GetDateTime("Date").ToShortDateString();
                    DataToList.Place = rdr.GetString("Place");
                    DataToList.Length = rdr.GetInt32("Length");
                    DataToList.Time = rdr.GetString("Time").ToString();
                    DataToList.StartTime = rdr.GetString("Start_Time");
                    DataToList.NumOfLaps = rdr.GetInt32("NumOfLaps");
                    DataToList.Status = rdr.GetString("Status_text");

                    _items.Add(DataToList);
                }
                rdr.Close();
                RacesItems.Clear();
                RacesItems = _items;
                Dispatcher.BeginInvoke(((Action)(() =>
                {
                    RaceList.ItemsSource = null;
                    RaceList.ItemsSource = RacesItems;
                    //RaceList.SelectedItem = SelectedIndex;
                })));
            }
            catch
            {
                if (rdr != null && !rdr.IsClosed)
                    rdr.Close();
            }
        }

        private void AddRace_btn_Click(object sender, RoutedEventArgs e)
        {
            if (this.UpdateWindow != null)
                this.UpdateWindow.Close();

            this.UpdateWindow = null;
            this.UpdateWindow = new Add_new_race(this, RacesOperationType.Add, null);
        }

        private void RaceList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (this.UpdateWindow != null)
                this.UpdateWindow.Close();

            this.UpdateWindow = null;
            this.UpdateWindow = new Add_new_race(this, RacesOperationType.Update, LastRacesSelectedItem);
        }

        private void Close_btn_Click(object sender, RoutedEventArgs e)
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
            print.Print(PrintManager.PrintType.Races);
        }
    }
}
