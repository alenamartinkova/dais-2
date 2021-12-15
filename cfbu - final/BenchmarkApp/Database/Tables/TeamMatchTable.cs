using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using AspNetExampleApp.Database;
using BenchmarkApp.Database.Entities;

namespace ProjektORM.Databse.SQLS
{
    public class TeamMatchTable : DbTable
    {
       
        public TeamMatchTable(Database database) : base(database, "TeamMatch")
        {
            
        }

        public static string SQL_SELECT = "SELECT teamMatchID, firstTeamID, secondTeamID, pitchID, firstTeamGoals, secondTeamGoals, postponed, date FROM TeamMatch";
        public static string SQL_INSERT = "INSERT INTO TeamMatch (firstTeamID, secondTeamID, pitchID, firstTeamGoals, secondTeamGoals, postponed, date) VALUES (@firstTeamID, @secondTeamID, @pitchID, @firstTeamGoals, @secondTeamGoals, @postponed, @date)";
        public static string SQL_UPDATE = "UPDATE TeamMatch SET firstTeamID = @firstTeamID, secondTeamID = @secondTeamID, pitchID = @pitchID, firstTeamGoals = @firstTeamGoals, secondTeamGoals = @secondTeamGoals, postponed = @postponed, date = @date WHERE teamMatchID = @teamMatchID";
        // stránkovanie
        public static String SQL_SELECT_ALL = @"SELECT *
                                                FROM TeamMatch
                                                ORDER BY teamMatchID
                                                OFFSET 29999 ROWS FETCH NEXT 20 ROWS ONLY;";

        private static void PrepareCommand(SqlCommand cmd, TeamMatch teamMatch)
        {
            cmd.Parameters.AddWithValue("@teamMatchID", teamMatch.teamMatchID);
            cmd.Parameters.AddWithValue("@firstTeamID", teamMatch.firstTeamID);
            cmd.Parameters.AddWithValue("@secondTeamID", teamMatch.secondTeamID);
            cmd.Parameters.AddWithValue("@pitchID", teamMatch.pitchID);
            cmd.Parameters.AddWithValue("@firstTeamGoals", teamMatch.firstTeamGoals);
            cmd.Parameters.AddWithValue("@secondTeamGoals", teamMatch.secondTeamGoals);
            cmd.Parameters.AddWithValue("@postponed", teamMatch.postponed);
            cmd.Parameters.AddWithValue("@date", teamMatch.date);
        }

        public static List<TeamMatch> Read(SqlDataReader reader)
        {
            List<TeamMatch> teamMatches = new List<TeamMatch>();

            while (reader.Read())
            {
                int i = -1;
                TeamMatch tm = new TeamMatch();
                tm.teamMatchID = reader.GetInt32(++i);
                tm.firstTeamID = reader.GetInt32(++i);
                tm.secondTeamID = reader.GetInt32(++i);
                tm.pitchID = reader.GetInt32(++i);
                tm.firstTeamGoals = reader.GetInt32(++i);
                tm.secondTeamGoals = reader.GetInt32(++i);
                tm.postponed = reader.GetInt32(++i);
                tm.date = reader.GetDateTime(++i);

                teamMatches.Add(tm);
            }

            return teamMatches;
        }

        /**
       * tm4 - Select all matches - without paging
       */
        public List<TeamMatch> Select()
        {
            mDatabase.Connect();

            SqlCommand command = mDatabase.CreateCommand(SQL_SELECT);
            SqlDataReader reader = mDatabase.Select(command);

            List<TeamMatch> teamMatches = Read(reader);
            reader.Close();

            mDatabase.Close();
            return teamMatches;
        }

        /**
         * tm4 - Select all matches - paging
         */
        public List<TeamMatch> SelectAll()
        {
            mDatabase.Connect();

            SqlCommand command = mDatabase.CreateCommand(SQL_SELECT_ALL);
            SqlDataReader reader = mDatabase.Select(command);

            List<TeamMatch> teamMatches = Read(reader);
            reader.Close();

            mDatabase.Close();
            return teamMatches;
        }

        public int Insert(TeamMatch teamMatch)
        {
            mDatabase.Connect();

            SqlCommand cmd = mDatabase.CreateCommand(SQL_INSERT);
            PrepareCommand(cmd, teamMatch);
            int rtn = mDatabase.ExecuteNonQuery(cmd);

            mDatabase.Close();

            return rtn;
        }

        public int Update(TeamMatch teamMatch)
        {
            mDatabase.Connect();

            SqlCommand cmd = mDatabase.CreateCommand(SQL_UPDATE);
            PrepareCommand(cmd, teamMatch);
            int rtn = mDatabase.ExecuteNonQuery(cmd);
            mDatabase.Close();     

            return rtn;
        }

        public int UpdateMatchDate(int teamMatchID, DateTime date)
        {
            mDatabase.Connect();

            SqlCommand cmd = mDatabase.CreateCommand("updateMatchDate");
            cmd.CommandType = CommandType.StoredProcedure;

            // team match ID
            SqlParameter tMID = new SqlParameter();
            tMID.ParameterName = "@t_m_id";
            tMID.DbType = DbType.Int32;
            tMID.Value = teamMatchID;
            tMID.Direction = ParameterDirection.Input;
            cmd.Parameters.Add(tMID);

            // date
            SqlParameter date_s = new SqlParameter();
            date_s.ParameterName = "@date";
            date_s.DbType = DbType.Date;
            date_s.Value = date;
            date_s.Direction = ParameterDirection.Input;
            cmd.Parameters.Add(date_s);

            mDatabase.ExecuteNonQuery(cmd);

            mDatabase.Close();

            return 0;
        }
    }
}
