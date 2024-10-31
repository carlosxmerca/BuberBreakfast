using BuberBreakfast.Models;
using BuberBreakfast.Repositories.Users;
using BuberBreakfast.ServiceErrors;
using ErrorOr;

namespace BuberBreakfast.Services.Users;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<ErrorOr<Created>> CreateUserAsync(User user)
    {
        user.SetPassword(HashPassword(user.Password));
        await _userRepository.AddAsync(user);
        return Result.Created;
    }

    public async Task<ErrorOr<Updated>> UpdateUserAsync(User user)
    {
        await _userRepository.UpdateAsync(user);
        return Result.Updated;
    }

    public async Task<ErrorOr<Deleted>> DeleteUserAsync(Guid id)
    {
        var exists = await _userRepository.ExistsAsync(id);
        if (!exists)
        {
            return Errors.User.NotFound;
        }

        await _userRepository.DeleteAsync(id);
        return Result.Deleted;
    }

    public async Task<ErrorOr<User>> GetUserAsync(Guid id)
    {
        var user = await _userRepository.GetByIdAsync(id);

        if (user is null)
        {
            return Errors.User.NotFound;
        }

        return user;
    }

    public async Task<ErrorOr<User>> GetUserAsync(string username)
    {
        var user = await _userRepository.GetByUsernameAsync(username);

        if (user is null)
        {
            return Errors.User.NotFound;
        }

        return user;
    }

    public bool VerifyPassword(string password, string hashedPassword)
    {
        return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
    }

    private static string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }
}
