using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;
using Regularity_Rally.Control;
using System.Threading.Tasks;
using System.Windows.Documents;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Text.RegularExpressions;

namespace Regularity_Rally.Control
{
    #region Language
    public class Add_New_Category_Language : INotifyPropertyChanged
    {
        public string name_category_label_
        {
            get { return Properties.Resources.adc_name_category_label; }
        }
        public string min_racerscategory_label_
        {
            get { return Properties.Resources.adc_min_racerscategory_label; }
        }
        public string coef_cut_points_racerscategory_label_
        {
            get { return Properties.Resources.adc_coef_cut_points_racerscategory_label; }
        }
        public string save_category_btn_
        {
            get { return Properties.Resources.adc_save_category_btn; }
        }
        public string delete_category_btn_
        {
            get { return Properties.Resources.adc_delete_category_btn; }
        }
        public string assignpoints_category_btn_
        {
            get { return Properties.Resources.adc_assignpoints_category_btn; }
        }
        public string place_category_label_
        {
            get { return Properties.Resources.adc_place_category_label; }
        }
        public string points_category_label_
        {
            get { return Properties.Resources.adc_points_category_label; }
        }
        public string cancel_category_btn_
        {
            get { return Properties.Resources.adc_cancel_category_btn_; }
        }
        public string cat_title
        {
            get { return Properties.Resources.adc_cat_title; }
        }
        public string CountOfPosition_racerscategory_label_
        {
            get { return Properties.Resources.adc_CountOfPosition_racerscategory_label_; }
        }
        public string catPoints_title
        {
            get { return Properties.Resources.adc_catPoints_title; }
        }


        public Add_New_Category_Language()
        {

        }

        public void Reload()
        {
            OnPropertyRaised("name_category_label_");
            OnPropertyRaised("catPoints_title");
            OnPropertyRaised("CountOfPosition_racerscategory_label_");
            OnPropertyRaised("min_racerscategory_label_");
            OnPropertyRaised("coef_cut_points_racerscategory_label_");
            OnPropertyRaised("save_category_btn_");
            OnPropertyRaised("delete_category_btn_");
            OnPropertyRaised("assignpoints_category_btn_");
            OnPropertyRaised("place_category_label_");
            OnPropertyRaised("points_category_label_");
            OnPropertyRaised("cancel_category_btn_");
            OnPropertyRaised("cat_title");
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

    public enum CategoryOperationType
    {
        Add,
        Update
    }

    /// <summary>
    /// Interaction logic for Add_new_Category.xaml
    /// </summary>
    public partial class Add_new_Category : Window
    {
        private CategoryOperationType CurrentOperation = CategoryOperationType.Add;
        Category parent = null;
        CatView data = null;
        User LogUser = MainWindow.GetUser();
        int CountScoredPossition = 0;
        int CatID = 0;

        private List<PointView> pointList = new List<PointView>();

        public Add_new_Category(Category parent, CategoryOperationType operation, object data)
        {
            InitializeComponent();
            this.Title = (Resources["lang"] as Add_New_Category_Language).cat_title;

            MainWindow.RegisterLanguageHandler(Set_Language);

            if ((LogUser.Permission) <= UserPermission.Update ) { action_delete.Visibility = Visibility.Hidden; }
            if ((LogUser.Permission) <= UserPermission.View ) { SaveCat_btn.Visibility = Visibility.Hidden; }
            if ((LogUser.Permission) <= UserPermission.View ) { PointsAdd.Visibility = Visibility.Hidden; }
            // reister operations 
            this.CurrentOperation = operation;
            this.parent = parent;
            this.data = data as CatView;
            SetOperation(operation, this.data);
            if(CatID == 0) { PointsAdd.Visibility = Visibility.Hidden; }
        }

        public void Set_Language()
        {
            //Console.WriteLine("Add_new_Category: language update");
            var language_ = Resources["lang"] as Add_New_Category_Language;
            language_.Reload();
            Dispatcher.BeginInvoke(((Action)(() =>
            {
                this.Title = language_.cat_title;
            })));
        }

        protected override void OnClosed(EventArgs e)
        {
            MainWindow.UnRegisterLanguageHandler(Set_Language);
            base.OnClosed(e);
        }

        private void SetOperation(CategoryOperationType operation, CatView data)
        {
            switch (operation)
            {
                case CategoryOperationType.Add:
                    // new window is opened each operation no need to clean up 
                    break;
                case CategoryOperationType.Update:
                    Dispatcher.BeginInvoke(((Action)(() =>
                    {
                        CatName.Text = data.Name;
                        CatLimit.Text = data.Limit.ToString();
                        CatCoef.Text = data.Coef.ToString();
                    })));
                    UpdatePointTable(data.ID);
                    UpdateCountOfPos(data.ID);
                    CatID = Category.LastCatSelectedItem.ID;
                    break;
            }
            try
            {
                this.Show();
            }
            catch { }
        }

        public void UpdateCountOfPos(int _ID)
        {
            MySqlCommand countofscoredpos = Database.Database.CreateCommand(string.Format(Database.Database.QueryStack["CountScorPos"], _ID));
            CountScoredPossition = Convert.ToInt32(countofscoredpos.ExecuteScalar());
            CountPos.Text = CountScoredPossition.ToString();
        }

        public void Refresh()
        {
            Dispatcher.BeginInvoke(((Action)(() =>
            {
                UpdatePointTable(data.ID);
                UpdateCountOfPos(data.ID);
                parent.Refresh();
            })));
        }

        private void UpdatePointTable(int s_id)
        {
            pointList.Clear();

            try
            {
                // load data base on filter
                MySqlCommand command = Database.Database.CreateCommand(string.Format(Database.Database.QueryStack["GetPointData"], s_id));
                MySqlDataReader rdr = command.ExecuteReader();

                while (rdr.Read())
                {
                    pointList.Add(
                        new PointView()
                        {
                            ID = rdr.GetInt32("ID_Category_Points"),
                            Position = rdr.GetInt32("Position"),
                            Points = rdr.GetInt32("Points"),
                        }
                    );
                }
                rdr.Close();
            }
            catch { }   // no need to worry

            Dispatcher.BeginInvoke(((Action)(() =>
            {
                PointInCat.ItemsSource = null;
                PointInCat.ItemsSource = pointList;
                PointInCat.SelectedIndex = 0;
            })));
        }

        private void Save_Category_Click(object sender, RoutedEventArgs e)
        {
            SaveData();
            this.Close();
        }

        private void SaveData()
        {
            try
            {
                int CategoryID = CatID;
                string _catname = MySqlHelper.EscapeString(CatName.Text);
                int _limit = 0;
                float _coef = 0;
                try
                {   // direct parse exeption catch
                    _coef = float.Parse(CatCoef.Text, CultureInfo.InvariantCulture.NumberFormat);
                    int.TryParse(CatLimit.Text, out _limit);
                }
                catch { }

                MySqlCommand cat_cmd = (CategoryID == 0) ?
                    Database.Database.CreateCommand(string.Format(Database.Database.QueryStack["AddCatData"],
                    _catname,
                    _limit,
                    _coef.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture)))
                    :   //L
                    Database.Database.CreateCommand(string.Format(Database.Database.QueryStack["RepCatData"],
                    CategoryID,
                    _catname,
                    _limit,
                    _coef.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture)));
                //if (CategoryID == 0)
                //{
                //    CatID = (int)cat_cmd.LastInsertedId;
                //}

                int retcc = cat_cmd.ExecuteNonQuery();
            }
            catch { }
            parent.Refresh();
            //this.Hide();
        }

        private void Delete_Category_Click(object sender, RoutedEventArgs e)
        {
            //delete record from database
            if (MessageBox.Show("Delete Record?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Warning) != MessageBoxResult.No)
            {
                try
                {
                    MySqlCommand rc = Database.Database.CreateCommand(string.Format(Database.Database.QueryStack["RemPointData"], data.ID));
                    rc.ExecuteNonQuery();
                }
                catch { }
                try
                {
                    // rem main record
                    MySqlCommand command = Database.Database.CreateCommand(string.Format(Database.Database.QueryStack["RemCatData"], data.ID));
                    int RowEffected = command.ExecuteNonQuery();
                }
                catch { }
                parent.Refresh();
                this.Hide();
            }
        }

        private void Cancel_Category_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void action_add_Click(object sender, RoutedEventArgs e)
        {
            SaveData();
            Add_points_to_Category AddPointsDataWindow = new Add_points_to_Category(this, CatID);
            AddPointsDataWindow.Show();
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

