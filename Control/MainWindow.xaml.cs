using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media.Animation;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using System.Timers;
using System.IO.Ports;
using System.Text;

namespace Regularity_Rally.Control
{
    public enum ApplicationStage : UInt16
    {
        Login,
        Homescreen,
        Cars,
        Measurer_Brands,
        Paddock,
        Races,
        Reporter_MainWindow,
        Teams,
        Tournaments,
        Users,
        Category,
        Racers,
        PortalWin
    };
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ApplicationStage CurentStage = ApplicationStage.Login;  // keep track on latest ui update

        public delegate void UpdateLanguage();

        public delegate void SendProccessorMessage(ProccesorComand cmd);

        // USER
        User login_user = null;

        // global interator fol holding all language update holders
        private static List<UpdateLanguage> LanguageUpdate = new List<UpdateLanguage>();

        public MainWindow()
        {
            InitializeComponent();
            // we wana make main window load default state, prediction wia sequence

            InternalMsg = new SendProccessorMessage(RequestOperation);

            // create thread thatis responncible for handlind sipose operation of timer events
            // from that thread i need to refreshui base on selected operation
            // define operation that will be sisposed to child suybsystems/ui threads
            ProccesorQueStart();
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            SetStage(CurentStage);
        }

        // "core" thread
        #region OperationsProccesor

        public enum ProccesorOperations : int
        {
            UpdateRace,
            ClearRace,
            SetPort,                // used to set port for measurer device
            SetTablePort,           // used to set porrt of display device
            SendTableData,
            RaceTick,
            UpdateTornament,
            RegisterTimeManager,
            DissposeRecord,
            TrackRace,
            Terminate
        }

        public struct ProccesorComand
        {
            public ProccesorOperations operation { get; set; }
            public string args { get; set; }
            public object target { get; set; }
        }

        //private Task ProccesorDispatchingQue = null;
        private BufferBlock<ProccesorComand> ProccesorQue = new BufferBlock<ProccesorComand>();
        private static SerialPort COMPort = null;
        private static SerialPort COMTablePort = null;
        private static bool COMTableInit = false;
        private static byte[] TableByteSeq = { 0x0A, 0x02, 0x4C, 0x30, 0x09, 0x41, 0x20, 0x20, 0x20, 0x20, 0x30, 0x7F, 0x7F, 0x7F, 0x7F, 0x09 };
        // direct link to last opened windw of lap data manager
        private static Timekeeping RecordManager = null;
        private static SendProccessorMessage InternalMsg = null;//= RequestOperation;

        private async Task ProccesorQueStart()
        {
            // refresh rate on data per race operations
            System.Timers.Timer race_refresh = new System.Timers.Timer(5000); //5s
            race_refresh.Elapsed += RaceRefreshTick;
            race_refresh.AutoReset = true;

            TimeSpan race_start = TimeSpan.Zero;
            //TimeSpan race_clock = TimeSpan.Zero;

            // race id
            Int32 RaceID = 0;

            bool imonit = true;
            while (imonit)
            {
                ProccesorComand item = await ProccesorQue.ReceiveAsync();
                switch (item.operation)
                {
                    case ProccesorOperations.SetPort:

                        DeviceForm form = null;
                        // requester may expect notice
                        if (item.target.GetType() == typeof(Regularity_Rally.Control.DeviceForm))
                        {
                            form = item.target as DeviceForm;
                        }

                        // test if com exis and is validly set
                        if (COMPort == null)
                        {
                            COMPort = new SerialPort();
                        }
                        else
                        {
                            try
                            {
                                //serial_continue = false;
                                //readThread.Join();
                                COMPort.Close();
                                COMPort.Dispose();
                                COMPort = null;
                                COMPort = new SerialPort();

                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e.Message);
                            }
                        }

                        COMPort.PortName = item.args;
                        COMPort.Parity = Parity.None;
                        COMPort.BaudRate = 1200;                    //prirazeni rychlosti prenosu
                        COMPort.DataBits = 8;                       //urceni kolik bude posilano datovych bitu
                        COMPort.Parity = Parity.None;               //jaka bude parita
                        COMPort.StopBits = StopBits.One;             //pocet stopbitu
                        COMPort.Handshake = Handshake.None;         //zadnz handshake, nedostanu odpoved

                        COMPort.ReadTimeout = 150;
                        COMPort.WriteTimeout = 150;

                        if (COMPort.PortName != null && COMPort.IsOpen)
                            COMPort.Close();

                        COMPort.DataReceived -= new SerialDataReceivedEventHandler(serialPort_DataReceived);
                        COMPort.DataReceived += new SerialDataReceivedEventHandler(serialPort_DataReceived);

                        try
                        {
                            COMPort.Open();
                        }
                        catch (Exception)
                        {
                            // make sure we wont reuse instance of port in wrong state
                            try
                            {
                                COMPort.Dispose();
                            }
                            catch { } 
                            COMPort = null;
                        }

                        if (form != null)
                            form.ReloadStatus();

                        if (RecordManager != null)
                            RecordManager.UpdateAutomatStatus();

                        break;
                    case ProccesorOperations.SetTablePort:
                        DeviceForm form_r = null;
                        // requester may expect notice
                        if (item.target.GetType() == typeof(Regularity_Rally.Control.DeviceForm))
                        {
                            form_r = item.target as DeviceForm;
                        }

                        // apply com send soignal and init
                        // test if com exis and is validly set
                        if (COMTablePort == null)
                        {
                            COMTablePort = new SerialPort();
                        }
                        else
                        {
                            try
                            {
                                //serial_continue = false;
                                //readThread.Join();
                                COMTablePort.Close();
                                COMTablePort.Dispose();
                                COMTablePort = null;
                                COMTablePort = new SerialPort();

                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e.Message);
                            }
                        }

                        // open port to connect and wrte test
                        COMTablePort.PortName = item.args;    //prirazeni COM Port cisla
                        COMTablePort.BaudRate = 9600; //prirazeni rychlosti prenosu
                        COMTablePort.DataBits = 8; //urceni koliik bude posilano datovych bitu
                        COMTablePort.Parity = Parity.None; //jaka bude parita
                        COMTablePort.StopBits = StopBits.One; //pocet stopbitu
                        COMTablePort.Handshake = Handshake.None; //zadnz handshake, nedostanu odpoved
                        COMTablePort.WriteTimeout = 1000; //timeout na zapisovani, aby jednotka mela cas na zobrazeni


                        TableByteSeq = new byte[] { 0x0A, 0x02, 0x4C, 0x30, 0x09, 0x41, 0x20, 0x20, 0x20, 0x20, 0x30, 0x7F, 0x7F, 0x7F, 0x7F, 0x09 };

                        try
                        {
                            COMTablePort.Open(); //open COMPortu
                            COMTablePort.Write(TableByteSeq, 0, TableByteSeq.Length); //zapsani na COMPortu
                            COMTablePort.Close(); //zavreni COMPortu
                        }
                        catch (Exception ec)
                        {
                            // make sure we wont reuse instance of port in wrong state
                            try
                            {
                                COMTablePort.Dispose();
                            }
                            catch { }
                            COMTablePort = null;
                            COMTableInit = false;
                            Console.WriteLine(ec.Message);
                        }
                        finally
                        {
                            // i succesfully opened and wote test record,
                            // we keep port opject in memory for future write sequence
                            COMTableInit = true;
                        }

                        if (form_r != null)
                            form_r.ReloadStatus();

                        break;
                    case ProccesorOperations.SendTableData:
                        // device is'nt in connected state
                        if (COMTablePort == null || !COMTableInit)
                            return;

                        // port state alreadty set

                        string aStr = item.args;
                        switch (aStr.Length)
                        {
                            case 0:
                                break;
                            case 1:
                                TableByteSeq[11] = BitConverter.GetBytes(aStr[0])[0];
                                break;
                            case 2:
                                TableByteSeq[11] = BitConverter.GetBytes(aStr[0])[0];
                                TableByteSeq[12] = BitConverter.GetBytes(aStr[1])[0];
                                break;
                            case 3:
                                TableByteSeq[11] = BitConverter.GetBytes(aStr[0])[0];
                                TableByteSeq[12] = BitConverter.GetBytes(aStr[1])[0];
                                TableByteSeq[13] = BitConverter.GetBytes(aStr[2])[0];
                                break;
                            case 4:
                                TableByteSeq[11] = BitConverter.GetBytes(aStr[0])[0];
                                TableByteSeq[12] = BitConverter.GetBytes(aStr[1])[0];
                                TableByteSeq[13] = BitConverter.GetBytes(aStr[2])[0];
                                TableByteSeq[14] = BitConverter.GetBytes(aStr[3])[0];
                                break;
                            default:
                                break;
                        }

                        try
                        {
                            COMPort.Open(); //open COMPortu
                            COMPort.Write(TableByteSeq, 0, TableByteSeq.Length); //zapsani na COMPortu
                            COMPort.Close(); //zavreni COMPortu
                        }
                        catch (Exception)
                        {
                            
                        }

                        break;
                    case ProccesorOperations.Terminate:
                        if (COMPort != null)
                        {
                            try
                            {
                                COMPort.Close();
                                COMPort.Dispose();
                            }
                            // prevent from bubbling dispose crast to main thread
                            catch (Exception)
                            { }
                        }
                        // close port of table
                        if (COMTablePort != null)
                        {
                            try
                            {
                                COMTablePort.Close();
                                COMTablePort.Dispose();
                            }
                            // prevent from bubbling dispose crast to main thread
                            catch (Exception)
                            { }
                        }
                        // termiante listening loop
                        imonit = false;
                        break;
                    case ProccesorOperations.DissposeRecord:
                        TimeSpan race_clock;

                        // priority is device source
                        if (item.args != string.Empty)
                        {
                            // cut pass counter
                            string timeonly = item.args.Substring(4);
                            Int32 hours_10 =            Int32.Parse((timeonly[0]).ToString());
                            Int32 hours =               Int32.Parse((timeonly[1]).ToString());
                            Int32 mins_10 =             Int32.Parse((timeonly[2]).ToString());
                            Int32 mins =                Int32.Parse((timeonly[3]).ToString());
                            Int32 seconds_10 =          Int32.Parse((timeonly[4]).ToString());
                            Int32 seconds =             Int32.Parse((timeonly[5]).ToString());
                            Int32 seconds_010 =         Int32.Parse((timeonly[6]).ToString());    // internal format ignores
                            Int32 seconds_0100 =        Int32.Parse((timeonly[7]).ToString());    // internal format ignores
                            Int32 seconds_01000 =       Int32.Parse((timeonly[8]).ToString());    // internal format ignores

                            TimeSpan parsed = new TimeSpan((hours_10*10) + hours, (mins_10*10) + mins, (seconds_10*10) + seconds);

                            race_clock = parsed;
                        }
                        else
                        {
                            // set time based on race clock
                            race_clock = (DateTime.Now.TimeOfDay - race_start);
                        }

                        if (RecordManager != null)
                            RecordManager.DissposeDialog(race_clock);
                        break;
                    case ProccesorOperations.RegisterTimeManager:
                        if (item.target != null)
                            RecordManager = item.target as Timekeeping;
                        break;
                    case ProccesorOperations.UpdateRace:
                        RaceID = Int32.Parse(item.args);// back to int
                        race_refresh.Start();
                        Race.RaceTime times = _homescreen.CreateRaceTracker(RaceID);
                        DateTime present = DateTime.Now;
                        // need to sync over apps, lets get clock and sync to it
                        // determinate if day is day i run
                        if (present.Date == times.RaceDay.Date)
                        {
                            // detect if is not already done that day, or too eraly
                            if (present < (times.RaceDay + times.Length) && present > (times.RaceDay + times.Start))
                            {
                                //race_clock = (present.TimeOfDay - times.Start);
                                race_start = times.Start;

                                // feed wot ever
                                if (_homescreen != null)
                                    _homescreen.DisplayRaceTime(race_start);
                            }
                        }
                        RequestOperation(new ProccesorComand { operation = ProccesorOperations.RaceTick });
                        break;
                    case ProccesorOperations.RaceTick:
                        // this will be responcible to invoke subwindow to resfresh inside data state
                        _homescreen.RefreshRaceData(RaceID);
                        break;
                    case ProccesorOperations.ClearRace:
                        // dispose state machine we dont care anymore
                        race_refresh.Stop();
                        break;

                    case ProccesorOperations.UpdateTornament:
                        Console.WriteLine("Proccesor update tornament");
                        break;
                    case ProccesorOperations.TrackRace:
                        Console.WriteLine("Procesor track race");
                        break;
                }
            }
            //Console.WriteLine("Proccesor died");
        }

        public void RequestOperation(ProccesorComand cmd)
        {
            // make sure post event wont stack on thread exec
            ProccesorQue.Post<ProccesorComand>(cmd);
        }

        private void RaceRefreshTick(Object source, ElapsedEventArgs e)
        {
            RequestOperation(new ProccesorComand { operation = ProccesorOperations.RaceTick });
        }

        // =======================
        // DEVICES 
        // =======================

        public static string[] GetPorts()
        {
            return SerialPort.GetPortNames();
        }

        public enum DeviceStatus
        {
            Disconnected,
            Connecting,
            Connected
        };

        public static DeviceStatus GetDeviceStatus()
        {
            if (COMPort == null)
            {
                return DeviceStatus.Disconnected;
            }

            if (COMPort.IsOpen)
            {
                return DeviceStatus.Connected;
            }
            else
                return DeviceStatus.Connecting;
        }

        public static DeviceStatus GetTableStatus()
        {
            if (COMTablePort == null)
            {
                return DeviceStatus.Disconnected;
            }

            if (COMTablePort.IsOpen && COMTableInit)
            {
                return DeviceStatus.Connected;
            }
            else
                return DeviceStatus.Connecting;
        }

        public static string GetDevicePort()
        {
            if (COMPort == null)
                return string.Empty;

            return COMPort.PortName;
        }

        public static string GetTablePort()
        {
            if (COMTablePort == null)
                return string.Empty;

            return COMTablePort.PortName;
        }

        private static List<byte> message = new List<byte>();
        private static void serialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            int dataLength = COMPort.BytesToRead;
            byte[] data = new byte[dataLength];
            int num_DataRead = COMPort.Read(data, 0, dataLength);
            if (num_DataRead == 0)
                return;

            message.Add(data[0]);

            // message is full
            if (message.Count == 15)
            {
                string clear = Encoding.Default.GetString(message.ToArray()).Substring(1, message.Count - 2);
                message.Clear();
                // dispatch event into listening object
                InternalMsg(new ProccesorComand
                {
                    operation = ProccesorOperations.DissposeRecord,
                    args = clear
                });
            }
        }

        #endregion

        public static void UpdateLanguageGlobal(string _language)
        {
            // select language
            switch (_language)
            {
                case "en-US":
                    Regularity_Rally.Properties.Resources.Culture = new System.Globalization.CultureInfo("");
                    Settings.SetValue("language", _language);
                    break;
                case "cs-CZ":
                    Regularity_Rally.Properties.Resources.Culture = new System.Globalization.CultureInfo("cs-CZ");
                    Settings.SetValue("language", _language);
                    break;
                case "de-DE":
                    Regularity_Rally.Properties.Resources.Culture = new System.Globalization.CultureInfo("de-DE");
                    Settings.SetValue("language", _language);
                    break;
                default:
                    Console.WriteLine("unable to select language:" + _language);
                    break;

            }

            // determinate state oh handler, is it still avaible ?
            for (int i = 0; i < LanguageUpdate.Count; i++)
            {
                // invoke function over delegate, propagate over thread logic
                if (LanguageUpdate[i] != null)
                    LanguageUpdate[i]();
                else
                    LanguageUpdate.Remove(LanguageUpdate[i]);
            }
        }

        public static void RegisterLanguageHandler(UpdateLanguage _delegate)
        {
            LanguageUpdate.Add(_delegate);
        }

        public static void UnRegisterLanguageHandler(UpdateLanguage _delegate)
        {
            LanguageUpdate.Remove(_delegate);
        }

        public void OnDBConnect(object sender, EventArgs args)
        {
            // value is reversed cos we refering to state of connection as needed not established
            Dispatcher.BeginInvoke(((Action)(() =>
            {

            })));
        }

        public void Login(User user, AsyncCallback callback)
        {
            if (user == null)
                return;

            this.login_user = user;


            // make modul disaper :)
            var dissaper = new DoubleAnimation
            {
                To = 0,
                BeginTime = TimeSpan.FromSeconds(0),
                Duration = TimeSpan.FromSeconds(0.5),
                FillBehavior = FillBehavior.Stop
            };
            dissaper.Completed += (s, a) =>
            {
                callback.BeginInvoke(null, null, null);
                Dispatcher.BeginInvoke(((Action)(() =>
                {
                    if (user.Role == UserRole.Padock)
                    {
                        SetStage(ApplicationStage.Paddock);
                    }
                    else if (user.Role == UserRole.Reporter)
                    {
                        SetStage(ApplicationStage.Reporter_MainWindow);
                    }
                    else
                    {
                        SetStage(ApplicationStage.Homescreen);
                    }
                })));
            };
            WorkArea.BeginAnimation(UIElement.OpacityProperty, dissaper);
        }

        static public ref User GetUser()
        {
            return ref (Application.Current.MainWindow as MainWindow).login_user;
        }

        static public void GetTimeCollection()
        {

        }

        private Cars CarsWindow = null;
        private Category CategoryWindow = null;
        private Measurer_Brands Measurer_BrandsWindow = null;
        private Paddock PaddockWindow = null;
        private Racers RacersWindow = null;
        private Races RacesWindow = null;
        private Reporter_MainWindow ReporterWindow = null;
        private Teams TeamsWindow = null;
        private Tournaments TournamentsWindow = null;
        private Users UsersWindow = null;

        private LoginWindow logscreen = new LoginWindow();  // prevent from unregistering object
        private Homescreen _homescreen = null;


        public void SetStage(ApplicationStage _stage)
        {
            this.CurentStage = _stage;  // update local handler
            WorkArea.Children.Clear();
            switch (_stage)
            {
                case ApplicationStage.Cars: // alocate window when needed only
                    CarsWindow = new Cars();
                    CarsWindow.Show();
                    break;
                case ApplicationStage.Category:
                    CategoryWindow = new Category();
                    CategoryWindow.Show();
                    break;
                case ApplicationStage.Measurer_Brands:
                    Measurer_BrandsWindow = new Measurer_Brands();
                    Measurer_BrandsWindow.Show();
                    break;
                case ApplicationStage.Paddock:
                    PaddockWindow = new Paddock();
                    PaddockWindow.Show();
                    break;
                case ApplicationStage.Races:
                    RacesWindow = new Races();
                    RacesWindow.Show();
                    break;
                case ApplicationStage.Reporter_MainWindow:
                    ReporterWindow = new Reporter_MainWindow();
                    ReporterWindow.Show();
                    break;
                case ApplicationStage.Teams:
                    TeamsWindow = new Teams();
                    TeamsWindow.Show();
                    break;
                case ApplicationStage.Tournaments:
                    TournamentsWindow = new Tournaments();
                    TournamentsWindow.Show();
                    break;
                case ApplicationStage.Users:
                    UsersWindow = new Users();
                    UsersWindow.Show();
                    break;
                case ApplicationStage.Homescreen:
                    if (_homescreen == null)
                    {
                        _homescreen = new Homescreen();
                    }
                    WorkArea.Children.Add(_homescreen);
                    break;
                case ApplicationStage.Login:
                    logscreen.Opacity = 0;
                    WorkArea.Children.Add(logscreen);
                    var showtime = new DoubleAnimation
                    {
                        To = 1,
                        BeginTime = TimeSpan.FromSeconds(0),
                        Duration = TimeSpan.FromSeconds(0.5),
                        FillBehavior = FillBehavior.Stop
                    };
                    showtime.Completed += (s, a) =>
                    {
                        logscreen.Opacity = 1;
                    };
                    logscreen.BeginAnimation(UIElement.OpacityProperty, showtime);
                    break;
                case ApplicationStage.Racers:
                    RacersWindow = new Racers();
                    RacersWindow.Show();
                    break;                    
            }
        }

        // prevent window from deallocating and rather just hide and relay on tray system to be reinvoked
        protected override void OnClosing(CancelEventArgs e)
        {
            Application.Current.Shutdown();     // for debug perpouse only, cos shuting down manualz will cos me crazy

            // befor we exit, we hea to collect responce from proccesor thread!
            RequestOperation(new ProccesorComand { operation = ProccesorOperations.Terminate });

            // wait till proccessor finalize, means of releasing all resources associated with port etc. can be operation heavy,
            // soo we heav to wait befor current thread exit and release memory thats hold all our static handlers
            Thread.Sleep(500);

            //this.Hide();
            //e.Cancel = true;
            base.OnClosing(e);
        }
    }
}
