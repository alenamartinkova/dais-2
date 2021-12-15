using System;
namespace cfbu.Database
{
    public class Statistics
    {
        public int statisticsID { get; set; }
        public int playerID { get; set; }
        public int teamID { get; set; }
        public int assists { get; set; }
        public int goals { get; set; }
        public int points { get; set; }

        public Statistics()
        {

        }
    }
}
