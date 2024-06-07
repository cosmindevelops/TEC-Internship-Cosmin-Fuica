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

    public async Task<IActionResult> Index()
    {
        var departments = await _departmentService.GetAllDepartmentsAsync();
        ViewData["ApiUrl"] = _configuration["ApiSettings:ApiUrl"];
        return View(departments);
    }

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

    [HttpPost]
    public async Task<IActionResult> CreateDepartment([FromForm] string departmentName)
    {
        var success = await _departmentService.CreateDepartmentAsync(departmentName);
        if (!success)
        {
            return BadRequest("Failed to create department");
        }

        return RedirectToAction("Index");
    }

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