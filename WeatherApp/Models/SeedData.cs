using Microsoft.EntityFrameworkCore;
using WeatherApp.Data;

namespace WeatherApp.Models;

public static class SeedData
{
    public static void Initialize(IServiceProvider serviceProvider)
    {
        using (var context = new WeatherAppContext(
            serviceProvider.GetRequiredService<
                DbContextOptions<WeatherAppContext>>()))
        {
            if (context == null || context.DataPoint == null)
            {
                throw new ArgumentNullException("Null WeatherAppContext");
            }

            // Look for data points
            if (context.DataPoint.Any())
            {
                return;   // DB has been seeded
            }

            context.DataPoint.AddRange(
                new DataPoint
                {
                    Location = "Office",
                    DateTime = DateTime.Now,
                    Temperature = 68,
                    Humidity = 46
                },

                new DataPoint
                {
                    Location = "Office",
                    DateTime = DateTime.Now,
                    Temperature = 69,
                    Humidity = 46
                },

                new DataPoint
                {
                    Location = "Office",
                    DateTime = DateTime.Now,
                    Temperature = 68,
                    Humidity = 45
                },

                new DataPoint
                {
                    Location = "Office",
                    DateTime = DateTime.Now,
                    Temperature = 69,
                    Humidity = 45
                }
            );
            context.SaveChanges();
        }
    }
}
