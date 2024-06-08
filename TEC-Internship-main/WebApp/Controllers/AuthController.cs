using ApiApp.Common.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using WebApp.Services.Interfaces;

namespace WebApp.Controllers;

public class AuthController : Controller
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService ?? throw new ArgumentNullException(nameof(authService));
    }

    [HttpGet("auth")]
    public IActionResult Index()
    {
        return View();
    }

    [HttpPost("auth/login")]
    public async Task<IActionResult> Login([FromBody] LoginModelDto model)
    {
        var result = await _authService.LoginAsync(model);
        if (result == null)
        {
            return Unauthorized(new { Message = "Invalid login attempt." });
        }

        HttpContext.Session.SetString("Token", result.Token);
        HttpContext.Session.SetString("Username", result.Username);

        return Ok(new
        {
            Token = result.Token,
            Username = result.Username,
            RedirectUrl = Url.Action("Index", "Dashboard")
        });
    }

    [HttpPost("auth/register")]
    public async Task<IActionResult> Register([FromBody] RegisterModelDto model)
    {
        await _authService.RegisterAsync(model);
        return Ok(new { Messsage = "User registered successfully." });
    }
}