using BuberBreakfast.Models;
using BuberBreakfast.Repositories.Breakfasts;
using BuberBreakfast.ServiceErrors;
using ErrorOr;

namespace BuberBreakfast.Services.Breakfasts;

public class BreakfastService : IBreakfastService
{
    private readonly IBreakfastRepository _breakfastRepository;

    public BreakfastService(IBreakfastRepository breakfastRepository)
    {
        _breakfastRepository = breakfastRepository;
    }

    public async Task<ErrorOr<Created>> CreateBreakfastAsync(Breakfast breakfast)
    {
        await _breakfastRepository.AddAsync(breakfast);
        return Result.Created;
    }

    public async Task<ErrorOr<Breakfast>> GetBreakfastAsync(Guid id)
    {
        var breakfast = await _breakfastRepository.GetByIdAsync(id);

        if (breakfast is null)
        {
            return Errors.Breakfast.NotFound;
        }

        return breakfast;
    }

    public async Task<ErrorOr<UpsertedBreakfast>> UpsertBreakfastAsync(Breakfast breakfast)
    {
        var isNewlyCreated = !await _breakfastRepository.ExistsAsync(breakfast.Id);
        await _breakfastRepository.UpsertAsync(breakfast);
        return new UpsertedBreakfast(isNewlyCreated);
    }

    public async Task<ErrorOr<Deleted>> DeleteBreakfastAsync(Guid id)
    {
        var exists = await _breakfastRepository.ExistsAsync(id);
        if (!exists)
        {
            return Errors.Breakfast.NotFound;
        }

        await _breakfastRepository.DeleteAsync(id);
        return Result.Deleted;
    }
}
