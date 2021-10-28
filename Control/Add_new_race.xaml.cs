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
  //.Webcontrols.BaseValidator; // RegularWxpressionValidator


namespace Regularity_Rally.Control
{
    #region Language
    public class Add_New_Races_Language : INotifyPropertyChanged
    {
        public string name_race_label
        {
            get { return Properties.Resources.anr_name_race_label; }
        }
        public string location_race_label
        {
            get { return Properties.Resources.anr_location_race_label; }
        }
        public string date_race_label_
        {
            get { return Properties.Resources.anr_date_race_label; }
        }
        public string state_race_label_
        {
            get { return Properties.Resources.anr_state_race_label; }
        }
        public string meascom_race_label_
        {
            get { return Properties.Resources.anr_meascom_race_label; }
        }
        public string length_race_label_
        {
            get { return Properties.Resources.anr_length_race_label; }
        }
        public string save_race_btn_
        {
            get { return Properties.Resources.anr_save_race_btn_; }
        }
        public string delete_race_btn_
        {
            get { return Properties.Resources.anr_delete_race_btn_; }
        }
        public string race_title
        {
            get { return Properties.Resources.anr_race_title; }
        }
        public string cancel_race_btn_
        {
            get { return Properties.Resources.anr_cancel_race_btn_; }
        }
        public string add_racer_btn_
        {
            get { return Properties.Resources.anr_add_racer_btn_; }
        }
        public string delete_racer_btn_
        {
            get { return Properties.Resources.anr_delete_racer_btn_; }
        }
        public string start_num_header
        {
            get { return Properties.Resources.anr_start_num_header; }
        }
        public string name_racer_header
        {
            get { return Properties.Resources.anr_name_racer_header; }
        }
        public string team_racer_header
        {
            get { return Properties.Resources.anr_team_racer_header; }
        }
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
        public string addracers_btn_
        {
            get { return Properties.Resources.racer_addracers_btn_; }
        }
        public string closeracers_btn_
        {
            get { return Properties.Resources.racer_closeracers_btn_; }
        }
        public string add_timekeeper_btn_
        {
            get { return Properties.Resources.racer_add_timekeeper_btn_; }
        }
        public string delete_timekeeper_btn_
        {
            get { return Properties.Resources.racer_delete_timekeeper_btn_; }
        }
        public string First_nameTimekeeper_head
        {
            get { return Properties.Resources.racer_First_nameTimekeeper_head; }
        }
        public string Last_nameTimekeeper_head
        {
            get { return Properties.Resources.racer_Last_nameTimekeeper_head; }
        }
        public string CompanynameTimekeeper_head
        {
            get { return Properties.Resources.racer_CompanynameTimekeeper_head; }
        }
        public string Position_label_
        {
            get { return Properties.Resources.race_Position_label_; }
        }
        public string Addrace_title
        {
            get { return Properties.Resources.race_Addrace_title; }
        }
        public string LoginTimekeeper_head
        {
            get { return Properties.Resources.race_LoginTimekeeper_head; }
        }
        public string RoleTimekeeper_head
        {
            get { return Properties.Resources.race_RoleTimekeeper_head; }
        }
        public string name_timekeep_lbl
        {
            get { return Properties.Resources.race_name_timekeep_lbl; }
        }
        public string place_timekeep_lbl
        {
            get { return Properties.Resources.race_place_timekeep_lbl; }
        }
        public string timekeepers_lbl
        {
            get { return Properties.Resources.race_timekeepers_lbl; }
        }
        public string CarSelectRacer_label_
        {
            get { return Properties.Resources.race_CarSelectRacer_label_; }
        }
        public string StatusSelectRacer_label_
        {
            get { return Properties.Resources.race_StatusSelectRacer_label_; }
        }
        public string RefTimeSelectRacer_label_
        {
            get { return Properties.Resources.race_RefTimeSelectRacer_label_; }
        }
        public string cat_race_label_
        {
            get { return Properties.Resources.race_cat_race_label_; }
        }
        public string note_race_label_
        {
            get { return Properties.Resources.race_note_race_label_; }
        }
        public string laps_race_label_
        {
            get { return Properties.Resources.race_laps_race_label__; }
        }
        public string time_race_label_
        {
            get { return Properties.Resources.race_time_race_label_; }
        }


        public Add_New_Races_Language()
        {

        }

        public void Reload()
        {
            OnPropertyRaised("name_race_label");
            OnPropertyRaised("time_race_label_");
            OnPropertyRaised("laps_race_label_");
            OnPropertyRaised("note_race_label_");
            OnPropertyRaised("cat_race_label_");
            OnPropertyRaised("CarSelectRacer_label_");
            OnPropertyRaised("StatusSelectRacer_label_");
            OnPropertyRaised("RefTimeSelectRacer_label_");
            OnPropertyRaised("location_race_label");
            OnPropertyRaised("date_race_label_");
            OnPropertyRaised("state_race_label_");
            OnPropertyRaised("meascom_race_label_");
            OnPropertyRaised("length_race_label_");
            OnPropertyRaised("save_race_btn_");
            OnPropertyRaised("delete_race_btn_");
            OnPropertyRaised("race_title");
            OnPropertyRaised("cancel_race_btn_");
            OnPropertyRaised("add_racer_btn_");
            OnPropertyRaised("delete_racer_btn_");
            OnPropertyRaised("start_num_header");
            OnPropertyRaised("name_racer_header");
            OnPropertyRaised("team_racer_header");
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
            OnPropertyRaised("addracers_btn_");
            OnPropertyRaised("closeracers_btn_");
            OnPropertyRaised("add_timekeeper_btn_");
            OnPropertyRaised("delete_timekeeper_btn_");
            OnPropertyRaised("First_nameTimekeeper_head");
            OnPropertyRaised("Last_nameTimekeeper_head");
            OnPropertyRaised("CompanynameTimekeeper_head");
            OnPropertyRaised("Position_label_");
            OnPropertyRaised("Addrace_title");
            OnPropertyRaised("LoginTimekeeper_head");
            OnPropertyRaised("RoleTimekeeper_head");
            OnPropertyRaised("name_timekeep_lbl");
            OnPropertyRaised("place_timekeep_lbl");
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
    /// Interaction logic for Add_new_race.xaml
    /// </summary>
    /// 

    public class RacersInRaceData
    {
        public int ID_Racer_Car { get; set; }
        public int ID_Racer { get; set; }
        public int ID_Race { get; set; }
        public int ID_Car { get; set; }
        public int ID_RacerStatus { get; set; }
        public string Start_num { get; set; }
        public string First_Name { get; set; }
        public string Last_Name { get; set; }
        public string Team_name { get; set; }
        public string RacerReferenceTime { get; set; }

        //public bool Compare(RacersInRaceData _ref)
        //{
        //    if (_ref == null)
        //        return false;

        //    return (_ref.Start_num == this.Start_num && _ref.First_Name == this.First_Name && _ref.Last_Name == this.Last_Name &&
        //            _ref.Team_name == this.Team_name);
        //}
    }

    public class SelectRacerToRace
    {
        public int ID { get; set; }
        public string First_name { get; set; }
        public string Last_name { get; set; }
        public string Short_name { get; set; }
        public string Gender { get; set; }
        public string Born { get; set; }
        public string Nationality { get; set; }
        public string Address { get; set; }
        public string Tel { get; set; }
        public string Mail { get; set; }
        public int TeamID { get; set; }
        public string Team { get; set; }

        public bool Compare(SelectRacerToRace _ref)
        {
            if (_ref == null)
                return false;

            return (_ref.First_name == this.First_name && _ref.Last_name == this.Last_name && _ref.Short_name == this.Short_name &&
                    _ref.Gender == this.Gender && _ref.Born == this.Born && _ref.Nationality == this.Nationality && _ref.Address == this.Address &&
                    _ref.Tel == this.Tel && _ref.Mail == this.Mail);
        }
    }

    public class SelectTimekeeperToRace
    {
        public int ID { get; set; }
        public string Login { get; set; }
        public string Name { get; set; }
        public string Lastname { get; set; }
        public int Role { get; set; }
        public int Position { get; set; }

        public bool Compare(SelectTimekeeperToRace _ref)
        {
            if (_ref == null)
                return false;
            return (_ref.Login == this.Login && _ref.Name == this.Name && _ref.Lastname == this.Lastname && _ref.Role == this.Role);
        }

    }

    public class MeasurerInRace
    {
        public int ID { get; set; }
        public int Meas_Company { get; set; }
        public int Device_Settings { get; set; }
        public int Device { get; set; }
        public string User { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int UserID { get; set; }
        public string Position { get; set; }
        public int Role { get; set; }

        //public bool Compare(MeasurerInRace _ref)
        //{
        //    if (_ref == null)
        //        return false;
        //    return (_ref.ID == this.ID && _ref.Meas_Company == this.Meas_Company && _ref.Device_Settings == this.Device_Settings && _ref.Device == this.Device
        //        && _ref.User == this.User && _ref.Position == this.Position);
        //}
    }

    public class RacesState
    {
        public int ID { get; set; }
        public string StatusText { get; set; }
    }

    public class RacersState
    {
        public int ID { get; set; }
        public string StatusText { get; set; }
    }

    public class ReferenceTime
    {
        public int ID { get; set; }
        public string Reference_Time { get; set; }
    }

    public class ExistMeascomMeasurerID
    {
        public int ID { get; set; }
    }

    public class RaceNote
    {
        public string RaceNoteString { get; set; }
    }

    public class LapData
    {
        public int LapDataID { get; set; }
    }

    public class CarCombo
    {
        public Int32 CarID { get; set; }
        public string CarFull { get; set; }
    }

    public enum RacesOperationType
    {
        Add,
        Update
    }

    public enum TimekeeperOperationType
    {
        Add,
        Update
    }

    public partial class Add_new_race : Window
    {
       // private Add_new_race UpdateWindow = null;
        private RacesOperationType CurrentOperation = RacesOperationType.Add;
        private TimekeeperOperationType CurrentOperationTK = TimekeeperOperationType.Add;
        Races parent = null;
        RacesView data = null;
        User LogUser = MainWindow.GetUser();


        public Add_new_race(Races parent, RacesOperationType operation, object data)
        {
            InitializeComponent();
            this.Title = (Resources["lang"] as Add_New_Races_Language).Addrace_title;
            MainWindow.RegisterLanguageHandler(SetLanguage);
            if ((LogUser.Permission) < UserPermission.All) { ActionDelete.Visibility = Visibility.Hidden; }
            if ((LogUser.Permission) < UserPermission.Update) { ActionSave.Visibility = Visibility.Hidden; }
            if ((LogUser.Permission) < UserPermission.Update) { AddRacer.Visibility = Visibility.Hidden; }
            if ((LogUser.Permission) < UserPermission.All) { RemoveRacer.Visibility = Visibility.Hidden; }
            if ((LogUser.Permission) < UserPermission.Update) { AddTimeKeeper.Visibility = Visibility.Hidden; }
            if ((LogUser.Permission) < UserPermission.All) { RemoveTimeKeeper.Visibility = Visibility.Hidden; }

            this.CurrentOperation = operation;
            this.parent = parent;
            this.data = data as RacesView;
            //Console.WriteLine(this.data.ID);
            //TimeRace_txt.Typography

            SetOperation(this.CurrentOperation, this.data);
            FillStateCombo();
            FillMeasComboBox();
            FillCarsCombo();
            FillRacerStateCombo();
            FillCatRaceCombo();

        }

        public List<CarCombo> Combo = new List<CarCombo>();
        public List<CarView> ComboRacerCar = new List<CarView>();
        private void FillCarsCombo()
        {
            List<CarView> _items = new List<CarView>();
            MySqlDataReader rdr = null;
            try
            {
                MySqlCommand command = Database.Database.CreateCommand(Database.Database.QueryStack["GetCarData"]);
                rdr = command.ExecuteReader();

                while (rdr.Read())
                {
                    _items.Add(new CarView()
                    {
                        ID = rdr.GetInt32("ID_Car"),
                        Brand = rdr.GetString("Brand"),
                        Model = rdr.GetString("Model"),
                        Year = rdr.GetInt32("Year"),
                        Vin = rdr.GetString("VIN")
                    });
                }

                ComboRacerCar.Clear();
                ComboRacerCar = _items;
                foreach (CarView rac in ComboRacerCar)
                {
                    Combo.Add(
                        new CarCombo
                        {
                            CarID = rac.ID,   
                            CarFull = string.Format("{0} {1} ({2})", rac.Brand, rac.Model, rac.Vin)
                        }
                    );
                }
                Dispatcher.BeginInvoke(((Action)(() =>
                {
                    Car_Combo.ItemsSource = null;
                    Car_Combo.ItemsSource = Combo;
                })));
                rdr.Close();
            }
            catch
            {
                if (rdr != null && !rdr.IsClosed)
                    rdr.Close();
            }
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
                while(rdr.Read())
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

        public List<RacersState> ComboRacersState = new List<RacersState>();
        private void FillRacerStateCombo()
        {
            List<RacersState> _items = new List<RacersState>();
            MySqlDataReader rdr = null;
            try
            {
                // load data base on filter
                MySqlCommand command = Database.Database.CreateCommand(Database.Database.QueryStack["GetRacerStatusData"]);
                rdr = command.ExecuteReader();

                while (rdr.Read())
                {
                    _items.Add(new RacersState()
                    {
                        ID = rdr.GetInt32("ID_Status_Racer"),
                        StatusText = rdr.GetString("Status_Text"),

                    });
                }
                ComboRacersState.Clear();
                ComboRacersState = _items;
                Dispatcher.BeginInvoke(((Action)(() =>
                {
                    RacerStatus_Combo.ItemsSource = null;
                    RacerStatus_Combo.ItemsSource = ComboRacersState;
                })));
                rdr.Close();
            }
            catch { }
        }

        public List<RacesState> ComboRacesState = new List<RacesState>();
        private void FillStateCombo()
        {
            List<RacesState> _items = new List<RacesState>();
            MySqlDataReader rdr = null;
            try
            {
                // load data base on filter
                MySqlCommand command = Database.Database.CreateCommand(Database.Database.QueryStack["GetRaceStatusData"]);
                rdr = command.ExecuteReader();

                while (rdr.Read())
                {
                    _items.Add(new RacesState()
                    {
                        ID = rdr.GetInt32("ID_Race_Status"),
                        StatusText = rdr.GetString("Status_Text"),

                    });
                }
                ComboRacesState.Clear();
                ComboRacesState = _items;
                Dispatcher.BeginInvoke(((Action)(() =>
                {
                    StateRace.ItemsSource = null;
                    StateRace.ItemsSource = ComboRacesState;
                })));
                rdr.Close();
            }
            catch { }
        }

        public List<BrandView> ComboBrandFill = new List<BrandView>();
        private void FillMeasComboBox()
        {
            List<BrandView> _items = new List<BrandView>();
            MySqlDataReader rdr = null;
            try
            {
                // load data base on filter
                MySqlCommand command = Database.Database.CreateCommand(Database.Database.QueryStack["GetBrandData"]);
                rdr = command.ExecuteReader();

                while (rdr.Read())
                {
                    _items.Add(new BrandView()
                    {
                        ID = rdr.GetInt32("ID_Meas_Company"),
                        Company = rdr.GetString("Company"),
                        Address = rdr.GetString("Address"),
                        Telephone = rdr.GetString("Telephone"),
                        Email = rdr.GetString("Email"),
                        Web = rdr.GetString("Web")

                    });
                }
                ComboBrandFill.Clear();
                ComboBrandFill = _items;
                Dispatcher.BeginInvoke(((Action)(() =>
                {
                    MeasCom.ItemsSource = null;
                    MeasCom.ItemsSource = ComboBrandFill;
                })));
                rdr.Close();
            }
            catch { }
        }

        protected override void OnClosed(EventArgs e)
        {
            MainWindow.UnRegisterLanguageHandler(SetLanguage);
            base.OnClosed(e);
        }

        public void SetLanguage()
        {
            var language_ = Resources["lang"] as Add_New_Races_Language;
            language_.Reload();
            Dispatcher.BeginInvoke(((Action)(() =>
            {
                this.Title = language_.Addrace_title;
            })));
        }

        string RaceNoteDB;


        public List<RaceNote> RaceNoteList = new List<RaceNote>();
        private void SetOperation(RacesOperationType operation, RacesView data)
        {
            switch (operation)
            {
                case RacesOperationType.Add:
                    TimeRace_txt.Text = "00:00:00";
                    break;
                case RacesOperationType.Update:
                    Dispatcher.BeginInvoke(((Action)(() =>
                    {
                        RaceName_txt.Text = data.Title;
                        LocationRace_txt.Text =data.Place;
                        DateRace.Text = data.Date;
                        StateRace.SelectedIndex = data.StatusID-1;
                        MeasCom.SelectedIndex = data.MeasID-1;
                        LegthRace.Text = data.Length.ToString();
                        CatRaceCombo.SelectedIndex = data.Category-1;
                        NumOfLapsRace_txt.Text = data.NumOfLaps.ToString();
                        MySqlCommand racenote_cmd = Database.Database.CreateCommand(string.Format(Database.Database.QueryStack["GetRaceNote"], data.RaceNote));
                        MySqlDataReader rdr_racenote = racenote_cmd.ExecuteReader();
                        while (rdr_racenote.Read()){RaceNoteList.Add(new RaceNote(){RaceNoteString = rdr_racenote.GetString("Note_Text")});}
                        foreach(RaceNote note in RaceNoteList){RaceNoteDB = note.RaceNoteString;}
                        rdr_racenote.Close();
                        NoteRace.Text = RaceNoteDB;
                        TimeRace_txt.Text = data.Time;

                        UpdateTimekeepersInRace(data.ID);
                        UpdateRacersInRace(data.ID);
                        int retcc = racenote_cmd.ExecuteNonQuery();
                    })));
                    break;
            }
            try
            {
                this.Show();
            }
            catch { }
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
                int ID = (CurrentOperation == RacesOperationType.Add) ? 0 : data.ID;
                int _State = (CurrentOperation == RacesOperationType.Add) ? 1 : StateRace.SelectedIndex + 1;
                string _Note = MySqlHelper.EscapeString(NoteRace.Text);
                int _Category = CatRaceCombo.SelectedIndex + 1;
                int _MeasID = MeasCom.SelectedIndex + 1;
                string _Title = MySqlHelper.EscapeString(RaceName_txt.Text);
                string _Place = MySqlHelper.EscapeString(LocationRace_txt.Text);
                string _Date = MySqlHelper.EscapeString(DateRace.Text);
                string _Length = MySqlHelper.EscapeString(LegthRace.Text);
                string _NumOfLaps = MySqlHelper.EscapeString(NumOfLapsRace_txt.Text);
                string _Time = TimeRace_txt.Text;
                string _StartTime = "00:00:00";
                if(ID != 0) {_StartTime = data.StartTime; }
                
                //string _StartTime = data.StartTime;

                DateTime ParseTime = DateTime.ParseExact(_Date, "dd.MM.yyyy", null);
                _Date = ParseTime.ToString("yyyy-MM-dd");


                MySqlCommand racenote_cmd = Database.Database.CreateCommand(string.Format(Database.Database.QueryStack["RaceNote"], _Note));
                int retccnote = racenote_cmd.ExecuteNonQuery();
                int LastInsertID = (int)racenote_cmd.LastInsertedId;

                Console.WriteLine(_State);
                Console.WriteLine(LastInsertID);
                Console.WriteLine(_Category);
                Console.WriteLine(_MeasID);
                Console.WriteLine(_Title);
                Console.WriteLine(_Date);
                Console.WriteLine(_Place);
                Console.WriteLine(_Length);
                Console.WriteLine(_NumOfLaps);


                MySqlCommand cmd = (ID == 0) ?
                    Database.Database.CreateCommand(string.Format(Database.Database.QueryStack["AddRaceFinalData"],
                    _State,
                    LastInsertID,
                    _Category,
                    _MeasID,
                    _Title,
                    _Date,
                    _Place,
                    _Length,
                    _NumOfLaps,
                    _Time
                    ))
                    :   //L
                    Database.Database.CreateCommand(string.Format(Database.Database.QueryStack["RepRaceFinalData"], 
                    ID,
                    _State,
                    LastInsertID,
                    _Category,
                    _MeasID,
                    _Title,
                    _Date,
                    _Place,
                    _Length,
                    _Time,
                    _StartTime,
                    _NumOfLaps
                    ));
                //Console.WriteLine(cmd.CommandText);
                int retcc = cmd.ExecuteNonQuery();


            }
            catch(Exception exp) { Console.WriteLine(exp.Message); }

            

            parent.Refresh();
            this.Close();
        }

        private void ActionDelete_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Delete Record?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Warning) != MessageBoxResult.No)
            {
                //Check if some LapData exist
                //RaceIDinLapData
                //Done
                int rows_LapDataCount = 0;
                try
                {
                    MySqlCommand check_LapData = Database.Database.CreateCommand(string.Format(Database.Database.QueryStack["RaceIDinLapData"], data.ID));
                    rows_LapDataCount = (int)Convert.ToInt32(check_LapData.ExecuteScalar());
                    int rowRaceIDinLapData = check_LapData.ExecuteNonQuery();
                }
                catch { }
                if (rows_LapDataCount > 0)
                {
                    MessageBox.Show("Cant remove data!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                //Remove racer_car data
                //RaceIDinRacerCar
                //Done
                try
                {
                    MySqlCommand check_RacerCar = Database.Database.CreateCommand(string.Format(Database.Database.QueryStack["RaceIDinRacerCar"], data.ID));
                    int rows_RacerCar = (int)Convert.ToInt32(check_RacerCar.ExecuteScalar());
                    int RaceIDinRacerCar = check_RacerCar.ExecuteNonQuery();
                    if (rows_RacerCar > 0)
                    {
                        MySqlCommand cmd_RacerCar = Database.Database.CreateCommand(string.Format(Database.Database.QueryStack["RemRacerCarANR"], data.ID));
                        int rowRemRacerCarANR = cmd_RacerCar.ExecuteNonQuery();
                    }
                }
                catch { }

                //Remove meascom_measurer
                //Done
                try
                {
                    MySqlCommand check_meascom_measurer = Database.Database.CreateCommand(string.Format(Database.Database.QueryStack["RaceIDinMeasComMeas"], data.ID));
                    int rows_meascom_measurer = (int)Convert.ToInt32(check_meascom_measurer.ExecuteScalar());
                    int rowRaceIDinMeasComMeas = check_meascom_measurer.ExecuteNonQuery();
                    if (rows_meascom_measurer > 0)
                    {
                        MySqlCommand cmd_meascom_measurer = Database.Database.CreateCommand(string.Format(Database.Database.QueryStack["RemMeasComMeasANR"], data.ID));
                        int rowRemMeasComMeasANR = cmd_meascom_measurer.ExecuteNonQuery();
                    }
                }
                catch { }

                //Remove race_jury
                try
                {
                    MySqlCommand check_race_jury = Database.Database.CreateCommand(string.Format(Database.Database.QueryStack["RaceIDinLapData"], data.ID));
                    int rows_race_jury = (int)Convert.ToInt32(check_race_jury.ExecuteScalar());
                    int rowRaceIDinLapData = check_race_jury.ExecuteNonQuery();
                    if (rows_race_jury > 0)
                    {
                        MySqlCommand cmd_race_jury = Database.Database.CreateCommand(string.Format(Database.Database.QueryStack["RemRacerCarANR"], data.ID));
                        int rowRemRacerCarANR = cmd_race_jury.ExecuteNonQuery();
                    }
                }
                catch { }

                //Remove race_tour
                try
                {
                    MySqlCommand check_race_tour = Database.Database.CreateCommand(string.Format(Database.Database.QueryStack["RaceIDinRaceJury"], data.ID));
                    int rows_race_tour = (int)Convert.ToInt32(check_race_tour.ExecuteScalar());
                    int rowRaceIDinRaceJury = check_race_tour.ExecuteNonQuery();
                    if (rows_race_tour > 0)
                    {
                        MySqlCommand cmd_race_tour = Database.Database.CreateCommand(string.Format(Database.Database.QueryStack["RemRaceJuryANR"], data.ID));
                        int rowRemRaceJuryANR = cmd_race_tour.ExecuteNonQuery();
                    }
                }
                catch { }

                try
                {
                    MySqlCommand command = Database.Database.CreateCommand(string.Format(Database.Database.QueryStack["RemRace"],data.ID));
                    int rowRemRace = command.ExecuteNonQuery();
                }
                catch { }                
            }
            parent.Refresh();
            this.Close();
        }

        private void ActionCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }


        #region RACERS SELECTION

        public static RacersInRaceData LastRacerInRaceSelectedItem = null;
        public List<EventHandler> RacerInRaceSelectionUpdateNotify = new List<EventHandler>();
        private void RacersInRaceList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (RacersInRaceList.SelectedItem != null)
            {
                RacersInRaceData _t = LastRacerInRaceSelectedItem;
                LastRacerInRaceSelectedItem = RacersInRaceList.SelectedItem as RacersInRaceData;
                if (_t != null)
                {
                    if (LastRacerInRaceSelectedItem.ID_Racer_Car != _t.ID_Racer_Car)
                    {
                        foreach (EventHandler _event in RacerInRaceSelectionUpdateNotify)
                        {
                            if (_event != null)
                                _event.BeginInvoke(null, null, null, null);
                        }
                    }
                }
            }
        }

        private static List<RacersInRaceData> RacerInRaceItems = new List<RacersInRaceData>();
        private void UpdateRacersInRace(int _id)
        {
            RacersInRaceList.ItemsSource = RacerInRaceItems;
            List<RacersInRaceData> _items = new List<RacersInRaceData>();
            MySqlDataReader rdra = null;
            try
            {
                // load tata to be feeded into selector
                MySqlCommand cmd = Database.Database.CreateCommand(string.Format(Database.Database.QueryStack["GetRacersInRaceData"], _id));
                rdra = cmd.ExecuteReader();

                while (rdra.Read())
                {
                    _items.Add(new RacersInRaceData
                    {
                        ID_Racer_Car = rdra.GetInt32("ID_Racer_Car"),
                        Start_num = rdra.GetInt32("Start_num").ToString(),
                        ID_Car = rdra.GetInt32("FK_Car"),
                        ID_RacerStatus = rdra.GetInt32("FK_Status_Racer"),
                        RacerReferenceTime = rdra.GetString("Reference_Time"),
                        ID_Racer = rdra.GetInt32("ID_Racer"),
                        First_Name = rdra.GetString("First_Name"),
                        Last_Name = rdra.GetString("Last_Name"),
                        Team_name = rdra.GetString("Team_name"),
                        ID_Race = rdra.GetInt32("ID_Race"),
                    });
                }

                rdra.Close();
                Console.WriteLine(_items.Count);

                RacerInRaceItems.Clear();
                RacerInRaceItems = _items;
                Dispatcher.BeginInvoke(((Action)(() =>
                {
                    RacersInRaceList.ItemsSource = null;
                    RacersInRaceList.ItemsSource = RacerInRaceItems;
                })));
            }
            catch
            {
                if (rdra != null && !rdra.IsClosed)
                    rdra.Close();
            }
        }

        private static List<SelectRacerToRace> RacersItems = new List<SelectRacerToRace>();
        private void UpdateRacerSelection(string filter)
        {
            AddRacerInRaceList.ItemsSource = null;
            AddRacerInRaceList.ItemsSource = RacersItems;
            List<SelectRacerToRace> _items = new List<SelectRacerToRace>();
            int SelectedIndex = 0;
            MySqlDataReader rdr = null;
            try
            {
                // load data base on filter
                MySqlCommand command = Database.Database.CreateCommand(Database.Database.QueryStack["GetRacerData"]);
                rdr = command.ExecuteReader();

                while (rdr.Read())
                {
                    _items.Add(new SelectRacerToRace()
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
                        Team = rdr.GetString("Team_name"),
                        TeamID = rdr.GetInt32("FK_Team")
                    });
                }
                rdr.Close();

                RacersItems.Clear();
                RacersItems = _items;
                Dispatcher.BeginInvoke(((Action)(() =>
                {
                    AddRacerInRaceList.ItemsSource = null;
                    AddRacerInRaceList.ItemsSource = RacersItems;
                    AddRacerInRaceList.SelectedIndex = SelectedIndex;
                })));
            }
            catch
            {
                if (rdr != null && !rdr.IsClosed)
                    rdr.Close();
            }
        }
        
        public static SelectRacerToRace LastRacerSelectedItem = null;
        private void AddRacerInRaceList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (AddRacerInRaceList.SelectedItem != null)
            {
                LastRacerSelectedItem = AddRacerInRaceList.SelectedItem as SelectRacerToRace;
            }
        }

        private void AddRacer_Click(object sender, RoutedEventArgs e)
        {
            Dispatcher.BeginInvoke(((Action)(() =>
            {
                UpdateRacerSelection(string.Empty);
            })));

            RacerSelect.Visibility = Visibility.Visible;
            TimekeeperSelect.Visibility = Visibility.Hidden;
        }

        private void RemoveRacer_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Delete Record?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Warning) != MessageBoxResult.No)
            {
                try
                {
                    MySqlCommand command = Database.Database.CreateCommand(
                        string.Format(Database.Database.QueryStack["RemRacerFromRace"],
                        LastRacerInRaceSelectedItem.ID_Racer_Car
                        ));
                    int RowEffected = command.ExecuteNonQuery();
                }
                catch { }
                Dispatcher.BeginInvoke(((Action)(() =>
                {
                    UpdateRacersInRace(data.ID);
                    UpdateRacerSelection(string.Empty);
                })));
                RacerSelect.Visibility = Visibility.Hidden;
            }
        }

        private void SetOperationR(RacerOperationType operation, RacersInRaceData data)
        {
            switch (operation)
            {
                case RacerOperationType.Add:
                    break;
                case RacerOperationType.Update:
                    Dispatcher.BeginInvoke(((Action)(() =>
                    {
                        RacerStartNum_txt_.Text = data.Start_num.ToString();
                        Car_Combo.SelectedIndex = data.ID_Car-1;
                        RacerStatus_Combo.SelectedIndex = data.ID_RacerStatus-1;
                        ReferenceTime_txt_.Text = data.RacerReferenceTime.ToString();
                        CurrentOperationRacer = RacerOperationType.Update;

                    })));
                    break;
            }
        }

        private void RacersInRaceList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            RacerSelect.Visibility = Visibility.Visible;
            TimekeeperSelect.Visibility = Visibility.Hidden;
            Dispatcher.BeginInvoke(((Action)(() =>
            {

                SetOperationR(RacerOperationType.Update, LastRacerInRaceSelectedItem);
                UpdateRacerSelection(string.Empty);
            })));
        }

        private RacerOperationType CurrentOperationRacer = RacerOperationType.Add;
        private void AddRacerToRace_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int RacerCarID = (CurrentOperationRacer == RacerOperationType.Add) ? 0 : LastRacerInRaceSelectedItem.ID_Racer_Car;
                int RacerID = (CurrentOperationRacer == RacerOperationType.Add) ?  LastRacerSelectedItem.ID : LastRacerInRaceSelectedItem.ID_Racer;
                int RaceID = data.ID;
                string _StartNum = MySqlHelper.EscapeString(RacerStartNum_txt_.Text);
                int _Car = Car_Combo.SelectedIndex + 1;
                int _RacerStatus_Combo = (CurrentOperationRacer == RacerOperationType.Add) ? 1 : RacerStatus_Combo.SelectedIndex + 1;
                string _referencetime = ReferenceTime_txt_.Text;

                MySqlCommand refttime_cmd = Database.Database.CreateCommand(string.Format(Database.Database.QueryStack["AddRefTimeANR"], _referencetime));
                int retc_ref = refttime_cmd.ExecuteNonQuery();

                int LastInsertID = (int)refttime_cmd.LastInsertedId;


                MySqlCommand check_pos = Database.Database.CreateCommand(string.Format(Database.Database.QueryStack["Check_IDRacerCar"], RacerID, RaceID));
                Int32 rows_count = Convert.ToInt32(check_pos.ExecuteScalar());
                if (rows_count > 0)
                {
                    MySqlCommand points_cmd = Database.Database.CreateCommand(string.Format(Database.Database.QueryStack["RepRacerToRace"],
                    RacerCarID,
                    RaceID,
                    RacerID,
                    LastInsertID,
                    _Car,
                    _RacerStatus_Combo,
                    _StartNum
                    ));
                    int retcc = points_cmd.ExecuteNonQuery();
                }
                else
                {
                    MySqlCommand points_cmd = Database.Database.CreateCommand(string.Format(Database.Database.QueryStack["AddRacerToRace"],
                    RaceID,
                    RacerID,
                    LastInsertID,
                    _Car,
                    _RacerStatus_Combo,
                    _StartNum
                    ));
                    int retcc = points_cmd.ExecuteNonQuery();
                }
            }
            catch { }
            Dispatcher.BeginInvoke(((Action)(() =>
            {
                UpdateRacersInRace(data.ID);
                UpdateRacerSelection(string.Empty);
            })));
            RacerSelect.Visibility = Visibility.Hidden;
            TimekeeperSelect.Visibility = Visibility.Hidden;
        }

        private void CloseRacerToRace_Click(object sender, RoutedEventArgs e)
        {
            RacerSelect.Visibility = Visibility.Hidden;
            TimekeeperSelect.Visibility = Visibility.Hidden;
        }

        #endregion

        #region TIMEKEEPER SELECTION

        private void AddTimeKeeper_Click(object sender, RoutedEventArgs e)
        {
            Dispatcher.BeginInvoke(((Action)(() =>
            {
                UpdateTimekeepersSelection(string.Empty);
                SetOperationTK(TimekeeperOperationType.Add, null);
            })));
            RacerSelect.Visibility = Visibility.Hidden;
            TimekeeperSelect.Visibility = Visibility.Visible;
        }

        private static List<SelectTimekeeperToRace> TimekeepersItems = new List<SelectTimekeeperToRace>();

        private void UpdateTimekeepersSelection(string filter)
        {
            AddTimekeeperInRaceList.ItemsSource = null;
            AddTimekeeperInRaceList.ItemsSource = TimekeepersItems;
            List<SelectTimekeeperToRace> _items = new List<SelectTimekeeperToRace>();
            int SelectedIndex = 0;
            MySqlDataReader rdr = null;
            try
            {
                // load data base on filter
                MySqlCommand command = Database.Database.CreateCommand(string.Format(Database.Database.QueryStack["GetTKbyBrand"], data.MeasID));
                rdr = command.ExecuteReader();

                while (rdr.Read())
                {
                    _items.Add(new SelectTimekeeperToRace()
                    {
                        ID = rdr.GetInt32("ID_User"),
                        Login = rdr.GetString("Login"),
                        Name = rdr.GetString("Name"),
                        Lastname = rdr.GetString("Lastname"),
                        Role = rdr.GetInt32("Role")
                    });
                }
                rdr.Close();

                TimekeepersItems.Clear();
                TimekeepersItems = _items;
                Dispatcher.BeginInvoke(((Action)(() =>
                {
                    AddTimekeeperInRaceList.ItemsSource = null;
                    AddTimekeeperInRaceList.ItemsSource = TimekeepersItems;
                    AddTimekeeperInRaceList.SelectedIndex = SelectedIndex;
                })));
            }
            catch
            {
                if (rdr != null && !rdr.IsClosed)
                    rdr.Close();
            }
        }


        private static List<MeasurerInRace> MeasurerItems = new List<MeasurerInRace>();
        private void UpdateTimekeepersInRace(int _id)
        {
            TimeKeepersInRace.ItemsSource = MeasurerItems;
            List<MeasurerInRace> _items = new List<MeasurerInRace>();
            MySqlDataReader rdr = null;
            try
            {
                Console.WriteLine(_id);
                // load tata to be feeded into selector
                MySqlCommand cmd = Database.Database.CreateCommand(string.Format(Database.Database.QueryStack["GetTimekeepersRace"], _id));
                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    _items.Add(new MeasurerInRace
                    {
                        ID = rdr.GetInt32("ID_MeasCom_Measurer"),
                        UserID = rdr.GetInt32("FK_User"),
                        User = rdr.GetString("Login"),
                        FirstName = rdr.GetString("Name"),
                        LastName = rdr.GetString("Lastname"),
                        Role = rdr.GetInt32("Role"),
                        Position = rdr.GetInt32("Position").ToString()
                    });
                }
                rdr.Close();

                MeasurerItems.Clear();
                MeasurerItems = _items;
                Dispatcher.BeginInvoke(((Action)(() =>
                {
                    TimeKeepersInRace.ItemsSource = null;
                    TimeKeepersInRace.ItemsSource = MeasurerItems;
                })));
                int RowEffected = cmd.ExecuteNonQuery();
            }
            catch
            {
                if (rdr != null && !rdr.IsClosed)
                    rdr.Close();
            }
        }

        private void SetOperationTK(TimekeeperOperationType operation, MeasurerInRace data)
        {
            switch(operation)
            {
                case TimekeeperOperationType.Add:
                    break;
                case TimekeeperOperationType.Update:
                    Dispatcher.BeginInvoke(((Action)(() =>
                    {
                        LoginTimeKeeper.Text = data.User;
                        FirstnameKeeper.Text = data.FirstName;
                        LastnameTimekeeper.Text = data.LastName;
                        Position.Text = data.Position.ToString();
                    })));
                    break;
            }
        }

        public static SelectTimekeeperToRace LastTimekeeperSelectedItem = null;
        private void AddTimekeeperInRaceList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (AddTimekeeperInRaceList.SelectedItem != null)
            {
                SelectTimekeeperToRace _t = LastTimekeeperSelectedItem;
                LastTimekeeperSelectedItem = AddTimekeeperInRaceList.SelectedItem as SelectTimekeeperToRace;
                LoginTimeKeeper.Text = LastTimekeeperSelectedItem.Login;
                FirstnameKeeper.Text = LastTimekeeperSelectedItem.Name;
                LastnameTimekeeper.Text = LastTimekeeperSelectedItem.Lastname;
            }
        }

        private List<ExistMeascomMeasurerID> MeascomMeasurerID = new List<ExistMeascomMeasurerID>();
        private void AddTimeKeeperToRace_Click(object sender, RoutedEventArgs e)
        {
            int TimeKeeperToRaceID = 0;

            int IDTK = (CurrentOperationTK == TimekeeperOperationType.Add) ? 0 : LastTimekeeperInRaceSelectedItem.ID;
            int _User = (CurrentOperationTK == TimekeeperOperationType.Add) ? LastTimekeeperSelectedItem.ID : LastTimekeeperInRaceSelectedItem.UserID;
            int _RaceID = data.ID;
            string _position = MySqlHelper.EscapeString(Position.Text);
            int pos = int.Parse(_position);
            if(pos >= data.Length) { MessageBox.Show("Wrong position record!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning); }
            else
            {
                try
                {
                    MySqlCommand point_cmd = Database.Database.CreateCommand(string.Format(Database.Database.QueryStack["CheckIDMCM"], _RaceID, _User));
                    MySqlDataReader rdr_point = point_cmd.ExecuteReader();

                    while (rdr_point.Read())
                    {
                        MeascomMeasurerID.Add(
                            new ExistMeascomMeasurerID()
                            {
                                ID = rdr_point.GetInt32("ID_MeasCom_Measurer")
                            }
                        );
                    }
                    rdr_point.Close();
                    int retccpoints = point_cmd.ExecuteNonQuery();

                    foreach (ExistMeascomMeasurerID FindID in MeascomMeasurerID)
                    {
                        TimeKeeperToRaceID = FindID.ID;
                    }
                }
                catch { }
                try
                {
                    MySqlCommand check_existUIR = Database.Database.CreateCommand(string.Format(Database.Database.QueryStack["Check_UserInRace"], _RaceID, _User));
                    Int32 rows_count = Convert.ToInt32(check_existUIR.ExecuteScalar());

                    if (rows_count > 0)
                    {
                        MySqlCommand cmd_checkUIR = Database.Database.CreateCommand(string.Format(Database.Database.QueryStack["RepTKtoRaceANR"],
                        TimeKeeperToRaceID,
                        _User,
                        _RaceID,
                        _position
                        ));
                        int retcc_checkUIR = cmd_checkUIR.ExecuteNonQuery();
                    }
                    else
                    {
                        MySqlCommand cmd_checkUIR = Database.Database.CreateCommand(string.Format(Database.Database.QueryStack["AddTKtoRaceANR"],
                        _User,
                        _position,
                        _RaceID
                        ));
                        int retcc_checkUIR = cmd_checkUIR.ExecuteNonQuery();
                    }
                }
                catch { }

                Dispatcher.BeginInvoke(((Action)(() =>
                {
                    UpdateTimekeepersSelection(string.Empty);
                    UpdateTimekeepersInRace(data.ID);
                })));
            }

            RacerSelect.Visibility = Visibility.Hidden;
            TimekeeperSelect.Visibility = Visibility.Hidden;
        }


        private void RemoveTimeKeeper_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Delete Record?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Warning) != MessageBoxResult.No)
            {
                try
                {
                    MySqlCommand command = Database.Database.CreateCommand(
                        string.Format(Database.Database.QueryStack["RemMeascomMeasurer"],
                        LastTimekeeperInRaceSelectedItem.ID
                        )) ;
                    int RowEffected = command.ExecuteNonQuery();
                }
                catch { }
                Dispatcher.BeginInvoke(((Action)(() =>
                {
                    UpdateTimekeepersInRace(this.data.ID);
                })));
                TimekeeperSelect.Visibility = Visibility.Hidden;
            }
        }

        public static MeasurerInRace LastTimekeeperInRaceSelectedItem = null;
        private void TimeKeepersInRace_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TimeKeepersInRace.SelectedItem != null)
            {
                MeasurerInRace _t = LastTimekeeperInRaceSelectedItem;
                LastTimekeeperInRaceSelectedItem = TimeKeepersInRace.SelectedItem as MeasurerInRace;
            }
        }

        private void TimeKeepersInRace_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Dispatcher.BeginInvoke(((Action)(() =>
            {
                SetOperationTK(TimekeeperOperationType.Update, LastTimekeeperInRaceSelectedItem);
                UpdateTimekeepersSelection(string.Empty);

            })));
            RacerSelect.Visibility = Visibility.Hidden;
            TimekeeperSelect.Visibility = Visibility.Visible;
        }

        private void CloseTimeKeeperToRace_Click(object sender, RoutedEventArgs e)
        {
            RacerSelect.Visibility = Visibility.Hidden;
            TimekeeperSelect.Visibility = Visibility.Hidden;
        }

        #endregion
    }
}
