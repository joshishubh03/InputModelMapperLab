namespace InputModelMapperLab.Application.Employees;

public sealed record CreateEmployeeCommand(
    string EmployeeNumber,
    string FirstName,
    string LastName,
    string Email,
    DateOnly HireDate,
    decimal Salary,
    int DepartmentId);
