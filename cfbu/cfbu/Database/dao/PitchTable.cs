using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace cfbu.Database.dao
{
    public class PitchTable
    {
        public static string SQL_SELECT = "SELECT pitchID, capacity, name FROM Pitch";
        public static string SQL_INSERT = "INSERT INTO Pitch (capacity, name) VALUES (@capacity, @name)";
        public static string SQL_UPDATE = "UPDATE Pitch SET capacity = @capacity, name = @name WHERE pitchID = @pitchID";

        private static void PrepareCommand(SqlCommand cmd, Pitch pitch)
        {
            cmd.Parameters.AddWithValue("@pitchID", pitch.pitchID);
            cmd.Parameters.AddWithValue("@capacity", pitch.capacity);
            cmd.Parameters.AddWithValue("@name", pitch.name);
        }

        public static List<Pitch> Read(SqlDataReader reader)
        {
            List<Pitch> pitches = new List<Pitch>();

            while (reader.Read())
            {
                int i = -1;
                Pitch p = new Pitch();
                p.pitchID = reader.GetInt32(++i);
                p.capacity = reader.GetInt32(++i);
                p.name = reader.GetString(++i);

                pitches.Add(p);
            }

            return pitches;
        }

        public static int Insert(Database database, Pitch pitch)
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
            PrepareCommand(cmd, pitch);
            int rtn = db.ExecuteNonQuery(cmd);

            if (database == null)
            {
                db.Close();
            }

            return rtn;
        }

        public static int Update(Pitch pitch, Database database = null)
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
            PrepareCommand(cmd, pitch);
            int rtn = db.ExecuteNonQuery(cmd);

            if (database == null)
            {
                db.Close();
            }

            return rtn;
        }
    }
}
