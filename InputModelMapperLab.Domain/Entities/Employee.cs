namespace InputModelMapperLab.Domain.Entities;

public sealed class Employee
{
    public int EmployeeId { get; set; }
    public string EmployeeNumber { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public DateOnly HireDate { get; set; }
    public decimal Salary { get; set; }
    public string EmploymentStatus { get; set; } = "Active";
    public int DepartmentId { get; set; }
    public DateTime CreatedAtUtc { get; set; }

    public Department? Department { get; set; }
}
