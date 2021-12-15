using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using AspNetExampleApp.Database;
using BenchmarkApp.Database.Entities;

namespace ProjektORM.Databse.SQLS
{
    public class LeagueTable : DbTable
    {
        public LeagueTable(Database database) : base(database, "League")
        {
           
        }

        public static String SQL_SELECT = "SELECT leagueID, name, category FROM League";
        public static String SQL_INSERT = "INSERT INTO League (name, category) VALUES (@name, @category)";
        public static String SQL_UPDATE = "UPDATE League SET name = @name, category = @category WHERE leagueID = @leagueID";

        // stránkovanie
        public static String SQL_SELECT_ALL = @"SELECT *
                                                FROM League
                                                ORDER BY leagueID
                                                OFFSET 25 ROWS FETCH NEXT 20 ROWS ONLY;";

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

        public List<League> SelectAll()
        {
            mDatabase.Connect();

            SqlCommand command = mDatabase.CreateCommand(SQL_SELECT_ALL);
            SqlDataReader reader = mDatabase.Select(command);

            List<League> leagues = Read(reader);
            reader.Close();

            mDatabase.Close();
            return leagues;
        }

        public int Insert(League league)
        {
            mDatabase.Connect();

            SqlCommand cmd = mDatabase.CreateCommand(SQL_INSERT);
            PrepareCommand(cmd, league);
            int rtn = mDatabase.ExecuteNonQuery(cmd);
            mDatabase.Close();
            return rtn;
        }

        public int Update(League league)
        {
            mDatabase.Connect();

            SqlCommand cmd = mDatabase.CreateCommand(SQL_UPDATE);
            PrepareCommand(cmd, league);
            int rtn = mDatabase.ExecuteNonQuery(cmd);
            mDatabase.Close();

            return rtn;
        }
    }
}
