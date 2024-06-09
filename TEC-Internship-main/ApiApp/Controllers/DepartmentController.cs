using ApiApp.Common.Dto;
using ApiApp.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiApp.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "User,Admin")]
public class DepartmentController : BaseController
{
    private readonly IDepartmentService _departmentService;

    public DepartmentController(IDepartmentService departmentService)
    {
        _departmentService = departmentService ?? throw new ArgumentNullException(nameof(departmentService));
    }

    /// <summary>
    /// Gets all departments.
    /// </summary>
    /// <returns>A list of all departments.</returns>
    [HttpGet]
    public async Task<IActionResult> GetAllDepartments()
    {
        var departments = await _departmentService.GetAllDepartmentsAsync();
        return Ok(departments);
    }

    /// <summary>
    /// Gets a department by its ID.
    /// </summary>
    /// <param name="id">The ID of the department to retrieve.</param>
    /// <returns>The department with the specified ID.</returns>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetDepartment(int id)
    {
        var department = await _departmentService.GetDepartmentByIdAsync(id);
        if (department == null)
        {
            return NotFound();
        }

        return Ok(department);
    }

    /// <summary>
    /// Gets the total number of departments.
    /// </summary>
    /// <returns>The total number of departments.</returns>
    [HttpGet("total")]
    public async Task<IActionResult> GetTotalDepartments()
    {
        var totalDepartments = await _departmentService.GetTotalDepartmentsAsync();
        return Ok(new { TotalDepartments = totalDepartments });
    }

    /// <summary>
    /// Creates a new department.
    /// </summary>
    /// <param name="departmentDto">The DTO containing department data.</param>
    /// <returns>The created department.</returns>
    [HttpPost]
    public async Task<IActionResult> CreateDepartment([FromBody] CreateUpdateDepartmentDto departmentDto)
    {
        var createdDepartment = await _departmentService.CreateDepartmentAsync(departmentDto);
        return CreatedAtAction(nameof(GetDepartment), new { id = createdDepartment.DepartmentId }, createdDepartment);
    }

    /// <summary>
    /// Deletes a department.
    /// </summary>
    /// <param name="id">The ID of the department to delete.</param>
    /// <returns>No content if the deletion was successful.</returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteDepartment(int id)
    {
        var deleted = await _departmentService.DeleteDepartmentAsync(id);
        if (!deleted)
        {
            return NotFound();
        }

        return NoContent();
    }

    /// <summary>
    /// Changes the department of a person.
    /// </summary>
    /// <param name="personId">The ID of the person to update.</param>
    /// <param name="newDepartmentName">The new department name for the person.</param>
    /// <returns>No content if the update was successful.</returns>
    [HttpPut("ChangePersonDepartment")]
    public async Task<IActionResult> ChangePersonDepartment(int personId, string newDepartmentName)
    {
        var result = await _departmentService.ChangePersonDepartmentAsync(personId, newDepartmentName);
        return NoContent();
    }

    /// <summary>
    /// Updates the name of a department.
    /// </summary>
    /// <param name="id">The ID of the department to update.</param>
    /// <param name="departmentDto">The DTO containing the new department name.</param>
    /// <returns>No content if the update was successful.</returns>
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateDepartmentName(int id, [FromBody] CreateUpdateDepartmentDto departmentDto)
    {
        var updated = await _departmentService.UpdateDepartmentNameAsync(id, departmentDto.DepartmentName);
        if (!updated)
        {
            return NotFound();
        }

        return NoContent();
    }
}