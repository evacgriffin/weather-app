using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
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

        [BindProperty(SupportsGet = true)]
        public string? SearchString { get; set; }

        public SelectList? Locations { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? DataLocation { get; set; }

        public async Task OnGetAsync()
        {
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
