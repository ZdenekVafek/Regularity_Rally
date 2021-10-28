using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Threading;
using Regularity_Rally.Control;

namespace Regularity_Rally
{
    class Regularity_Rally_Main : Application
    {
        public static Regularity_Rally_Main App;
        //definovani pristupu ke standartnimu typu modelu COM
        //vlakna jsou rozdilna, proto musi byt definovan pristup primo ke COM !!!DOHLEDAT!!!
        [STAThread]
        static void Main(string[] args)
        {
            // register updaraUII funstion from manin window later
            //Database.Database.RegisterConnectionNotify(OnConnection);

            //if(!Database.Database.isConnect())
            //{
            //    Database.Database.OpenConnection("127.0.0.1","root", "db_regularity_rally","3306","123456");
            //    Console.WriteLine("Connected");
            //}

            App = new Regularity_Rally_Main();

            //nastavení jazyka 
            var startlanguage = Settings.GetValue("language", "unk");

            switch (startlanguage)
            {
                case "en-US":
                    Regularity_Rally.Properties.Resources.Culture = new System.Globalization.CultureInfo("");
                    break;
                case "cs-CZ":
                    Regularity_Rally.Properties.Resources.Culture = new System.Globalization.CultureInfo("cs-CZ");
                    break;
                case "de-DE":
                    Regularity_Rally.Properties.Resources.Culture = new System.Globalization.CultureInfo("de-DE");
                    break;
                default:
                    Settings.SetValue("language", "cs-CZ");
                    Regularity_Rally.Properties.Resources.Culture = new System.Globalization.CultureInfo("cs-CZ");
                    break;

            }

            App.MainWindow = new Control.MainWindow();
            App.Run();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            App.MainWindow.Show();
            base.OnStartup(e);
        }

        //ukoncovaci funkce, vola se pri ukonceni programu a umoznuje zaverecny odalokovani zbyvajici pameti
        protected override void OnExit(ExitEventArgs e)
        {
            Database.Database.TerminateConnection();
            base.OnExit(e);
        }
    }
}
