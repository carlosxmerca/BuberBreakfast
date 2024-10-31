using BuberBreakfast.Contracts.User;
using Entity = BuberBreakfast.Models;
using BuberBreakfast.Services.Users;
using ErrorOr;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BuberBreakfast.Controllers;

public class UsersController : ApiController
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    private IActionResult CreatedAtGetUser(Entity.User user)
    {
        return CreatedAtAction(
                    actionName: nameof(GetUser),
                    routeValues: new { id = user.Id },
                    value: MapUserResponse(user));
    }

    private static UserResponse MapUserResponse(Entity.User user)
    {
        return new UserResponse(
            user.Id,
            user.UserName
        );
    }

    [Authorize]
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetUser(Guid id)
    {
        ErrorOr<Entity.User> getUserResult = await _userService.GetUserAsync(id);

        return getUserResult.Match(
            user => Ok(MapUserResponse(user)),
            errors => Problem(errors));
    }
    
    [HttpPost()]
    public async Task<IActionResult> CreateUser(CreateUserRequest request)
    {
        ErrorOr<Entity.User> createUser = Entity.User.From(request);

        if (createUser.IsError)
        {
            return Problem(createUser.Errors);
        }

        var user = createUser.Value;
        ErrorOr<Created> createUserResult = await _userService.CreateUserAsync(user);

        return createUserResult.Match(
           created => CreatedAtGetUser(user),
           errors => Problem(errors));
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateUser(Guid id, UpdateUserRequest request)
    {
        ErrorOr<Entity.User> getUser = await _userService.GetUserAsync(id);

        if (getUser.IsError)
        {
            return Problem(getUser.Errors);
        }

        var user = getUser.Value;
        user.UpdateUsername(request);

        ErrorOr<Updated> updateUserResult = await _userService.UpdateUserAsync(user);

        return updateUserResult.Match(
           updated => Ok(),
           errors => Problem(errors));
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteUser(Guid id)
    {
        ErrorOr<Deleted> deleteUserResult = await _userService.DeleteUserAsync(id);

        return deleteUserResult.Match(
             deleted => NoContent(),
             errors => Problem(errors));
    }
}
