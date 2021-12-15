using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using AspNetExampleApp.Database;
using BenchmarkApp.Database.Entities;

namespace ProjektORM.Databse.SQLS
{
    public class PlayerTable : DbTable
    {
      
        public PlayerTable(Database database) : base(database, "Player")
        {
          
        }

        public static String SQL_SELECT = "SELECT playerID, teamID, email, dateOfBirth, firstName, lastName, stick, covid FROM Player";
        public static String SQL_INSERT = "INSERT INTO Player (teamID, email, dateOfBirth, firstName, lastName, stick, covid) VALUES (@teamID, @email, @dateOfBirth, @firstName, @lastName, @stick, @covid)";
        public static String SQL_UPDATE = "UPDATE Player SET teamID = @teamID, email = @email, dateOfBirth = @dateOfBirth, firstName = @firstName, lastName = @lastName, stick = @stick, covid = @covid WHERE playerID = @playerID";
        // stránkovanie
        public static String SQL_SELECT_ALL = @"SELECT *
                                                FROM Player
                                                ORDER BY playerID
                                                OFFSET 4999 ROWS FETCH NEXT 20 ROWS ONLY;";

        public String SQL_SELECT_ALL_TEAM = "SELECT playerID, teamID, email, dateOfBirth, firstName, lastName, stick, covid FROM Player WHERE teamID = @teamID";
        public static String SQL_UPDATE_P9 = "UPDATE Player SET covid = 1 WHERE teamID = @teamID";


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

        public List<Player> Select()
        {
            mDatabase.Connect();

            SqlCommand command = mDatabase.CreateCommand(SQL_SELECT);
            SqlDataReader reader = mDatabase.Select(command);

            List<Player> players = Read(reader);
            reader.Close();

            mDatabase.Close();
            return players;
        }

        public List<Player> SelectAll()
        {
            mDatabase.Connect();

            SqlCommand command = mDatabase.CreateCommand(SQL_SELECT_ALL);
            SqlDataReader reader = mDatabase.Select(command);

            List<Player> players = Read(reader);
            reader.Close();

            mDatabase.Close();
            return players;
        }

        public int Insert(Player player)
        {
            mDatabase.Connect();

            SqlCommand cmd = mDatabase.CreateCommand(SQL_INSERT);
            PrepareCommand(cmd, player);
            int rtn = mDatabase.ExecuteNonQuery(cmd);

            mDatabase.Close();

            return rtn;
        }

        public int Update(Player player)
        {
            mDatabase.Connect();

            SqlCommand cmd = mDatabase.CreateCommand(SQL_UPDATE);
            PrepareCommand(cmd, player);
            int rtn = mDatabase.ExecuteNonQuery(cmd);

            mDatabase.Close();

            return rtn;
        }

        // TRANSACTION
        public int PlayerTransfer(int playerID, int newTeamID)
        {
            mDatabase.Connect();

            SqlCommand cmd = mDatabase.CreateCommand("playerTransfer");
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

            mDatabase.ExecuteNonQuery(cmd);

            mDatabase.Close();  

            return 0;
        }

        /**
         * p4 - Select all players from team
         */
        public List<Player> GetPlayersFromTeam(int teamID)
        {
            mDatabase.Connect();

            SqlCommand command = mDatabase.CreateCommand(SQL_SELECT_ALL_TEAM);

            //Vytvoreni parametru
            SqlParameter input = new SqlParameter();
            input.ParameterName = "@teamID";
            input.DbType = DbType.Int32;
            input.Value = teamID;
            input.Direction = ParameterDirection.Input;
            command.Parameters.Add(input);

            SqlDataReader reader = command.ExecuteReader();
            List<Player> players = Read(reader);

            mDatabase.Close();
            reader.Close();

            return players;
        }

        /**
         * p9 - update players based on teamID
         */
        public int UpdateP9(int teamID)
        {
            mDatabase.Connect();

            SqlCommand command = mDatabase.CreateCommand(SQL_UPDATE_P9);

            //Vytvoreni parametru
            SqlParameter input = new SqlParameter();
            input.ParameterName = "@teamID";
            input.DbType = DbType.Int32;
            input.Value = teamID;
            input.Direction = ParameterDirection.Input;
            command.Parameters.Add(input);

            int rtn = mDatabase.ExecuteNonQuery(command);
            mDatabase.Close();

            return rtn;
        }
    }
}
