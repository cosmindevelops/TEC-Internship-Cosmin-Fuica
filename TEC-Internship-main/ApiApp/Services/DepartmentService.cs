using ApiApp.Common.Dto;
using ApiApp.Common.Exceptions;
using ApiApp.Data;
using ApiApp.Services.Interfaces;
using AutoMapper;
using Internship.Model;
using Microsoft.EntityFrameworkCore;

namespace ApiApp.Services;

public class DepartmentService : IDepartmentService
{
    private readonly APIDbContext _context;
    private readonly IMapper _mapper;

    public DepartmentService(APIDbContext context, IMapper mapper)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    /// <summary>
    /// Retrieves all departments from the database, excluding "Unassigned" departments.
    /// </summary>
    /// <returns>A list of <see cref="DepartmentDto"/>.</returns>
    public async Task<IEnumerable<DepartmentDto>> GetAllDepartmentsAsync()
    {
        var departments = await _context.Departments
                                        .Where(d => d.DepartmentName != "Unassigned")
                                        .ToListAsync();
        return _mapper.Map<IEnumerable<DepartmentDto>>(departments);
    }

    /// <summary>
    /// Retrieves a department by its ID.
    /// </summary>
    /// <param name="id">The ID of the department to retrieve.</param>
    /// <returns>The <see cref="DepartmentDto"/> of the retrieved department.</returns>
    /// <exception cref="DepartmentNotFoundException">Thrown when the department is not found.</exception>
    public async Task<DepartmentDto> GetDepartmentByIdAsync(int id)
    {
        var department = await _context.Departments.FindAsync(id);

        if (department == null) throw new DepartmentNotFoundException($"Department with ID {id} was not found");

        return _mapper.Map<DepartmentDto>(department);
    }

    /// <summary>
    /// Gets the total number of departments excluding "Unassigned".
    /// </summary>
    /// <returns>The total number of departments as an integer.</returns>
    public async Task<int> GetTotalDepartmentsAsync()
    {
        return await _context.Departments
            .Where(d => d.DepartmentName != "Unassigned")
            .CountAsync();
    }

    /// <summary>
    /// Creates a new department and adds it to the database.
    /// </summary>
    /// <param name="departmentDto">The DTO containing department data.</param>
    /// <returns>The created <see cref="DepartmentDto"/>.</returns>
    /// <exception cref="DuplicateDepartmentException">Thrown when a department with the same name already exists.</exception>
    public async Task<DepartmentDto> CreateDepartmentAsync(CreateUpdateDepartmentDto departmentDto)
    {
        var existingDepartment = await _context.Departments
            .FirstOrDefaultAsync(d => d.DepartmentName.ToLower() == departmentDto.DepartmentName.ToLower());

        if (existingDepartment != null) throw new DuplicateDepartmentException($"A department with the name '{departmentDto.DepartmentName}' already exists.");

        var department = _mapper.Map<Department>(departmentDto);
        _context.Departments.Add(department);
        await _context.SaveChangesAsync();

        return _mapper.Map<DepartmentDto>(department);
    }

    /// <summary>
    /// Deletes a department from the database.
    /// </summary>
    /// <param name="departmentId">The ID of the department to delete.</param>
    /// <returns><c>true</c> if the deletion was successful; otherwise, <c>false</c>.</returns>
    /// <exception cref="DepartmentNotFoundException">Thrown when the department is not found.</exception>
    public async Task<bool> DeleteDepartmentAsync(int departmentId)
    {
        var department = await _context.Departments.FindAsync(departmentId);

        if (department == null) throw new DepartmentNotFoundException($"Department with ID {departmentId} was not found");

        var unassignedDepartment = await GetOrCreateUnassignedDepartmentAsync();

        await ReassignPersonsToUnassignedDepartmentAsync(departmentId, unassignedDepartment.DepartmentId);

        _context.Departments.Remove(department);
        await _context.SaveChangesAsync();
        return true;
    }

    /// <summary>
    /// Updates the name of an existing department.
    /// </summary>
    /// <param name="departmentId">The ID of the department to update.</param>
    /// <param name="newDepartmentName">The new name of the department.</param>
    /// <returns><c>true</c> if the update was successful; otherwise, <c>false</c>.</returns>
    /// <exception cref="DepartmentNotFoundException">Thrown when the department is not found.</exception>
    public async Task<bool> UpdateDepartmentNameAsync(int departmentId, string newDepartmentName)
    {
        var department = await _context.Departments.FindAsync(departmentId);

        if (department == null) throw new DepartmentNotFoundException($"Department with ID {departmentId} was not found");

        department.DepartmentName = newDepartmentName;
        await _context.SaveChangesAsync();

        return true;
    }

    /// <summary>
    /// Changes the department of a person.
    /// </summary>
    /// <param name="personId">The ID of the person to update.</param>
    /// <param name="newDepartmentName">The new department name for the person.</param>
    /// <returns><c>true</c> if the update was successful; otherwise, <c>false</c>.</returns>
    /// <exception cref="PersonNotFoundException">Thrown when the person is not found.</exception>
    public async Task<bool> ChangePersonDepartmentAsync(int personId, string newDepartmentName)
    {
        var person = await _context.Persons.Include(p => p.Position).ThenInclude(pos => pos.Department).FirstOrDefaultAsync(p => p.Id == personId);

        if (person == null) throw new PersonNotFoundException($"Person with ID {personId} was not found");

        var newDepartment = await _context.Departments.FirstOrDefaultAsync(d => d.DepartmentName == newDepartmentName);
        if (newDepartment == null)
        {
            newDepartment = new Department { DepartmentName = newDepartmentName };
            _context.Departments.Add(newDepartment);
            await _context.SaveChangesAsync();
        }

        person.Position.Department = newDepartment;
        await _context.SaveChangesAsync();

        return true;
    }

    /// <summary>
    /// Retrieves or creates an "Unassigned" department.
    /// </summary>
    /// <returns>The "Unassigned" <see cref="Department"/> entity.</returns>
    private async Task<Department> GetOrCreateUnassignedDepartmentAsync()
    {
        var unassignedDepartment = await _context.Departments
            .FirstOrDefaultAsync(d => d.DepartmentName == "Unassigned");

        if (unassignedDepartment == null)
        {
            unassignedDepartment = new Department { DepartmentName = "Unassigned" };
            _context.Departments.Add(unassignedDepartment);
            await _context.SaveChangesAsync();
        }

        return unassignedDepartment;
    }

    /// <summary>
    /// Reassigns persons from a specified department to the "Unassigned" department.
    /// </summary>
    /// <param name="departmentId">The ID of the department to reassign from.</param>
    /// <param name="unassignedDepartmentId">The ID of the "Unassigned" department.</param>
    private async Task ReassignPersonsToUnassignedDepartmentAsync(int departmentId, int unassignedDepartmentId)
    {
        var personsToReassign = await _context.Persons
            .Include(p => p.Position)
            .Where(p => p.Position.DepartmentId == departmentId)
            .ToListAsync();

        foreach (var person in personsToReassign)
        {
            person.Position.DepartmentId = unassignedDepartmentId;
        }

        await _context.SaveChangesAsync();
    }
}