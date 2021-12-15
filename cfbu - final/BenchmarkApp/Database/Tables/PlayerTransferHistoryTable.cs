using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using AspNetExampleApp.Database;
using BenchmarkApp.Database.Entities;

namespace ProjektORM.Databse.SQLS
{
    public class PlayerTransferHistoryTable : DbTable
    {
       
        public PlayerTransferHistoryTable(Database database) : base(database, "PlayerTransferHistory")
        {
 
        }

        public static String SQL_SELECT = "SELECT playerTransferID, oldTeamID, newTeamID, playerID, date FROM PlayerTransferHistory";
        public static String SQL_INSERT = "INSERT INTO PlayerTransferHistory (oldTeamID, newTeamID, playerID, date) VALUES (@oldTeamID, @newTeamID, @playerID, @date)";
        public static String SQL_UPDATE = "UPDATE PlayerTransferHistory SET oldTeamID = @oldTeamID, newTeamID = @newTeamID, playerID = @playerID, date = @date WHERE playerTransferID = @playerTransferID";
        // stránkovanie
        public static String SQL_SELECT_ALL = @"SELECT *
                                                FROM PlayerTransferHistory
                                                ORDER BY pitchID
                                                OFFSET 2499 ROWS FETCH NEXT 20 ROWS ONLY;";

        public static String SQL_SELECT_ALL_PLAYER = "SELECT playerTransferID, oldTeamID, newTeamID, playerID, date FROM PlayerTransferHistory WHERE playerID  = @playerID";

        private static void PrepareCommand(SqlCommand cmd, PlayerTransferHistory playerTH)
        {
            cmd.Parameters.AddWithValue("@playerTransferID", playerTH.playerTransferID);
            cmd.Parameters.AddWithValue("@oldTeamID", playerTH.oldTeamID);
            cmd.Parameters.AddWithValue("@newTeamID", playerTH.newTeamID);
            cmd.Parameters.AddWithValue("@playerID", playerTH.playerID);
            cmd.Parameters.AddWithValue("@date", playerTH.date);
        }

        public static List<PlayerTransferHistory> Read(SqlDataReader reader)
        {
            List<PlayerTransferHistory> playersTH = new List<PlayerTransferHistory>();
            
            while (reader.Read())
            {
                int i = -1;
                PlayerTransferHistory p = new PlayerTransferHistory();
                p.playerTransferID = reader.GetInt32(++i);
                p.oldTeamID = reader.GetInt32(++i);
                p.newTeamID = reader.GetInt32(++i);
                p.playerID = reader.GetInt32(++i);
                p.date = reader.GetDateTime(++i);

                playersTH.Add(p);
            }

            return playersTH;
        }

        public List<PlayerTransferHistory> SelectAll()
        {
            mDatabase.Connect();

            SqlCommand command = mDatabase.CreateCommand(SQL_SELECT_ALL);
            SqlDataReader reader = mDatabase.Select(command);

            List<PlayerTransferHistory> pth = Read(reader);
            reader.Close();

            mDatabase.Close();
            return pth;
        }

        public int Insert(PlayerTransferHistory playerTH)
        {
            mDatabase.Connect();
            SqlCommand cmd = mDatabase.CreateCommand(SQL_INSERT);
            PrepareCommand(cmd, playerTH);
            int rtn = mDatabase.ExecuteNonQuery(cmd);

            mDatabase.Close();        

            return rtn;
        }

        public int Update(PlayerTransferHistory playerTH)
        {
            mDatabase.Connect();

            SqlCommand cmd = mDatabase.CreateCommand(SQL_UPDATE);
            PrepareCommand(cmd, playerTH);
            int rtn = mDatabase.ExecuteNonQuery(cmd);

            mDatabase.Close();
            
            return rtn;
        }

        /**
         * pth4 - Select full history for player
         */
        public List<PlayerTransferHistory> GetPlayerTransferHistoriesForPlayer(int playerID)
        {
            mDatabase.Connect();

            SqlCommand command = mDatabase.CreateCommand(SQL_SELECT_ALL_PLAYER);

            //Vytvoreni parametru
            SqlParameter input = new SqlParameter();
            input.ParameterName = "@playerID";
            input.DbType = DbType.Int32;
            input.Value = playerID;
            input.Direction = ParameterDirection.Input;
            command.Parameters.Add(input);

            SqlDataReader reader = command.ExecuteReader();
            List<PlayerTransferHistory> playerTransferHistories = Read(reader);

            mDatabase.Close();
            reader.Close();

            return playerTransferHistories;
        }
    }
}
