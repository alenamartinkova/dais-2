using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace cfbu.Database.dao
{
    public class PlayerTable
    {
        public static string SQL_SELECT = "SELECT playerID, teamID, email, dateOfBirth, firstName, lastName, stick, covid FROM Player";
        public static string SQL_INSERT = "INSERT INTO Player (teamID, email, dateOfBirth, firstName, lastName, stick, covid) VALUES (@teamID, @email, @dateOfBirth, @firstName, @lastName, @stick, @covid)";
        public static string SQL_UPDATE = "UPDATE Player SET teamID = @teamID, email = @email, dateOfBirth = @dateOfBirth, firstName = @firstName, lastName = @lastName, stick = @stick, covid = @covid WHERE playerID = @playerID";

        private static void PrepareCommand(SqlCommand cmd, Player player)
        {
            cmd.Parameters.AddWithValue("@playerID", player.playerID);
            cmd.Parameters.AddWithValue("@teamID", player.teamID);
            cmd.Parameters.AddWithValue("@email", player.email);
            cmd.Parameters.AddWithValue("@dateOfBirth", player.dateOfBirth);
            cmd.Parameters.AddWithValue("@firstName", player.firstName);
            cmd.Parameters.AddWithValue("@lastName", player.lastName);

            SqlParameter stick = cmd.Parameters.AddWithValue("@stick", player.stick);
            if (player.stick == null)
            {
                stick.Value = DBNull.Value;
            }

            cmd.Parameters.AddWithValue("@covid", player.covid);
        }

        public static List<Player> Read(SqlDataReader reader)
        {
            List<Player> players = new List<Player>();

            while (reader.Read())
            {
                int i = -1;
                Player p = new Player();
                p.playerID = reader.GetInt32(++i);
                p.teamID = reader.GetInt32(++i);
                p.email = reader.GetString(++i);
                p.dateOfBirth = reader.GetDateTime(++i);
                p.firstName = reader.GetString(++i);
                p.lastName = reader.GetString(++i);
                if (!reader.IsDBNull(++i)) { p.stick = reader.GetChar(i); };
                p.covid = reader.GetInt32(++i);

                players.Add(p);
            }

            return players;
        }

        public static int Insert(Database database, Player player)
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
            PrepareCommand(cmd, player);
            int rtn = db.ExecuteNonQuery(cmd);

            if (database == null)
            {
                db.Close();
            }

            return rtn;
        }

        public static int Update(Player player, Database database = null)
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
            PrepareCommand(cmd, player);
            int rtn = db.ExecuteNonQuery(cmd);

            if (database == null)
            {
                db.Close();
            }

            return rtn;
        }

        // TRANSACTION
        public static int PlayerTransfer(int playerID, int newTeamID, Database database = null)
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

            SqlCommand cmd = db.CreateCommand("playerTransfer");
            cmd.CommandType = CommandType.StoredProcedure;

            // player ID
            SqlParameter s_playerID = new SqlParameter();
            s_playerID.ParameterName = "@p_id";
            s_playerID.DbType = DbType.Int32;
            s_playerID.Value = playerID;
            s_playerID.Direction = ParameterDirection.Input;
            cmd.Parameters.Add(s_playerID);

            // team ID
            SqlParameter s_teamID = new SqlParameter();
            s_teamID.ParameterName = "@new_t_id";
            s_teamID.DbType = DbType.Int32;
            s_teamID.Value = newTeamID;
            s_teamID.Direction = ParameterDirection.Input;
            cmd.Parameters.Add(s_teamID);

            db.ExecuteNonQuery(cmd);

            if (database == null)
            {
                db.Close();
            }

            return 0;
        }
    }
}
