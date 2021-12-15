using System;
namespace cfbu.Database
{
    public class TeamMatch
    {
        public int teamMatchID { get; set; }
        public int firstTeamID { get; set; }
        public int secondTeamID { get; set; }
        public int pitchID { get; set; }
        public int firstTeamGoals { get; set; }
        public int secondTeamGoals { get; set; }
        public int postponed { get; set; }
        public DateTime date { get; set; }


        public TeamMatch()
        {
        }
    }
}
