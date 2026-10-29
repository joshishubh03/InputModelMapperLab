namespace InputModelMapperLab.Application.Employees;

public interface IEmployeeCreator
{
    Task<int> CreateAsync(
        CreateEmployeeCommand command,
        CancellationToken cancellationToken = default);
}
