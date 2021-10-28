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
    public class Add_New_Car_Language : INotifyPropertyChanged
    {
        
        public string car_title
        {
            get { return Regularity_Rally.Properties.Resources.ancar_car_title; }
        }        
        public string name_car_label_
        {
            get { return Regularity_Rally.Properties.Resources.ancar_name_car_label_; }
        }        
        public string model_car_label_
        {
            get { return Regularity_Rally.Properties.Resources.ancar_model_car_label_; }
        }       
        public string year_car_label_
        {
            get { return Regularity_Rally.Properties.Resources.ancar_year_car_label_; }
        }       
        public string vin_car_label_
        {
            get { return Regularity_Rally.Properties.Resources.ancar_vin_car_label_; }
        }       
        public string save_car_btn_
        {
            get { return Regularity_Rally.Properties.Resources.ancar_save_car_btn_; }
        }       
        public string delete_car_btn_
        {
            get { return Regularity_Rally.Properties.Resources.ancar_delete_car_btn_; }
        }       
        public string cancel_car_btn_
        {
            get { return Regularity_Rally.Properties.Resources.ancar_cancel_car_btn_; }
        } 


        public void Reload()
        {
            OnPropertyRaised("car_title");
            OnPropertyRaised("name_car_label_");
            OnPropertyRaised("model_car_label_");
            OnPropertyRaised("year_car_label_");
            OnPropertyRaised("vin_car_label_");
            OnPropertyRaised("save_car_btn_");
            OnPropertyRaised("delete_car_btn_");
            OnPropertyRaised("cancel_car_btn_");
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

    public class CheckVinInCar
    {
        public int CountVIN { get; set; }
    }

    public enum CarOperationType
    {
        Add,
        Update
    }
    /// <summary>
    /// Interaction logic for Add_new_car.xaml
    /// </summary>
    public partial class Add_new_car : Window
    {
        //public EventHandler SetLang;
        private CarOperationType CurrentOperation = CarOperationType.Add;
        Cars parent = null;
        CarView data = null;
        User LogUser = MainWindow.GetUser();

        public Add_new_car(Cars parent, CarOperationType operation, object data)
        {

            // preprate window
            InitializeComponent();
            this.Title = (Resources["lang"] as Add_New_Car_Language).car_title;

            MainWindow.RegisterLanguageHandler(Set_Language);

            //MainWindow.GetUser();
            if ((LogUser.Permission) <= UserPermission.Update) { delete_car_btn.Visibility = Visibility.Hidden; }
            if ((LogUser.Permission) == UserPermission.View) { save_car_btn.Visibility = Visibility.Hidden; }
            // reister operations 
            this.CurrentOperation = operation;
            this.parent = parent;
            this.data = data as CarView;
            SetOperation(operation, this.data);
        }

        protected override void OnClosed(EventArgs e)
        {
            // unregister
            MainWindow.UnRegisterLanguageHandler(Set_Language);
            base.OnClosed(e);
        }

        public void Set_Language()
        {
            var language_ = Resources["lang"] as Add_New_Car_Language;
            language_.Reload();
            Dispatcher.BeginInvoke(((Action)(() =>
            {
                this.Title = language_.car_title;
            })));
        }

        private void SetOperation(CarOperationType operation, CarView data)
        {
            switch(operation)
            {
                case CarOperationType.Add:
                    delete_car_btn.Visibility = Visibility.Hidden;
                    break;
                case CarOperationType.Update:

                    Dispatcher.BeginInvoke(((Action)(() =>
                    {
                        name_car_txt_.Text = data.Brand;
                        model_car_txt_.Text = data.Model;
                        year_car_txt_.Text = data.Year.ToString();
                        vin_car_txt_.Text = data.Vin;
                    })));
                    break;
            }
            // try bring window in fron in case hasn't been invoked yet
            try
            {
                this.Show();
            }
            catch { }
        }

        private void SaveCarBtnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                int CarID = (CurrentOperation == CarOperationType.Add) ? 0 : data.ID;
                string _brand = MySqlHelper.EscapeString(name_car_txt_.Text);
                string _model = MySqlHelper.EscapeString(model_car_txt_.Text);
                string _year = MySqlHelper.EscapeString(year_car_txt_.Text);
                string _vin = MySqlHelper.EscapeString(vin_car_txt_.Text);

                MySqlCommand check_VIN = Database.Database.CreateCommand(string.Format(Database.Database.QueryStack["Check_CarVIN"], _vin));
                Int32 rows_count = Convert.ToInt32(check_VIN.ExecuteScalar());
                if(rows_count>0) { MessageBox.Show("Already exist!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning); }

                MySqlCommand car_cmd = (CarID == 0) ?
                        Database.Database.CreateCommand(string.Format(Database.Database.QueryStack["AddCarData"],
                        _brand,
                        _vin,
                        _model,
                        _year))
                        :   //L
                        Database.Database.CreateCommand(string.Format(Database.Database.QueryStack["RepCarData"],
                        CarID,
                        _brand,
                        _vin,
                        _model,
                        _year));

                    int retcc = car_cmd.ExecuteNonQuery();
            }
            catch { }
            parent.Refresh();
            this.Close();
        }

        private void DeleteCarBtnClick(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Delete Record?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Warning) != MessageBoxResult.No)
            {
                try
                {
                    MySqlCommand command = Database.Database.CreateCommand(
                        string.Format(Database.Database.QueryStack["RemCarData"],
                        data.ID
                        ));
                    int RowEffected = command.ExecuteNonQuery();
                }
                catch { }
                parent.Refresh();
                this.Close();
            }
        }

        private void CancelCarBtnClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void CarotSet(object sender, RoutedEventArgs e)
        {
            if (EditingCommands.MoveToLineEnd.CanExecute(null, sender as IInputElement))
            {
                EditingCommands.MoveToLineEnd.Execute(null, sender as IInputElement);
            }
        }
    }
    
}
