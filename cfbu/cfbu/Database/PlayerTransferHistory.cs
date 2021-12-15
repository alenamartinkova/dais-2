using System;

namespace cfbu.Database
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
