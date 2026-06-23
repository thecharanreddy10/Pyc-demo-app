using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PayrollApp.Data;
using PayrollApp.Models;

namespace PayrollApp.Pages.Employees;

public class CreateModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public CreateModel(ApplicationDbContext context)
    {
        _context = context;
    }

    [BindProperty]
    public Employee Employee { get; set; } = new();

    public IActionResult OnGet()
    {
        Employee.HireDate = DateTime.Today;
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

        _context.Employees.Add(Employee);
        await _context.SaveChangesAsync();

        return RedirectToPage("./Index");
    }
}
