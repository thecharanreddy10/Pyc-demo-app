using System.ComponentModel.DataAnnotations;

namespace PayrollApp.Models;

public class Employee
{
    public int Id { get; set; }

    [Required]
    [Display(Name = "Employee ID")]
    [StringLength(50)]
    public string EmployeeId { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [StringLength(100)]
    public string Email { get; set; } = string.Empty;

    [StringLength(100)]
    public string? Department { get; set; }

    [Required]
    [Display(Name = "Employment Type")]
    public EmploymentType EmploymentType { get; set; }

    [Display(Name = "Annual Salary")]
    [Range(0, double.MaxValue, ErrorMessage = "Salary must be a positive number")]
    public decimal? AnnualSalary { get; set; }

    [Display(Name = "Hourly Rate")]
    [Range(0, double.MaxValue, ErrorMessage = "Hourly rate must be a positive number")]
    public decimal? HourlyRate { get; set; }

    [Required]
    [Display(Name = "Hire Date")]
    [DataType(DataType.Date)]
    public DateTime HireDate { get; set; } = DateTime.Today;

    // Navigation property
    public ICollection<PayrollRecord> PayrollRecords { get; set; } = new List<PayrollRecord>();
}
