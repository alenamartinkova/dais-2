using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace cfbu.Database.dao
{
    public class StatisticsTable
    {
        public static string SQL_SELECT = "SELECT statisticsID, playerID, teamID, goals, assists FROM Statistics";
        public static string SQL_INSERT = "INSERT INTO Statistics (playerID, teamID, goals, assists) VALUES (@playerID, @teamID, @goals, @assists)";
        public static string SQL_UPDATE = "UPDATE Statistics SET playerID = @playerID, teamID = @teamID, goals = @goals, assists = @assists WHERE statisticsID = @statisticsID";

        private static void PrepareCommand(SqlCommand cmd, Statistics statistics)
        {
            cmd.Parameters.AddWithValue("@statisticsID", statistics.statisticsID);
            cmd.Parameters.AddWithValue("@playerID", statistics.playerID);
            cmd.Parameters.AddWithValue("@teamID", statistics.teamID);
            cmd.Parameters.AddWithValue("@goals", statistics.goals);
            cmd.Parameters.AddWithValue("@assists", statistics.assists);
            cmd.Parameters.AddWithValue("@points", statistics.points);
        }

        public static List<Statistics> Read(SqlDataReader reader)
        {
            List<Statistics> stats = new List<Statistics>();

            while (reader.Read())
            {
                int i = -1;
                Statistics s = new Statistics();
                s.statisticsID = reader.GetInt32(++i);
                s.playerID = reader.GetInt32(++i);
                s.teamID = reader.GetInt32(++i);
                s.goals = reader.GetInt32(++i);
                s.assists = reader.GetInt32(++i);

                stats.Add(s);
            }

            return stats;
        }

        public static int Insert(Database database, Statistics statistics)
        {
            Database db;
            if (database == null)
            {
                db = new Database();
                db.Connect();
            }
            else
            {
                db = (Database)database;
            }

            SqlCommand cmd = db.CreateCommand(SQL_INSERT);
            PrepareCommand(cmd, statistics);
            int rtn = db.ExecuteNonQuery(cmd);

            if (database == null)
            {
                db.Close();
            }

            return rtn;
        }

        public static int Update(Statistics statistics, Database database = null)
        {
            Database db;
            if (database == null)
            {
                db = new Database();
                db.Connect();
            }
            else
            {
                db = (Database)database;
            }

            SqlCommand cmd = db.CreateCommand(SQL_UPDATE);
            PrepareCommand(cmd, statistics);
            int rtn = db.ExecuteNonQuery(cmd);

            if (database == null)
            {
                db.Close();
            }

            return rtn;
        }
    }
}
