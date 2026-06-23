using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PayrollApp.Models;

public class PayrollRecord
{
    public int Id { get; set; }

    [Required]
    [Display(Name = "Employee")]
    public int EmployeeId { get; set; }

    [Display(Name = "Pay Period Start")]
    [DataType(DataType.Date)]
    public DateTime PayPeriodStart { get; set; }

    [Display(Name = "Pay Period End")]
    [DataType(DataType.Date)]
    public DateTime PayPeriodEnd { get; set; }

    [Display(Name = "Hours Worked")]
    [Range(0, 744, ErrorMessage = "Hours worked must be between 0 and 744 (31 days × 24 hours)")]
    public decimal? HoursWorked { get; set; }

    [Display(Name = "Gross Pay")]
    [Column(TypeName = "decimal(18,2)")]
    public decimal GrossPay { get; set; }

    [Display(Name = "Tax Amount")]
    [Column(TypeName = "decimal(18,2)")]
    public decimal TaxAmount { get; set; }

    [Display(Name = "Social Security")]
    [Column(TypeName = "decimal(18,2)")]
    public decimal SocialSecurityAmount { get; set; }

    [Display(Name = "Health Insurance")]
    [Column(TypeName = "decimal(18,2)")]
    public decimal HealthInsuranceAmount { get; set; }

    [Display(Name = "Total Deductions")]
    [Column(TypeName = "decimal(18,2)")]
    public decimal TotalDeductions { get; set; }

    [Display(Name = "Net Pay")]
    [Column(TypeName = "decimal(18,2)")]
    public decimal NetPay { get; set; }

    [Display(Name = "Payment Date")]
    [DataType(DataType.Date)]
    public DateTime PaymentDate { get; set; } = DateTime.Today;

    // Navigation property
    public Employee? Employee { get; set; }
}
