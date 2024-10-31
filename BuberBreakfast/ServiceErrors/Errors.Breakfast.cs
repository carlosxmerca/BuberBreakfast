using ErrorOr;

namespace BuberBreakfast.ServiceErrors;

public static class Errors
{
    public static class Breakfast
    {
        public static Error InvalidName => Error.Validation(
            code: "Breakfast.InvalidName",
            description: $"Breakfast name must be at least {Models.Breakfast.MinNameLength}" +
                $" characters long and at most {Models.Breakfast.MaxNameLength} characters long.");

        public static Error InvalidDescription => Error.Validation(
            code: "Breakfast.InvalidDescription",
            description: $"Breakfast description must be at least {Models.Breakfast.MinDescriptionLength}" +
                $" characters long and at most {Models.Breakfast.MaxDescriptionLength} characters long.");

        public static Error NotFound => Error.NotFound(
            code: "Breakfast.NotFound",
            description: "Breakfast not found");
    }

    public static class User
    {
        public static Error InvalidUserName => Error.Validation(
                   code: "User.InvalidUserName",
                   description: "Username must be at least 3 characters long and at most 20 characters long.");

        public static Error InvalidPassword => Error.Validation(
            code: "User.InvalidPassword",
            description: "Password must be at least 6 characters long.");

        public static Error NotFound => Error.NotFound(
            code: "User.NotFound",
            description: "User not found");

        public static Error DuplicateUserName => Error.Conflict(
            code: "User.DuplicateUserName",
            description: "The username provided is already in use.");
    }
}
