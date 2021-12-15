using System;
namespace BenchmarkApp.Database.Entities
{
    public class Statistic
    {
        public int statisticsID { get; set; }
        public int playerID { get; set; }
        public int teamID { get; set; }
        public int assists { get; set; }
        public int goals { get; set; }
        public int points { get; set; }

        public Statistic()
        {
        }
    }
}
