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
    public static Result<bool> ValidateId(int id)
    {
        if (id < 1) {
            return Result<bool>.Fail(new InvalidUserError("Invalid user id. Id cannot be less than 1"));
        }
        return Result<bool>.Ok();
    }

    public static Result<bool> ValidateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name)) {
            return Result<bool>.Fail(new InvalidUserError("Name not provided. Name is required and cannot be blank."));
        }
        if (name.Length < 3) {
            return Result<bool>.Fail(new InvalidUserError("Name is too short. Name must be at least 3 characters long."));
        }
        if (name.Length > 120) {
            return Result<bool>.Fail(new InvalidUserError("Name is too long. Name must be less than 121 characters long."));
        }
        if (!EntitiesUtils.IsValidName(name)) {
            return Result<bool>.Fail(new InvalidUserError("Name contains invalid characters."));
        }
        return Result<bool>.Ok();
    }

    public static Result<bool> ValidateEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email)) {
            return Result<bool>.Fail(new InvalidUserError("E-mail not provided. E-mail is required and cannot be blank."));
        }
        if (email.Length < 6) {
            return Result<bool>.Fail(new InvalidUserError("E-mail is too short. E-mail must be at least 6 characters long."));
        }
        if (!EntitiesUtils.IsValidEmail(email)) {
            return Result<bool>.Fail(new InvalidUserError("E-mail is invalid."));
        }
        return Result<bool>.Ok();
    }

    public static Result<bool> ValidatePassword(string password)
    {
        if (string.IsNullOrWhiteSpace(password)) {
            return Result<bool>.Fail(new InvalidUserError("Password not provided. Password is required and cannot be blank"));
        }
        if (password.Length < 3) {
            return Result<bool>.Fail(new InvalidUserError("Password is too short. Password must be at least 3 characters long."));
        }
        if (password.Length > 32) {
            return Result<bool>.Fail(new InvalidUserError("Password is too long. Password must less than 33 characters long."));
        }
        return Result<bool>.Ok();
    }

    public static Result<bool> ValidateEncodedToken(string? token)
    {
        if (string.IsNullOrWhiteSpace(token)) {
            return Result<bool>.Fail(new InvalidTokenError("Authentication token not provided. Token is required and cannot be blank"));
        }
        return Result<bool>.Ok();
    }

    public static async Task<Result<bool>> CheckEmailIsAvailable(UserFindByEmailQuery query, IUserQueryHandler handler)
    {
        try {
            UserDb? userFound = await handler.FindUserByEmail(query);
            if (userFound != null || userFound?.Id > 0) {
                return Result<bool>.Fail(new EmailAlreadyTakenError());
            }
            return Result<bool>.Ok();
        } catch (Exception e) {
            return Result<bool>.Fail("User:" + nameof(CheckEmailIsAvailable), "Failed to find user by e-mail: " + e.Message);
        }
    }

    public static Result<string> HashPassword(string password, IPasswordService passwordService)
    {
        try {
            string hash = passwordService.HashPassword(password);
            if (string.IsNullOrWhiteSpace(hash)) {
                throw new ArgumentNullException();
            }
            return Result<string>.Ok(hash);
        } catch (Exception e) {
            return Result<string>.Fail("User:" + nameof(HashPassword), "Error to create a password hash: " + e.Message);
        }
    }

    public static async Task<Result<bool>> CreateUser(CreateUserCommand command, IUserCommandHandler handler)
    {
        try {
            await handler.CreateUser(command);
            return Result<bool>.Ok();
        } catch (Exception e) {
            return Result<bool>.Fail("User:" + nameof(CreateUser), "Error to create user: " + e.Message);
        }
    }

    public static async Task<Result<UserDb>> FindUserByEmail(UserFindByEmailQuery query, IUserQueryHandler handler)
    {
        try {
            UserDb? user = await handler.FindUserByEmail(query);
            if (user == null) {
                return Result<UserDb>.Fail(new UserNotFoundError("User not found by e-mail"));
            }
            return Result<UserDb>.Ok(user.Value);
        } catch (Exception e) {
            return Result<UserDb>.Fail("User:" + nameof(FindUserByEmail), "Error to find user by email: " + e.Message);
        }
    }

    public static Result<bool> MatchPasswordAndHash(string password, string hash, IPasswordService passwordService)
    {
        try {
            bool isMatch = passwordService.CheckPassword(password, hash);
            if (!isMatch) {
                return Result<bool>.Fail(new PasswordMatchError());
            }
            return Result<bool>.Ok();
        } catch (Exception e) {
            return Result<bool>.Fail("User:" + nameof(MatchPasswordAndHash), "Error to match password: " + e.Message);
        }
    }

    public static Result<AuthToken> DecodeToken(string token, IAuthTokenService authTokenService)
    {
        try {
            var resultDecoded = authTokenService.Decode(token);
            if (!resultDecoded.IsSuccess) {
                return Result<AuthToken>.Fail(new InvalidTokenError("The token is invalid and could not be decoded"));
            }
            return Result<AuthToken>.Ok(resultDecoded.Payload);
        } catch (Exception e) {
            return Result<AuthToken>.Fail("User:" + nameof(DecodeToken), "Error to decode token: " + e.Message);
        }
    }

    public static Result<bool> ValidateExpiration(long expiration)
    {
        long now = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        if (now >= expiration) {
            return Result<bool>.Fail(new InvalidTokenError("Token is expired"));
        }
        return Result<bool>.Ok();
    }

    public static Result<string> GenerateAuthToken(int userId, IAuthTokenService authTokenService)
    {
        try {
            return authTokenService.GenerateJWT(userId);
        } catch (Exception e) {
            return Result<string>.Fail("User:" + nameof(GenerateAuthToken), "Error to generate auth token: " + e.Message);
        }
    }

    public static async Task<Result<bool>> CheckUserExists(UserFindByIdQuery query, IUserQueryHandler userQueryHandler)
    {
        try {
            UserDb? userDb = await userQueryHandler.FindUserById(query);
            if (userDb == null) {
                return Result<bool>.Fail(new UserNotFoundError("User not found by id"));
            }
            return Result<bool>.Ok();
        } catch (Exception e) {
            return Result<bool>.Fail("User:" + nameof(CheckUserExists), "Error to check if user exists: " + e.Message);
        }
    }

    public static async Task<Result<UserDb>> GetUserFromToken(string? token,
                                                              IAuthTokenService authTokenService,
                                                              IUserQueryHandler userQueryHandler)
    {
        if (string.IsNullOrWhiteSpace(token)) {
            return Result<UserDb>.Fail(new InvalidTokenError("Cannot get user from token. Token not provided."));
        }

        AuthToken decoded;
        try {
            var resultDecoded = authTokenService.Decode(token);
            if (!resultDecoded.IsSuccess) {
                return Result<UserDb>.Fail(new InvalidTokenError("The token is invalid and could not be decoded"));
            }
            decoded = resultDecoded.Payload;
        } catch (Exception e) {
            return Result<UserDb>.Fail("User:" + nameof(GetUserFromToken), "Error to decode token: " + e.Message);
        }

        var resultId = UserEntity.ValidateId(decoded.UserId);
        if (!resultId.IsSuccess) {
            return Result<UserDb>.Fail(new InvalidTokenError("User id in the token is not valid"));
        }

        try {
            var query = new UserFindByIdQuery { Id = decoded.UserId };
            var user = await userQueryHandler.FindUserById(query);
            if (user == null) {
                return Result<UserDb>.Fail(new InvalidTokenError("User not found by token's userId"));
            }
            return Result<UserDb>.Ok(user.Value);
        } catch (Exception e) {
            return Result<UserDb>.Fail("User:" + nameof(GetUserFromToken), "Error to find user by id: " + e.Message);
        }
    }
}
