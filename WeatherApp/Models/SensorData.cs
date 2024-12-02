namespace WeatherApp.Models
{
    public class SensorData
    {
        public string? Location { get; set; }
        public string? DateTime { get; set; }
        public int Temperature { get; set; }
        public int Humidity { get; set; }
    }
}
