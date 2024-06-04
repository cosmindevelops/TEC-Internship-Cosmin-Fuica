using ApiApp.Common.Dto;
using ApiApp.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiApp.Controllers;

[Route("api/[controller]")]
public class AuthController : BaseController
{
    private readonly IAuthService _authService;
    private readonly IConfiguration _configuration;

    public AuthController(IAuthService authService, IConfiguration configuration)
    {
        _authService = authService ?? throw new ArgumentNullException(nameof(authService));
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
    }

    /// <summary>
    /// Registers a new user with the provided registration details.
    /// </summary>
    /// <param name="model">The registration details of the user.</param>
    /// <returns>
    /// - If the registration details are invalid, returns a BadRequest response with an error message.
    /// - If the registration is successful, returns an Ok response with a success message.
    /// </returns>
    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterModelDto model)
    {
        if (model == null) return BadRequest("Invalid registration request");

        var validationResult = ValidateModel(model);
        if (validationResult != null)
        {
            return validationResult;
        }
        await _authService.RegisterAsync(model);

        return Ok(new { Messsage = "User registered successfully." });
    }

    /// <summary>
    /// Authenticates a user with the provided login details.
    /// </summary>
    /// <param name="model">The login details of the user.</param>
    /// <returns>
    /// - If the login request is null, returns a BadRequest response with an error message.
    /// - If the login details are invalid, returns a response with the validation result.
    /// - If the login is successful, returns an Ok response with the user's ID and authentication token.
    /// </returns>
    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginModelDto model)
    {
        if (model == null)
        {
            return BadRequest("Invalid login request.");
        }

        var validationResult = ValidateModel(model);
        if (validationResult != null)
        {
            return validationResult;
        }

        var result = await _authService.LoginAsync(model);

        return Ok(new AuthResponseDto { UserId = result.UserId, Token = result.Token });
    }
}