using System.ComponentModel.DataAnnotations;

namespace WeatherApp.Models
{
    public class DataPoint
    {
        public int Id { get; set; }  // Required by database
        public string? Location { get; set; }
        [DataType(DataType.Date)]  // Date property displays only date, not time
        public DateTime Date { get; set; }
        [DataType(DataType.Time)]  // Time property displays only time, not the date
        public DateTime Time { get; set; }
        public decimal Temperature { get; set; }
        public decimal Humidity { get; set; }
    }
}
