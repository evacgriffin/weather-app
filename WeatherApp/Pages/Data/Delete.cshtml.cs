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
    public class DeleteModel : PageModel
    {
        private readonly WeatherApp.Data.WeatherAppContext _context;

        public DeleteModel(WeatherApp.Data.WeatherAppContext context)
        {
            _context = context;
        }

        [BindProperty]
        public DataPoint DataPoint { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var datapoint = await _context.DataPoint.FirstOrDefaultAsync(m => m.Id == id);

            if (datapoint == null)
            {
                return NotFound();
            }
            else
            {
                DataPoint = datapoint;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var datapoint = await _context.DataPoint.FindAsync(id);
            if (datapoint != null)
            {
                DataPoint = datapoint;
                _context.DataPoint.Remove(DataPoint);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
