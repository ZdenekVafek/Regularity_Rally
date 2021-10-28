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
    public class Category_Language : INotifyPropertyChanged
    {

        public string namecategory_head
        {
            get { return Properties.Resources.category_namecategory_head; }
        }
        public string positionpoints_head
        {
            get { return Properties.Resources.category_positionpoints_head; }
        }
        public string points_head
        {
            get { return Properties.Resources.category_points_head; }
        }
        public string limitcategory_head
        {
            get { return Properties.Resources.category_limitcategory_head; }
        }
        public string coefcategory_head
        {
            get { return Properties.Resources.category_coefcategory_head; }
        }

        public string addcat_btn_
        {
            get { return Properties.Resources.category_addcat_btn_; }
        }
        public string closecat_btn_
        {
            get { return Properties.Resources.car_closecat_btn_; }
        }
        public string cat_title
        {
            get { return Properties.Resources.cat_cat_title; }
        }
        public string print_btn_
        {
            get { return Properties.Resources.print_btn_; }
        }

        public void Reload()
        {
            OnPropertyRaised("namecategory_head");
            OnPropertyRaised("print_btn_");
            OnPropertyRaised("positionpoints_head");
            OnPropertyRaised("points_head");
            OnPropertyRaised("addcat_btn_");
            OnPropertyRaised("closecat_btn_");
            OnPropertyRaised("limitcategory_head");
            OnPropertyRaised("coefcategory_head");
            OnPropertyRaised("cat_title");




        }

        public Category_Language()
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


    public class CatView
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int Limit { get; set; }
        public double Coef { get; set; }

        //public string Identifier { get { return string.Format("{0} ( {1}, {2} )", this.Name, this.Limit, this.Coef); } }

        public bool Compare(CatView _ref)
        {
            if (_ref == null)
                return false;

            return (_ref.Name == this.Name && _ref.Limit == this.Limit && _ref.Coef == this.Coef);
        }
    }

    public class PointView
    {
        public int ID { get; set; }
        public int Position { get; set; }
        public int Points { get; set; }

        public bool Compare(PointView _ref)
        {
            if (_ref == null)
                return false;

            return (_ref.Position == this.Position && _ref.Points == this.Points);
        }
    }

    /// <summary>
    /// Interaction logic for Category.xaml
    /// </summary>
    public partial class Category : Window
    {
        // Containers for Selections
        private static List<CatView> CategoryItems = new List<CatView>();
        private static List<PointView> PointsItems = new List<PointView>();
        private Add_new_Category UpdateWindow = null;
        User LogUser = MainWindow.GetUser();

        public Category()
        {
            InitializeComponent();
            this.Title = (Resources["lang"] as Category_Language).cat_title;
            MainWindow.RegisterLanguageHandler(SetLanguage);

            UpdateCatList(string.Empty);
            UpdatePointList(LastCatSelectedItem);
            if (LogUser.Permission < UserPermission.Update) { AddCat_btn.Visibility = Visibility.Hidden; }
        }

        public void SetLanguage()
        {
            var language_ = Resources["lang"] as Category_Language;
            language_.Reload();
            Dispatcher.BeginInvoke(((Action)(() =>
            {
                this.Title = language_.cat_title;
            })));
        }

        protected override void OnClosed(EventArgs e)
        {
            Close_Cat_Click(null, null);

            // unregister
            MainWindow.UnRegisterLanguageHandler(SetLanguage);
            base.OnClosed(e);
        }

        public void Refresh()
        {
            Dispatcher.BeginInvoke(((Action)(() =>
            {
                UpdateCatList(string.Empty);
            })));
        }

        public static CatView LastCatSelectedItem = null;
        private void CatList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LastCatSelectedItem = (CategoryList.SelectedItem != null) ? (CategoryList.SelectedItem as CatView) : LastCatSelectedItem;
            Task task = new Task(() => UpdatePointList(LastCatSelectedItem));
            task.Start();
        }

        private void UpdateCatList(string filter)
        {
            Dispatcher.BeginInvoke(((Action)(() =>
            {
                action_search.Visibility = Visibility.Visible;
            })));

            CategoryList.ItemsSource = CategoryItems;
            List<CatView> _items = new List<CatView>();
            int SelectedIndex = 0;
            MySqlDataReader rdr = null;
            try
            {
                // load data base on filter
                MySqlCommand command = Database.Database.CreateCommand(Database.Database.QueryStack["GetCatData"]);
                rdr = command.ExecuteReader();

                while (rdr.Read())
                {
                    _items.Add(new CatView()
                    {
                        ID = rdr.GetInt32("ID_Category"),
                        Name = rdr.GetString("Name"),
                        Limit = rdr.GetInt32("Limit"),
                        Coef = rdr.GetDouble("Coef"),

                    });
                }
                rdr.Close();

                foreach (CatView field in _items)
                {
                    Console.WriteLine(field.ID);
                }

                CategoryItems.Clear();
                CategoryItems = _items;
                Dispatcher.BeginInvoke(((Action)(() =>
                {
                    // rebind data value
                    CategoryList.ItemsSource = null;
                    CategoryList.ItemsSource = CategoryItems;
                    CategoryList.SelectedIndex = SelectedIndex;
                })));
            }
            catch
            {
                if (rdr != null && !rdr.IsClosed)
                    rdr.Close();
            }
        }
        
        private void UpdatePointList(CatView filter)
        {
            if (filter == null)
                return;

            Dispatcher.BeginInvoke(((Action)(() =>
            {
                action_search.Visibility = Visibility.Visible;
            })));


            PointsItems.Clear();
            MySqlDataReader rdr = null;
            try
            {
                // load data base on filter
                MySqlCommand command = Database.Database.CreateCommand(string.Format(Database.Database.QueryStack["GetPointData"], filter.ID));
                rdr = command.ExecuteReader();

                while (rdr.Read())
                {
                    PointsItems.Add(
                        new PointView()
                        {
                            ID = rdr.GetInt32("ID_Category_Points"),
                            Position = rdr.GetInt32("Position"),
                            Points = rdr.GetInt32("Points")
                        }
                    );
                }
                rdr.Close();
            }
            catch
            {
                if (rdr != null && !rdr.IsClosed)
                    rdr.Close();
            }

            Dispatcher.BeginInvoke(((Action)(() =>
            {
                PointsList.ItemsSource = null;
                PointsList.ItemsSource = PointsItems;
                action_search.Visibility = Visibility.Hidden;
                if (PointsItems.Count != 0)
                    PointsList.SelectedItem = PointsItems[0];
            })));

        }

        private void CatList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            //if (MainWindow.GetUser().role != UserRole.Admin && User.role != UserRole.TrackCommissar)
            //    return;
            
            if (this.UpdateWindow != null)
                this.UpdateWindow.Close();

            // opened on selected index, just alovate object aand push data operation
            if (LogUser.Permission > UserPermission.View)
            {
                this.UpdateWindow = null;
                this.UpdateWindow = new Add_new_Category(this, CategoryOperationType.Update, LastCatSelectedItem);
            }
        }

        private void Add_New_Cat_Click(object sender, RoutedEventArgs e)
        {
            if (this.UpdateWindow != null)
                this.UpdateWindow.Close();

            this.UpdateWindow = null;
            this.UpdateWindow = new Add_new_Category(this, CategoryOperationType.Add, null);
        }

        private void Close_Cat_Click(object sender, RoutedEventArgs e)
        {
            if (this.UpdateWindow != null)
            {
                this.UpdateWindow.Close();
                this.UpdateWindow = null;
            }
            this.Close();
        }

        private void PrintCar_btn_Click(object sender, RoutedEventArgs e)
        {
            PrintManager print = new PrintManager();
            print.Print(PrintManager.PrintType.Category);
        }
    }
}
