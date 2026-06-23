using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PayrollApp.Data;
using PayrollApp.Models;

namespace PayrollApp.Pages.Employees;

public class DetailsModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public DetailsModel(ApplicationDbContext context)
    {
        _context = context;
    }

    public Employee Employee { get; set; } = default!;
    public List<PayrollRecord> PayrollRecords { get; set; } = new();

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var employee = await _context.Employees.FirstOrDefaultAsync(m => m.Id == id);
        if (employee == null)
        {
            return NotFound();
        }

        Employee = employee;
        
        // Load recent payroll records
        PayrollRecords = await _context.PayrollRecords
            .Where(p => p.EmployeeId == id)
            .OrderByDescending(p => p.PaymentDate)
            .Take(10)
            .ToListAsync();

        return Page();
    }
}
