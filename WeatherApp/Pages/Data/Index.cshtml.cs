using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WeatherApp.Data;
using WeatherApp.Models;
using WeatherApp.Services;
using Microsoft.AspNetCore.Antiforgery;

namespace WeatherApp.Pages.Data
{
    public class IndexModel : PageModel
    {
        private readonly WeatherApp.Data.WeatherAppContext _context;
        private readonly IAntiforgery _antiforgery;
        private readonly ZeroMqClient _zeroMqImageClient;
        private readonly ZeroMqClient _zeroMqNotificationClient;
        private readonly ZeroMqClient _zeroMqGraphClient;
        private readonly ZeroMqClient _zeroMqSensorClient;

        public string AntiForgeryToken { get; private set; }

        public IndexModel(WeatherApp.Data.WeatherAppContext context, IAntiforgery antiforgery)
        {
            _context = context;
            _antiforgery = antiforgery;

            // Initialize the ZMQ clients
            _zeroMqImageClient          = new ZeroMqClient("tcp://localhost:5555");  // Image Generator Service
            _zeroMqNotificationClient   = new ZeroMqClient("tcp://localhost:5566");  // Notification Service
            _zeroMqGraphClient          = new ZeroMqClient("tcp://localhost:5577");  // Graph Service
            _zeroMqSensorClient         = new ZeroMqClient("tcp://evapi:5588");  // Weather Sensor Service
        }

        public string ImageUrl { get; set; }
        public string SensorData { get; set; }
        public string NotificationString { get; set; }
        public string GraphConfirmation { get; set; }

        public async Task<IActionResult> OnPostFetchSensorData()
        {
            // Send message to the microservice B sensor server - sending a 1 bit will request new sensor data
            bool requestSensorData = true;
            string jsonResponse = _zeroMqSensorClient.SendMessage(requestSensorData.ToString());
            Console.WriteLine("Received sensor data: " + jsonResponse);

            if (string.IsNullOrWhiteSpace(jsonResponse))
            {
                return Content("Failed to fetch sensor data from the server.");
            }

            try
            {
                // Deserialize the JSON response into a SensorData object
                var sensorData = JsonSerializer.Deserialize<SensorData>(jsonResponse);

                if (sensorData != null)
                {
                    // Map sensor data to the DataPoint model
                    var dataPoint = new DataPoint
                    {
                        Location = sensorData.Location,
                        DateTime = DateTime.Parse(sensorData.DateTime),
                        Temperature = sensorData.Temperature,
                        Humidity = sensorData.Humidity
                    };

                    // Save to the database
                    _context.DataPoint.Add(dataPoint);
                    await _context.SaveChangesAsync();

                    // Get the new temperature that was just returned from the sensor
                    string sensorTemp = sensorData.Temperature.ToString();
                    // Send the temperature to the image microservice to request a matching image for the new temp
                    StaticState.weatherImageUrl = _zeroMqImageClient.SendMessage(sensorTemp);
                    StaticState.weatherNotificationString = _zeroMqNotificationClient.SendMessage(sensorTemp);

                    return new JsonResult(new { success = true, imageUrl = StaticState.weatherImageUrl, notificationString = StaticState.weatherNotificationString });
                }
                else
                {
                    return new JsonResult(new { success = false, message = "Invalid sensor data." });
                }
            }
            catch (Exception ex)
            {
                // Handle deserialization or database errors
                Console.WriteLine($"Error: {ex.Message}");
                return Content("An error occurred while processing the sensor data.");
            }
        }

        public ContentResult OnPostGenerateGraph()
        {
            // Get data points from the database
            var dataPoints = _context.DataPoint.ToList();

            if (dataPoints == null || !dataPoints.Any())
            {
                return Content("No data points available.");
            }

            var jsonData = dataPoints.Select(dp => new
            {
                date = dp.DateTime.ToString("MM/dd/yyyy"),
                temp = dp.Temperature
            }).ToList();

            // Serialize to JSON
            string serializedJson = JsonSerializer.Serialize(jsonData);

            // Send message to the graph microservice
            string response = _zeroMqGraphClient.SendMessage(serializedJson);

            // Return server response
            return Content(response);
        }

        public IList<DataPoint> DataPoint { get;set; } = default!;

        [BindProperty(SupportsGet = true)]
        public string? SearchString { get; set; }

        public SelectList? Locations { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? DataLocation { get; set; }

        public async Task OnGetAsync()
        {
            AntiForgeryToken = _antiforgery.GetAndStoreTokens(HttpContext).RequestToken;

            IQueryable<string> locationQuery = from d in _context.DataPoint
                                            orderby d.Location
                                            select d.Location;

            var datapoints = from d in _context.DataPoint
                         select d;

            if (!string.IsNullOrEmpty(SearchString))
            {
                datapoints = datapoints.Where(s => s.Location.Contains(SearchString));
            }

            if (!string.IsNullOrEmpty(DataLocation))
            {
                datapoints = datapoints.Where(x => x.Location == DataLocation);
            }
            Locations = new SelectList(await locationQuery.Distinct().ToListAsync());
            DataPoint = await datapoints.ToListAsync();
        }
    }
}
