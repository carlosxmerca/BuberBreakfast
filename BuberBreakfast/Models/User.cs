namespace BuberBreakfast.Models;

public class User
{
    public Guid Id { get; }
    public string UserName { get; }
    public string Password { get; }


    public User(
        string userName,
        string password)
    {
        Id = Guid.NewGuid();
        UserName = userName;
        Password = password;
    }
}