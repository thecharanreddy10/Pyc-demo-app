using PayrollApp.Models;

namespace PayrollApp.Services;

public class PayrollCalculationService
{
    // Tax rate: 15%
    private const decimal TaxRate = 0.15m;
    
    // Social Security rate: 6.2%
    private const decimal SocialSecurityRate = 0.062m;
    
    // Health Insurance fixed amount
    private const decimal HealthInsuranceAmount = 200m;

    /// <summary>
    /// Calculate gross pay for an employee
    /// </summary>
    public decimal CalculateGrossPay(Employee employee, decimal? hoursWorked = null)
    {
        if (employee.EmploymentType == EmploymentType.Salaried)
        {
            // Monthly salary (annual salary / 12)
            return employee.AnnualSalary.GetValueOrDefault() / 12m;
        }
        else
        {
            // Hourly: rate × hours worked
            var hours = hoursWorked ?? 160m; // Default to 160 hours (4 weeks × 40 hours)
            return employee.HourlyRate.GetValueOrDefault() * hours;
        }
    }

    /// <summary>
    /// Calculate tax deduction (15% of gross pay)
    /// </summary>
    public decimal CalculateTax(decimal grossPay)
    {
        return Math.Round(grossPay * TaxRate, 2);
    }

    /// <summary>
    /// Calculate Social Security deduction (6.2% of gross pay)
    /// </summary>
    public decimal CalculateSocialSecurity(decimal grossPay)
    {
        return Math.Round(grossPay * SocialSecurityRate, 2);
    }

    /// <summary>
    /// Get Health Insurance deduction (fixed amount)
    /// </summary>
    public decimal GetHealthInsuranceDeduction()
    {
        return HealthInsuranceAmount;
    }

    /// <summary>
    /// Calculate total deductions
    /// </summary>
    public decimal CalculateTotalDeductions(decimal tax, decimal socialSecurity, decimal healthInsurance)
    {
        return tax + socialSecurity + healthInsurance;
    }

    /// <summary>
    /// Calculate net pay (gross pay - total deductions)
    /// </summary>
    public decimal CalculateNetPay(decimal grossPay, decimal totalDeductions)
    {
        return Math.Round(grossPay - totalDeductions, 2);
    }

    /// <summary>
    /// Process complete payroll calculation for an employee
    /// </summary>
    public PayrollRecord ProcessPayroll(Employee employee, DateTime payPeriodStart, DateTime payPeriodEnd, decimal? hoursWorked = null)
    {
        var grossPay = CalculateGrossPay(employee, hoursWorked);
        var tax = CalculateTax(grossPay);
        var socialSecurity = CalculateSocialSecurity(grossPay);
        var healthInsurance = GetHealthInsuranceDeduction();
        var totalDeductions = CalculateTotalDeductions(tax, socialSecurity, healthInsurance);
        var netPay = CalculateNetPay(grossPay, totalDeductions);

        return new PayrollRecord
        {
            EmployeeId = employee.Id,
            PayPeriodStart = payPeriodStart,
            PayPeriodEnd = payPeriodEnd,
            HoursWorked = employee.EmploymentType == EmploymentType.Hourly ? hoursWorked : null,
            GrossPay = grossPay,
            TaxAmount = tax,
            SocialSecurityAmount = socialSecurity,
            HealthInsuranceAmount = healthInsurance,
            TotalDeductions = totalDeductions,
            NetPay = netPay,
            PaymentDate = DateTime.Today
        };
    }
}
