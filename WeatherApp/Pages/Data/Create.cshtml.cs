using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using WeatherApp.Data;
using WeatherApp.Models;

namespace WeatherApp.Pages.Data
{
    public class CreateModel : PageModel
    {
        private readonly WeatherApp.Data.WeatherAppContext _context;

        public CreateModel(WeatherApp.Data.WeatherAppContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public DataPoint DataPoint { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.DataPoint.Add(DataPoint);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }

        // Adds new sensor data to the model
        public async Task<IActionResult> OnPostAddSensorDataAsync([FromBody] DataPoint sensorData)
        {
            try
            {
                // Validate and add the sensor data to the database
                if (sensorData != null)
                {
                    _context.DataPoint.Add(sensorData);
                    await _context.SaveChangesAsync();
                    return new JsonResult(new { success = true });
                }

                return new JsonResult(new { success = false, message = "Invalid sensor data." });
            }
            catch (Exception error)
            {
                // Log the error and return a failure response
                Console.Error.WriteLine($"Error adding sensor data: {error.Message}");
                return new JsonResult(new { success = false, message = "An error occurred while processing the sensor data." });
            }
        }
    }
}
