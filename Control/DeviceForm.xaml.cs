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

namespace Regularity_Rally.Control
{
    #region Language
    public class DeviceForm_Language : INotifyPropertyChanged
    {
        public string Device_connection
        {
            get { return Properties.Resources.DF_Device_connection; }
        }
        public string Status_label
        {
            get { return Properties.Resources.DF_Status_label; }
        }
        public string Table_connection
        {
            get { return Properties.Resources.DF_Table_connection; }
        }

        public DeviceForm_Language()
        {

        }

        public void Reload()
        {
            OnPropertyRaised("Device_connection");
            OnPropertyRaised("Status_label");
            OnPropertyRaised("Table_connection");
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyRaised(string propertyname)
        {
            if(PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyname));
            }
        }
    }
#endregion

    public class ComPortsSelect
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }


    /// <summary>
    /// Interaction logic for DeviceForm.xaml
    /// </summary>
    public partial class DeviceForm : Window
    {
        public DeviceForm()
        {
            InitializeComponent();

            // feed com select
            List<ComPortsSelect> coms = new List<ComPortsSelect>();
            foreach (string port in MainWindow.GetPorts())
                coms.Add(new ComPortsSelect { Name = "Port", Value = port });

            COMPortsList.ItemsSource = coms;
            TableDisplayCOM.ItemsSource = coms;

            // determiante is port is selected
            if (MainWindow.GetDeviceStatus() == MainWindow.DeviceStatus.Connected)
            {
                COMPortsList.SelectedIndex = coms.FindIndex(ValMatch);
            }

            if (MainWindow.GetTableStatus() == MainWindow.DeviceStatus.Connected)
            {
                TableDisplayCOM.SelectedIndex = coms.FindIndex(ValMatchTable);
            }

            ListStatus();
        }

        private static bool ValMatch(ComPortsSelect rec)
        {
            return rec.Value == MainWindow.GetDevicePort();
        }

        private static bool ValMatchTable(ComPortsSelect rec)
        {
            return rec.Value == MainWindow.GetTablePort();
        }



        private void COMPortsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComPortsSelect sel = (COMPortsList.SelectedItem as ComPortsSelect);
            string port = sel.Value;

            if (port == MainWindow.GetDevicePort())
                return;

            // send port name into connector
            (Application.Current.MainWindow as MainWindow).RequestOperation(
                new MainWindow.ProccesorComand
                {
                    operation = MainWindow.ProccesorOperations.SetPort,
                    args = port,
                    target = this
                }
            );
        }

        public void ReloadStatus()
        {
            Dispatcher.BeginInvoke(((Action)(() =>
            {
                ListStatus();
            })));
        }

        private void ListStatus()
        {
            // print satus TODO: language support
            switch (MainWindow.GetDeviceStatus())
            {
                case MainWindow.DeviceStatus.Disconnected:
                    PortStatus.Text = "DISCONNECTED";
                    break;
                case MainWindow.DeviceStatus.Connecting:
                    PortStatus.Text = "CONNECTING..";
                    break;
                case MainWindow.DeviceStatus.Connected:
                    PortStatus.Text = "CONNECTED";
                    break;
            }

            // do it for table
            switch (MainWindow.GetTableStatus())
            {
                case MainWindow.DeviceStatus.Disconnected:
                    TableDisplayStatus.Text = "DISCONNECTED";
                    break;
                case MainWindow.DeviceStatus.Connecting:
                    TableDisplayStatus.Text = "CONNECTING..";
                    break;
                case MainWindow.DeviceStatus.Connected:
                    TableDisplayStatus.Text = "CONNECTED";
                    break;
            }
        }

        // table
        private void TableDisplayCOM_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComPortsSelect sel = (TableDisplayCOM.SelectedItem as ComPortsSelect);
            string port = sel.Value;

            if (port == MainWindow.GetTablePort())
                return;

            // send port name into connector
            (Application.Current.MainWindow as MainWindow).RequestOperation(
                new MainWindow.ProccesorComand
                {
                    operation = MainWindow.ProccesorOperations.SetTablePort,
                    args = port,
                    target = this
                }
            );
        }
    }
}
