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
    public class Cars_Language : INotifyPropertyChanged
    {

        public string brandcar_head
        {
            get { return Properties.Resources.car_brandcar_head; }
        }
        public string model_head
        {
            get { return Properties.Resources.car_model_head; }
        }
        public string year_head
        {
            get { return Properties.Resources.car_year_head; }
        }
        public string vin_head
        {
            get { return Properties.Resources.car_vin_head; }
        }
        public string addcar_btn_
        {
            get { return Properties.Resources.car_addcar_btn_; }
        }
        public string closecar_btn_
        {
            get { return Properties.Resources.car_closecar_btn_; }
        }
        public string car_title
        {
            get { return Properties.Resources.car_car_title; }
        }
        public string print_btn_
        {
            get { return Properties.Resources.print_btn_; }
        }

        public void Reload()
        {
            OnPropertyRaised("brandcar_head");
            OnPropertyRaised("model_head");
            OnPropertyRaised("year_head");
            OnPropertyRaised("vin_head");
            OnPropertyRaised("addcar_btn_");
            OnPropertyRaised("closecar_btn_");
            OnPropertyRaised("car_title");
            OnPropertyRaised("print_btn_");
            


        }

        public Cars_Language()
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


    public class CarView
    {
        public int ID { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public string Vin { get; set; }

        public string Identifier { get { return string.Format("{0} ( {1}, {2} )", this.Brand, this.Model, this.Vin); } }

        public bool Compare(CarView _ref)
        {
            if (_ref == null)
                return false;

            return (_ref.Brand == this.Brand && _ref.Model == this.Model && _ref.Vin == this.Vin && _ref.Year == this.Year);
        }
    }

    /// <summary>
    /// Interaction logic for Cars.xaml
    /// </summary>
    public partial class Cars : Window
    {
        private List<CarView> CarsItems = new List<CarView>();
        private Add_new_car UpdateWindow = null;
        User LogUser = MainWindow.GetUser();

        public Cars()
        {
            InitializeComponent();
            this.Title = (Resources["lang"] as Cars_Language).car_title;
            MainWindow.RegisterLanguageHandler(SetLanguage);
            UpdateCarList(string.Empty);
            if (LogUser.Permission < UserPermission.Update){ AddCar_btn.Visibility = Visibility.Hidden;}
        }

        protected override void OnClosed(EventArgs e)
        {
            Close_Car_Click(null, null);
            MainWindow.UnRegisterLanguageHandler(SetLanguage);
            base.OnClosed(e);
        }

        public void SetLanguage()
        {
            var language_ = Resources["lang"] as Cars_Language;
            language_.Reload();
            Dispatcher.BeginInvoke(((Action)(() =>
            {
                this.Title = language_.car_title;
            })));
        }

        public void Refresh()
        {
            Dispatcher.BeginInvoke(((Action)(() =>
            {
                UpdateCarList(string.Empty);
            })));
        }

        public List<EventHandler> CarSelectionUpdateNotify = new List<EventHandler>();
        public CarView LastCarSelectedItem = null;

        private void CarList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CarList.SelectedItem != null)
            {
                CarView _t = LastCarSelectedItem;
                LastCarSelectedItem = CarList.SelectedItem as CarView;
                if (_t != null)
                {
                    if (LastCarSelectedItem.ID != _t.ID)
                    {
                        foreach (EventHandler _event in CarSelectionUpdateNotify)
                        {
                            if (_event != null)
                                _event.BeginInvoke(null, null, null, null);
                        }
                    }
                }
            }
        }

        private void UpdateCarList(string filter)
        {
            Dispatcher.BeginInvoke(((Action)(() =>
            {
                action_search.Visibility = Visibility.Visible;
            })));

            CarList.ItemsSource = CarsItems;
            List<CarView> _items = new List<CarView>();
            int SelectedIndex = 0;
            MySqlDataReader rdr = null;
            try
            {
                // load data base on filter
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
                rdr.Close();

                CarsItems.Clear();
                CarsItems = _items;
                Dispatcher.BeginInvoke(((Action)(() =>
                {
                    // rebind data value
                    CarList.ItemsSource = null;
                    CarList.ItemsSource = CarsItems;
                    CarList.SelectedIndex = SelectedIndex;
                })));
            }
            catch
            {
                if (rdr != null && !rdr.IsClosed)
                    rdr.Close();
            }
        }

        private void CarList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (this.UpdateWindow != null)
                this.UpdateWindow.Close();

            if(LogUser.Permission > UserPermission.View)
            {
                this.UpdateWindow = null;
                this.UpdateWindow = new Add_new_car(this, CarOperationType.Update, LastCarSelectedItem);
            }
        }

        private void Close_Car_Click(object sender, RoutedEventArgs e)
        {
            if (this.UpdateWindow != null)
            {
                this.UpdateWindow.Close();
                this.UpdateWindow = null;
            }
            this.Close();
        }


        private void Add_New_Car_Click(object sender, RoutedEventArgs e)
        {
            if (this.UpdateWindow != null)
                this.UpdateWindow.Close();

            this.UpdateWindow = null;
            this.UpdateWindow = new Add_new_car(this, CarOperationType.Add, null);
        }

        private void PrintCar_btn_Click(object sender, RoutedEventArgs e)
        {
            PrintManager print = new PrintManager();
            print.Print(PrintManager.PrintType.Cars);
        }
    }
}
