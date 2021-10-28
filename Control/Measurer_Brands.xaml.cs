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
    public class Measurer_Brands_Language : INotifyPropertyChanged
    {

        public string brand_head
        {
            get { return Properties.Resources.measurerbrand_brand_head; }
        }
        public string Address_head
        {
            get { return Properties.Resources.measurerbrand_Address_head; }
        }
        public string Telephone_head
        {
            get { return Properties.Resources.measurerbrand_Telephone_head; }
        }
        public string Email_head
        {
            get { return Properties.Resources.measurerbrand_Email_head; }
        }
        public string Web_head
        {
            get { return Properties.Resources.measurerbrand_Web_head; }
        }
        public string addbrand_btn_
        {
            get { return Properties.Resources.measurerbrand_addbrand_btn_; }
        }
        public string closebrand_btn_
        {
            get { return Properties.Resources.measurerbrand_closebrand_btn_; }
        }
        public string meas_title
        {
            get { return Properties.Resources.measurerbrand_meas_title; }
        }
        public string print_btn_
        {
            get { return Properties.Resources.print_btn_; }
        }


        public void Reload()
        {
            OnPropertyRaised("brand_head");
            OnPropertyRaised("print_btn_");
            OnPropertyRaised("Address_head");
            OnPropertyRaised("Telephone_head");
            OnPropertyRaised("Email_head");
            OnPropertyRaised("Web_head");
            OnPropertyRaised("addbrand_btn_");
            OnPropertyRaised("closebrand_btn_");
            OnPropertyRaised("meas_title");
        }

        public Measurer_Brands_Language()
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

    public class BrandView
    {
        public int ID { get; set; }
        public string Company { get; set; }
        public string Address { get; set; }
        public string Telephone { get; set; }
        public string Email { get; set; }
        public string Web { get; set; }


        public bool Compare(BrandView _ref)
        {
            if (_ref == null)
                return false;

            return (_ref.Company == this.Company && _ref.Address == this.Address && _ref.Telephone == this.Telephone && _ref.Email == this.Email && _ref.Web == this.Web);
        }
    }

    /// <summary>
    /// Interaction logic for Measurer_Brands.xaml
    /// </summary>
    public partial class Measurer_Brands : Window
    {
        //Language
        //public EventHandler SetLang;
        private Add_new_Meascom UpdateWindow = null;

        //Containers dor selection
        private List<BrandView> BrandItem = new List<BrandView>();
        User LogUser = MainWindow.GetUser();

        public Measurer_Brands()
        {
            InitializeComponent();
            this.Title = (Resources["lang"] as Measurer_Brands_Language).meas_title;
            MainWindow.RegisterLanguageHandler(SetLanguage);
            UpdateBrandList(string.Empty);
            if (LogUser.Permission < UserPermission.Update) { AddCom_btn.Visibility = Visibility.Hidden; }
        }


        public void SetLanguage()
        {
            var language_ = Resources["lang"] as Measurer_Brands_Language;
            language_.Reload();
            Dispatcher.BeginInvoke(((Action)(() =>
            {
                this.Title = language_.meas_title;
            })));
        }

        protected override void OnClosed(EventArgs e)
        {
            Close_Com_Click(null, null);
            MainWindow.UnRegisterLanguageHandler(SetLanguage);
            base.OnClosed(e);
        }

        private void OnCarDialogUpdate(object sender, EventArgs args)
        {
            UpdateBrandList(string.Empty);
        }

        private void BrandList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Measurer_Brands_List.SelectedItem != null)
            {
                BrandView _t = LastBrandSelectedItem;
                LastBrandSelectedItem = Measurer_Brands_List.SelectedItem as BrandView;
            }
        }

        private BrandView LastBrandSelectedItem = null;

        private void UpdateBrandList(string filter)
        {
            Dispatcher.BeginInvoke(((Action)(() =>
            {
                action_search.Visibility = Visibility.Visible;
            })));

            Measurer_Brands_List.ItemsSource = BrandItem;
            List<BrandView> _items = new List<BrandView>();
            int SelectedIndex = 0;
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
                rdr.Close();

                BrandItem.Clear();
                BrandItem = _items;
                Dispatcher.BeginInvoke(((Action)(() =>
                {
                    // rebind data value
                    Measurer_Brands_List.ItemsSource = null;
                    Measurer_Brands_List.ItemsSource = BrandItem;
                    Measurer_Brands_List.SelectedIndex = SelectedIndex;
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
                UpdateBrandList(string.Empty);
            })));
        }

        private void Add_New_Com_Click(object sender, RoutedEventArgs e)
        {
            if (this.UpdateWindow != null)
                this.UpdateWindow.Close();

            this.UpdateWindow = null;
            this.UpdateWindow = new Add_new_Meascom(this, MeasurementOperationType.Add, null);
        }

        private void Close_Com_Click(object sender, RoutedEventArgs e)
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

        private void Measurer_Brands_List_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (this.UpdateWindow != null)
                this.UpdateWindow.Close();

            if (LogUser.Permission > UserPermission.View)
            {
                this.UpdateWindow = null;
                this.UpdateWindow = new Add_new_Meascom(this, MeasurementOperationType.Update, LastBrandSelectedItem);
            }
        }

        private void Print_btn_Click(object sender, RoutedEventArgs e)
        {
            PrintManager print = new PrintManager();
            print.Print(PrintManager.PrintType.Brands);
        }
    }
}
