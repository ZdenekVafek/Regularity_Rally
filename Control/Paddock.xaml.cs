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
    /// <summary>
    /// Interaction logic for Paddock.xaml
    /// </summary>
    public partial class Paddock : Window
    {
        public Paddock()
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

            this.Title = (Resources["lang"] as Reporter_MainWindow_Language).Paddock_Title;

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
                this.Title = language_.Paddock_Title;
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
                MySqlCommand cmd = Database.Database.CreateCommand(Database.Database.QueryStack["GetRaceDataSelect"]); 
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

            // pop it
            RaceSelector.Visibility = Visibility.Visible;
            PaddockTable.Visibility = Visibility.Hidden;
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
            if (SelectedRace != null)
            {
                var language_ = Resources["lang"] as Reporter_MainWindow_Language;
                language_.Reload();
                Dispatcher.BeginInvoke(((Action)(() =>
                {

                })));

                CreateRaceTracker(SelectedRace.ID);
                UpdateRacerFinalList();
                RaceSelector.Visibility = Visibility.Hidden;
                PaddockTable.Visibility = Visibility.Visible;
            }
        }

        Race RaceTracker = null;
        public Race.RaceTime CreateRaceTracker(Int32 race_id)
        {
            RaceTracker = new Race(race_id);
            return RaceTracker.GetRaceTimes();
        }

        public Homescreen.RaceSelectBrief SelectedRace = null; // new List<Homescreen.RaceSelectBrief>();
        private void RaceList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (RaceList.SelectedItems != null)
            {
                SelectedRace = RaceList.SelectedItem as Homescreen.RaceSelectBrief;
            }
        }

        private void UpdateRacerFinalList()
        {
            if (RaceTracker != null && RaceTracker.LoadLaps())
            {
                Dispatcher.BeginInvoke(((Action)(() =>
                {
                    RacersInRacePad.ItemsSource = null;
                    RacersInRacePad.ItemsSource = RaceTracker.GetSummary();
                })));
            }
        }
    }
}
