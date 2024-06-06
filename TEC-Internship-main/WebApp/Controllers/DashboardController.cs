using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using WebApp.Models;
using WebApp.Services.Interfaces;

namespace WebApp.Controllers;

public class DashboardController : Controller
{
    private readonly IDashboardService _dashboardService;

    public DashboardController(IDashboardService dashboardService)
    {
        _dashboardService = dashboardService ?? throw new ArgumentNullException(nameof(dashboardService));
    }

    public async Task<IActionResult> Index()
    {
        var totalPersons = await _dashboardService.GetTotalPersonsAsync();
        var totalDepartments = await _dashboardService.GetTotalDepartmentsAsync();

        var model = new DashboardViewModel
        {
            TotalPersons = totalPersons,
            TotalDepartments = totalDepartments,
        };

        return View(model);
    }
}