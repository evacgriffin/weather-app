using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WeatherApp.Data;
using WeatherApp.Models;

namespace WeatherApp.Pages.Data
{
    public class IndexModel : PageModel
    {
        private readonly WeatherApp.Data.WeatherAppContext _context;

        public IndexModel(WeatherApp.Data.WeatherAppContext context)
        {
            _context = context;
        }

        public IList<DataPoint> DataPoint { get;set; } = default!;

        public async Task OnGetAsync()
        {
            DataPoint = await _context.DataPoint.ToListAsync();
        }
    }
}
