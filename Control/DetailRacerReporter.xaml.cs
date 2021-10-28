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
    public class DetailRacerReporter_Language : INotifyPropertyChanged
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
        public string NationalityRacer_head
        {
            get { return Properties.Resources.racer_NationalityRacer_head; }
        }
        public string TeamRacer_head
        {
            get { return Properties.Resources.racer_TeamRacer_head; }
        }
        public string choose_race_btn_
        {
            get { return Properties.Resources.mwi_choose_race_btn_; }
        }
        public string name_nameRacer_head
        {
            get { return Properties.Resources.mwi_name_nameRacer_head; }
        }
        public string Born_nameRacer_head
        {
            get { return Properties.Resources.mwi_Born_nameRacer_head; }
        }
        public string Team_nameRacer_head
        {
            get { return Properties.Resources.mwi_Team_nameRacer_head; }
        }
        public string Car_nameRacer_head
        {
            get { return Properties.Resources.mwi_Car_nameRacer_head; }
        }
        public string Nationality_nameRacer_head
        {
            get { return Properties.Resources.mwi_Nationality_nameRacer_head; }
        }
        public string Reftime_nameRacer_head
        {
            get { return Properties.Resources.mwi_Reftime_nameRacer_head; }
        }
        public string note_racerdetail_head
        {
            get { return Properties.Resources.mwi_note_racerdetail_head; }
        }
        public string AddNote_btn_
        {
            get { return Properties.Resources.mwi_AddNote_btn_; }
        }
        public string Note_Racer_head
        {
            get { return Properties.Resources.mwi_note_racerdetail_head; }
        }

        public void Reload()
        {
            OnPropertyRaised("First_nameRacer_head");
            OnPropertyRaised("Reftime_nameRacer_head");
            OnPropertyRaised("Nationality_nameRacer_head");
            OnPropertyRaised("Car_nameRacer_head");
            OnPropertyRaised("Team_nameRacer_head");
            OnPropertyRaised("name_nameRacer_head");
            OnPropertyRaised("Born_nameRacer_head");
            OnPropertyRaised("Last_nameRacer_head");
            OnPropertyRaised("Short_nameRacer_head");
            OnPropertyRaised("NationalityRacer_head");
            OnPropertyRaised("TeamRacer_head");
            OnPropertyRaised("choose_race_btn_");
            OnPropertyRaised("note_racerdetail_head");
            OnPropertyRaised("AddNote_btn_");
            OnPropertyRaised("Note_Racer_head");
        }


        public DetailRacerReporter_Language()
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
    /// Interaction logic for DetailRacerReporter.xaml
    /// </summary>
    public class RacersDataDetailRep
    {
        public int ID_Racer_Car { get; set; }
        public int ID_Racer { get; set; }
        public string First_Name { get; set; }
        public string Last_Name { get; set; }
        public string Short_Name { get; set; }
        public string Born { get; set; }
        public string Nationality { get; set; }
        public string Team_name { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public string Year { get; set; }
        public string Reference_Time { get; set; }
        public string StartNum { get; set; }
    }

    public class RacerNoteRep
    {
        public int ID { get; set; }
        public string Note { get; set; }
    }

    public partial class DetailRacerReporter : Window
    {
        Int32 RaceID = 0;
        public DetailRacerReporter(int datID)
        {
            InitializeComponent();
            this.RaceID = (Int32)datID;
            LoadRacersInRace(this.RaceID);
        }

        public List<RacersDataDetailRep> racersDataDetailReps = new List<RacersDataDetailRep>();
        public void LoadRacersInRace(Int32 RaceID)
        {
            RacersListRep.ItemsSource = racersDataDetailReps;
            List<RacersDataDetailRep> _items = new List<RacersDataDetailRep>();
            MySqlCommand command;
            MySqlDataReader rdr = null;
            try
            {
                // load data base on filter
                command = Database.Database.CreateCommand(string.Format(Database.Database.QueryStack["GetRacerDetailData"], RaceID));
                rdr = command.ExecuteReader();

                while (rdr.Read())
                {
                    _items.Add(new RacersDataDetailRep()
                    {
                        ID_Racer_Car = rdr.GetInt32("ID_Racer_Car"),
                        ID_Racer = rdr.GetInt32("FK_Racer"),
                        First_Name = rdr.GetString("First_Name"),
                        Last_Name = rdr.GetString("Last_Name"),
                        Short_Name = rdr.GetString("Short_Name"),
                        Born = rdr.GetDateTime("Born").ToShortDateString(),
                        Nationality = rdr.GetString("Nationality"),
                        Team_name = rdr.GetString("Team_name"),
                        Brand = rdr.GetString("Brand"),
                        Model = rdr.GetString("Model"),
                        Year = rdr.GetString("Year"),
                        Reference_Time = rdr.GetString("Reference_Time"),
                        StartNum = rdr.GetString("Start_num")
                    });
                }
                rdr.Close();

                racersDataDetailReps.Clear();
                racersDataDetailReps = _items;
                Dispatcher.BeginInvoke(((Action)(() =>
                {
                    // rebind data value
                    RacersListRep.ItemsSource = null;
                    RacersListRep.ItemsSource = racersDataDetailReps;
                })));
            }
            catch
            {
                if (rdr != null && !rdr.IsClosed)
                    rdr.Close();
            }
        }

        public int RacerID = 0;
        public RacersDataDetailRep SelectedRacer = null;
        private void RacersListRep_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(RacersListRep.SelectedItems != null)
            {
                SelectedRacer = RacersListRep.SelectedItem as RacersDataDetailRep;
                
            }
        }

        private void SelectRacer_Click(object sender, RoutedEventArgs e)
        {
            RacerSelectionRep.Visibility = Visibility.Hidden;
            SelectRacer.Visibility = Visibility.Visible;
            RacerName_txt_.Text = SelectedRacer.First_Name + " " + SelectedRacer.Last_Name;
            RacerBorn_txt_.Text = SelectedRacer.Born;
            Team_txt.Text = SelectedRacer.Team_name;
            Car_txt.Text = SelectedRacer.Brand + " " + SelectedRacer.Model;
            Nationality_txt.Text = SelectedRacer.Nationality;
            RaceTime_txt.Text = SelectedRacer.Reference_Time;
            RacerID = SelectedRacer.ID_Racer;
            this.Title = SelectedRacer.First_Name + " " + SelectedRacer.Last_Name + "(" + SelectedRacer.StartNum +")";
            UpdateRacerNotes();
            RacerDetail.Visibility = Visibility.Visible;
        }

        private void CarotSet(object sender, RoutedEventArgs e)
        {
            if (EditingCommands.MoveToLineEnd.CanExecute(null, sender as IInputElement))
            {
                EditingCommands.MoveToLineEnd.Execute(null, sender as IInputElement);
            }
        }

        //class RacerNoteRep
        //RacerNoteRep IDRacer
        public List<RacerNoteRep> _notes = new List<RacerNoteRep>();
        private void UpdateRacerNotes()
        {
            RacersNotes.ItemsSource = _notes;
            List<RacerNoteRep> _items = new List<RacerNoteRep>();
            int _ID = RacerID;
            MySqlCommand command;
            MySqlDataReader rdr = null;
            try
            {
                // load data base on filter
                command = Database.Database.CreateCommand(string.Format(Database.Database.QueryStack["RacerNoteRep"], _ID));
                rdr = command.ExecuteReader();

                while (rdr.Read())
                {
                    _items.Add(new RacerNoteRep()
                    {
                        ID = rdr.GetInt32("ID_Racer_Note"),
                        Note = rdr.GetString("Note")
                    });
                }
                rdr.Close();

                _notes.Clear();
                _notes = _items;
                Dispatcher.BeginInvoke(((Action)(() =>
                {
                    // rebind data value
                    RacersNotes.ItemsSource = null;
                    RacersNotes.ItemsSource = _notes;
                })));
            }
            catch {  }
        }

        private void AddNoteToRacer_Click(object sender, RoutedEventArgs e)
        {
            //AddRacerNoteRep
            string _noteText = NoteRacer_txt.Text;
            int _ID = RacerID;
            try
            {
                MySqlCommand note_cmd = Database.Database.CreateCommand(string.Format(Database.Database.QueryStack["AddRacerNoteRep"], _noteText, _ID));

                int retcc = note_cmd.ExecuteNonQuery();
            }
            catch { }
            UpdateRacerNotes();
            NoteRacer_txt.Text = string.Empty;
        }
    }
}
