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

    public async Task<IEnumerable<SalaryWithFullNameDto>> GetAllSalariesAsync()
    {
        var salaries = await _context.Persons
            .Include(p => p.Salary)
            .Where(p => p.Salary != null)
            .Select(p => new SalaryWithFullNameDto
            {
                FullName = p.Name + " " + p.Surname,
                Amount = p.Salary.Amount
            })
            .ToListAsync();

        return salaries;
    }

    public async Task<SalaryDto> GetSalaryByPersonIdAsync(int personId)
    {
        var person = await _context.Persons
            .Include(p => p.Salary)
            .FirstOrDefaultAsync(p => p.Id == personId);

        if (person == null || person.Salary == null) throw new SalaryNotFoundException($"Salary for person with ID {personId} was not found");

        return _mapper.Map<SalaryDto>(person.Salary);
    }

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

    public async Task<bool> UpdateSalaryAsync(int personId, int newSalaryAmount)
    {
        var person = await _context.Persons
            .Include(p => p.Salary)
            .FirstOrDefaultAsync(p => p.Id == personId);

        if (person == null || person.Salary == null) throw new SalaryNotFoundException($"Salary for person with ID {personId} was not found");

        person.Salary.Amount = newSalaryAmount;
        await _context.SaveChangesAsync();
        return true;
    }
}