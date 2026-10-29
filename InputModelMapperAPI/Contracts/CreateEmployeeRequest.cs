using System.ComponentModel.DataAnnotations;

namespace InputModelMapperLab.Api.Contracts;

public sealed class CreateEmployeeRequest
{
    [Required]
    [StringLength(30)]
    public string EmployeeNumber { get; init; } = string.Empty;

    [Required]
    [StringLength(80)]
    public string FirstName { get; init; } = string.Empty;

    [Required]
    [StringLength(80)]
    public string LastName { get; init; } = string.Empty;

    [Required]
    [EmailAddress]
    [StringLength(200)]
    public string Email { get; init; } = string.Empty;

    public DateOnly HireDate { get; init; }

    [Range(typeof(decimal), "0", "999999999")]
    public decimal Salary { get; init; }

    [Range(1, int.MaxValue)]
    public int DepartmentId { get; init; }
}
