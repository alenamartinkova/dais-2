using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace cfbu.Database.dao
{
    public class TeamMatchTable
    {
        public static string SQL_SELECT = "SELECT teamMatchID, firstTeamID, secondTeamID, pitchID, firstTeamGoals, secondTeamGoals, postponed, date FROM TeamMatch";
        public static string SQL_INSERT = "INSERT INTO TeamMatch (firstTeamID, secondTeamID, pitchID, firstTeamGoals, secondTeamGoals, postponed, date) VALUES (@firstTeamID, @secondTeamID, @pitchID, @firstTeamGoals, @secondTeamGoals, @postponed, @date)";
        public static string SQL_UPDATE = "UPDATE TeamMatch SET firstTeamID = @firstTeamID, secondTeamID = @secondTeamID, pitchID = @pitchID, firstTeamGoals = @firstTeamGoals, secondTeamGoals = @secondTeamGoals, postponed = @postponed, date = @date WHERE teamMatchID = @teamMatchID";

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

        public static int Insert(Database database, TeamMatch teamMatch)
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
            PrepareCommand(cmd, teamMatch);
            int rtn = db.ExecuteNonQuery(cmd);

            if (database == null)
            {
                db.Close();
            }

            return rtn;
        }

        public static int Update(TeamMatch teamMatch, Database database = null)
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
            PrepareCommand(cmd, teamMatch);
            int rtn = db.ExecuteNonQuery(cmd);

            if (database == null)
            {
                db.Close();
            }

            return rtn;
        }

        public static int UpdateMatchDate(int teamMatchID, DateTime date, Database database = null)
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

            SqlCommand cmd = db.CreateCommand("updateMatchDate");
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

            db.ExecuteNonQuery(cmd);

            if (database == null)
            {
                db.Close();
            }

            return 0;
        }
    }
}
