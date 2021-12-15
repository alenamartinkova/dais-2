using System;
using System.Data.SqlTypes;

namespace BenchmarkApp.Database.Entities
{
    public class Ticket
    {
        public int ticketID { get; set; }
        public int teamMatchID { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string email { get; set; }
        public SqlMoney price { get; set; }
        public int storno { get; set; }

        public Ticket()
        {
        }
    }
}
