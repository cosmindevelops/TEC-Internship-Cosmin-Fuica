using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using WebApp.Services.Interfaces;

namespace WebApp.Controllers;

public class SalaryController : Controller
{
    private readonly ISalaryService _salaryService;

    public SalaryController(ISalaryService salaryService)
    {
        _salaryService = salaryService ?? throw new ArgumentNullException(nameof(salaryService));
    }

    public async Task<IActionResult> Index()
    {
        var salaries = await _salaryService.GetAllSalariesAsync();
        return View(salaries);
    }

    [HttpPost]
    public async Task<IActionResult> UpdateSalary(int personId, int newSalaryAmount)
    {
        var success = await _salaryService.UpdateSalaryAsync(personId, newSalaryAmount);
        if (!success)
        {
            return BadRequest("Failed to update salary");
        }

        return RedirectToAction("Index");
    }
}