using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BenchmarkApp.Database.Entities;
using AspNetExampleApp.Database;

namespace ProjektORM.Databse.SQLS
{
    public class TeamTable : DbTable
    {
        private Database db;

        public TeamTable(Database database) : base(database, "Team")
        {
            db = database;
        }

        public static string SQL_SELECT = "SELECT teamID, leagueID, name, rank, covid, quarantinedFrom FROM Team";
        public static string SQL_INSERT = "INSERT INTO Team (leagueID, name, rank, covid, quarantinedFrom) VALUES (@leagueID, @name, @rank, @covid, @quarantinedFrom)";
        public static string SQL_UPDATE = "UPDATE Team SET leagueID = @leagueID, name = @name, rank = @rank, covid = @covid, quarantinedFrom = @quarantinedFrom WHERE teamID = @teamID";

        private static void PrepareCommand(SqlCommand cmd, Team team)
        {
            cmd.Parameters.AddWithValue("@teamID", team.teamID);
            cmd.Parameters.AddWithValue("@leagueID", team.leagueID);
            cmd.Parameters.AddWithValue("@name", team.name);
            cmd.Parameters.AddWithValue("@rank", team.rank);
            cmd.Parameters.AddWithValue("@covid", team.covid);

            SqlParameter quarantinedFrom = cmd.Parameters.AddWithValue("@quarantinedFrom", team.quarantinedFrom);
            if (team.quarantinedFrom == null)
            {
                quarantinedFrom.Value = DBNull.Value;
            }
        }

        public static List<Team> Read(SqlDataReader reader)
        {
            List<Team> teams = new List<Team>();

            while (reader.Read())
            {
                int i = -1;
                Team t = new Team();
                t.teamID = reader.GetInt32(++i);
                t.leagueID = reader.GetInt32(++i);
                t.name = reader.GetString(++i);
                t.rank = reader.GetInt32(++i);
                t.covid = reader.GetInt32(++i);
                if (!reader.IsDBNull(++i)) { t.quarantinedFrom = reader.GetDateTime(i); };

                teams.Add(t);
            }

            return teams;
        }

        /**
         * t1 - Insert team 
         */
        public int Insert(Team team)
        {
            db.Connect();

            SqlCommand cmd = db.CreateCommand(SQL_INSERT);
            PrepareCommand(cmd, team);
            int rtn = db.ExecuteNonQuery(cmd);
            db.Close();   

            return rtn;
        }

        public int Update(Team team)
        {
            db.Connect();

            SqlCommand cmd = db.CreateCommand(SQL_UPDATE);
            PrepareCommand(cmd, team);
            int rtn = db.ExecuteNonQuery(cmd);
            db.Close();    

            return rtn;
        }

        // TRANSACTION
        public int TeamUpdateCovid(int teamID)
        {
            db.Connect();

            SqlCommand cmd = db.CreateCommand("teamUpdateCovid");
            cmd.CommandType = CommandType.StoredProcedure;

            // team ID
            SqlParameter s_teamID = new SqlParameter();
            s_teamID.ParameterName = "@t_id";
            s_teamID.DbType = DbType.Int32;
            s_teamID.Value = teamID;
            s_teamID.Direction = ParameterDirection.Input;
            cmd.Parameters.Add(s_teamID);

            db.ExecuteNonQuery(cmd);
     
            db.Close();
        
            return 0;
        }

        // TRANSACTION
        public int TransferTeam(int teamID, int leagueID)
        {
            db.Connect();

            SqlCommand cmd = db.CreateCommand("transferTeam");
            cmd.CommandType = CommandType.StoredProcedure;

            // team ID
            SqlParameter s_teamID = new SqlParameter();
            s_teamID.ParameterName = "@t_id";
            s_teamID.DbType = DbType.Int32;
            s_teamID.Value = teamID;
            s_teamID.Direction = ParameterDirection.Input;
            cmd.Parameters.Add(s_teamID);

            // league ID
            SqlParameter s_leagueID = new SqlParameter();
            s_leagueID.ParameterName = "@new_l_id";
            s_leagueID.DbType = DbType.Int32;
            s_leagueID.Value = leagueID;
            s_leagueID.Direction = ParameterDirection.Input;
            cmd.Parameters.Add(s_leagueID);

            db.ExecuteNonQuery(cmd);

            db.Close();

            return 0;
        }
    }
}
