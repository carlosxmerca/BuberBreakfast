using BuberBreakfast.Data;
using BuberBreakfast.Models;
using Microsoft.EntityFrameworkCore;

namespace BuberBreakfast.Repositories.Breakfasts;

public class BreakfastRepository : IBreakfastRepository
{
    private readonly AppDbContext _context;

    public BreakfastRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Breakfast?> GetByIdAsync(Guid id)
    {
        return await _context.Breakfast.FindAsync(id);
    }

    public async Task<IEnumerable<Breakfast>> GetAllAsync()
    {
        return await _context.Breakfast.ToListAsync();
    }

    public async Task AddAsync(Breakfast breakfast)
    {
        await _context.Breakfast.AddAsync(breakfast);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Breakfast breakfast)
    {
        _context.Breakfast.Update(breakfast);
        await _context.SaveChangesAsync();
    }

    public async Task UpsertAsync(Breakfast breakfast)
    {
        _context.Breakfast.Update(breakfast);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var breakfast = await GetByIdAsync(id);
        if (breakfast != null)
        {
            _context.Breakfast.Remove(breakfast);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> ExistsAsync(Guid id)
    {
        return await _context.Breakfast.AnyAsync(b => b.Id == id);
    }

    public async Task<List<Breakfast>> GetByUserIdAsync(Guid userId)
    {
        return await _context.Breakfast
            .Where(b => b.UserId == userId)
            .ToListAsync();
    }
}
