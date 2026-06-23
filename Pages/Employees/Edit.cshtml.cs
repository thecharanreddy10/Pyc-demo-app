using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PayrollApp.Data;
using PayrollApp.Models;

namespace PayrollApp.Pages.Employees;

public class EditModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public EditModel(ApplicationDbContext context)
    {
        _context = context;
    }

    [BindProperty]
    public Employee Employee { get; set; } = default!;

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
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        // Validate employment type specific fields
        if (Employee.EmploymentType == EmploymentType.Salaried && !Employee.AnnualSalary.HasValue)
        {
            ModelState.AddModelError("Employee.AnnualSalary", "Annual Salary is required for salaried employees.");
            return Page();
        }

        if (Employee.EmploymentType == EmploymentType.Hourly && !Employee.HourlyRate.HasValue)
        {
            ModelState.AddModelError("Employee.HourlyRate", "Hourly Rate is required for hourly employees.");
            return Page();
        }

        _context.Attach(Employee).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!EmployeeExists(Employee.Id))
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

    private bool EmployeeExists(int id)
    {
        return _context.Employees.Any(e => e.Id == id);
    }
}
