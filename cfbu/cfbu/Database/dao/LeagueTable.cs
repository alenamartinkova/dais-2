using System;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace cfbu.Database.dao
{
    public class LeagueTable
    {
        public static string SQL_SELECT = "SELECT leagueID, name, category FROM League";
        public static string SQL_INSERT = "INSERT INTO League (name, category) VALUES (@name, @category)";
        public static string SQL_UPDATE = "UPDATE League SET name = @name, category = @category WHERE leagueID = @leagueID";

        private static void PrepareCommand(SqlCommand cmd, League league)
        {
            cmd.Parameters.AddWithValue("@leagueID", league.leagueID);
            cmd.Parameters.AddWithValue("@name", league.name);
            cmd.Parameters.AddWithValue("@category", league.category);
        }

        public static List<League> Read(SqlDataReader reader)
        {
            List<League> leagues = new List<League>();

            while (reader.Read())
            {
                int i = -1;
                League l = new League();
                l.leagueID = reader.GetInt32(++i);
                l.name = reader.GetString(++i);
                l.category = reader.GetInt32(++i);

                leagues.Add(l);
            }

            return leagues;
        }

        public static int Insert(Database database, League league)
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
            PrepareCommand(cmd, league);
            int rtn = db.ExecuteNonQuery(cmd);

            if (database == null)
            {
                db.Close();
            }

            return rtn;
        }

        public static int Update(League league, Database database = null)
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
            PrepareCommand(cmd, league);
            int rtn = db.ExecuteNonQuery(cmd);

            if (database == null)
            {
                db.Close();
            }

            return rtn;
        }
    }
}
