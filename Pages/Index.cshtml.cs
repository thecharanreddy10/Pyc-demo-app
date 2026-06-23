using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PayrollApp.Data;
using PayrollApp.Models;

namespace PayrollApp.Pages;

public class IndexModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public IndexModel(ApplicationDbContext context)
    {
        _context = context;
    }

    public int TotalEmployees { get; set; }
    public int SalariedEmployees { get; set; }
    public int HourlyEmployees { get; set; }
    public int TotalPayrollRecords { get; set; }
    
    public decimal CurrentMonthGrossPay { get; set; }
    public decimal CurrentMonthDeductions { get; set; }
    public decimal CurrentMonthNetPay { get; set; }
    public int CurrentMonthPayrollCount { get; set; }

    public List<PayrollRecord> RecentPayrollRecords { get; set; } = new();

    public async Task OnGetAsync()
    {
        // Employee statistics
        TotalEmployees = await _context.Employees.CountAsync();
        SalariedEmployees = await _context.Employees.CountAsync(e => e.EmploymentType == EmploymentType.Salaried);
        HourlyEmployees = await _context.Employees.CountAsync(e => e.EmploymentType == EmploymentType.Hourly);
        
        // Payroll statistics
        TotalPayrollRecords = await _context.PayrollRecords.CountAsync();

        // Current month statistics
        var firstDayOfMonth = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
        var currentMonthRecords = await _context.PayrollRecords
            .Where(p => p.PaymentDate >= firstDayOfMonth)
            .ToListAsync();

        CurrentMonthGrossPay = currentMonthRecords.Sum(p => p.GrossPay);
        CurrentMonthDeductions = currentMonthRecords.Sum(p => p.TotalDeductions);
        CurrentMonthNetPay = currentMonthRecords.Sum(p => p.NetPay);
        CurrentMonthPayrollCount = currentMonthRecords.Count;

        // Recent payroll records
        RecentPayrollRecords = await _context.PayrollRecords
            .Include(p => p.Employee)
            .OrderByDescending(p => p.PaymentDate)
            .Take(10)
            .ToListAsync();
    }
}
