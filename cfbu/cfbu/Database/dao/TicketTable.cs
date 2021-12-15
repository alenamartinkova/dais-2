using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;

namespace cfbu.Database.dao
{
    public class TicketTable
    {
        public static string SQL_SELECT = "SELECT ticketID, teamMatchID, firstName, lastName, price, storno, email FROM Ticket";
        public static string SQL_INSERT = "INSERT INTO Ticket (teamMatchID, firstName, lastName, price, storno, email) VALUES (@teamMatchID, @firstName, @lastName, @price, @storno, @email)";
        public static string SQL_UPDATE = "UPDATE Ticket SET teamMatchID = @teamMatchID, firstName = @firstName, lastName = @lastName, price = @price, storno = @storno, email = @email WHERE ticketID = @ticketID";

        private static void PrepareCommand(SqlCommand cmd, Ticket ticket)
        {
            cmd.Parameters.AddWithValue("@ticketID", ticket.ticketID);
            cmd.Parameters.AddWithValue("@teamMatchID", ticket.teamMatchID);
            cmd.Parameters.AddWithValue("@firstName", ticket.firstName);
            cmd.Parameters.AddWithValue("@lastName", ticket.lastName);
            cmd.Parameters.AddWithValue("@price", ticket.price);
            cmd.Parameters.AddWithValue("@storno", ticket.storno);
            cmd.Parameters.AddWithValue("@email", ticket.email);
        }

        public static List<Ticket> Read(SqlDataReader reader)
        {
            List<Ticket> tickets = new List<Ticket>();

            while (reader.Read())
            {
                int i = -1;
                Ticket t = new Ticket();
                t.ticketID = reader.GetInt32(++i);
                t.teamMatchID = reader.GetInt32(++i);
                t.firstName = reader.GetString(++i);
                t.lastName = reader.GetString(++i);
                t.price = reader.GetSqlMoney(++i);
                t.storno = reader.GetInt32(++i);
                t.email = reader.GetString(++i);

                tickets.Add(t);
            }

            return tickets;
        }

        public static int Insert(Database database, Ticket ticket)
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
            PrepareCommand(cmd, ticket);
            int rtn = db.ExecuteNonQuery(cmd);

            if (database == null)
            {
                db.Close();
            }

            return rtn;
        }

        public static int Update(Ticket ticket, Database database = null)
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
            PrepareCommand(cmd, ticket);
            int rtn = db.ExecuteNonQuery(cmd);

            if (database == null)
            {
                db.Close();
            }

            return rtn;
        }

        public static int CreateTicket(int teamMatchID, String firstName, String lastName, String email, SqlMoney price, Database database = null)
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

            SqlCommand cmd = db.CreateCommand("createTicket");
            cmd.CommandType = CommandType.StoredProcedure;

            // team Match ID
            SqlParameter tMID = new SqlParameter();
            tMID.ParameterName = "@t_m_id";
            tMID.DbType = DbType.Int32;
            tMID.Value = teamMatchID;
            tMID.Direction = ParameterDirection.Input;
            cmd.Parameters.Add(tMID);

            // first name
            SqlParameter firstName_p = new SqlParameter();
            firstName_p.ParameterName = "@first_name";
            firstName_p.DbType = DbType.String;
            firstName_p.Value = firstName;
            firstName_p.Direction = ParameterDirection.Input;
            cmd.Parameters.Add(firstName_p);

            // last name
            SqlParameter lastName_p = new SqlParameter();
            lastName_p.ParameterName = "@last_name";
            lastName_p.DbType = DbType.String;
            lastName_p.Value = lastName;
            lastName_p.Direction = ParameterDirection.Input;
            cmd.Parameters.Add(lastName_p);

            // email
            SqlParameter email_p = new SqlParameter();
            email_p.ParameterName = "@email";
            email_p.DbType = DbType.String;
            email_p.Value = email;
            email_p.Direction = ParameterDirection.Input;
            cmd.Parameters.Add(email_p);

            // price
            SqlParameter price_p = new SqlParameter();
            price_p.ParameterName = "@price";
            price_p.DbType = DbType.Currency;
            price_p.Value = price;
            price_p.Direction = ParameterDirection.Input;
            cmd.Parameters.Add(price_p);

            db.ExecuteNonQuery(cmd);

            if (database == null)
            {
                db.Close();
            }

            return 0;
        }
    }
}
