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

    public BreakfastsController(IBreakfastService breakfastService)
    {
        _breakfastService = breakfastService;
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

    [HttpPost()]
    public async Task<IActionResult> CreateBreakfast(CreateBreakfastRequest request)
    {
        ErrorOr<Breakfast> createBreakfast = Breakfast.From(request);

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

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpsertBreakfast(Guid id, UpsertBreakfastRequest request)
    {
        ErrorOr<Breakfast> createBreakfast = Breakfast.From(id, request);

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
