using System;
using System.Data;
using System.Data.SqlClient;

namespace SQLInsert
{
    public class SQLBulkInsert
    {
        public static void insertBulk(SqlConnection conn, DataTable dataTable, String tableName)
        {
            SqlBulkCopy bulkCopy = new SqlBulkCopy(conn);
            bulkCopy.DestinationTableName = tableName;
            try
            {
                conn.Open();
                bulkCopy.BulkCopyTimeout = 300;
                DateTime start = DateTime.Now;
                bulkCopy.WriteToServer(dataTable);
                var time = DateTime.Now - start;
                Console.WriteLine("Time: {0:0.00} seconds.", time.TotalSeconds);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Mistake. {0}", ex.Message);

            }
            finally
            {
                conn.Close();
            }
        }
    }
}