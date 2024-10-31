using BuberBreakfast.Models;
using ErrorOr;

namespace BuberBreakfast.Services.Breakfasts;

public interface IBreakfastService
{
    Task<ErrorOr<Created>> CreateBreakfastAsync(Breakfast breakfast);
    Task<ErrorOr<Breakfast>> GetBreakfastAsync(Guid id);
    Task<ErrorOr<UpsertedBreakfast>> UpsertBreakfastAsync(Breakfast breakfast);
    Task<ErrorOr<Deleted>> DeleteBreakfastAsync(Guid id);
    Task<ErrorOr<List<Breakfast>>> GetBreakfastsByUsersync(Guid userId);
}
