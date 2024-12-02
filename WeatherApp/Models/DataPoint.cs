using System.ComponentModel.DataAnnotations;

namespace WeatherApp.Models
{
    public class DataPoint
    {
        public int Id { get; set; }  // Required by database
        public string? Location { get; set; }
        public DateTime DateTime { get; set; }
        public int Temperature { get; set; }
        public int Humidity { get; set; }
    }
}
