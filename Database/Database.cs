using System;
using System.Collections.Generic;
using System.Text;
using Regularity_Rally;
using MySql.Data.MySqlClient;

namespace Regularity_Rally.Database
{
    public sealed class Database
    {
        public static readonly Database instance = new Database();

        //public static List<EventHandler> ConnectionNotify = new List<EventHandler>(); //static

        public List<EventHandler> ConnectionNotify;

        // Data for server connection
        public string Server; // = "127.0.0.1";
        public string Server_port; // = "3306";
        public string Server_user; // = "root";
        public string Server_password; // = "123456";       // saved in setting value as sc1
        public string Database_Name; // = "db_regularity_rally";         // saved in setting value as sc0 - saved using BASE64 "protection"


        // THE DATABASE
        private MySqlConnection Connection = null;

        public static Dictionary<string, string> QueryStack = new Dictionary<string, string>()
        {
            // USER
            { "UserCreate",         "INSERT INTO `user` (`Login`, `Password`, `Salt`, `Name`, `Lastname`, `E_mail`, `Permisson`, `Role`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}');"},       
            { "UserUpdate",         "REPLACE INTO `user` (`ID_User`, `Login`, `Password`, `Salt`, `Name`, `Lastname`, `E_mail`, `Permisson`, `Role`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}');"},
            { "UserSelect",         "SELECT `ID_User`, `Password`, `Salt`, `Active` FROM `user` WHERE `Login`='{0}';"},
            { "UserSelectID",       "SELECT `ID_User` FROM `user` WHERE `Login`='{0}' AND `Password`='{1}';" },
            { "UserSelectData",     "SELECT * FROM `user` WHERE `Login`='{0}' AND `Password`='{1}';" },
            { "UserSelectList",     "SELECT * FROM `user` WHERE `Login` LIKE '{0}';" },
            // DATA
            { "GetCarData",         "SELECT * FROM `car`" },
            { "GetTourData",        "SELECT * FROM `tournament`;" },
            { "GetCatData",         "SELECT * FROM `category`"},
            { "GetRaceCategoryANR", "SELECT  `ID_Category`, `Name` FROM `category`"},
            { "GetPointData",       "SELECT * FROM category_points WHERE `FK_Category`='{0}' ORDER BY `position`;"},
            { "AddPointData",       "INSERT INTO `category_points` (`Position`,`Points`,`FK_Category`) VALUES ('{0}','{1}','{2}')"},
            { "GetBrandData",       "SELECT * FROM meas_company;"},
            { "GetRacerData",       "SELECT A.ID_Racer, B.Team_name, A.FK_Team, A.First_Name, A.Last_Name, A.Short_Name, A.Born, A.Gender, A.Nationality, A.Address, A.Telephone, A.Email FROM `racer` A JOIN `team` B on B.ID_Team=A.FK_Team;"},
            { "GetRacerInTeamData", "SELECT A.ID_Racer, A.FK_Team, A.First_Name, A.Last_Name, A.Short_Name, A.Born, A.Gender, A.Nationality, A.Address, A.Telephone, A.Email FROM `racer` A WHERE A.FK_Team = '{0}';"},
            { "GetTeamData",        "SELECT * FROM team;"},
            { "GetUserData",        "SELECT * FROM user;"},
            { "AddRacerData",       "INSERT INTO `racer` (`First_Name`, `Last_Name`, `Short_Name`, `Born`, `Gender`, `Nationality`, `Address`, `Telephone`, `Email`,`FK_Team`) VALUES ('{0}', '{1}', '{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}');"},
            { "RepRacerData",       "REPLACE INTO `racer` (`ID_Racer`,`First_Name`, `Last_Name`, `Short_Name`, `Born`, `Gender`, `Nationality`, `Address`, `Telephone`, `Email`,`FK_Team`) VALUES ('{0}', '{1}', '{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}');"},
            { "RemRacerData",       "DELETE FROM `racer` WHERE `ID_Racer`='{0}';"},
            { "GetRefTimeData",     "SELECT * FROM reference_time;"},
            { "GetRaceData",        "SELECT * FROM race;"},
            { "SelLapData",         "SELECT A.ID_Lap_Data, A.FK_Race, A.FK_MeasCom_Measurer, A.FK_Racer_Car, A.Lap, A.Time, A.Penalization, B.Note FROM `lap_data` A JOIN `lap_note` B on B.FK_Lap_Data = A.ID_Lap_Data WHERE A.FK_Race = '{0}'; "}, //"SELECT * FROM lap_data WHERE `FK_Race`='{0}';"},
            { "SelRacerCar",        "SELECT A.ID_Racer_Car, B.Reference_Time, A.FK_Racer, A.FK_Car, A.FK_Status_Racer, A.Start_num, C.FK_Team, D.Status_Text  FROM `racer_car` A JOIN `reference_time` B on B.ID_Reference_Time = A.FK_Reference_Time JOIN `racer` C on C.ID_Racer= A.FK_Racer JOIN `status_racer` D on D.ID_Status_Racer = A.FK_Status_Racer  WHERE `FK_Race`= '{0}';"},
            { "GetRaceDataSelect",  "SELECT A.ID_Race, A.FK_Race_Status, A.FK_Race_Note, A.FK_Category, A.FK_Meas_company, A.Name, A.Date, A.Place, A.Length, A.Time, A.Start_Time, A.NumOfLaps, B.Status_text FROM `race` A JOIN `race_status` B on B.ID_Race_Status = A.FK_Race_Status;"},
            { "GetRaceDataCond",    "SELECT A.ID_Race, A.Name, A.Date, A.Place, A.Length, B.Status_text, A.NumOfLaps, C.Name FROM `race` A	JOIN `race_status` B on B.ID_Race_Status = A.FK_Race_Status JOIN `category` C on C.ID_Category = A.FK_Category WHERE A.ID_Race='{0}';"},
            { "AddCarData",         "INSERT INTO `car` (`Brand`, `VIN`, `Model`, `Year`) VALUES ('{0}', '{1}', '{2}', '{3}');"},
            { "RepCarData",         "REPLACE INTO `car` (`ID_Car`, `Brand`, `VIN`, `Model`,`Year`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}');"},
            { "RemCarData",         "DELETE FROM `car` WHERE `ID_Car`='{0}';"},
            { "RemPointData",       "DELETE FROM category_points WHERE `FK_Category`='{0}';"},
            { "RemPointCatData",    "DELETE FROM `category_points` WHERE  `ID_Category_Points`='{0}';"},
            { "RepPointData",       "REPLACE INTO `category_points` (`ID_Category_Points`, `FK_Category`, `Position`, `Points`) VALUES ('{0}', '{1}', '{2}', '{3}');"},
            { "AddCatData",         "INSERT INTO `category` (`Name`, `Limit`, `Coef`) VALUES ('{0}', '{1}', '{2}');"},
            { "RepCatData",         "REPLACE INTO `category` (`ID_Category`, `Name`, `Limit`, `Coef`) VALUES ('{0}', '{1}', '{2}', '{3}');"},
            { "RemCatData",         "DELETE FROM `category` WHERE `ID_Category` = '{0}';"},
            { "AddMeasCorpData",    "INSERT INTO `meas_company` (`Company`, `Address`, `Telephone`, `Email`, `Web`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}');" },
            { "RepMeasCorpData",    "REPLACE INTO `meas_company` (`ID_Meas_Company`,`Company`, `Address`, `Telephone`, `Email`, `Web`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}');" },
            { "RemMeasComData",     "DELETE FROM `meas_company` WHERE `ID_Meas_Company`='{0}';"},
            { "AddTeamData",        "INSERT INTO `team` (`Team_name`, `Short_name`, `E_mail`) VALUES ('{0}', '{1}', '{2}');"},
            { "RepTeamData",        "REPLACE INTO `team` (`ID_Team`, `Team_name`, `Short_name`, `E_mail`) VALUES ('{0}', '{1}', '{2}', '{3}');"},
            { "RemTeamData",        "DELETE FROM `team` WHERE  `ID_Team`={0};"},
            { "GetRacerBrief",      "SELECT * FROM `racer`;"},    
            { "GetTimekeepersRace", "SELECT A.ID_MeasCom_Measurer, A.FK_User, B.Login, B.Name, B.Lastname, B.Role, A.Position FROM `meascom_measurer` A JOIN `user` B on A.FK_User = B.ID_User WHERE A.FK_Race = '{0}';"},    
            { "GetRacersInRaceData","SELECT  A.ID_Racer_Car, A.Start_num, A.FK_Car, A.FK_Status_Racer, C.Reference_Time, B.ID_Racer, B.First_Name, B.Last_Name, E.Team_name, F.ID_Race FROM `race` F JOIN `racer_car` A on A.FK_Race = F.ID_Race JOIN `racer` B on B.ID_Racer = A.FK_Racer JOIN `reference_time` C on C.ID_Reference_Time=A.FK_Reference_Time JOIN `team` E on E.ID_Team = B.FK_Team WHERE F.ID_Race='{0}';"},    
            { "GetRaceStatusData",  "SELECT * FROM `race_status`;"},    
            { "GetRacerStatusData", "SELECT * FROM `status_racer`;"},    
            { "RemRacerFromRace",   "DELETE FROM `racer_car` WHERE `ID_Racer_Car` = '{0}';"},    
            { "RemTKFromRace",      "DELETE FROM `meascom_measurer` WHERE `ID_MeasCom_Measurer` = '{0}';"},    
            { "RemRace",            "DELETE FROM `race` WHERE `ID_Race` = '{0}';"},    
            { "AddRefTimeANR",      "INSERT INTO `reference_time` (`Reference_Time`) VALUES ('{0}');"},    
            { "GetMeasMeasur",      "SELECT * FROM `meascom_measurer` WHERE `FK_Race`='{0}' AND `FK_User`='{1}';"},    
            { "InsertLapNote",      "INSERT INTO `lap_note` (`FK_Lap_Data`, `Note`) VALUES ('{0}', '{1}');"},    
            { "InsertLap",          "INSERT INTO `lap_data` (`FK_Race`, `FK_MeasCom_Measurer`, `FK_Racer_Car`, `Lap`, `Time`, `Penalization`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}');"},    
            { "GetRefTimeANR",      "SELECT * FROM `reference_time`;"},    
            { "AddRacerToRace",     "INSERT INTO `racer_car` (`FK_Race`, `FK_Racer`, `FK_Reference_Time`, `FK_Car`, `FK_Status_Racer`, `Start_num`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}');"},    
            { "RepRacerToRace",     "REPLACE INTO `racer_car` (`ID_Racer_Car`, `FK_Race`, `FK_Racer`, `FK_Reference_Time`, `FK_Car`, `FK_Status_Racer`, `Start_num`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}');"},    
            { "GetLastInsertID",    "SELECT LAST_INSERT_ID();"},    
            { "AddTKtoRaceANR",     "INSERT INTO `meascom_measurer` (`FK_User`, `Position`, `FK_Race`) VALUES ('{0}', '{1}', '{2}' );"},    
            { "RepTKtoRaceANR",     "REPLACE INTO `meascom_measurer` (`ID_MeasCom_Measurer`, `FK_User`, `FK_Race`, `Position`) VALUES ('{0}','{1}','{2}','{3}');"},    
            { "RaceNote",           "INSERT INTO `race_note` (`Note_Text`) VALUES ('{0}');"},    
            { "AddRaceFinalData",   "INSERT INTO `race` (`FK_Race_Status`,`FK_Race_Note`,`FK_Category`, `FK_Meas_company`,`Name`,`Date`,`Place`,`Length`,`NumOfLaps`,`Time`) VALUES ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}');"},    
            { "RepRaceFinalData",   "UPDATE `race` SET `FK_Race_Status`='{1}',`FK_Race_Note`='{2}',`FK_Category`='{3}', `FK_Meas_company`='{4}',`Name`='{5}',`Date`='{6}',`Place`='{7}',`Length`='{8}',`Time`='{9}',`Start_Time`='{10}',`NumOfLaps`='{11}' WHERE `ID_Race`='{0}';" },//UPDATE `race` SET FK_Race_Status = '{0}', FK_Race_Note = '{1}', FK_Category = '{2}', FK_Meas_company = '{3}', Name = '{4}', Date = '{5}', Place = '{6}', Length = '{7}', NumOfLaps = '{8}' WHERE `ID_Race` = '{9}';"},//"REPLACE INTO `race` (`ID_Race`,`FK_Race_Status`,`FK_Race_Note`,`FK_Tour`,`FK_Category`, `FK_Meas_company`,`Name`,`Date`,`Place`,`Length`,`NumOfLaps`, `Start_Time`, `Time` ) VALUES ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','00:00:00','00:00:00');"},//REPLACE INTO `race` (`ID_Race`,`FK_Race_Status`,`FK_Race_Note`,`FK_Category`, `FK_Meas_company`,`Name`,`Date`,`Place`,`Length`,`NumOfLaps`, `Start_Time`, `Time` ) VALUES ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','00:00:00','00:00:00')" },//REPLACE INTO `race` (`ID_Race`,`FK_Race_Status`,`FK_Race_Note`,`FK_Category`, `FK_Meas_company`,`Name`,`Date`,`Place`,`Length`,`NumOfLaps`) VALUES ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}');"},    
            { "GetTourViewData",    "SELECT B.ID_Race, B.Name, B.Place, B.Date  FROM `tournament` A  JOIN `race` B on B.FK_Tour = A.ID_Tournament WHERE A.ID_Tournament = '{0}';"},    
            { "GetRacersViewTour",  "SELECT C.FK_Racer FROM `tournament` A  JOIN `race` B on B.FK_Tour = A.ID_Tournament JOIN `racer_car` C on C.FK_Race = B.ID_Race WHERE A.ID_Tournament = '{0}';"},    
            { "GetRacesInTourT",    "SELECT A.ID_Tournament, A.Tournament_name, A.Tournamen_shortname, A.Year, A.FK_Category, C.Name  FROM `tournament` A  JOIN `category` C on C.ID_Category = A.FK_Category;"},    
            { "GetRacesInTourANT",  "SELECT A.ID_Race, A.Name, A.Date, A.Place, A.Length, A.Time, A.NumOfLaps  FROM `race_tour` B JOIN `race` A on B.FK_Race = A.ID_Race WHERE B.FK_Tour ='{0}';"},    
            { "GetRacesToTourANT",  "SELECT A.ID_Race, A.Name, A.Date, A.Place, A.Time, A.Length, A.NumOfLaps FROM `race` A WHERE A.FK_Category = '{0}' AND ID_race NOT IN (SELECT `FK_Race` FROM `race_tour` WHERE `FK_Tour` = '{1}');"},    
            { "AddRaceToTourANT",   "INSERT INTO `race_tour` (`FK_Race`, `FK_Tour`) VALUES ('{0}', '{1}');"},    
            { "AddNewTour",         "INSERT INTO `tournament` (`Tournament_name`, `Tournamen_shortname`, `Year`, `FK_Category`) VALUES ('{0}', '{1}', '{2}','{3}');"},    
            { "RepNewTour",         "REPLACE INTO `tournament` (ID_Tournament, `FK_Category`, `Tournament_name`, `Tournamen_shortname`, `Year`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}');"},    
            { "RemNewTour",         "DELETE FROM `tournament` WHERE `ID_Tournament` = '{0}';"},    
            { "Check_CarVIN",       "SELECT count(*) FROM `car` WHERE `VIN` = '{0}';"},    
            { "Check_CatPosition",  "SELECT COUNT(*) FROM `category_points` WHERE `FK_Category`= '{0}' AND `Position` = '{1}';"},    
            { "Check_IDPosition",   "SELECT A.ID_Category_Points FROM `category_points` A WHERE A.FK_Category = '{0}' AND `Position` = '{1}';"},    
            { "CountScorPos",       "SELECT COUNT(*) FROM `category_points` WHERE `FK_Category`= '{0}';"},    
            { "GetUserToBrand",     "SELECT A.ID_User, A.Login, A.Name, A.Lastname, A.E_mail, A.Role FROM `user` A WHERE A.Active = '1';"},    
            { "CheckUserToBrand",   "SELECT count(*) FROM `user_meascom` WHERE `FK_User` = '{0}' AND `FK_Meas_company` = '{1}';"},    
            { "AddUserToBrand",     "INSERT INTO `user_meascom` (`FK_User`, `FK_Meas_company`) VALUES ('{0}', '{1}');"},    
            { "RepUserToBrand",     "REPLACE INTO `user_meascom` (`ID_User_MeasCom`, `FK_User`, `FK_Meas_company`) VALUES ('{0}', '{1}', '{2}');"},    
            { "GetUsersInBrand",    "SELECT B.ID_User, B.Login, B.Name, B.Lastname, B.E_mail, B.Role FROM `user_meascom` A JOIN `user` B on A.FK_User = B.ID_User WHERE A.FK_Meas_company = '{0}';"},    
            { "RemUsersFromBrand",  "DELETE FROM `user_meascom` WHERE `FK_User` = '{0}' AND `FK_Meas_company` = '{1}';"},    
            { "CheckExisUsersBrand","SELECT count(*) FROM `user_meascom` WHERE `FK_Meas_company` = '{0}';"},    
            { "RemAllUserBrand",    "DELETE FROM `user_meascom` WHERE `FK_Meas_company` = '{0}';"},    
            { "GetRaceNote",        "SELECT `Note_Text` FROM `race_note` WHERE `ID_Race_Note` = '{0}';"},    
            { "Check_IDRacerCar",   "SELECT A.ID_Racer_Car FROM `racer_car` A WHERE A.FK_Racer = '{0}' AND A.FK_Race =  '{1}';"},    
            { "GetTKbyBrand",       "SELECT B.ID_User, B.Login, B.Name, B.Lastname, B.Role FROM `user_meascom` A JOIN `user` B on A.FK_User = B.ID_User WHERE A.FK_Meas_company = '{0}' AND B.Active='1';"},    
            { "Check_UserInRace",   "SELECT COUNT(*) FROM `meascom_measurer` WHERE `FK_Race`= '{0}' AND `FK_User` = '{1}';"},    
            { "GetRaceTimes",       "SELECT A.Date, A.Time, A.Start_Time FROM `race` A WHERE A.ID_Race = '{0}';"},    
            { "CheckIDMCM",         "SELECT A.ID_MeasCom_Measurer FROM `meascom_measurer` A WHERE A.FK_Race = '{0}' AND A.FK_User = '{1}';"},    
            { "RemMeascomMeasurer", "DELETE FROM `meascom_measurer` WHERE  `ID_MeasCom_Measurer`={0};"},    
            { "RaceIDinLapData",    "SELECT count(*) FROM `lap_data` WHERE `FK_Race` = '{0}';"},    
            { "RaceIDinRacerCar",   "SELECT count(*) FROM `racer_car` WHERE `FK_Race` = '{0}';"},    
            { "RaceIDinMeasComMeas","SELECT count(*) FROM `meascom_measurer` WHERE `FK_Race` = '{0}';"},    
            { "RaceIDinRaceJury",   "SELECT count(*) FROM `race_jury` WHERE `FK_Race` = '{0}';"},    
            { "RemRacerCarANR",     "DELETE FROM `racer_car` WHERE  `FK_Race`={0};"},    
            { "RemMeasComMeasANR",  "DELETE FROM `meascom_measurer` WHERE  `FK_Race`='{0}';"},    
            { "RemRaceJuryANR",     "DELETE FROM `race_jury` WHERE  `FK_Race`='{0}';"},    
            { "RemRaceTourID",      "DELETE FROM `race_tour` WHERE  `FK_Race`='{0}' AND `FK_Tour`='{1}';"},    
            { "CheckRaceTourID",    "SELECT A.ID_Race_Tour FROM `race_tour` A WHERE A.FK_Race = '{0}' AND A.FK_Tour = '{1}';"},    
            { "CheckExistRIT",      "SELECT count(*) FROM `race_tour` WHERE `FK_Tour` = '{0}';"},    
            { "RemRacesRIT",        "DELETE FROM `race_tour` WHERE  `FK_Tour`='{0}';"},    
            { "CheckRaceStatusTK",  "SELECT  A.FK_Race_Status, B.Status_text FROM `race` A JOIN `race_status` B on B.ID_Race_Status = A.FK_Race_Status  WHERE A.ID_Race = '{0}';"},    
            { "RacesInTourHomeS",   "SELECT A.ID_Race, A.Name, A.Date, A.Place, A.Length, A.Time, A.NumOfLaps, C.Status_text  FROM `race_tour` B JOIN `race` A on B.FK_Race = A.ID_Race JOIN `race_status` C on C.ID_Race_Status = A.FK_Race_Status  WHERE B.FK_Tour ='{0}';"},    
            { "RacesIDinTourHS",    "SELECT `FK_Race` FROM `race_tour` WHERE `FK_Tour`= '{0}';"},
            { "PointsInTourHS",     "SELECT A.ID_Category_Points, A.Position, A.Points FROM `category_points` A JOIN `tournament` B on B.FK_Category = A.FK_Category WHERE B.ID_Tournament = '{0}';"},
            { "CountOfScoredPos",   "SELECT count(*) FROM `category_points` A JOIN `tournament` B on A.FK_Category = B.FK_Category  WHERE B.ID_Tournament = '{0}';"},
            { "CountOfRacerInTour", "SELECT count(*) FROM `racer` A JOIN `racer_car` B on A.ID_Racer = B.FK_Racer JOIN `race` C on C.ID_Race=B.FK_Race JOIN `race_tour` D on D.FK_Race = C.ID_Race WHERE D.FK_Tour = '{0}';"},
            { "GetNumOfLaps",       "SELECT  A.NumOfLaps FROM `race` A WHERE A.ID_Race = '{0}';"},
            { "ChangeRacerStatus",  "UPDATE `racer_car` SET `FK_Status_Racer`='{0}' WHERE `ID_Racer_Car`='{1}';"},
            { "ChangeRaceStatus",   "UPDATE `race` SET `FK_Race_Status`='{0}' WHERE `ID_Race`='{1}';"},
            { "Countofracersinrace","SELECT COUNT(*) FROM `racer_car` WHERE `FK_Race` = '{0}';"},
            { "AddRacerNoteRep",    "INSERT INTO `racer_note` (`Note`, `FK_Racer`) VALUES ('{0}','{1}')"},
            { "RacerNoteRep",       "SELECT `ID_Racer_Note`, `Note` FROM `racer_note` WHERE `FK_Racer` = '{0}';"},
            { "GetTourID",          "SELECT B.ID_Tournament, B.Tournament_name, B.Tournamen_shortname, B.Year, C.Name FROM `race_tour` A JOIN  `tournament` B on A.FK_Tour = B.ID_Tournament JOIN  `category` C on B.FK_Category = C.ID_Category WHERE `FK_Race` = '{0}';"},
            { "CountOfRacersInFin", "SELECT COUNT(*) FROM `racer_car` WHERE `FK_Race` = '{0}' AND `FK_Status_Racer` = '7' OR `FK_Status_Racer` = '6'  OR `FK_Status_Racer` = '5' OR `FK_Status_Racer` = '4' OR `FK_Status_Racer` = '3'  ;"},
            { "GetRacerDetailData", "SELECT A.ID_Racer_Car, A.FK_Racer, A.Start_num, D.First_Name, D.Last_Name, D.Short_Name, D.Born, D.Nationality, E.Team_name, C.Brand, C.Model, C.Year, B.Reference_Time FROM `racer_car` A JOIN  `reference_time` B on A.FK_Reference_Time = B.ID_Reference_Time JOIN  `car` C on A.FK_Car=C.ID_Car JOIN  `racer` D on D.ID_Racer=A.FK_Racer  JOIN  `team` E on E.ID_Team = D.FK_Team  WHERE A.FK_Race = '{0}';" }






        };

        //musi byt static kvuli instanci sealed DOHLEDAT!!!!
        static Database()
        {

            // load default values of connection info from setting once singleton is created
            byte[] sc0data = Settings.GetValue("sc0");
            byte[] sc1data = Settings.GetValue("sc1");
            instance.Server = Settings.GetValue("db_server", "");
            instance.Server_port = Settings.GetValue("db_port", "");
            instance.Server_user = Settings.GetValue("db_user", "");
            instance.Server_password = (sc0data != null) ? Encoding.UTF8.GetString(sc1data) : "";
            instance.Database_Name = (sc0data != null) ? Encoding.UTF8.GetString(sc0data) : "";

            instance.ConnectionNotify = new List<EventHandler>();
            // connect, no need to remember
            OpenConnection(instance.Server, instance.Server_user, instance.Database_Name, instance.Server_port, instance.Server_password, false);

        }

        public static void RegisterConnectionNotify(EventHandler Notify)
        {
            instance.ConnectionNotify.Add(Notify);
        }
        /// </summary>
        /// <returns>Whener is database allowed to connect from stored setting</returns>
        public static bool isConnect()
        {
            if (instance.Connection == null)
                return false;

            return instance.Connection.State == System.Data.ConnectionState.Open;
        }

        public static MySqlCommand CreateCommand()
        {
            return instance.Connection.CreateCommand();
        }

        public static MySqlCommand CreateCommand(string q)
        {
            return new MySqlCommand(q, instance.Connection);
        }

        //pomoci promenne keep predavame hodnotu true, ktera nam zaznamenava konfiguraci pripojeni k databazovemu serveru, uklada do konfigurace aplikace
        public static bool OpenConnection(string server, string db_user, string db, string db_port, string db_pass, bool keep = true)
        {
            string connStr = String.Format("server={0};user={1};database={2};port={3};password={4}", server, db_user, db, db_port, db_pass);

            instance.Connection = new MySqlConnection(connStr);
            try
            {
                instance.Connection.Open();

                // if requested save to config
                if (instance.Connection.State == System.Data.ConnectionState.Open)
                {
                    if (keep)   // store connection data to config is requested
                    {
                        byte[] sc0data = Encoding.UTF8.GetBytes(db);
                        byte[] sc1data = Encoding.UTF8.GetBytes(db_pass);

                        Settings.SetValue("db_server", server);
                        Settings.SetValue("db_port", db_port);
                        Settings.SetValue("db_user", db_user);
                        Settings.SetValue("sc0", sc0data);
                        Settings.SetValue("sc1", sc1data);
                    }

                    // set values to local store
                    instance.Server = server;
                    instance.Server_port = db_port;
                    instance.Server_user = db_user;
                    instance.Server_password = db_pass;
                    instance.Database_Name = db;

                    // connection is esttablished, notify all listening events
                    foreach (EventHandler _e in instance.ConnectionNotify)
                    {
                        _e.BeginInvoke(null, null, null, null);
                    }
                }

                return true;
            }
            catch (Exception)
            {
                // it failed wot ever
            }
            return false;
        }

        // acync version
        public void OpenConnection(string server, string db_user, string db, string db_port, string db_pass, bool keep, EventHandler callback)
        {
            bool responce = OpenConnection(server, db_user, db, db_port, db_pass, keep);

            if (callback != null)
                callback.BeginInvoke(responce, EventArgs.Empty, null, null);
        }

        public static void TerminateConnection()
        {
            if (instance != null && instance.Connection != null)
                instance.Connection.Close();

            instance.Connection = null;
        }
    }
}
