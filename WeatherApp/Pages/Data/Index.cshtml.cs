using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
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
        private readonly ZeroMqClient _zeroMqClient;

        public string AntiForgeryToken { get; private set; }

        public IndexModel(WeatherApp.Data.WeatherAppContext context, IAntiforgery antiforgery, ZeroMqClient zeroMqClient)
        {
            _context = context;
            _antiforgery = antiforgery;
            _zeroMqClient = zeroMqClient;
        }

        public string ImageUrl { get; set; }

        public ContentResult OnPostFetchImage()
        {
            // Send message to the microservice A server
            string message = "50";
            string imageUrl = _zeroMqClient.SendMessage(message);

            return Content(imageUrl);
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
