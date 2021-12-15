using System;
namespace BenchmarkApp.Database.Entities
{
    public class PlayerTransferHistory
    {
        public int playerTransferID { get; set; }
        public int playerID { get; set; }
        public int oldTeamID { get; set; }
        public int newTeamID { get; set; }
        public DateTime date { get; set; }

        public PlayerTransferHistory()
        {
        }
    }
}
