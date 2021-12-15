using System;
using Bogus;
namespace BenchmarkApp.Database.Entities
{
    public class Pitch
    {
        public int pitchID { get; set; }
        public int capacity { get; set; }
        public string name { get; set; }

        public Pitch()
        {
        }

        public static Pitch FakeData()
        {
            var faker = new Faker("cz");
            var size = 100;

            return new Pitch()
            {
                capacity = faker.Random.Int(50, 2000),
                name = faker.Random.Chars(count: size).ToString()
            };
        }
    }
}
