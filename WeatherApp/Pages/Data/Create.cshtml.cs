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
    }
}
