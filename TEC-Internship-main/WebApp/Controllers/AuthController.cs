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

    /// <summary>
    /// Displays the authentication view.
    /// </summary>
    /// <returns>The authentication view.</returns>
    [HttpGet("auth")]
    public IActionResult Index()
    {
        return View();
    }

    /// <summary>
    /// Logs in a user.
    /// </summary>
    /// <param name="model">The login model containing user credentials.</param>
    /// <returns>An <see cref="IActionResult"/> indicating the result of the login operation.</returns>
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

    /// <summary>
    /// Registers a new user.
    /// </summary>
    /// <param name="model">The registration model containing user details.</param>
    /// <returns>An <see cref="IActionResult"/> indicating the result of the registration operation.</returns>
    [HttpPost("auth/register")]
    public async Task<IActionResult> Register([FromBody] RegisterModelDto model)
    {
        await _authService.RegisterAsync(model);
        return Ok(new { Messsage = "User registered successfully." });
    }
}