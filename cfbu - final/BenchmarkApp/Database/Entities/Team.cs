using System;
using Bogus;
namespace BenchmarkApp.Database.Entities

{
    public class Team
    {
        public int teamID { get; set; }
        public int leagueID { get; set; }
        public string name { get; set; }
        public int rank { get; set; }
        public int covid { get; set; }
        public DateTime quarantinedFrom { get; set; }

        public Team()
        {
        }
    }
}
