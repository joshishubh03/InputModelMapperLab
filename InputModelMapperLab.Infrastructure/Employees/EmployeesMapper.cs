using InputModelMapperLab.Application.Employees;
using InputModelMapperLab.Domain.Entities;

namespace InputModelMapperLab.Infrastructure.Employees;

internal static class EmployeeMapper
{
    public static Employee ToEntity(
        CreateEmployeeCommand command,
        DateTime createdAtUtc)
    {
        return new Employee
        {
            EmployeeNumber = command.EmployeeNumber.Trim(),
            FirstName = command.FirstName.Trim(),
            LastName = command.LastName.Trim(),
            Email = command.Email.Trim(),
            HireDate = command.HireDate,
            Salary = command.Salary,
            DepartmentId = command.DepartmentId,
            EmploymentStatus = "Active",
            CreatedAtUtc = createdAtUtc
        };
    }
}
