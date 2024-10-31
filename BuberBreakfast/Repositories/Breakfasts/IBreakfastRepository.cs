using BuberBreakfast.Models;

namespace BuberBreakfast.Repositories.Breakfasts;

public interface IBreakfastRepository
{
    Task AddAsync(Breakfast breakfast);
    Task<Breakfast?> GetByIdAsync(Guid id);
    Task UpsertAsync(Breakfast breakfast);
    Task DeleteAsync(Guid id);
    Task<bool> ExistsAsync(Guid id);
}
