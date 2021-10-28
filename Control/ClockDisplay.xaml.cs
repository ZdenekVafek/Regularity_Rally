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
using System.Windows.Threading;

namespace Regularity_Rally.Control
{
    /// <summary>
    /// Interaction logic for ClockDisplay.xaml
    /// </summary>
    public partial class ClockDisplay : UserControl
    {
        public ClockDisplay()
        {
            InitializeComponent();
            StartClock();
        }

        private void StartClock()
        {
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(100);
            timer.Tick += Tickevent;
            timer.Start();
        }

        void Tickevent(object sender, EventArgs e)
        {
            DateData.Text = DateTime.Now.Date.ToString();
            TimeData.Text = string.Format("{0}:{1}:", DateTime.Now.Hour, DateTime.Now.Minute);
            TimeSecData.Text = DateTime.Now.Second.ToString();
        }
    }
}

