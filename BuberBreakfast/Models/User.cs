using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BuberBreakfast.Contracts.User;
using BuberBreakfast.ServiceErrors;
using ErrorOr;

namespace BuberBreakfast.Models;

public class User
{
    public const int MinUserNameLength = 3;
    public const int MaxUserNameLength = 50;

    [Key]
    public Guid Id { get; private set; }
    public string UserName { get; private set; }
    public string Password { get; private set; }
    
    [InverseProperty("User")]
    public List<Breakfast> Breakfasts { get; private set; } = new List<Breakfast>();

    private User(
        Guid id,
        string userName,
        string password)
    {
        Id = id;
        UserName = userName;
        Password = password;
    }

    public static ErrorOr<User> Create(
      string userName,
      string password,
      Guid? id = null)
    {
        List<Error> errors = new();

        if (userName.Length is < MinUserNameLength or > MaxUserNameLength)
        {
            errors.Add(Errors.User.InvalidUserName);
        }

        if (errors.Count > 0)
        {
            return errors;
        }

        return new User(
            id ?? Guid.NewGuid(),
            userName,
            password);
    }

    public static ErrorOr<User> From(CreateUserRequest request)
    {
        return Create(
            request.UserName,
            request.Password);
    }

    public void UpdateUsername(UpdateUserRequest request)
    {
        UserName = request.UserName;
    }
    
    public void SetPassword(string password)
    {
        Password = password;
    }
}