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
                    Location = "Sammamish",
                    Date = DateTime.Today,
                    Time = DateTime.Now,
                    Temperature = 58,
                    Humidity = 74
                },

                new DataPoint
                {
                    Location = "Issaquah",
                    Date = DateTime.Today,
                    Time = DateTime.Now,
                    Temperature = 56,
                    Humidity = 74
                },

                new DataPoint
                {
                    Location = "Redmond",
                    Date = DateTime.Today,
                    Time = DateTime.Now,
                    Temperature = 51,
                    Humidity = 74
                },

                new DataPoint
                {
                    Location = "Seattle",
                    Date = DateTime.Today,
                    Time = DateTime.Now,
                    Temperature = 61,
                    Humidity = 83
                }
            );
            context.SaveChanges();
        }
    }
}
