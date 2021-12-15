using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using AspNetExampleApp.Database;
using BenchmarkApp.Database.Entities;

namespace ProjektORM.Databse.SQLS
{
    public class PitchTable : DbTable
    {
        public PitchTable(Database database) : base(database, "Pitch")
        {
            
        }

        public static string SQL_SELECT = "SELECT pitchID, capacity, name FROM Pitch";
        public static string SQL_INSERT = "INSERT INTO Pitch (capacity, name) VALUES (@capacity, @name)";
        public static string SQL_UPDATE = "UPDATE Pitch SET capacity = @capacity, name = @name WHERE pitchID = @pitchID";
        // stránkovanie
        public static String SQL_SELECT_ALL = @"SELECT *
                                                FROM Pitch
                                                ORDER BY pitchID
                                                OFFSET 2499 ROWS FETCH NEXT 20 ROWS ONLY;";

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

        public List<Pitch> SelectAll()
        {
            mDatabase.Connect();

            SqlCommand command = mDatabase.CreateCommand(SQL_SELECT_ALL);
            SqlDataReader reader = mDatabase.Select(command);

            List<Pitch> pitches = Read(reader);
            reader.Close();

            mDatabase.Close();
            return pitches;
        }

        /**
         * pi1 - Insert pitch
         */
        public int Insert(Pitch pitch)
        {
            mDatabase.Connect();

            SqlCommand cmd = mDatabase.CreateCommand(SQL_INSERT);
            PrepareCommand(cmd, pitch);
            int rtn = mDatabase.ExecuteNonQuery(cmd);

            mDatabase.Close();

            return rtn;
        }

        public int Update(Pitch pitch)
        {
            mDatabase.Connect();

            SqlCommand cmd = mDatabase.CreateCommand(SQL_UPDATE);
            PrepareCommand(cmd, pitch);
            int rtn = mDatabase.ExecuteNonQuery(cmd);

            mDatabase.Close();   

            return rtn;
        }
    }
}
