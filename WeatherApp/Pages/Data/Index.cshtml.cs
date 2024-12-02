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

        public ContentResult OnPostFetchImage()
        {
            // Send message to the microservice A image server
            string message = "90";
            string imageUrl = _zeroMqImageClient.SendMessage(message);

            return Content(imageUrl);
        }

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

                    return Content("Sensor data saved successfully.");
                }
                else
                {
                    return Content("Failed to parse sensor data.");
                }
            }
            catch (Exception ex)
            {
                // Handle deserialization or database errors
                Console.WriteLine($"Error: {ex.Message}");
                return Content("An error occurred while processing the sensor data.");
            }
        }

        public ContentResult OnPostFetchNotification()
        {
            // Send message to the microservice C server
            string message = "90";
            string imageUrl = _zeroMqNotificationClient.SendMessage(message);

            return Content(imageUrl);
        }

        public ContentResult OnPostGenerateGraph()
        {
            // Some test data
            var testData = new List<object>
            {
                new { date = "11/22/2024", temp = 40 },
                new { date = "11/23/2024", temp = 40 },
                new { date = "11/24/2024", temp = 41 },
                new { date = "11/25/2024", temp = 42 },
                new { date = "11/26/2024", temp = 40 }
            };

            // Initialize a list to store server responses
            var responses = new List<string>();

            foreach (var dataPoint in testData)
            {
                // Serialize to JSON
                string jsonData = JsonSerializer.Serialize(dataPoint);

                // Send message to the microservice D server
                string response = _zeroMqGraphClient.SendMessage(jsonData);

                // Store the server response
                responses.Add(response);
            }

            // Return the combined responses
            string combinedResponse = string.Join(", ", responses);
            return Content(combinedResponse);
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
