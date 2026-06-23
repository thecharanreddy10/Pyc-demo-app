using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PayrollApp.Data;
using PayrollApp.Models;

namespace PayrollApp.Pages.Payroll;

public class HistoryModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public HistoryModel(ApplicationDbContext context)
    {
        _context = context;
    }

    public List<PayrollRecord> PayrollRecords { get; set; } = new();
    public List<Employee> Employees { get; set; } = new();
    
    public int? FilterEmployeeId { get; set; }
    public DateTime? FilterStartDate { get; set; }
    public DateTime? FilterEndDate { get; set; }

    public async Task OnGetAsync(int? employeeId, DateTime? startDate, DateTime? endDate)
    {
        FilterEmployeeId = employeeId;
        FilterStartDate = startDate;
        FilterEndDate = endDate;

        // Load all employees for filter dropdown
        Employees = await _context.Employees
            .OrderBy(e => e.EmployeeId)
            .ToListAsync();

        // Build query
        var query = _context.PayrollRecords
            .Include(p => p.Employee)
            .AsQueryable();

        // Apply filters
        if (employeeId.HasValue)
        {
            query = query.Where(p => p.EmployeeId == employeeId.Value);
        }

        if (startDate.HasValue)
        {
            query = query.Where(p => p.PaymentDate >= startDate.Value);
        }

        if (endDate.HasValue)
        {
            query = query.Where(p => p.PaymentDate <= endDate.Value);
        }

        // Get results
        PayrollRecords = await query
            .OrderByDescending(p => p.PaymentDate)
            .ThenBy(p => p.Employee!.Name)
            .ToListAsync();
    }
}
