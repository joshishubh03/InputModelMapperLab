using System.ComponentModel.DataAnnotations;
using InputModelMapperLab.Api.Contracts;
using InputModelMapperLab.Application.Employees;
using InputModelMapperLab.Infrastructure.Data;
using InputModelMapperLab.Infrastructure.Employees;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration
    .GetConnectionString("EmployeeOperations");

if (string.IsNullOrWhiteSpace(connectionString))
{
    throw new InvalidOperationException(
        "Connection string 'EmployeeOperations' was not configured.");
}

builder.Services.AddDbContext<EmployeeOperationsDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddScoped<IEmployeeCreator, EfEmployeeCreator>();

var app = builder.Build();

app.MapGet("/", () => Results.Ok(new
{
    Application = "Input Model Mapper Lab",
    Status = "Running",
    Endpoint = "POST /api/employees"
}));

app.MapGet("/health/database", async (
    EmployeeOperationsDbContext db,
    CancellationToken cancellationToken) =>
{
    var canConnect = await db.Database.CanConnectAsync(cancellationToken);

    return canConnect
        ? Results.Ok(new { Database = "Connected" })
        : Results.Problem("Database connection failed.");
});

app.MapGet("/api/departments", async (
    EmployeeOperationsDbContext db,
    CancellationToken cancellationToken) =>
{
    var departments = await db.Departments
        .AsNoTracking()
        .OrderBy(d => d.DepartmentId)
        .Select(d => new
        {
            d.DepartmentId,
            d.Name
        })
        .ToListAsync(cancellationToken);

    return Results.Ok(departments);
});

app.MapPost("/api/employees", async (
    CreateEmployeeRequest request,
    IEmployeeCreator creator,
    CancellationToken cancellationToken) =>
{
    var validationResults = new List<ValidationResult>();
    var validationContext = new ValidationContext(request);

    var isValid = Validator.TryValidateObject(
        request,
        validationContext,
        validationResults,
        validateAllProperties: true);

    if (!isValid)
    {
        var errors = validationResults
            .SelectMany(result =>
                result.MemberNames.DefaultIfEmpty(string.Empty)
                    .Select(member => new
                    {
                        Field = member,
                        Error = result.ErrorMessage ?? "Invalid value."
                    }))
            .GroupBy(item => item.Field)
            .ToDictionary(
                group => group.Key,
                group => group.Select(item => item.Error).ToArray());

        return Results.ValidationProblem(errors);
    }

    var command = new CreateEmployeeCommand(
        request.EmployeeNumber,
        request.FirstName,
        request.LastName,
        request.Email,
        request.HireDate,
        request.Salary,
        request.DepartmentId);

    try
    {
        var employeeId = await creator.CreateAsync(
            command,
            cancellationToken);

        return Results.Created(
            $"/api/employees/{employeeId}",
            new { EmployeeId = employeeId });
    }
    catch (InvalidOperationException ex)
    {
        return Results.BadRequest(new { Error = ex.Message });
    }
});

app.Run();
