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
    public class Tournament
    {       
        public class RacersInTourPoints
        {
            public int IDRacer { get; set; }
            public int Points { get; set; } //from PointsInTour
            public int StartNum { get; set; } 
            public string RacerTeam { get; set; }
            public string RacerName { get; set; }
        }

        public class ScoredRacerInTourByPos
        {
            public int Position { get; set; }
            public int Points { get; set; }
            public int StartNum { get; set; }
            public string RacerName { get; set; }
            public Int32 RacerId { get; set; }
            public string RacerTeam { get; set; }
        }

        public class PointsInTour
        {
            public int IDPoints { get; set; }
            public int Position { get; set; }
            public int Points { get; set; }
        }

        public class RacesInTourHS
        {
            public int RaceID { get; set; }
        }
        

        Race RaceTracker = null;

        int SelectedTourID = 0;
        List<ScoredRacerInTourByPos> ExpDataRaceInTour = new List<ScoredRacerInTourByPos>();
        List<RacesInTourHS> RaceIDs = new List<RacesInTourHS>();
        List<PointsInTour> PointsListInTour = new List<PointsInTour>();
        public List<ScoredRacerInTourByPos> FinalExportDataTour;
        Dictionary<Int32, ScoredRacerInTourByPos> DataFinalList = new Dictionary<int, ScoredRacerInTourByPos>();
        List<Race.RaceSummary> final_list = null;// new List<Race.RaceSummary>();

        public Tournament(Int32 TourID)
        {
            FinalExportDataTour = new List<ScoredRacerInTourByPos>();
            SelectedTourID = TourID;
            try
            {
                MySqlCommand command = Database.Database.CreateCommand(string.Format(Database.Database.QueryStack["RacesIDinTourHS"], SelectedTourID));
                MySqlDataReader rdr = command.ExecuteReader();

                while (rdr.Read()){RaceIDs.Add(new RacesInTourHS(){RaceID = rdr.GetInt32("FK_Race")});}
                rdr.Close();
            }
            catch { }

            try
            {
                MySqlCommand command_pc = Database.Database.CreateCommand(string.Format(Database.Database.QueryStack["PointsInTourHS"], SelectedTourID));
                MySqlDataReader rdr = command_pc.ExecuteReader();

                while (rdr.Read())
                {
                    PointsListInTour.Add(
                        new PointsInTour()
                        {
                            IDPoints = rdr.GetInt32("ID_Category_Points"),
                            Position = rdr.GetInt32("Position"),
                            Points = rdr.GetInt32("Points")
                        }
                    );
                }
                rdr.Close();
            }
            catch { }

            //CountOfRacerInTour
            int count_ofracers = 0;
            try
            {
                MySqlCommand check_scorpos = Database.Database.CreateCommand(string.Format(Database.Database.QueryStack["CountOfRacerInTour"], SelectedTourID));
                count_ofracers = (int)Convert.ToInt32(check_scorpos.ExecuteScalar());
               // int rrrr = check_scorpos.ExecuteNonQuery();
            }
            catch { }

            Dictionary<int, int> PointsByPossition = new Dictionary<int, int>();
            foreach(PointsInTour points in PointsListInTour)
            {
                if (!PointsByPossition.ContainsKey(points.Position))
                    PointsByPossition.Add(points.Position,points.Points);
            }

            int count_scorpos = 0;
            try
            {
                MySqlCommand check_scorpos = Database.Database.CreateCommand(string.Format(Database.Database.QueryStack["CountOfScoredPos"], SelectedTourID));
                count_scorpos = (int)Convert.ToInt32(check_scorpos.ExecuteScalar());
                //int rrr = check_scorpos.ExecuteNonQuery();
            }
            catch { }

            try
            {
                // make local tmp for positioned data, this will be based not on data but id of racer
                Dictionary<int, ScoredRacerInTourByPos> fin_t = new Dictionary<int, ScoredRacerInTourByPos>();

                foreach (RacesInTourHS RacesInTourIDs in RaceIDs)
                {
                    Race _RaceTracker = new Race(RacesInTourIDs.RaceID);
                    if (_RaceTracker != null && _RaceTracker.LoadLaps())
                    {
                        // data from specific race. limit based on scared positions, fetch as dic
                        final_list = _RaceTracker.GetSummary();
                        Dictionary<int, Race.RaceSummary> scored_ldata = new Dictionary<int, Race.RaceSummary>();
                        foreach (Race.RaceSummary sum in final_list)
                        {
                            if (sum.Position <= count_scorpos)
                            {
                                scored_ldata.Add(sum.Position, sum);
                            }
                        }

                        foreach (var re in scored_ldata)
                        {
                            if (!fin_t.ContainsKey(re.Value.RacerID))
                            {
                                fin_t.Add(re.Value.RacerID, new ScoredRacerInTourByPos
                                {
                                    Points = PointsByPossition[re.Value.Position],
                                    Position = 0,
                                    RacerId = re.Value.RacerID,
                                    RacerName = re.Value.FullName,
                                    RacerTeam = re.Value.Team,
                                    StartNum = re.Value.StartNumber
                                });
                            }
                            else
                            {
                                fin_t[re.Value.RacerID].Points += PointsByPossition[re.Value.Position];
                            }
                        }
                    }
                }

                List<ScoredRacerInTourByPos> tsort = new List<ScoredRacerInTourByPos>();
                foreach(var rec in fin_t)
                {
                    tsort.Add(rec.Value);
                    //Console.WriteLine(rec.Value.Points + ":" + rec.Value.RacerName + ":" + rec.Value.RacerId);
                }
                tsort.Sort((x, y) => x.Points.CompareTo(y.Points));
                int PostFinal = 0;
                int temp_points = 0;
                for (int pos = tsort.Count-1; pos != -1; pos--)
                {
                    ScoredRacerInTourByPos t = new ScoredRacerInTourByPos
                    {
                        Points = tsort[pos].Points,
                        Position = PostFinal,
                        RacerId = tsort[pos].RacerId,
                        RacerName = tsort[pos].RacerName,
                        RacerTeam = tsort[pos].RacerTeam,
                        StartNum = tsort[pos].StartNum
                    };


                    if (tsort[pos].Points == temp_points)
                    {
                        FinalExportDataTour.Add(t);
                    }
                    else
                    {
                        temp_points = tsort[pos].Points;
                        t.Position += 1;
                        PostFinal++;
                        FinalExportDataTour.Add(t);
                    }
                }

                foreach(ScoredRacerInTourByPos re in FinalExportDataTour)
                {
                    Console.WriteLine(re.Points + ":" + re.RacerName + ":" + re.Position);
                }
            }
            catch { }
        }

        public List<ScoredRacerInTourByPos> GetPositionInTour()
        {
            return this.FinalExportDataTour;
        }

        public List<ScoredRacerInTourByPos> GetRacers()
        {
            return this.ExpDataRaceInTour;
        }
    }
}
