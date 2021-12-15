using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace cfbu.Database.dao
{
    public class PlayerTransferHistoryTable
    {
        public static string SQL_SELECT = "SELECT playerTransferID, oldTeamID, newTeamID, playerID, date FROM PlayerTransferHistory";
        public static string SQL_INSERT = "INSERT INTO PlayerTransferHistory (oldTeamID, newTeamID, playerID, date) VALUES (@oldTeamID, @newTeamID, @playerID, @date)";
        public static string SQL_UPDATE = "UPDATE PlayerTransferHistory SET oldTeamID = @oldTeamID, newTeamID = @newTeamID, playerID = @playerID, date = @date WHERE playerTransferID = @playerTransferID";

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

        public static int Insert(Database database, PlayerTransferHistory playerTH)
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
            PrepareCommand(cmd, playerTH);
            int rtn = db.ExecuteNonQuery(cmd);

            if (database == null)
            {
                db.Close();
            }

            return rtn;
        }

        public static int Update(PlayerTransferHistory playerTH, Database database = null)
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
            PrepareCommand(cmd, playerTH);
            int rtn = db.ExecuteNonQuery(cmd);

            if (database == null)
            {
                db.Close();
            }

            return rtn;
        }
    }
}
