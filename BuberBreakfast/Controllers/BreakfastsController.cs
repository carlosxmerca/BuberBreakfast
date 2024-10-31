using BuberBreakfast.Contracts.Breakfast;
using BuberBreakfast.Models;
using BuberBreakfast.Services.Breakfasts;
using ErrorOr;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BuberBreakfast.Controllers;

public class BreakfastsController : ApiController
{
    private readonly IBreakfastService _breakfastService;
    private readonly ILogger<BreakfastsController> _logger;


    public BreakfastsController(IBreakfastService breakfastService, ILogger<BreakfastsController> logger)
    {
        _breakfastService = breakfastService;
        _logger = logger;
    }

    private static BreakfastResponse MapBreakfastResponse(Breakfast breakfast)
    {
        return new BreakfastResponse(
                    breakfast.Id,
                    breakfast.Name,
                    breakfast.Description,
                    breakfast.StartDateTime,
                    breakfast.EndDateTime,
                    breakfast.LastModifiedDateTime,
                    breakfast.Savory,
                    breakfast.Sweet
                );
    }

    private IActionResult CreatedAtGetBreakfast(Breakfast breakfast)
    {
        return CreatedAtAction(
                    actionName: nameof(GetBreakfast),
                    routeValues: new { id = breakfast.Id },
                    value: MapBreakfastResponse(breakfast));
    }

    [Authorize]
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetBreakfast(Guid id)
    {
        ErrorOr<Breakfast> getBreakfastResult = await _breakfastService.GetBreakfastAsync(id);

        return getBreakfastResult.Match(
            breakfast => Ok(MapBreakfastResponse(breakfast)),
            errors => Problem(errors));
    }

    [Authorize]
    [HttpGet("user")]
    public async Task<IActionResult> GetBreakfastsByUser()
    {
        _logger.LogInformation("Starting CreateBreakfast");
        // TODO: abstract get user id into util
        var userIdString = User.FindFirst("userId")?.Value;

        if (userIdString == null)
        {
            _logger.LogWarning("Could not find userId in the JWT token.");
            return Unauthorized("User could not be identified.");
        }

        _logger.LogInformation("userId obtained from JWT token: {UserId}", userIdString);

        if (!Guid.TryParse(userIdString, out Guid userId))
        {
            _logger.LogWarning("The userId is not a valid GUID.");
            return Unauthorized("User could not be identified.");
        }

        ErrorOr<List<Breakfast>> getBreakfastsResult = await _breakfastService.GetBreakfastsByUsersync(userId);

        return getBreakfastsResult.Match(
                breakfasts => Ok(breakfasts.Select(MapBreakfastResponse)),
                errors => Problem(errors));
    }

    [Authorize]
    [HttpPost()]
    public async Task<IActionResult> CreateBreakfast(CreateBreakfastRequest request)
    {
        _logger.LogInformation("Starting CreateBreakfast");
        // TODO: abstract get user id into util
        var userIdString = User.FindFirst("userId")?.Value;

        if (userIdString == null)
        {
            _logger.LogWarning("Could not find userId in the JWT token.");
            return Unauthorized("User could not be identified.");
        }

        _logger.LogInformation("userId obtained from JWT token: {UserId}", userIdString);

        if (!Guid.TryParse(userIdString, out Guid userId))
        {
            _logger.LogWarning("The userId is not a valid GUID.");
            return Unauthorized("User could not be identified.");
        }

        ErrorOr<Breakfast> createBreakfast = Breakfast.From(request, userId);

        if (createBreakfast.IsError)
        {
            return Problem(createBreakfast.Errors);
        }
        var breakfast = createBreakfast.Value;
        ErrorOr<Created> createBreakfastResult = await _breakfastService.CreateBreakfastAsync(breakfast);

        return createBreakfastResult.Match(
           created => CreatedAtGetBreakfast(breakfast),
           errors => Problem(errors));
    }

    [Authorize]
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpsertBreakfast(Guid id, UpsertBreakfastRequest request)
    {
        _logger.LogInformation("Starting UpsertBreakfast for breakfast ID: {Id}", id);
        // TODO: abstract get user id into util
        var userIdString = User.FindFirst("userId")?.Value;

        if (userIdString == null)
        {
            _logger.LogWarning("Could not find userId in the JWT token.");
            return Unauthorized("User could not be identified.");
        }

        _logger.LogInformation("userId obtained from JWT token: {UserId}", userIdString);

        if (!Guid.TryParse(userIdString, out Guid userId))
        {
            _logger.LogWarning("The userId is not a valid GUID.");
            return Unauthorized("User could not be identified.");
        }

        ErrorOr<Breakfast> createBreakfast = Breakfast.From(id, request, userId);

        if (createBreakfast.IsError)
        {
            return Problem(createBreakfast.Errors);
        }
        var breakfast = createBreakfast.Value;
        ErrorOr<UpsertedBreakfast> upsertedBreakfastResult = await _breakfastService.UpsertBreakfastAsync(breakfast);

        return upsertedBreakfastResult.Match(
            upserted => upserted.IsNewlyCreated ? CreatedAtGetBreakfast(breakfast) : NoContent(),
            errors => Problem(errors));
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteBreakfast(Guid id)
    {
        ErrorOr<Deleted> deleteBreakfastResult = await _breakfastService.DeleteBreakfastAsync(id);

        return deleteBreakfastResult.Match(
            deleted => NoContent(),
            errors => Problem(errors));
    }
}
