using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace SQLInsert
{
    class Program
    {
   
        public static string DbServer = @"dbsys.cs.vsb.cz\STUDENT";
        public static string DbUser = "mar0702";
        public static string DbPassword = "tq71ge9681Iw9JRA";

        static void Main(string[] args)
        {
            string connString = string.Format("Data Source={0};User={1};Password={2};Initial Catalog={1}", DbServer,
                DbUser, DbPassword);
            SqlConnection conn = new SqlConnection(connString);
            /**conn.Open();
            SqlCommand command = new SqlCommand("Select firstName from Player where playerID = 10", conn);
            
            // int result = command.ExecuteNonQuery();
            using (SqlDataReader reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    Console.WriteLine(String.Format("{0}",reader["firstName"]));
                }
            }

            conn.Close(); **/
            
            DataTable data = getDataFromCSV("everything_new_1.csv");
            SQLBulkInsert.insertBulk(conn, data, "TeamMatch");
        }

        public static DataTable getDataFromCSV(String fileName)
        {
            var lines = System.IO.File.ReadAllLines(fileName);
            var columns = lines[0].Split(',');
            var dataTables = new DataTable();
            
            foreach (var column in columns) {
                dataTables.Columns.Add(column);
            }
            
            DataRow workRow;
            for (int i = 1; i < lines.Count(); i++)
            {
                var values = lines[i].Split(',');
                workRow = dataTables.NewRow();
                workRow[0] = values[0];
                workRow[1] = values[1];
                workRow[2] = values[2];
                workRow[3] = values[3];
                workRow[4] = values[4];
                workRow[5] = values[5];
                workRow[6] = values[6];
                var dateArr = values[7].Split('-');
                workRow[7] = new DateTime(Int32.Parse(dateArr[0]), Int32.Parse(dateArr[2]), Int32.Parse(dateArr[1]));

                dataTables.Rows.Add(workRow);
            }

            return dataTables;
        }
    }
}
