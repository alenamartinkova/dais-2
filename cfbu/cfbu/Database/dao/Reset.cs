using System;
using System.Data;
using System.Data.SqlClient;

namespace cfbu.Database.dao
{
    public class Reset
    {
        public static int Restart(Database database)
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

            SqlCommand cmd = db.CreateCommand("default");
            cmd.CommandType = CommandType.StoredProcedure;
            int rtn = db.ExecuteNonQuery(cmd);

            if (database == null)
            {
                db.Close();
            }
            return rtn;
        }
    }
}
