using BuberBreakfast.Models;
using BuberBreakfast.Repositories.Breakfasts;
using BuberBreakfast.ServiceErrors;
using ErrorOr;

namespace BuberBreakfast.Services.Breakfasts;

public class BreakfastService : IBreakfastService
{
    // private static readonly Dictionary<Guid, Breakfast> _breakfasts = new();

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

    // public ErrorOr<Created> CreateBreakfast(Breakfast breakfast)
    // {
    //     _breakfasts.Add(breakfast.Id, breakfast);
    //     return Result.Created;
    // }

    public async Task<ErrorOr<Breakfast>> GetBreakfastAsync(Guid id)
    {
        var breakfast = await _breakfastRepository.GetByIdAsync(id);

        if (breakfast is null)
        {
            return Errors.Breakfast.NotFound;
        }

        return breakfast;
    }

    // public ErrorOr<Breakfast> GetBreakfast(Guid id)
    // {
    //     if (_breakfasts.TryGetValue(id, out var breakfast))
    //     {
    //         return breakfast;
    //     }
    //     return Errors.Breakfast.NotFound;
    // }

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

    // public ErrorOr<UpsertedBreakfast> UpsertBreakfast(Breakfast breakfast)
    // {
    //     var IsNewlyCreated = !_breakfasts.ContainsKey(breakfast.Id);
    //     _breakfasts[breakfast.Id] = breakfast;
    //     return new UpsertedBreakfast(IsNewlyCreated);
    // }

    // public ErrorOr<Deleted> DeleteBreakfast(Guid id)
    // {
    //     _breakfasts.Remove(id);
    //     return Result.Deleted;
    // }
}
