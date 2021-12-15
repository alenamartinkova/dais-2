using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using AspNetExampleApp.Database;
using System.Data;
using BenchmarkApp.Database.Entities;

namespace ProjektORM.Databse.SQLS
{
    public class StatisticTable : DbTable
    {
        
        public StatisticTable(Database database) : base(database, "Statistic")
        {
   
        }

        public static String SQL_SELECT = "SELECT statisticsID, playerID, teamID, goals, assists FROM Statistic";
        public static String SQL_INSERT = "INSERT INTO Statistic (playerID, teamID, goals, assists) VALUES (@playerID, @teamID, @goals, @assists)";
        public static String SQL_UPDATE = "UPDATE Statistic SET playerID = @playerID, teamID = @teamID, goals = @goals, assists = @assists WHERE statisticsID = @statisticsID";

        // stránkovanie
        public static String SQL_SELECT_ALL = @"SELECT *
                                                FROM Statistic
                                                ORDER BY statisticsID
                                                OFFSET 2499 ROWS FETCH NEXT 20 ROWS ONLY;";

        public static String SQL_UPDATE_S2 = "UPDATE Statistic SET goals = 10, assists = 20 WHERE playerID = @playerID AND teamID = @teamID";
        public static String SQL_SELECT_PLAYER_STATS = "SELECT statisticsID, playerID, teamID, goals, assists FROM Statistic WHERE playerID = @playerID";

        private static void PrepareCommand(SqlCommand cmd, Statistic statistics)
        {
            cmd.Parameters.AddWithValue("@statisticsID", statistics.statisticsID);
            cmd.Parameters.AddWithValue("@playerID", statistics.playerID);
            cmd.Parameters.AddWithValue("@teamID", statistics.teamID);
            cmd.Parameters.AddWithValue("@goals", statistics.goals);
            cmd.Parameters.AddWithValue("@assists", statistics.assists);
            cmd.Parameters.AddWithValue("@points", statistics.points);
        }

        public static List<Statistic> Read(SqlDataReader reader)
        {
            List<Statistic> stats = new List<Statistic>();

            while (reader.Read())
            {
                int i = -1;
                Statistic s = new Statistic();
                s.statisticsID = reader.GetInt32(++i);
                s.playerID = reader.GetInt32(++i);
                s.teamID = reader.GetInt32(++i);
                s.goals = reader.GetInt32(++i);
                s.assists = reader.GetInt32(++i);

                stats.Add(s);
            }

            return stats;
        }

        public List<Statistic> Select()
        {
            mDatabase.Connect();

            SqlCommand command = mDatabase.CreateCommand(SQL_SELECT);
            SqlDataReader reader = mDatabase.Select(command);

            List<Statistic> statistics = Read(reader);
            reader.Close();

            mDatabase.Close();
            return statistics;
        }

        public List<Statistic> SelectAll()
        {
            mDatabase.Connect();

            SqlCommand command = mDatabase.CreateCommand(SQL_SELECT_ALL);
            SqlDataReader reader = mDatabase.Select(command);

            List<Statistic> statistics = Read(reader);
            reader.Close();

            mDatabase.Close();
            return statistics;
        }

        public int Insert(Statistic statistics)
        {
            mDatabase.Connect();

            SqlCommand cmd = mDatabase.CreateCommand(SQL_INSERT);
            PrepareCommand(cmd, statistics);
            int rtn = mDatabase.ExecuteNonQuery(cmd);

            mDatabase.Close();    

            return rtn;
        }

        public int Update(Statistic statistics)
        {
            mDatabase.Connect();

            SqlCommand cmd = mDatabase.CreateCommand(SQL_UPDATE);
            PrepareCommand(cmd, statistics);
            int rtn = mDatabase.ExecuteNonQuery(cmd);

            mDatabase.Close();

            return rtn;
        }

        /**
         * s2 - update statistics based on player and team IDs
         */
        public int UpdateS2(int playerID, int teamID)
        {
            mDatabase.Connect();

            SqlCommand command = mDatabase.CreateCommand(SQL_UPDATE_S2);

            //Vytvoreni parametru
            SqlParameter inputPlayer = new SqlParameter();
            inputPlayer.ParameterName = "@playerID";
            inputPlayer.DbType = DbType.Int32;
            inputPlayer.Value = playerID;
            inputPlayer.Direction = ParameterDirection.Input;
            command.Parameters.Add(inputPlayer);

            //Vytvoreni parametru
            SqlParameter inputTeam = new SqlParameter();
            inputTeam.ParameterName = "@teamID";
            inputTeam.DbType = DbType.Int32;
            inputTeam.Value = teamID;
            inputTeam.Direction = ParameterDirection.Input;
            command.Parameters.Add(inputTeam);

            int rtn = mDatabase.ExecuteNonQuery(command);
            mDatabase.Close();

            return rtn;
        }

        /**
         * s4 - select player stats 
         */
        public List<Statistic> SelectPlayerStats(int playerID)
        {
            mDatabase.Connect();

            SqlCommand command = mDatabase.CreateCommand(SQL_SELECT_PLAYER_STATS);

            //Vytvoreni parametru
            SqlParameter inputPlayer = new SqlParameter();
            inputPlayer.ParameterName = "@playerID";
            inputPlayer.DbType = DbType.Int32;
            inputPlayer.Value = playerID;
            inputPlayer.Direction = ParameterDirection.Input;
            command.Parameters.Add(inputPlayer);

            SqlDataReader reader = mDatabase.Select(command);

            List<Statistic> statistics = Read(reader);
            mDatabase.Close();

            return statistics;
        }
    }
}
