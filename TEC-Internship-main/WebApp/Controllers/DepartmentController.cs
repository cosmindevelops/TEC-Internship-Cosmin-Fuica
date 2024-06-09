using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;
using WebApp.Services.Interfaces;

namespace WebApp.Controllers;

public class DepartmentController : Controller
{
    private readonly IDepartmentService _departmentService;
    private readonly IConfiguration _configuration;

    public DepartmentController(IDepartmentService departmentService, IConfiguration configuration)
    {
        _departmentService = departmentService ?? throw new ArgumentNullException(nameof(departmentService));
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
    }

    /// <summary>
    /// Displays the department management view.
    /// </summary>
    /// <returns>The department management view with a list of departments.</returns>
    public async Task<IActionResult> Index()
    {
        var departments = await _departmentService.GetAllDepartmentsAsync();
        ViewData["ApiUrl"] = _configuration["ApiSettings:ApiUrl"];
        return View(departments);
    }

    /// <summary>
    /// Creates a new department.
    /// </summary>
    /// <param name="departmentName">The name of the department to create.</param>
    /// <returns>An <see cref="IActionResult"/> indicating the result of the creation operation.</returns>
    [HttpPost]
    public async Task<IActionResult> CreateDepartment([FromBody] string departmentName)
    {
        var success = await _departmentService.CreateDepartmentAsync(departmentName);
        if (!success)
        {
            return BadRequest("Failed to create department");
        }
        return Ok();
    }

    /// <summary>
    /// Updates an existing department.
    /// </summary>
    /// <param name="departmentId">The ID of the department to update.</param>
    /// <param name="newDepartmentName">The new name of the department.</param>
    /// <returns>An <see cref="IActionResult"/> indicating the result of the update operation.</returns>
    [HttpPost]
    public async Task<IActionResult> UpdateDepartment(int departmentId, string newDepartmentName)
    {
        var success = await _departmentService.UpdateDepartmentAsync(departmentId, newDepartmentName);
        if (!success)
        {
            return BadRequest("Failed to update department");
        }

        return RedirectToAction("Index");
    }

    /// <summary>
    /// Deletes a department.
    /// </summary>
    /// <param name="departmentId">The ID of the department to delete.</param>
    /// <returns>An <see cref="IActionResult"/> indicating the result of the deletion operation.</returns>
    [HttpPost]
    public async Task<IActionResult> DeleteDepartment(int departmentId)
    {
        var success = await _departmentService.DeleteDepartmentAsync(departmentId);
        if (!success)
        {
            return BadRequest("Failed to delete department");
        }

        return RedirectToAction("Index");
    }
}