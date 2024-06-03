using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ApiApp.Controllers;

public abstract class BaseController : ControllerBase
{
    /// <summary>
    /// Validates the specified model using the ASP.NET Core ModelState validation.
    /// If the model is not valid, it returns a BadRequest response with a list of error messages.
    /// Otherwise, it returns null.
    /// </summary>
    /// <param name="model">The model to validate.</param>
    /// <returns>A BadRequest response with a list of error messages if the model is not valid, otherwise null.</returns>
    protected IActionResult ValidateModel(object model)
    {
        if (!TryValidateModel(model))
        {
            var errorMessages = ModelState.Values.SelectMany(val => val.Errors)
                                                 .Select(err => err.ErrorMessage)
                                                 .ToList();
            return BadRequest(new { Errors = errorMessages });
        }

        return null;
    }

    protected Guid UserId
    {
        get
        {
            var userIdValue = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdValue) || !Guid.TryParse(userIdValue, out var userId)) throw new UnauthorizedAccessException("Invalid or missing User ID in the user's claims.");

            return userId;
        }
    }
}