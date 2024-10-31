using BuberBreakfast.Models;
using ErrorOr;

namespace BuberBreakfast.Services.Users;

public interface IUserService
{
    Task<ErrorOr<Created>> CreateUserAsync(User user);
    Task<ErrorOr<Updated>> UpdateUserAsync(User user);
    Task<ErrorOr<User>> GetUserAsync(Guid id);
    Task<ErrorOr<Deleted>> DeleteUserAsync(Guid id);
}
