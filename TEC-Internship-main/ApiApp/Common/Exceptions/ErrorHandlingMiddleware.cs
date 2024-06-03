using System.Net;
using System.Text.Json;

namespace ApiApp.Common.Exceptions;

public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ErrorHandlingMiddleware(RequestDelegate next)
    {
        _next = next ?? throw new ArgumentNullException(nameof(next));
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var code = HttpStatusCode.InternalServerError;
        var result = string.Empty;

        switch (exception)
        {
            case UnauthorizedAccessException _:
                code = HttpStatusCode.Unauthorized; // 401
                result = JsonSerializer.Serialize(new { error = exception.Message });
                break;

            case UserAlreadyExistsException _:
                code = HttpStatusCode.Conflict; // 409
                result = JsonSerializer.Serialize(new { error = exception.Message });
                break;

            case InvalidCredentialsException _:
                code = HttpStatusCode.Unauthorized; // 401
                result = JsonSerializer.Serialize(new { error = exception.Message });
                break;

            default:
                code = HttpStatusCode.InternalServerError; // 500
                result = JsonSerializer.Serialize(new { error = "An unexpected error has occurred." });
                break;
        }

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)code;
        return context.Response.WriteAsync(result);
    }
}