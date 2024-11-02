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
    public class EditModel : PageModel
    {
        private readonly WeatherApp.Data.WeatherAppContext _context;

        public EditModel(WeatherApp.Data.WeatherAppContext context)
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

            var datapoint =  await _context.DataPoint.FirstOrDefaultAsync(m => m.Id == id);
            if (datapoint == null)
            {
                return NotFound();
            }
            DataPoint = datapoint;
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(DataPoint).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DataPointExists(DataPoint.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool DataPointExists(int id)
        {
            return _context.DataPoint.Any(e => e.Id == id);
        }
    }
}
