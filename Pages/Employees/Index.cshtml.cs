using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PayrollApp.Data;
using PayrollApp.Models;

namespace PayrollApp.Pages.Employees;

public class IndexModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public IndexModel(ApplicationDbContext context)
    {
        _context = context;
    }

    public List<Employee> Employees { get; set; } = new();

    public async Task OnGetAsync()
    {
        Employees = await _context.Employees
            .OrderBy(e => e.EmployeeId)
            .ToListAsync();
    }
}
