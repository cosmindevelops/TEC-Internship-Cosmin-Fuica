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
        // Find or create the department
        var department = await _context.Departments
            .FirstOrDefaultAsync(d => d.DepartmentName == personDto.Position.Department.DepartmentName);

        if (department == null)
        {
            department = new Department
            {
                DepartmentName = personDto.Position.Department.DepartmentName
            };
            _context.Departments.Add(department);
        }

        // Create the position
        var position = new Position
        {
            Name = personDto.Position.Name,
            Department = department
        };

        // Create the salary
        var salary = new Salary
        {
            Amount = personDto.Salary.Amount
        };

        // Create the person without PersonDetails
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

        // Add the person to the context and save changes to generate the PersonId
        _context.Persons.Add(person);
        await _context.SaveChangesAsync();

        // Now that the person is created, create the PersonDetails if provided
        if (personDto.PersonDetails != null)
        {
            var personDetails = new PersonDetails
            {
                PersonId = person.Id, // Assign the generated PersonId to PersonDetails
                BirthDay = personDto.PersonDetails.BirthDay,
                PersonCity = personDto.PersonDetails.PersonCity
            };

            // Add the PersonDetails to the context and save changes
            _context.PersonDetails.Add(personDetails);
            await _context.SaveChangesAsync();

            // Update the person object with the new PersonDetails
            person.PersonDetails = personDetails;
        }

        // Map the created person to a PersonDto and return it
        return _mapper.Map<PersonDto>(person);
    }

    // Method to update a person's information
    public async Task<bool> UpdatePersonAsync(int personId, CreateUpdatePersonDto personDto)
    {
        var person = await _context.Persons.Include(p => p.PersonDetails).FirstOrDefaultAsync(p => p.Id == personId);
        if (person == null)
        {
            throw new PersonNotFoundException($"Person with ID {personId} was not found");
        }

        _mapper.Map(personDto, person);

        // Update or add PersonDetails
        if (personDto.PersonDetails != null)
        {
            if (person.PersonDetails == null)
            {
                person.PersonDetails = new PersonDetails
                {
                    PersonId = personId
                };
            }

            _mapper.Map(personDto.PersonDetails, person.PersonDetails);
        }

        // Update or add Salary
        if (personDto.Salary != null)
        {
            if (person.Salary == null)
            {
                person.Salary = new Salary();
            }

            _mapper.Map(personDto.Salary, person.Salary);
        }

        // Update or add Position and Department
        if (personDto.Position != null)
        {
            var department = await _context.Departments
                .FirstOrDefaultAsync(d => d.DepartmentName == personDto.Position.Department.DepartmentName);

            if (department == null)
            {
                department = new Department
                {
                    DepartmentName = personDto.Position.Department.DepartmentName
                };
                _context.Departments.Add(department);
            }

            var position = new Position
            {
                Name = personDto.Position.Name,
                Department = department
            };

            person.Position = position;
        }

        await _context.SaveChangesAsync();
        return true;
    }

    // Method to delete a person
    public async Task<bool> DeletePersonAsync(int personId)
    {
        var person = await _context.Persons.FindAsync(personId);
        if (person == null)
        {
            throw new PersonNotFoundException($"Person with ID {personId} was not found");
        }

        _context.Persons.Remove(person);
        await _context.SaveChangesAsync();
        return true;
    }

    // Method to delete a department
    public async Task<bool> DeleteDepartmentAsync(int departmentId)
    {
        var department = await _context.Departments.FindAsync(departmentId);
        if (department == null)
        {
            throw new DepartmentNotFoundException($"Department with ID {departmentId} was not found");
        }

        _context.Departments.Remove(department);
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

        if (person == null)
        {
            throw new PersonNotFoundException($"Person with ID {personId} was not found");
        }

        return _mapper.Map<PersonDto>(person);
    }
}