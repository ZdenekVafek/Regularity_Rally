using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Threading.Tasks;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace Regularity_Rally
{
    public class Race
    {
        private struct RacersData
        {
            public Int32 ID { get; set; }               // ID handled by lap_data referencing to NN
            public TimeSpan ReferenceTime { get; set; } // extracted by join on Reference_Time
            public Int32 Team { get; set; }             // refer to team
            public Int32 RacerCar { get; set; }         // reference into Car
            public Int32 Racer { get; set; }            // refers to Racer
            public Int32 RacrStatus { get; set; }       // refer to Status_Racer, valuation ??
            public Int32 StartNumber { get; set; }      // start number in selected race
            public string RacerStatus { get; set; }     // racer status
        }

        public struct LapData //private
        {
            public Int32 ID { get; set; }
            //public Int32 Race { get; set; }
            public Int32 Measurer { get; set; }
            public Int32 RacerCar { get; set; }         // refers to interanl cache created during init
            public Int32 Lap { get; set; }
            public TimeSpan Time { get; set; }          // calc internal
            public TimeSpan Penal { get; set; }         // calc internal
            // calced
            public TimeSpan TimeFinal { get; set; }
            public Int32 Position { get; set; }
            public string Note { get; set; }

            public void Calc()
            {
                this.TimeFinal = Time + Penal;
            }
        }

        private struct RefTimes
        {
            public  Int32 ID { get; set; }
            public TimeSpan Time { get; set; }          // keep raw for comparing
        }

        private struct Teams
        {
            public Int32 ID { set; get; }
            public string Name { get; set; }
        }

        public struct Racer
        {
            public Int32 ID { get; set; }
            public string FName { get; set; }
            public string LName { get; set; }

            public Int32 RacerStatus { get; set; }
            public Int32 StartNumber { get; set; }
        }

        private struct Car
        {
            public Int32 ID { get; set; }
            public string Brand { get; set; }
            public string Model { get; set; }
        }

        public struct RaceSummary
        {
            public Int32 Position { get; set; }
            public string FullName { get; set; }
            public Int32 StartNumber { get; set; }
            public string Team { get; set; }
            public string RefTime { get; set; }
            public TimeSpan Time { set; get; }
            public TimeSpan PenTime { set; get; }
            public TimeSpan FinalTime { set; get; }
            public string Car { get; set; }
            public Int32 RacerID { get; set; }
            public Int32 RacerStatusID { get; set; }
            public string RacerStatusText { get; set; }
        }

        public struct RaceTime
        {
            public DateTime RaceDay { get; set; }
            public TimeSpan Start { get; set; }
            public TimeSpan Length { get; set; }
        }



        private Dictionary<Int32, RacersData> RacersDataList;
        private List<RaceSummary> FinalSummary;
        private Dictionary<Int32, Teams> TeamsData;
        private Dictionary<Int32, RefTimes> RefTimesData;
        private Dictionary<Int32, Racer> RacerData;
        private Dictionary<Int32, Car> CarData;
        private RaceTime _RaceTime;
        public List<RaceLaps> LapCache;
        public RaceStatus GetStatusRace;
        private Int32 SelectedRace = 0;
        public RaceLapsInRace LapsInSelectedRace; 
        //public Int32 SelectedRaceStatus = 0;

        public Int32 RaceID
        {
            get { return SelectedRace; }
        }

        public struct RaceStatus
        {
            public int SelectedRaceStatusID { get; set; }
            public string SelectedRaceStatusText { get; set; }
        }

        public struct RaceLapsInRace
        {
            public int Laps { get; set; }
        }

        public Race(Int32 RaceID)
        {
            // init
            RacersDataList = new Dictionary<int, RacersData>();
            FinalSummary = new List<RaceSummary>();
            TeamsData = new Dictionary<int, Teams>();
            RefTimesData = new Dictionary<int, RefTimes>();
            RacerData = new Dictionary<int, Racer>();
            CarData = new Dictionary<int, Car>();
            LapCache = new List<RaceLaps>();
            this.LapsInSelectedRace = new RaceLapsInRace();
            this.SelectedRace = RaceID;
            this.GetStatusRace = new RaceStatus();
            this._RaceTime = new RaceTime();

            // select Racer_car data refering to selected race
            // pack all selected into refering use values
            try
            {
                // load tata to be feeded into selector
                MySqlCommand cmd = Database.Database.CreateCommand(string.Format(Database.Database.QueryStack["SelRacerCar"], RaceID));
                MySqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    // push selected value
                    RacersDataList.Add(rdr.GetInt32("ID_Racer_Car"), new RacersData
                    {
                        ID = rdr.GetInt32("ID_Racer_Car"),
                        ReferenceTime = rdr.GetTimeSpan("Reference_Time"),
                        Team = rdr.GetInt32("FK_Team"),
                        RacerCar = rdr.GetInt32("FK_Car"),
                        Racer = rdr.GetInt32("FK_Racer"),
                        RacrStatus = rdr.GetInt32("FK_Status_Racer"),
                        StartNumber = rdr.GetInt32("Start_num"),
                        RacerStatus = rdr.GetString("Status_Text")
                    });
                }
                rdr.Close();

                // loadteamss
                MySqlCommand teams_cmd = Database.Database.CreateCommand(Database.Database.QueryStack["GetTeamData"]);
                MySqlDataReader teams_reader = teams_cmd.ExecuteReader();

                while (teams_reader.Read())
                {
                    TeamsData.Add(teams_reader.GetInt32("ID_Team"), new Teams
                    {
                        ID = teams_reader.GetInt32("ID_Team"),
                        Name = teams_reader.GetString("Team_name")
                    });
                }
                teams_reader.Close();

                // load ref times
                MySqlCommand reft_cmd = Database.Database.CreateCommand(Database.Database.QueryStack["GetRefTimeData"]);
                MySqlDataReader reft_reader = reft_cmd.ExecuteReader();

                while (reft_reader.Read())
                {
                    RefTimesData.Add(reft_reader.GetInt32("ID_Reference_Time"), new RefTimes
                    {
                        ID = reft_reader.GetInt32("ID_Reference_Time"),
                        Time = reft_reader.GetTimeSpan("Reference_Time")
                    });
                }
                reft_reader.Close();

                // load racer
                MySqlCommand racer_cmd = Database.Database.CreateCommand(Database.Database.QueryStack["GetRacerData"]);
                MySqlDataReader racer_reader = racer_cmd.ExecuteReader();

                while (racer_reader.Read())
                {
                    RacerData.Add(racer_reader.GetInt32("ID_Racer"), new Racer
                    {
                        ID = racer_reader.GetInt32("ID_Racer"),
                        FName = racer_reader.GetString("First_Name"),
                        LName = racer_reader.GetString("Last_Name")
                    });
                }
                racer_reader.Close();

                // load car data
                MySqlCommand car_cmd = Database.Database.CreateCommand(Database.Database.QueryStack["GetCarData"]);
                MySqlDataReader car_reader = car_cmd.ExecuteReader();

                while (car_reader.Read())
                {
                    CarData.Add(car_reader.GetInt32("ID_Car"), new Car
                    {
                        ID = car_reader.GetInt32("ID_Car"),
                        Brand = car_reader.GetString("Brand"),
                        Model = car_reader.GetString("Model")
                    });
                }
                car_reader.Close();

                // load race time spans 
                MySqlCommand time_cmd = Database.Database.CreateCommand(Database.Database.QueryStack["GetRaceTimes"]);
                MySqlDataReader time_rdr = time_cmd.ExecuteReader();

                while (time_rdr.Read())
                {
                    _RaceTime.RaceDay = time_rdr.GetDateTime("Date");
                    _RaceTime.Start = time_rdr.GetTimeSpan("Start_Time");
                    _RaceTime.Length = time_rdr.GetTimeSpan("Time");
                }
                time_rdr.Close();

                //load race status
                MySqlCommand racestatus_cmd = Database.Database.CreateCommand(string.Format(Database.Database.QueryStack["CheckRaceStatusTK"],RaceID));
                MySqlDataReader racestatus_rdr = racestatus_cmd.ExecuteReader();

                while (racestatus_rdr.Read())
                {
                    GetStatusRace.SelectedRaceStatusID = racestatus_rdr.GetInt32("FK_Race_Status");
                    GetStatusRace.SelectedRaceStatusText = racestatus_rdr.GetString("Status_text");
                }
                racestatus_rdr.Close();

                //load race status
                MySqlCommand numoflapsinrace_cmd = Database.Database.CreateCommand(string.Format(Database.Database.QueryStack["GetNumOfLaps"],RaceID));
                MySqlDataReader numoflapsinrace_rdr = numoflapsinrace_cmd.ExecuteReader();

                while (numoflapsinrace_rdr.Read())
                {
                    LapsInSelectedRace.Laps = numoflapsinrace_rdr.GetInt32("NumOfLaps");
                }
                numoflapsinrace_rdr.Close();
            }
            catch (Exception)
            {
                // in case select crash
                //Console.WriteLine(e.Message);
            }
        }

        public int GetNumberOfLapsofRace()
        {
            return LapsInSelectedRace.Laps;
        }

        public int GetStatusID()
        {
            return GetStatusRace.SelectedRaceStatusID;
        }

        public string GetStatusText()
        {
            return GetStatusRace.SelectedRaceStatusText;
        }


        public bool LoadLaps()
        {
            // load database data into internal local bufffer
            List<LapData> laps = new List<LapData>();

            try
            {
                // load tata to be feeded into selector
                MySqlCommand cmd = Database.Database.CreateCommand(string.Format(Database.Database.QueryStack["SelLapData"], SelectedRace));
                MySqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    // push selected value
                    LapData record = new LapData
                    {
                        ID = rdr.GetInt32("ID_Lap_Data"),
                        Measurer = rdr.GetInt32("FK_MeasCom_Measurer"),
                        RacerCar = rdr.GetInt32("FK_Racer_Car"),
                        Lap = rdr.GetInt32("Lap"),
                        Time = rdr.GetTimeSpan("Time"),
                        Penal = rdr.GetTimeSpan("Penalization"),
                        Note = rdr.GetString("Note")
                    };

                    // calcualte final
                    record.Calc();

                    laps.Add(record);
                }
                rdr.Close();
            }
            catch (Exception)
            {
                // there is no need to make calc over intenal data when this loading failed, data will be old or incorect(psot-init)
                return false;
            }

            // make position calculation, wil lrelease final data onto internal cache
            return CalculateLaps(laps);
        }

        private bool CalculateLaps(List<LapData> data)
        {
            // refers to racerCar intenal, lap over acumulation
            List<LapData> FinalData = data;
            Dictionary<Int32, TimeSpan> AcumulationPerLap = new Dictionary<int, TimeSpan>();

            // reflect data into local cache after proccess
            Dictionary<Int32, RaceSummary> summary = new Dictionary<int, RaceSummary>();

            try
            {

                foreach (LapData record in data)
                {
                    // calc diff again recorded cache
                    RacersData RacerRef;
                    if (!RacersDataList.TryGetValue(record.RacerCar, out RacerRef) || (RacerRef.RacrStatus == 3) || (RacerRef.RacrStatus == 4) || (RacerRef.RacrStatus == 5) || (RacerRef.RacrStatus == 7))
                    {
                        return false;
                    }

                    TimeSpan alfa = RacerRef.ReferenceTime - record.TimeFinal;

                    TimeSpan time;
                    if (AcumulationPerLap.TryGetValue(record.RacerCar, out time))
                    {
                        AcumulationPerLap[record.RacerCar] += alfa.Duration();
                    }
                    else
                    {
                        AcumulationPerLap.Add(record.RacerCar, alfa.Duration());
                    }
                }

                // flush && sort
                List<KeyValuePair<Int32, TimeSpan>> ToSort = new List<KeyValuePair<int, TimeSpan>>();
                foreach (var item in AcumulationPerLap)
                {
                    ToSort.Add(new KeyValuePair<int, TimeSpan>(item.Key, item.Value));
                }
                ToSort.Sort((x, y) => x.Value.CompareTo(y.Value));

                // ok get position based on sorted acumulation
                for (int pos = 0; pos < ToSort.Count; pos++)
                {
                    for (int it = 0; it < FinalData.Count; it++)
                    {
                        if (FinalData[it].RacerCar == ToSort[pos].Key)
                        {
                            LapData tmp = FinalData[it];
                            tmp.Position = pos + 1;
                            FinalData[it] = tmp;
                        }
                    }
                }

                // make sum of all lap data into one strcture, flush data into local cache later
                LapCache.Clear();
                foreach (LapData lap_d in FinalData)
                {
                    if (summary.ContainsKey(lap_d.RacerCar))
                    {
                        RaceSummary tmp = summary[lap_d.RacerCar];
                        tmp.Time += lap_d.Time.Duration();
                        tmp.PenTime += lap_d.Penal.Duration();
                        tmp.FinalTime += lap_d.TimeFinal;
                        //tmp.RacerStatusID = (lap_d.Lap < LapsInSelectedRace.Laps) ? RacersDataList[lap_d.RacerCar].RacrStatus : 6;
                        summary[lap_d.RacerCar] = tmp;
                    }
                    else
                    {
                        RaceSummary record = new RaceSummary
                        {
                            Position = lap_d.Position,
                            // sxtract from cache
                            FullName = RacerData[RacersDataList[lap_d.RacerCar].Racer].FName + " " + RacerData[RacersDataList[lap_d.RacerCar].Racer].LName,
                            StartNumber = RacersDataList[lap_d.RacerCar].StartNumber,
                            Team = TeamsData[RacersDataList[lap_d.RacerCar].Team].Name,
                            RefTime = RacersDataList[lap_d.RacerCar].ReferenceTime.ToString(),
                            Time = lap_d.Time.Duration(),
                            PenTime = lap_d.Penal.Duration(),
                            FinalTime = lap_d.TimeFinal,
                            Car = CarData[RacersDataList[lap_d.RacerCar].RacerCar].Brand + "(" + CarData[RacersDataList[lap_d.RacerCar].RacerCar].Model + ")",
                            RacerStatusID = RacersDataList[lap_d.RacerCar].RacrStatus,
                            RacerStatusText = RacersDataList[lap_d.RacerCar].RacerStatus,
                            RacerID = RacersDataList[lap_d.RacerCar].Racer
                        };
                        summary.Add(lap_d.RacerCar, record);
                    }

                    if (lap_d.Lap > LapsInSelectedRace.Laps)//summary[lap_d.RacerCar].RacerStatusID == 6)
                    {
                        int StatOfRacer = 6;
                        // IDRacerCar lap_d.RacerCar
                        // ChangeRacerStatus
                        try
                        {
                            MySqlCommand ChangeRacerState_cmd = Database.Database.CreateCommand(string.Format(Database.Database.QueryStack["ChangeRacerStatus"], StatOfRacer));
                            int retCmd = ChangeRacerState_cmd.ExecuteNonQuery();
                        }
                        catch { }
                    }

                    // make lap record inside cache
                    LapCache.Add(new RaceLaps
                    {

                        Position = lap_d.Position,
                        Lap = lap_d.Lap,
                        FullName = RacerData[RacersDataList[lap_d.RacerCar].Racer].FName + " " + RacerData[RacersDataList[lap_d.RacerCar].Racer].LName,
                        StartNumber = RacersDataList[lap_d.RacerCar].StartNumber,
                        Team = TeamsData[RacersDataList[lap_d.RacerCar].Team].Name,
                        RefTime = RacersDataList[lap_d.RacerCar].ReferenceTime.ToString(),
                        Time = lap_d.Time,
                        PenTime = lap_d.Penal,
                        FinalTime = lap_d.TimeFinal,
                        Car = CarData[RacersDataList[lap_d.RacerCar].RacerCar].Brand + "(" + CarData[RacersDataList[lap_d.RacerCar].RacerCar].Model + ")",
                        Note = lap_d.Note
                    }) ;
                    //LapCache;
                }

                

                // convert into list, clear cache befor adding !!!!
                FinalSummary.Clear();
                foreach (var rec in summary)
                {
                    FinalSummary.Add(rec.Value);
                }

                FinalSummary.Sort((x,y)=> x.Position.CompareTo(y.Position));

                //position base on acumulation
                //foreach (var item in FinalData)
                //{
                //    Console.WriteLine(string.Format("Racer {0} with time {1}", item.RacerCar, item.Position));
                //}
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }

            return true;
        }

        public RaceLaps CalculateLap(TimeSpan time, Int32 RacerID)
        {
            RaceLaps lap = new RaceLaps();

            // acumulate laps time for racer
            List<LapData> laps = new List<LapData>();
            try
            {
                // load tata to be feeded into selector
                MySqlCommand cmd = Database.Database.CreateCommand(string.Format(Database.Database.QueryStack["SelLapData"], SelectedRace));
                MySqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {

                    // push selected value
                    LapData record = new LapData
                    {
                        ID = rdr.GetInt32("ID_Lap_Data"),
                        Measurer = rdr.GetInt32("FK_MeasCom_Measurer"),
                        RacerCar = rdr.GetInt32("FK_Racer_Car"),
                        Lap = rdr.GetInt32("Lap"),
                        Time = rdr.GetTimeSpan("Time"),
                        Penal = rdr.GetTimeSpan("Penalization")
                    };

                    if (record.RacerCar == RacerID)
                    {
                        laps.Add(record);
                    }
                }
                rdr.Close();
            }
            catch (Exception)  {}

            TimeSpan timeAcumulated = TimeSpan.Zero;
            foreach(LapData ld in laps)
            {
                timeAcumulated += (ld.Time);
            }

            TimeSpan calTime = new TimeSpan(time.Hours - timeAcumulated.Hours, time.Minutes - timeAcumulated.Minutes, time.Seconds - timeAcumulated.Seconds);
            lap.Time = calTime;
            lap.Lap = laps.Count;   // counter start with zero

            //if(laps.Count == LapsInSelectedRace.Laps)
            //{
            //    RacerStatusText.
            //}


            int Finisher = 0;
            int CountOfRacer = 0;
            //DB CountOfRacersInFin insert RaceID
            try
            {
                MySqlCommand CountOfRacersInFinish = Database.Database.CreateCommand(string.Format(Database.Database.QueryStack["CountScorPos"], this.SelectedRace));
                Finisher = Convert.ToInt32(CountOfRacersInFinish.ExecuteScalar());
            }
            catch { }


            //DB Countofracersinrace insert RaceID
            try
            {
                MySqlCommand CountOfRacersInRace = Database.Database.CreateCommand(string.Format(Database.Database.QueryStack["Countofracersinrace"], this.SelectedRace));
                CountOfRacer = Convert.ToInt32(CountOfRacersInRace.ExecuteScalar());
            }
            catch { }

            //DB ChangeRaceStatus insert statusID raceID
            if (Finisher == CountOfRacer)
            {
                try
                {
                    MySqlCommand ChangeRaceState_cmd = Database.Database.CreateCommand(string.Format(Database.Database.QueryStack["ChangeRaceStatus"], 3, this.SelectedRace));
                    int retCmd = ChangeRaceState_cmd.ExecuteNonQuery();
                }
                catch { }
            }


            return lap;
        }


        #region RaceLapsDataExport

        public struct RaceLaps
        {
            public Int32 ID { get; set; }
            public Int32 Position { set; get; }
            public Int32 Lap { get; set; }
            public string FullName { get; set; }
            public Int32 StartNumber { get; set; }
            public string Team { get; set; }
            public string RefTime { get; set; }
            public TimeSpan Time { set; get; }
            public TimeSpan PenTime { set; get; }
            public TimeSpan FinalTime { set; get; }
            public string Car { get; set; }
            public string Note { get; set; }
        }

        public List<RaceLaps> GetLaps()
        {
            return LapCache;
        }

        public List<RaceSummary> GetSummary()
        {
            return this.FinalSummary;
        }

        public RaceTime GetRaceTimes()
        {
            return this._RaceTime;
        }

        public List<Racer> GetRacersInRace()
        {
            // filter base on NN koncept
            List<Racer> final_list = new List<Racer>();
            try
            {
                MySqlCommand cmd = Database.Database.CreateCommand(string.Format( Database.Database.QueryStack["GetRacersInRaceData"], SelectedRace));
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    final_list.Add(new Racer
                    {
                        ID = reader.GetInt32("ID_Racer_Car"),
                        FName = reader.GetString("First_Name"),
                        LName = reader.GetString("Last_Name"),
                        StartNumber = reader.GetInt32("Start_num"),
                        RacerStatus = reader.GetInt32("FK_Status_Racer"),
                    });
                }
                reader.Close();
            }
            catch (Exception e) { Console.WriteLine(e.Message); }

            return final_list;
        }

        #endregion
    }
}
