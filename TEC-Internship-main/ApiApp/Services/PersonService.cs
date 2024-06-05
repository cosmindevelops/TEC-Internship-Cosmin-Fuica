using ApiApp.Common.Dto;
using ApiApp.Common.Exceptions;
using ApiApp.Data;
using ApiApp.Services.Interfaces;
using AutoMapper;
using Internship.Model;
using Microsoft.EntityFrameworkCore;

namespace ApiApp.Services;

public class PersonService : IPersonService
{
    private readonly APIDbContext _context;
    private readonly IMapper _mapper;

    public PersonService(APIDbContext context, IMapper mapper)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    // Method to get all persons
    public async Task<IEnumerable<PersonDto>> GetAllPersonsAsync()
    {
        var persons = await _context.Persons
            .Include(p => p.PersonDetails)
            .Include(p => p.Position).ThenInclude(pos => pos.Department)
            .Include(p => p.Salary)
            .ToListAsync();

        return _mapper.Map<IEnumerable<PersonDto>>(persons);
    }

    // Method to create a new person
    public async Task<PersonDto> CreatePersonAsync(CreateUpdatePersonDto personDto)
    {
        var department = await GetOrCreateDepartmentAsync(personDto.Position.Department.DepartmentName);

        var position = new Position
        {
            Name = personDto.Position.Name,
            Department = department
        };

        var salary = new Salary
        {
            Amount = personDto.Salary.Amount
        };

        var person = new Person
        {
            Name = personDto.Name,
            Surname = personDto.Surname,
            Age = personDto.Age,
            Email = personDto.Email,
            Address = personDto.Address,
            Position = position,
            Salary = salary
        };

        using (var transaction = await _context.Database.BeginTransactionAsync())
        {
            _context.Persons.Add(person);
            await _context.SaveChangesAsync();

            if (personDto.PersonDetails != null)
            {
                var personDetails = new PersonDetails
                {
                    PersonId = person.Id,
                    BirthDay = personDto.PersonDetails.BirthDay,
                    PersonCity = personDto.PersonDetails.PersonCity
                };

                _context.PersonDetails.Add(personDetails);
                await _context.SaveChangesAsync();

                person.PersonDetails = personDetails;
            }

            await transaction.CommitAsync();
        }

        return _mapper.Map<PersonDto>(person);
    }

    // Method to update a person's information
    public async Task<bool> UpdatePersonAsync(int personId, CreateUpdatePersonDto personDto)
    {
        var person = await _context.Persons
            .Include(p => p.PersonDetails)
            .Include(p => p.Position).ThenInclude(pos => pos.Department)
            .Include(p => p.Salary)
            .FirstOrDefaultAsync(p => p.Id == personId);

        if (person == null)
        {
            throw new PersonNotFoundException($"Person with ID {personId} was not found");
        }

        person.Name = personDto.Name;
        person.Surname = personDto.Surname;
        person.Age = personDto.Age;
        person.Email = personDto.Email;
        person.Address = personDto.Address;

        if (personDto.PersonDetails != null)
        {
            if (person.PersonDetails == null)
            {
                person.PersonDetails = new PersonDetails { PersonId = personId };
            }

            person.PersonDetails.BirthDay = personDto.PersonDetails.BirthDay;
            person.PersonDetails.PersonCity = personDto.PersonDetails.PersonCity;
        }

        if (personDto.Salary != null)
        {
            if (person.Salary == null)
            {
                person.Salary = new Salary();
            }

            person.Salary.Amount = personDto.Salary.Amount;
        }

        if (personDto.Position != null)
        {
            var department = await GetOrCreateDepartmentAsync(personDto.Position.Department.DepartmentName);

            person.Position = new Position
            {
                Name = personDto.Position.Name,
                Department = department
            };
        }

        await _context.SaveChangesAsync();
        return true;
    }

    // Method to delete a person
    public async Task<bool> DeletePersonAsync(int personId)
    {
        var person = await _context.Persons.FindAsync(personId);

        if (person == null) throw new PersonNotFoundException($"Person with ID {personId} was not found");

        _context.Persons.Remove(person);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<PersonDto> GetPersonAsync(int personId)
    {
        var person = await _context.Persons
            .Include(p => p.PersonDetails)
            .Include(p => p.Position).ThenInclude(pos => pos.Department)
            .Include(p => p.Salary)
            .FirstOrDefaultAsync(p => p.Id == personId);

        if (person == null) throw new PersonNotFoundException($"Person with ID {personId} was not found");

        return _mapper.Map<PersonDto>(person);
    }

    private async Task<Department> GetOrCreateDepartmentAsync(string departmentName)
    {
        var department = await _context.Departments
            .FirstOrDefaultAsync(d => d.DepartmentName == departmentName);

        if (department == null)
        {
            department = new Department { DepartmentName = departmentName };
            _context.Departments.Add(department);
            await _context.SaveChangesAsync();
        }

        return department;
    }
}