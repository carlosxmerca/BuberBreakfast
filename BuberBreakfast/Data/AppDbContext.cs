using BuberBreakfast.Models;
using Microsoft.EntityFrameworkCore;

namespace BuberBreakfast.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> User { get; set; } = null!;
    public DbSet<Breakfast> Breakfast { get; set; } = null!;
}
