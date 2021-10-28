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
    public class Tournaments_Language : INotifyPropertyChanged
    {
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
        public string NumOfRacers_head
        {
            get { return Properties.Resources.t_NumOfRacers_head; }
        }
        public string NumOfRaces_head
        {
            get { return Properties.Resources.t_NumOfRaces_head; }
        }
        public string Category_head
        {
            get { return Properties.Resources.t_Category_head; }
        }
        public string addTour_
        {
            get { return Properties.Resources.t_addTour_; }
        }
        public string closeTour_
        {
            get { return Properties.Resources.t_closeTour_; }
        }
        public string tournaments_title
        {
            get { return Properties.Resources.t_tournaments_title; }
        }
        public string print_btn_
        {
            get { return Properties.Resources.print_btn_; }
        }

        public Tournaments_Language()
        {

        }

        public void Reload()
        {
            OnPropertyRaised("Name_head");
            OnPropertyRaised("print_btn_");
            OnPropertyRaised("t_ShortName_head");
            OnPropertyRaised("Year_head");
            OnPropertyRaised("NumOfRacers_head");
            OnPropertyRaised("NumOfRaces_head");
            OnPropertyRaised("Category_head");
            OnPropertyRaised("addTour_");
            OnPropertyRaised("deleteTour_");
            OnPropertyRaised("tournaments_title");
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

    public class TournamentsView
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Name_Short { get; set; }
        public int Year { get; set; }
        public int NumOfRacers { get; set; }
        public int NumOfRaces { get; set; }
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }

        //public bool Compare(TournamentsView _ref)
        //{
        //    if (_ref == null)
        //        return false;

        //    return (_ref.Name == this.Name && _ref.Name_Short == this.Name_Short && _ref.Year == this.Year && _ref.NumOfRacers == this.NumOfRacers &&
        //        _ref.NumOfRaces == this.NumOfRaces && _ref.Category == this.Category);
        //}
    }

    /// <summary>
    /// Interaction logic for Tournaments.xaml
    /// </summary>
    public partial class Tournaments : Window
    {
        private static List<TournamentsView> TournamentsItems = new List<TournamentsView>();
        private Add_new_tournament UpdateWindow = null;
        public static EventHandler TournamentsDialogUpdate;
        public static List<EventHandler> TournamentSelectionUpdateNotify = new List<EventHandler>();
        public static TournamentsView LastTournamentSelectedItem = null;
        User LogUser = MainWindow.GetUser();

        public Tournaments()
        {
            InitializeComponent();
            this.Title = (Resources["lang"] as Tournaments_Language).tournaments_title;

            if ((LogUser.Permission) == UserPermission.View) { AddTour_btn.Visibility = Visibility.Hidden; }
            MainWindow.RegisterLanguageHandler(SetLanguage);
            UpdateTournamentsList(string.Empty);
        }

        public void SetLanguage()
        {
            var language_ = Resources["lang"] as Tournaments_Language;
            language_.Reload();
            Dispatcher.BeginInvoke(((Action)(() =>
            {
                this.Title = language_.tournaments_title;
            })));
        }

        protected override void OnClosed(EventArgs e)
        {
            MainWindow.UnRegisterLanguageHandler(SetLanguage);
            base.OnClosed(e);
        }

        public void Refresh()
        {
            Dispatcher.BeginInvoke(((Action)(() =>
            {
                UpdateTournamentsList(string.Empty);
            })));
        }

        private void OnTournamentDialogUpdate(object sender, EventArgs args)
        {
            UpdateTournamentsList(string.Empty);
        }
               
        private void UpdateTournamentsList(string filter)
        {
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
        }


        private void CloseTour_btn_Click(object sender, RoutedEventArgs e)
        {
            if (this.UpdateWindow != null)
            {
                this.UpdateWindow.Close();
                this.UpdateWindow = null;
            }
            this.Close();
        }

        private void AddTour_btn_Click(object sender, RoutedEventArgs e)
        {
            if (this.UpdateWindow != null)
                this.UpdateWindow.Close();
            // opened on selected index, just alovate object aand push data operation
            this.UpdateWindow = null;
            this.UpdateWindow = new Add_new_tournament(this, TournamentOperationType.Add, LastTournamentSelectedItem);
        }

        private void TourList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (this.UpdateWindow != null)
                this.UpdateWindow.Close();
            // opened on selected index, just alovate object aand push data operation
            this.UpdateWindow = null;
            this.UpdateWindow = new Add_new_tournament(this, TournamentOperationType.Update, LastTournamentSelectedItem);
        }


        private void TourList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TourList.SelectedItem != null)
            {
                LastTournamentSelectedItem = TourList.SelectedItem as TournamentsView;
                Console.WriteLine(LastTournamentSelectedItem.ID);
            }
        }

        private void Print_btn_Click(object sender, RoutedEventArgs e)
        {
            PrintManager print = new PrintManager();
            print.Print(PrintManager.PrintType.Tournament);
        }
    }
}
