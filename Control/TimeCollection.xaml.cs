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
    public class TimeCollection_Language : INotifyPropertyChanged
    {
        public string timecollection
        {
            get { return Properties.Resources.TC_timecollection; }
        }
        public string Racer_label_
        {
            get { return Properties.Resources.TC_Racer_label_; }
        }
        public string TimeKeeping_label_
        {
            get { return Properties.Resources.TK_TimeKeeping_label_; }
        }
        public string PenalizationKeeping_label_
        {
            get { return Properties.Resources.TK_PenalizationKeeping_label_; }
        }
        public string FinalTimeKeeping_label_
        {
            get { return Properties.Resources.TK_FinalTimeKeeping_label_; }
        }
        public string NoteKeeping_label_
        {
            get { return Properties.Resources.TK_NoteKeeping_label_; }
        }
        public string LapKeeping_header
        {
            get { return Properties.Resources.TK_LapKeeping_header; }
        }
        public string Positionkeeper_header
        {
            get { return Properties.Resources.TK_Positionkeeper_header; }
        }
        public string TimeKeeper_header
        {
            get { return Properties.Resources.TK_TimeKeeper_header; }
        }
        public string AddRecord_btn
        {
            get { return Properties.Resources.TK_AddRecord_btn; }
        }
        public string timekeeping_title
        {
            get { return Properties.Resources.TK_timekeeping_title; }
        }
        public string Car_label_
        {
            get { return Properties.Resources.TK_Car_label_; }
        }
        public string Fullname_header
        {
            get { return Properties.Resources.TK_Fullname_header; }
        }
        public string LapKeeping_label_
        {
            get { return Properties.Resources.TK_LapKeeping_label_; }
        }
        public string SelectAll_label_
        {
            get { return Properties.Resources.TK_SelectAll_label_; }
        }



        public void Reload()
        {
            OnPropertyRaised("timecollection"); 
            OnPropertyRaised("LapKeeping_label_"); 
            OnPropertyRaised("Racer_label_");
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
            OnPropertyRaised("SelectAll_label_");
        }

        public TimeCollection_Language()
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
    /// Interaction logic for TimeCollection.xaml
    /// </summary>
    /// 

    

    public partial class TimeCollection : Window
    {
        private Race _race = null;
        private List<Race.Racer> racers = null;
        private List<RacerDisplay> racers_ext = null;
        TimeSpan RecorderTime;
        private Race.RaceLaps recorder_overall;
        public class RacerDisplay
        {
            public string FullName { get; set; }
            public Int32 ID { get; set; }
        }

        public TimeCollection(ref Race Tracker, TimeSpan recorder_time)
        {
            this._race = Tracker;

            racers_ext = new List<RacerDisplay>();

            // dialog is independent
            racers = Tracker.GetRacersInRace();

            int pom = 0;
            foreach (Race.Racer rac in racers)
            {
                if (rac.RacerStatus != 1){++pom;}
            }

            if(pom > 0)
            {
                SelectAll.Visibility = Visibility.Visible;
                CheckLabel.Visibility = Visibility.Visible;
            }

            foreach (Race.Racer rac in racers)
            {
                if(rac.RacerStatus == 1 || rac.RacerStatus == 2)
                {
                    racers_ext.Add(
                        new RacerDisplay
                        {
                            FullName = string.Format("{0} {1} #{2}", rac.FName, rac.LName, rac.StartNumber),
                            ID = rac.ID
                        }
                    );
                }
            }

            // XAML
            InitializeComponent();

            // display formt to tolerate
            Penalization_txt_.Text = TimeSpan.Zero.ToString();

            SelectRacerTimeCollection.ItemsSource = racers_ext;

            RecorderTime = recorder_time;
            RecTimeSt.Content = "Recorder time: " + recorder_time.ToString();
        }

        public void SetLanguage()
        {
            var language_ = Resources["lang"] as TimeCollection_Language;
            language_.Reload();
            Dispatcher.BeginInvoke(((Action)(() =>
            {
                this.Title = language_.timecollection;
            })));
        }

        private void CarotSet(object sender, RoutedEventArgs e)
        {
            if (EditingCommands.MoveToLineEnd.CanExecute(null, sender as IInputElement))
            {
                EditingCommands.MoveToLineEnd.Execute(null, sender as IInputElement);
            }
        }

        private void AddRecord_btn_Click(object sender, RoutedEventArgs e)
        {
            recorder_overall.Note = Note_txt_.Text;
            TimeSpan result;
            if (TimeSpan.TryParse(Penalization_txt_.Text, out result))
            {
                recorder_overall.PenTime = result;
            }
            else
            {
                // value cant be parsed promp?
            }
            recorder_overall.FinalTime = (recorder_overall.Time + recorder_overall.PenTime);
            Int32 msrID = 0;
            // get record from MeasCom_Meassurer
            try
            {
                MySqlCommand cmd = Database.Database.CreateCommand(string.Format(Database.Database.QueryStack["GetMeasMeasur"], _race.RaceID, MainWindow.GetUser().ID));
                MySqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    msrID = rdr.GetInt32("ID_MeasCom_Measurer");
                }
                rdr.Close();
            }
            catch { }

            if(recorder_overall.Lap == 0)
            {
                try
                {
                    MySqlCommand ChangeRacerState_cmd = Database.Database.CreateCommand(string.Format(Database.Database.QueryStack["ChangeRacerStatus"], 2, SelectedUser.ID));
                    int retCmd = ChangeRacerState_cmd.ExecuteNonQuery();
                }
                catch { }
            }

            if (recorder_overall.Lap > _race.GetNumberOfLapsofRace()) 
            {
                try
                {
                    MySqlCommand ChangeRacerState_cmd = Database.Database.CreateCommand(string.Format(Database.Database.QueryStack["ChangeRacerStatus"], 6, SelectedUser.ID));
                    int retCmd = ChangeRacerState_cmd.ExecuteNonQuery();
                }
                catch { }
            }
            else
            {
                long InsertedID = 0;
                try
                {
                    MySqlCommand cmd = Database.Database.CreateCommand(string.Format(Database.Database.QueryStack["InsertLap"],
                        _race.RaceID,
                        msrID,
                        SelectedUser.ID,
                        recorder_overall.Lap,
                        recorder_overall.Time,
                        recorder_overall.PenTime
                    ));
                    cmd.ExecuteNonQuery();
                    InsertedID = cmd.LastInsertedId;
                }
                catch (Exception ep) { Console.WriteLine(ep.Message); }

                if (InsertedID != 0 && recorder_overall.Note != string.Empty)
                {
                    try
                    {
                        MySqlCommand cmd = Database.Database.CreateCommand(string.Format(Database.Database.QueryStack["InsertLapNote"],
                            InsertedID,
                            recorder_overall.Note
                        ));
                        cmd.ExecuteNonQuery();
                    }
                    catch { }
                }
            }

            // cutoff convert of final lap time
            //string _lap_time = string.Format("{0:D2}{1:D2}", recorder_overall.FinalTime.Minutes, recorder_overall.FinalTime.Seconds);
            string _lap_time = recorder_overall.FinalTime.ToString("mmss");

            // send port name into connector
            (Application.Current.MainWindow as MainWindow).RequestOperation(
                new MainWindow.ProccesorComand
                {
                    operation = MainWindow.ProccesorOperations.SendTableData,
                    args = _lap_time,
                }
            );

            // que to refresh parent data
            (Application.Current.MainWindow as MainWindow).RequestOperation(
                new MainWindow.ProccesorComand
                {
                    operation = MainWindow.ProccesorOperations.RaceTick
                }
            );

            // close this promp
            Close();
        }

        private RacerDisplay SelectedUser = null;
        private void SelectRacerTimeCollection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // detect racer id
            RacerDisplay it = (SelectRacerTimeCollection.SelectedItem as RacerDisplay);

            if (it == null /*&& SelectAll.IsChecked == false*/)
                return;

            SelectedUser = it;

            //if(SelectAll.IsChecked == true)
            //{
            //    //only when all racers start in same time
            //    foreach(RacerDisplay list in racers_ext)
            //    {
            //        Race.RaceLaps record1 = _race.CalculateLap(RecorderTime, list.ID);

            //        //record.Note = Note_txt_.Text;

            //        TimeSpan result1;
            //        if (TimeSpan.TryParse(Penalization_txt_.Text, out result1))
            //        {
            //            record1.PenTime = result1;
            //        }
            //        else
            //        {
            //            // value cant be parsed promp?
            //        }
            //        record1.Time = record1.Time.Duration();
            //        record1.FinalTime = (record1.Time + record1.PenTime);

            //        // fetch data back to ui
            //        Time_txt_.Text = record1.Time.ToString();
            //        Lap_txt_.Text = record1.Lap.ToString();
            //        FinalTime_txt_.Text = record1.FinalTime.ToString();

            //        recorder_overall = record1;
            //    }
            //}
            //else
            //{
                Race.RaceLaps record = _race.CalculateLap(RecorderTime, it.ID);

                //record.Note = Note_txt_.Text;

                TimeSpan result;
                if (TimeSpan.TryParse(Penalization_txt_.Text, out result))
                {
                    record.PenTime = result;
                }
                else
                {
                    // value cant be parsed promp?
                }
                record.Time = record.Time.Duration();
                record.FinalTime = (record.Time + record.PenTime);

                // fetch data back to ui
                Time_txt_.Text = record.Time.ToString();
                Lap_txt_.Text = record.Lap.ToString();
                FinalTime_txt_.Text = record.FinalTime.ToString();

                recorder_overall = record;
            //}
        }
    }
}
