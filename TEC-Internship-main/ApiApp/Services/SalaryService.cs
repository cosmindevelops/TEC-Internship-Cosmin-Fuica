using ApiApp.Common.Dto;
using ApiApp.Common.Exceptions;
using ApiApp.Data;
using ApiApp.Services.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace ApiApp.Services;

public class SalaryService : ISalaryService
{
    private readonly APIDbContext _context;
    private readonly IMapper _mapper;

    public SalaryService(APIDbContext context, IMapper mapper)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    /// <summary>
    /// Retrieves all salaries from the database.
    /// </summary>
    /// <returns>A list of <see cref="SalaryWithFullNameDto"/>.</returns>
    public async Task<IEnumerable<SalaryWithFullNameDto>> GetAllSalariesAsync()
    {
        var salaries = await _context.Persons
            .Include(p => p.Salary)
            .Where(p => p.Salary != null)
            .Select(p => new SalaryWithFullNameDto
            {
                PersonId = p.Id,
                FullName = p.Name + " " + p.Surname,
                Amount = p.Salary.Amount
            })
            .ToListAsync();

        return salaries;
    }

    /// <summary>
    /// Retrieves the salary of a person by their ID.
    /// </summary>
    /// <param name="personId">The ID of the person to retrieve the salary for.</param>
    /// <returns>The <see cref="SalaryDto"/> of the person's salary.</returns>
    /// <exception cref="SalaryNotFoundException">Thrown when the salary is not found.</exception>
    public async Task<SalaryDto> GetSalaryByPersonIdAsync(int personId)
    {
        var person = await _context.Persons
            .Include(p => p.Salary)
            .FirstOrDefaultAsync(p => p.Id == personId);

        if (person == null || person.Salary == null) throw new SalaryNotFoundException($"Salary for person with ID {personId} was not found");

        return _mapper.Map<SalaryDto>(person.Salary);
    }

    /// <summary>
    /// Deletes the salary of a person.
    /// </summary>
    /// <param name="personId">The ID of the person to delete the salary for.</param>
    /// <returns><c>true</c> if the deletion was successful; otherwise, <c>false</c>.</returns>
    /// <exception cref="SalaryNotFoundException">Thrown when the salary is not found.</exception>
    public async Task<bool> DeleteSalaryAsync(int personId)
    {
        var person = await _context.Persons
                .Include(p => p.Salary)
                .FirstOrDefaultAsync(p => p.Id == personId);

        if (person == null || person.Salary == null) throw new SalaryNotFoundException($"Salary for person with ID {personId} was not found");

        person.Salary.Amount = 0;
        await _context.SaveChangesAsync();
        return true;
    }

    /// <summary>
    /// Updates the salary of a person.
    /// </summary>
    /// <param name="personId">The ID of the person to update the salary for.</param>
    /// <param name="newSalaryAmount">The new salary amount.</param>
    /// <returns><c>true</c> if the update was successful; otherwise, <c>false</c>.</returns>
    /// <exception cref="SalaryNotFoundException">Thrown when the salary is not found.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the new salary amount is invalid.</exception>
    public async Task<bool> UpdateSalaryAsync(int personId, int newSalaryAmount)
    {
        const int MaxSalaryAmount = int.MaxValue;

        if (newSalaryAmount <= 0 || newSalaryAmount > MaxSalaryAmount) throw new ArgumentOutOfRangeException(nameof(newSalaryAmount), $"Salary amount must be greater than 0 and less than or equal to {MaxSalaryAmount}");

        var person = await _context.Persons
            .Include(p => p.Salary)
            .FirstOrDefaultAsync(p => p.Id == personId);

        if (person == null || person.Salary == null) throw new SalaryNotFoundException($"Salary for person with ID {personId} was not found");

        person.Salary.Amount = newSalaryAmount;
        await _context.SaveChangesAsync();
        return true;
    }
}