using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.ComponentModel;
using Regularity_Rally.Control;
using System.Threading.Tasks;
using System.Windows.Documents;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Collections.Generic;

namespace Regularity_Rally.Control
{
    #region Language
    public class TimekeepingLanguage : INotifyPropertyChanged
    {
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

        public TimekeepingLanguage()
        {

        }

        public void Reload()
        {
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
    /// Interaction logic for Timekeeping.xaml
    /// </summary>
    public partial class Timekeeping : Window
    {

        public Race _race = null;

        public delegate void SendTimeCollection(TimeSpan time);
        private static SendTimeCollection colMsg;
        Int32 StatusRace = 0;

        public Timekeeping(ref Race Tracker)
        {
            _race = Tracker;
            
            colMsg = new SendTimeCollection(OpenTimeCollector);

            InitializeComponent();
            this.Title = (Resources["lang"] as TimekeepingLanguage).timekeeping_title;
            MainWindow.RegisterLanguageHandler(SetLanguage);
            StatusRace = Tracker.GetStatusID();
            if(StatusRace != 1 && StatusRace != 2) { AddRecord_btn.Visibility = Visibility.Hidden; }
            // register surent instance into mainwindow proccessor, soo we can be informed when device dispose event
            (Application.Current.MainWindow as MainWindow).RequestOperation(
               new MainWindow.ProccesorComand
               {
                   operation = MainWindow.ProccesorOperations.RegisterTimeManager,
                   target = this
               }
            );


            
            UpdateAutomatStatus();
            UpdateDataList(ref Tracker);
        }

        public void UpdateAutomatStatus()
        {
            Dispatcher.BeginInvoke(((Action)(() =>
            {
                // determiante is device is connected
                if (MainWindow.GetDeviceStatus() == MainWindow.DeviceStatus.Connected)
                {
                    InputDeviceConnector.Content = "Device connected at " + MainWindow.GetDevicePort();
                }
                else
                {
                    InputDeviceConnector.Content = string.Empty;
                }
            })));
        }

        public void DissposeDialog(TimeSpan time)
        {
            if (StatusRace == 1 || StatusRace == 2)
            {
                colMsg(time);
            }
        }

        public void OpenTimeCollector(TimeSpan time)
        {
                try
                {
                    // use local thread to open dialog, STA required
                    Dispatcher.BeginInvoke(((Action)(() =>
                    {
                        TimeCollection col = new TimeCollection(ref _race, time);
                        col.Show();
                    })));
                }
                catch (Exception)
                {

                }
            
        }

        public void SetLanguage()
        {
            var language_ = Resources["lang"] as TimekeepingLanguage;
            language_.Reload();
            Dispatcher.BeginInvoke(((Action)(() =>
            {
                this.Title = language_.timekeeping_title;
            })));
        }

        public void UpdateDataList(ref Race data)
        {
            List<Race.RaceLaps> dd = data.GetLaps();
            Dispatcher.BeginInvoke(((Action)(() =>
            {
                RacersInRace.ItemsSource = null;
                RacersInRace.ItemsSource = dd;
            })));
        }

        protected override void OnClosing(CancelEventArgs e)
        {

            MainWindow.UnRegisterLanguageHandler(SetLanguage);
            this.Hide();
            //base.OnClosing(e);
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
            (Application.Current.MainWindow as MainWindow).RequestOperation(
               new MainWindow.ProccesorComand
               {
                   operation = MainWindow.ProccesorOperations.DissposeRecord,
                   args = string.Empty,
                   target = this
               }
            );
        }
        
        private void RacersInRace_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
