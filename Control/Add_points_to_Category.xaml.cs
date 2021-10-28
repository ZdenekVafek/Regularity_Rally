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
    public enum AddPointsCatOperationType
    {
        Add,
        Update
    }

    public class ExistPointID
    {
        public int ID { get; set; }
    }

    public partial class Add_points_to_Category : Window
    {
        AddPointsCatOperationType PointsOperation = AddPointsCatOperationType.Add;
        private List<PointView> PointsForChange = new List<PointView>();
        private List<ExistPointID> PointIDList = new List<ExistPointID>();
        int CategoryID = 0;
        Add_new_Category parent;

        public Add_points_to_Category(Add_new_Category parent, int CatID) 
        {
            InitializeComponent();
            this.Title = (Resources["lang"] as Add_New_Category_Language).catPoints_title;
            MainWindow.RegisterLanguageHandler(Set_Language);
            this.CategoryID = CatID;
            this.parent = parent;

            UpdatePointTableToCategory(CategoryID);
        }

        public void Set_Language()
        {
            var language_ = Resources["lang"] as Add_New_Category_Language;
            language_.Reload();
            Dispatcher.BeginInvoke(((Action)(() =>
            {
                this.Title = language_.catPoints_title;
            })));
        }

        private void UpdatePointTableToCategory(int s_id)
        {
            PointsForChange.Clear();
            try
            {
                MySqlCommand command = Database.Database.CreateCommand(string.Format(Database.Database.QueryStack["GetPointData"], s_id));
                MySqlDataReader rdr = command.ExecuteReader();

                while (rdr.Read())
                {
                    PointsForChange.Add(
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
            catch { }  

            Dispatcher.BeginInvoke(((Action)(() =>
            {
                AddPointInCat.ItemsSource = null;
                AddPointInCat.ItemsSource = PointsForChange;
                AddPointInCat.SelectedIndex = 0;
            })));
        }

        public PointView LastSelectedItem = null;
        private void PointInCat_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (AddPointInCat.SelectedItem != null)
            {
                PointView _t = LastSelectedItem;
                LastSelectedItem = AddPointInCat.SelectedItem as PointView;
            }
        }

        private void CarotSet(object sender, RoutedEventArgs e)
        {
            if (EditingCommands.MoveToLineEnd.CanExecute(null, sender as IInputElement))
            {
                EditingCommands.MoveToLineEnd.Execute(null, sender as IInputElement);
            }
        }

        private void AddPointInCat_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            PlaceInCat.Text = LastSelectedItem.Position.ToString();
            PointsInCat.Text = LastSelectedItem.Points.ToString();
            PointsOperation = AddPointsCatOperationType.Update;
        }

        private void SavePointsToCat_btn_Click(object sender, RoutedEventArgs e)
        {
            int PointID = 0;
            try
            {
                string _pos = MySqlHelper.EscapeString(PlaceInCat.Text);
                MySqlCommand point_cmd = Database.Database.CreateCommand(string.Format(Database.Database.QueryStack["Check_IDPosition"], CategoryID, _pos));
                MySqlDataReader rdr_point = point_cmd.ExecuteReader();

                while (rdr_point.Read())
                {
                    PointIDList.Add(
                        new ExistPointID()
                        {
                            ID = rdr_point.GetInt32("ID_Category_Points")
                        }
                    );
                }
                rdr_point.Close();
                int retccpoints = point_cmd.ExecuteNonQuery();

                foreach (ExistPointID FindID in PointIDList)
                {
                    PointID = FindID.ID;
                }
            }
            catch { }

            try
            {

                int PointsID = (PointsOperation == AddPointsCatOperationType.Add) ? 0 : LastSelectedItem.ID;
                string _Position = MySqlHelper.EscapeString(PlaceInCat.Text);
                string _Points = MySqlHelper.EscapeString(PointsInCat.Text);
                int _CategoryID = CategoryID;

                MySqlCommand check_pos = Database.Database.CreateCommand(string.Format(Database.Database.QueryStack["Check_CatPosition"], _CategoryID, _Position));
                Int32 rows_count = Convert.ToInt32(check_pos.ExecuteScalar());
                if (rows_count > 0)
                {
                    MySqlCommand points_cmd = Database.Database.CreateCommand(string.Format(Database.Database.QueryStack["RepPointData"],
                    PointID,
                    _CategoryID,
                    _Position,
                    _Points
                    ));
                    int retcc = points_cmd.ExecuteNonQuery();
                }
                else
                {
                    MySqlCommand points_cmd = Database.Database.CreateCommand(string.Format(Database.Database.QueryStack["AddPointData"],
                    _Position,
                    _Points,
                    _CategoryID
                    ));
                    int retcc = points_cmd.ExecuteNonQuery();
                }
            }
            catch { }
            UpdatePointTableToCategory(CategoryID);
            parent.Refresh();
        }

        private void DeletePointsToCat_btn_Click(object sender, RoutedEventArgs e)
        {
            MySqlCommand rc = Database.Database.CreateCommand(string.Format(Database.Database.QueryStack["RemPointCatData"], LastSelectedItem.ID));
            rc.ExecuteNonQuery();
            UpdatePointTableToCategory(CategoryID);
            parent.Refresh();
        }

        private void CancelPointsToCat_btn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            parent.Refresh();
        }
    }
}
