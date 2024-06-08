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

    [HttpGet]
    public async Task<IActionResult> GetAllDepartments()
    {
        var departments = await _departmentService.GetAllDepartmentsAsync();
        return Ok(departments);
    }

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

    [HttpGet("total")]
    public async Task<IActionResult> GetTotalDepartments()
    {
        var totalDepartments = await _departmentService.GetTotalDepartmentsAsync();
        return Ok(new { TotalDepartments = totalDepartments });
    }

    [HttpPost]
    public async Task<IActionResult> CreateDepartment([FromBody] CreateUpdateDepartmentDto departmentDto)
    {
        var createdDepartment = await _departmentService.CreateDepartmentAsync(departmentDto);
        return CreatedAtAction(nameof(GetDepartment), new { id = createdDepartment.DepartmentId }, createdDepartment);
    }

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

    [HttpPut("ChangePersonDepartment")]
    public async Task<IActionResult> ChangePersonDepartment(int personId, string newDepartmentName)
    {
        var result = await _departmentService.ChangePersonDepartmentAsync(personId, newDepartmentName);
        return NoContent();
    }

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