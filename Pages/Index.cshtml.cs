using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MyApp.Models;
using MyApp.Data;

namespace MyApp.Pages
{
    public class IndexModel : PageModel
    {
        private readonly SchoolDbContext _context;

        public IndexModel(SchoolDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Class NewClass { get; set; } = new()
        {
            Name = string.Empty,
            Description = string.Empty,
            IsActive = true // ✅ BURASI OLMALI
        };

        [BindProperty]
        public bool IsEditMode { get; set; } = false;

        [BindProperty(SupportsGet = true)]
        public string? ClassNameFilter { get; set; }

        [BindProperty(SupportsGet = true)]
        public int CurrentPage { get; set; } = 1;

        public int TotalPages { get; set; }
        public List<Class> FilteredData { get; set; } = new();

        public async Task OnGetAsync()
        {
            var data = await _context.Classes
                .Where(x => x.IsActive == true)
                .ToListAsync();

            if (!string.IsNullOrEmpty(ClassNameFilter))
            {
                data = data
                    .Where(x => x.Name != null && x.Name.Contains(ClassNameFilter!, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            int pageSize = 10;
            TotalPages = (int)Math.Ceiling((double)data.Count / pageSize);

            FilteredData = data
                .Skip((CurrentPage - 1) * pageSize)
                .Take(pageSize)
                .ToList();
        }

        public async Task<IActionResult> OnPostAddAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            NewClass.IsActive = true; // ✅ burada elle set ediyorsun
            _context.Classes.Add(NewClass);
            await _context.SaveChangesAsync();
            return RedirectToPage(new { ClassNameFilter = "", CurrentPage = 1 });
        }


        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var classToRemove = await _context.Classes.FindAsync(id);
            if (classToRemove != null)
            {
                classToRemove.IsActive = false;
                await _context.SaveChangesAsync();
            }
            return RedirectToPage(new { ClassNameFilter = ClassNameFilter, CurrentPage = CurrentPage });
        }

        public async Task<IActionResult> OnPostEditAsync()
        {
            var classToEdit = await _context.Classes.FindAsync(NewClass.Id);
            if (classToEdit != null)
            {
                classToEdit.Name = NewClass.Name ?? string.Empty;
                classToEdit.PersonCount = NewClass.PersonCount;
                classToEdit.Description = NewClass.Description ?? string.Empty;
                classToEdit.IsActive = NewClass.IsActive;

                await _context.SaveChangesAsync();
            }
            IsEditMode = false;
            return RedirectToPage(new { ClassNameFilter = ClassNameFilter, CurrentPage = CurrentPage });
        }

        public async Task<IActionResult> OnPostEditSelectAsync(int id)
        {
            var classToEdit = await _context.Classes.FindAsync(id);
            if (classToEdit != null)
            {
                NewClass = new Class
                {
                    Id = classToEdit.Id,
                    Name = classToEdit.Name ?? string.Empty,
                    PersonCount = classToEdit.PersonCount,
                    Description = classToEdit.Description ?? string.Empty,
                    IsActive = classToEdit.IsActive
                };
                IsEditMode = true;
            }
            return Page();
        }
    }
}
