using System.Text.Json.Serialization;

namespace CarUsed.Models
{
    public class Car
    {
        public int Id { get; set; }
        public string ShowImagePath { get; set; } 
        public List<CarImages> Images { get; set; } = new List<CarImages>();
        public int Price { get; set; }
        public int Down { get; set; }
        public string Bran { get; set; }
        public string Series { get; set; }
        public string Type { get; set; } //should be int
        public int Year { get; set; }
        public string Color { get; set; }
        public string Energy { get; set; }  //should be int
        public string Gear { get; set; }  //should be int
        public string Detail { get; set; }
        [JsonIgnore]
        public Guid UserId { get; set; }
        [JsonIgnore]
        public User User { get; set; }

    }
}
