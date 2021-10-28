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
    public class Racers_Language : INotifyPropertyChanged
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
        public string racers_title
        {
            get { return Properties.Resources.racer_racers_title; }
        }
        public string print_btn_
        {
            get { return Properties.Resources.print_btn_; }
        }

        public void Reload()
        {
            OnPropertyRaised("First_nameRacer_head");
            OnPropertyRaised("print_btn_");
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
            OnPropertyRaised("racers_title");
        }

        public Racers_Language()
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

    public class RacerView
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

        public bool Compare(RacerView _ref)
        {
            if (_ref == null)
                return false;

            return (_ref.First_name == this.First_name && _ref.Last_name == this.Last_name && _ref.Short_name == this.Short_name &&
                    _ref.Gender == this.Gender && _ref.Born == this.Born && _ref.Nationality == this.Nationality && _ref.Address == this.Address &&
                    _ref.Tel == this.Tel && _ref.Mail == this.Mail);
        }
    }

    /// <summary>
    /// Interaction logic for Racers.xaml
    /// </summary>
    public partial class Racers : Window
    {
        public EventHandler SetLang;

        private static List<RacerView> RacerItems = new List<RacerView>();
        private Add_new_racer UpdateWindow = null;
        User LogUser = MainWindow.GetUser();

        public Racers()
        {
            InitializeComponent();
            this.Title = (Resources["lang"] as Racers_Language).racers_title;
            UpdateRacerList(string.Empty);
            MainWindow.RegisterLanguageHandler(SetLanguage);
            if ((LogUser.Permission) < UserPermission.Update) { AddRacers_btn.Visibility = Visibility.Hidden; }
        }

        public void SetLanguage()
        {
            var language_ = Resources["lang"] as Racers_Language;
            language_.Reload();
            Dispatcher.BeginInvoke(((Action)(() =>
            {
                this.Title = language_.racers_title;
            })));
        }



        private void OnRacerDialogUpdate(object sender, EventArgs args)
        {
            UpdateRacerList(string.Empty);
        }

        public static EventHandler RacerDialogUpdate;
        public static List<EventHandler> RacerSelectionUpdateNotify = new List<EventHandler>();
        public static RacerView LastRacerSelectedItem = null;

        private void RacerList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (RacerList.SelectedItem != null)
            {
                RacerView _t = LastRacerSelectedItem;
                LastRacerSelectedItem = RacerList.SelectedItem as RacerView;
                if (_t != null)
                {
                    if (LastRacerSelectedItem.ID != _t.ID)
                    {
                        foreach (EventHandler _event in RacerSelectionUpdateNotify)
                        {
                            if (_event != null)
                                _event.BeginInvoke(null, null, null, null);
                        }
                    }
                }
            }
        }
        
        private void UpdateRacerList(string filter)
        {
            Dispatcher.BeginInvoke(((Action)(() =>
            {
                action_search.Visibility = Visibility.Visible;
            })));

            //Console.WriteLine("update list");

            RacerList.ItemsSource = RacerItems;
            List<RacerView> _items = new List<RacerView>();
            int SelectedIndex = 0;
            MySqlDataReader rdr = null;
            try
            {
                // load data base on filter
                MySqlCommand command = Database.Database.CreateCommand(Database.Database.QueryStack["GetRacerData"]);
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
                        //DateTime ParseTime = DateTime.ParseExact(rdr.GetString("Born"), "dd.MM.yyyy hh:mm:ss tt", null),
                        //Born = (DateTime.ParseExact(rdr.GetString("Born"), "dd.MM.yyyy hh:mm:ss tt", null).ToString("dd-MM-yyyy")),
                        Born = rdr.GetDateTime("Born").ToShortDateString(),
                        Nationality = rdr.GetString("Nationality"),
                        Address = rdr.GetString("Address"),
                        Tel = rdr.GetString("Telephone"),
                        Mail = rdr.GetString("Email"),
                        Team = rdr.GetString("Team_name"),
                        TeamID = rdr.GetInt32("FK_Team")
                    });
                }
                rdr.Close();



                RacerItems.Clear();
                RacerItems = _items;
                Dispatcher.BeginInvoke(((Action)(() =>
                {
                    // rebind data value
                    RacerList.ItemsSource = null;
                    RacerList.ItemsSource = RacerItems;
                    RacerList.SelectedIndex = SelectedIndex;
                })));
            }
            catch
            {
                if (rdr != null && !rdr.IsClosed)
                    rdr.Close();
            }
        }

        public void Refresh()
        {
            Dispatcher.BeginInvoke(((Action)(() =>
            {
                UpdateRacerList(string.Empty);
            })));
        }


        private void Add_New_Racer_Click(object sender, RoutedEventArgs e)
        {
            if (this.UpdateWindow != null)
                this.UpdateWindow.Close();

            this.UpdateWindow = null;
            this.UpdateWindow = new Add_new_racer(this, RacerOperationType.Add, null);
        }
    

        private void Close_Racer_Click(object sender, RoutedEventArgs e)
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

        private void RacerList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (this.UpdateWindow != null)
                this.UpdateWindow.Close();

            // opened on selected index, just alovate object aand push data operation
            this.UpdateWindow = null;
            this.UpdateWindow = new Add_new_racer(this, RacerOperationType.Update, LastRacerSelectedItem);
        }

        private void Print_btn_Click(object sender, RoutedEventArgs e)
        {
            PrintManager print = new PrintManager();
            print.Print(PrintManager.PrintType.Racers);
        }
    }
}
