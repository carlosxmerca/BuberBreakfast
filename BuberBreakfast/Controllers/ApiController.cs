using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace BuberBreakfast.Controllers;

[ApiController]
[Route("[controller]")]
public class ApiController : ControllerBase
{
    protected readonly ILogger<ApiController> _logger;

    public ApiController(ILogger<ApiController> logger)
    {
        _logger = logger;
    }

    protected IActionResult Problem(List<Error> errors)
    {
        if (errors.All(e => e.Type == ErrorType.Validation))
        {
            var modelStateDictionary = new ModelStateDictionary();

            foreach (var error in errors)
            {
                modelStateDictionary.AddModelError(error.Code, error.Description);
            }

            return ValidationProblem(modelStateDictionary);
        }

        if (errors.Any(e => e.Type == ErrorType.Unexpected))
        {
            return Problem();
        }


        var firstError = errors[0];
        var statusCode = firstError.Type switch
        {
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            _ => StatusCodes.Status500InternalServerError
        };

        return Problem(statusCode: statusCode, title: firstError.Description);
    }

    protected Guid GetUserId()
    {
        var userIdString = User.FindFirst("userId")?.Value;

        if (userIdString == null)
        {
            _logger.LogWarning("Could not find userId in the JWT token.");
            throw new UnauthorizedAccessException("User could not be identified.");
        }

        _logger.LogInformation("userId obtained from JWT token: {UserId}", userIdString);

        if (!Guid.TryParse(userIdString, out Guid userId))
        {
            _logger.LogWarning("The userId is not a valid GUID.");
            throw new UnauthorizedAccessException("User could not be identified.");
        }

        return userId;
    }
}
