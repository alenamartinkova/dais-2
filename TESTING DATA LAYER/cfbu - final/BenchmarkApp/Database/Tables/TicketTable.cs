using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using BenchmarkApp.Database.Entities;
using AspNetExampleApp.Database;

namespace ProjektORM.Databse.SQLS
{
    public class TicketTable : DbTable
    {


        public TicketTable(Database database) : base(database, "Ticket")
        {
            
        }

        public static String SQL_SELECT = "SELECT ticketID, teamMatchID, firstName, lastName, price, storno, email FROM Ticket";
        public static String SQL_INSERT = "INSERT INTO Ticket (teamMatchID, firstName, lastName, price, storno, email) VALUES (@teamMatchID, @firstName, @lastName, @price, @storno, @email)";
        public static String SQL_UPDATE = "UPDATE Ticket SET teamMatchID = @teamMatchID, firstName = @firstName, lastName = @lastName, price = @price, storno = @storno, email = @email WHERE ticketID = @ticketID";
        // stránkovanie
        public static String SQL_SELECT_ALL = @"SELECT *
                                                FROM Ticket
                                                ORDER BY ticketID
                                                OFFSET 74999 ROWS FETCH NEXT 20 ROWS ONLY;";

        public static String SQL_SELECT_ALL_MATCH = "SELECT ticketID, teamMatchID, firstName, lastName, price, storno, email FROM Ticket WHERE teamMatchid = @teamMatchID";

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

        /**
        * ti3 - Select all - without paging - used in testing
        */
        public List<Ticket> Select()
        {
            mDatabase.Connect();

            SqlCommand command = mDatabase.CreateCommand(SQL_SELECT);
            SqlDataReader reader = mDatabase.Select(command);

            List<Ticket> tickets = Read(reader);
            reader.Close();

            mDatabase.Close();

            return tickets;
        }

        /**
         * ti3 - Select all - paging - used in testing
         */
        public List<Ticket> SelectAll()
        {
            mDatabase.Connect();

            SqlCommand command = mDatabase.CreateCommand(SQL_SELECT_ALL);
            SqlDataReader reader = mDatabase.Select(command);

            List<Ticket> tickets = Read(reader);
            reader.Close();

            mDatabase.Close();
            
            return tickets;
        }

        public int Insert(Ticket ticket)
        {
            mDatabase.Connect();

            SqlCommand cmd = mDatabase.CreateCommand(SQL_INSERT);
            PrepareCommand(cmd, ticket);
            int rtn = mDatabase.ExecuteNonQuery(cmd);

            mDatabase.Close();

            return rtn;
        }

        public int Update(Ticket ticket)
        {
            mDatabase.Connect();
            SqlCommand cmd = mDatabase.CreateCommand(SQL_UPDATE);
            PrepareCommand(cmd, ticket);
            int rtn = mDatabase.ExecuteNonQuery(cmd);

            mDatabase.Close();
         
            return rtn;
        }

        public int CreateTicket(int teamMatchID, String firstName, String lastName, String email, SqlMoney price)
        {
            mDatabase.Connect();

            SqlCommand cmd = mDatabase.CreateCommand("createTicket");
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

            mDatabase.ExecuteNonQuery(cmd);
            mDatabase.Close();

            return 0;
        }

        /**
         * ti4 - Select all tickets for one match - index
         */
        public List<Ticket> GetTicketsForMatch(int teamMatchID)
        {
            mDatabase.Connect();

            SqlCommand command = mDatabase.CreateCommand(SQL_SELECT_ALL_MATCH);

            //Vytvoreni parametru
            SqlParameter input = new SqlParameter();
            input.ParameterName = "@teamMatchID";
            input.DbType = DbType.Int32;
            input.Value = teamMatchID;
            input.Direction = ParameterDirection.Input;
            command.Parameters.Add(input);

            SqlDataReader reader = command.ExecuteReader();
            List<Ticket> tickets = Read(reader);

            mDatabase.Close();
            reader.Close();

            return tickets;
        }
    }
}
