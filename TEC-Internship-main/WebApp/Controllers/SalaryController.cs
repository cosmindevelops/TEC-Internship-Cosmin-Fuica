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

    /// <summary>
    /// Displays the salary management view.
    /// </summary>
    /// <returns>The salary management view with a list of salaries.</returns>
    public async Task<IActionResult> Index()
    {
        var salaries = await _salaryService.GetAllSalariesAsync();
        return View(salaries);
    }

    /// <summary>
    /// Updates the salary of a person.
    /// </summary>
    /// <param name="personId">The ID of the person to update the salary for.</param>
    /// <param name="newSalaryAmount">The new salary amount.</param>
    /// <returns>An <see cref="IActionResult"/> indicating the result of the update operation.</returns>
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