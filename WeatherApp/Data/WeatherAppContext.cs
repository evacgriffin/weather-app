using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WeatherApp.Models;

namespace WeatherApp.Data
{
    public class WeatherAppContext : DbContext
    {
        public WeatherAppContext (DbContextOptions<WeatherAppContext> options)
            : base(options)
        {
        }

        public DbSet<WeatherApp.Models.DataPoint> DataPoint { get; set; } = default!;
    }
}
