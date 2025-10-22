using Todos.Core.Utils;
using Todos.Core.Services;
using Todos.Core.Commands;
using Todos.Core.Commands.Handlers;
using Todos.Core.Queries;
using Todos.Core.Queries.Handlers;
using Todos.Core.Db;
using Todos.Core.Errors;

namespace Todos.Core.Entities;

public static class UserEntity
{
    public static Result ValidateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name)) {
            return Result.Failed(new InvalidUserError("Name not provided. Name is required and cannot be blank."));
        }
        if (name.Length < 3) {
            return Result.Failed(new InvalidUserError("Name is too short. Name must be at least 3 characters long."));
        }
        if (name.Length > 120) {
            return Result.Failed(new InvalidUserError("Name is too long. Name must be less than 121 characters long."));
        }
        return Result.Successed();
    }

    public static Result ValidateEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email)) {
            return Result.Failed(new InvalidUserError("E-mail not provided. E-mail is required and cannot be blank."));
        }
        if (!email.Contains('@') || !email.Contains('.')) { // Just as example
            return Result.Failed(new InvalidUserError("E-mail is invalid. E-mail must contain the characters @ and . to be valid."));
        }
        if (email.Length < 6) {
            return Result.Failed(new InvalidUserError("E-mail is too short. E-mail must be at least 6 characters long."));
        }
        return Result.Successed();
    }

    public static Result ValidatePassword(string password)
    {
        if (string.IsNullOrWhiteSpace(password)) {
            return Result.Failed(new InvalidUserError("Password not provided. Password is required and cannot be blank"));
        }
        if (password.Length < 3) {
            return Result.Failed(new InvalidUserError("Password is too short. Password must be at least 3 characters long."));
        }
        if (password.Length > 32) {
            return Result.Failed(new InvalidUserError("Password is too long. Password must less than 33 characters long."));
        }
        return Result.Successed();
    }

    public static async Task<Result> CheckEmailIsAvailable(UserFindByEmailQuery query, IUserQueryHandler handler)
    {
        try {
            var userFound = await handler.FindUserByEmail(query);
            if (userFound != null) {
                // return Result.Failed("User.EmailUnavailable", "This e-mail is already in use. E-mails must be unique.");
                return Result.Failed(new EmailAlreadyTakenError());
            }
            return Result.Successed();
        } catch (Exception e) {
            return Result.Failed("User:" + nameof(CheckEmailIsAvailable), "Failed to find user by e-mail: " + e.Message);
        }
    }

    public static Result<string> HashPassword(string password, IPasswordService passwordService)
    {
        try {
            string hash = passwordService.HashPassword(password);
            return Result<string>.Successed(hash);
        } catch (Exception e) {
            return Result<string>.Failed("User:" + nameof(HashPassword), "Error to create a password hash: " + e.Message);
        }
    }

    public static async Task<Result> CreateUser(CreateUserCommand command, IUserCommandHandler handler)
    {
        try {
            await handler.CreateUser(command);
            return Result.Successed();
        } catch (Exception e) {
            return Result.Failed("User:" + nameof(CreateUser), "Error to create user: " + e.Message);
        }
    }

    public static async Task<Result<UserDb>> FindUserByEmail(UserFindByEmailQuery query, IUserQueryHandler handler)
    {
        try {
            UserDb? user = await handler.FindUserByEmail(query);
            if (user == null) {
                return Result<UserDb>.Failed(new UserNotFoundError("User not found by e-mail"));
            }
            return Result<UserDb>.Successed(user.Value);
        } catch (Exception e) {
            return Result<UserDb>.Failed("User:" + nameof(FindUserByEmail), "Error to find user by email: " + e.Message);
        }
    }

    public static Result MatchPasswordAndHash(string password, string hash, IPasswordService passwordService)
    {
        try {
            bool isMatch = passwordService.CheckPassword(password, hash);
            if (!isMatch) {
                return Result.Failed(new PasswordMatchError());
            }
            return Result.Successed();
        } catch (Exception e) {
            return Result.Failed("User:" + nameof(MatchPasswordAndHash), "Error to match password: " + e.Message);
        }
    }
}
