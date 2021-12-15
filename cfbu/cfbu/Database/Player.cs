using System;
namespace cfbu.Database
{
    public class Player
    {
        public int playerID { get; set; }
        public int teamID { get; set; }
        public string email { get; set; }
        public DateTime dateOfBirth { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public char? stick { get; set; }
        public int covid { get; set; }

        public Player()
        {
        }
    }
}
