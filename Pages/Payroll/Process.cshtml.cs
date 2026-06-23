using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PayrollApp.Data;
using PayrollApp.Models;
using PayrollApp.Services;

namespace PayrollApp.Pages.Payroll;

public class ProcessModel : PageModel
{
    private readonly ApplicationDbContext _context;
    private readonly PayrollCalculationService _payrollService;

    public ProcessModel(ApplicationDbContext context, PayrollCalculationService payrollService)
    {
        _context = context;
        _payrollService = payrollService;
    }

    public List<Employee> Employees { get; set; } = new();
    public PayrollRecord? ProcessedPayroll { get; set; }

    [BindProperty]
    public int SelectedEmployeeId { get; set; }

    [BindProperty]
    public DateTime PayPeriodStart { get; set; }

    [BindProperty]
    public DateTime PayPeriodEnd { get; set; }

    [BindProperty]
    public decimal? HoursWorked { get; set; }

    public async Task OnGetAsync()
    {
        Employees = await _context.Employees
            .OrderBy(e => e.EmployeeId)
            .ToListAsync();

        // Set default pay period (current month)
        PayPeriodStart = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
        PayPeriodEnd = PayPeriodStart.AddMonths(1).AddDays(-1);
    }

    public async Task<IActionResult> OnPostAsync()
    {
        Employees = await _context.Employees
            .OrderBy(e => e.EmployeeId)
            .ToListAsync();

        if (SelectedEmployeeId == 0)
        {
            ModelState.AddModelError("SelectedEmployeeId", "Please select an employee.");
        }

        if (PayPeriodEnd <= PayPeriodStart)
        {
            ModelState.AddModelError("PayPeriodEnd", "Pay period end must be after start date.");
        }

        if (!ModelState.IsValid)
        {
            return Page();
        }

        var employee = await _context.Employees.FindAsync(SelectedEmployeeId);
        if (employee == null)
        {
            ModelState.AddModelError("SelectedEmployeeId", "Employee not found.");
            return Page();
        }

        // Validate hours worked for hourly employees
        if (employee.EmploymentType == EmploymentType.Hourly && !HoursWorked.HasValue)
        {
            ModelState.AddModelError("HoursWorked", "Hours worked is required for hourly employees.");
            return Page();
        }

        // Process payroll
        var payrollRecord = _payrollService.ProcessPayroll(employee, PayPeriodStart, PayPeriodEnd, HoursWorked);
        
        // Save to database
        _context.PayrollRecords.Add(payrollRecord);
        await _context.SaveChangesAsync();

        // Load employee info for display
        ProcessedPayroll = await _context.PayrollRecords
            .Include(p => p.Employee)
            .FirstOrDefaultAsync(p => p.Id == payrollRecord.Id);

        TempData["SuccessMessage"] = $"Payroll processed successfully for {employee.Name}. Net Pay: ${payrollRecord.NetPay:N2}";

        return Page();
    }
}
