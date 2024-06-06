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

    public async Task<IEnumerable<DepartmentDto>> GetAllDepartmentsAsync()
    {
        var departments = await _context.Departments.ToListAsync();
        return _mapper.Map<IEnumerable<DepartmentDto>>(departments);
    }

    public async Task<DepartmentDto> GetDepartmentByIdAsync(int id)
    {
        var department = await _context.Departments.FindAsync(id);

        if (department == null) throw new DepartmentNotFoundException($"Department with ID {id} was not found");

        return _mapper.Map<DepartmentDto>(department);
    }

    // Method to get the total number of departments excluding "Unassigned"
    public async Task<int> GetTotalDepartmentsAsync()
    {
        return await _context.Departments
            .Where(d => d.DepartmentName != "Unassigned")
            .CountAsync();
    }

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