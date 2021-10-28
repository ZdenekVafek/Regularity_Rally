using System;
using System.Collections.Generic;
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

namespace Regularity_Rally.Control
{
    #region Language
    public class TourDetailReporter_Language
    {
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

        public void Reload()
        {
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
        }

        public TourDetailReporter_Language()
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
    /// Interaction logic for TourDetailReporter.xaml
    /// </summary>
    /// 
    public class TournamentData
    {
        public Int32 ID { get; set; }
        public string Tournament_name { get; set; }
        public string Tournament_shortname { get; set; }
        public string Year { get; set; }
        public string CatName { get; set; }
    }


    public partial class TourDetailReporter : Window
    {
        int DataRaceID = 0;
        public TourDetailReporter(Int32 RaceID)
        {
            InitializeComponent();
            this.DataRaceID = RaceID;
            UpdateTournamentModule(this.DataRaceID);
        }

        private static List<TournamentData> TourData = new List<TournamentData>(); 
        Tournament TClass = null;
        private static List<RacesInTourData> RacerInRaceItems = new List<RacesInTourData>();
        private void UpdateTournamentModule(int _id)
        {
            RacesInTour.ItemsSource = RacerInRaceItems;
            int TourID = 0;
            //GetTourID
            List<TournamentData> SelectedTourData = new List<TournamentData>();
            try
            {
                List<TournamentData> _itemsTour = new List<TournamentData>();
                MySqlDataReader rdr = null;
                MySqlCommand cmdTour = Database.Database.CreateCommand(string.Format(Database.Database.QueryStack["GetTourID"], _id));
                rdr = cmdTour.ExecuteReader();
                while( rdr.Read())
                {
                    _itemsTour.Add(new TournamentData
                    {
                        CatName = rdr.GetString("Name"),
                        Tournament_name = rdr.GetString("Tournament_name"),
                        Tournament_shortname = rdr.GetString("Tournamen_shortname"),
                        Year = rdr.GetString("Year"),
                        ID = rdr.GetInt32("ID_Tournament")
                    });
                }
                rdr.Close();
                TourData.Clear();
                TourData = _itemsTour;
            }
            catch { }

            foreach(TournamentData _data in TourData)
            {
                NameOfTourInput.Text = _data.Tournament_name;
                ShortNameInput.Text = _data.Tournament_shortname;
                CategoryInput.Text = _data.CatName;
                YearInput.Text = _data.Year.ToString();
                TourID = _data.ID;
                this.Title = _data.Tournament_name;
            }

            TClass = new Tournament(TourID);
            List<RacesInTourData> _items = new List<RacesInTourData>();
            MySqlDataReader rdra = null;
            try
            {

                MySqlCommand cmd = Database.Database.CreateCommand(string.Format(Database.Database.QueryStack["RacesInTourHomeS"], TourID));
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
    }
}
