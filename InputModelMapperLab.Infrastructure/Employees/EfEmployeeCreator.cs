using InputModelMapperLab.Application.Employees;
using InputModelMapperLab.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace InputModelMapperLab.Infrastructure.Employees;

public sealed class EfEmployeeCreator : IEmployeeCreator
{
    private readonly EmployeeOperationsDbContext _db;

    public EfEmployeeCreator(EmployeeOperationsDbContext db)
    {
        _db = db;
    }

    public async Task<int> CreateAsync(
        CreateEmployeeCommand command,
        CancellationToken cancellationToken = default)
    {
        var employeeNumber = command.EmployeeNumber.Trim();

        var departmentExists = await _db.Departments
            .AnyAsync(
                d => d.DepartmentId == command.DepartmentId,
                cancellationToken);

        if (!departmentExists)
        {
            throw new InvalidOperationException(
                $"Department {command.DepartmentId} does not exist.");
        }

        var employeeNumberExists = await _db.Employees
            .AnyAsync(
                e => e.EmployeeNumber == employeeNumber,
                cancellationToken);

        if (employeeNumberExists)
        {
            throw new InvalidOperationException(
                $"Employee number '{employeeNumber}' already exists.");
        }

        var entity = EmployeeMapper.ToEntity(
            command,
            DateTime.UtcNow);

        _db.Employees.Add(entity);

        try
        {
            await _db.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException)
        {
            // The unique database index is the final protection
            // against duplicate EmployeeNumber values under concurrency.
            throw;
        }

        return entity.EmployeeId;
    }
}
