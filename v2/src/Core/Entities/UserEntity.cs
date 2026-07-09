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
    public static Result ValidateId(int id)
    {
        if (id < 1) {
            return Result.Fail(new InvalidUserError("Invalid user id. Id cannot be less than 1"));
        }
        return Result.Ok();
    }

    public static Result ValidateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name)) {
            return Result.Fail(new InvalidUserError("Name not provided. Name is required and cannot be blank."));
        }
        if (name.Length < 3) {
            return Result.Fail(new InvalidUserError("Name is too short. Name must be at least 3 characters long."));
        }
        if (name.Length > 120) {
            return Result.Fail(new InvalidUserError("Name is too long. Name must be less than 121 characters long."));
        }
        if (!EntitiesUtils.IsValidName(name)) {
            return Result.Fail(new InvalidUserError("Name contains invalid characters."));
        }
        return Result.Ok();
    }

    public static Result ValidateEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email)) {
            return Result.Fail(new InvalidUserError("E-mail not provided. E-mail is required and cannot be blank."));
        }
        if (email.Length < 6) {
            return Result.Fail(new InvalidUserError("E-mail is too short. E-mail must be at least 6 characters long."));
        }
        if (!EntitiesUtils.IsValidEmail(email)) {
            return Result.Fail(new InvalidUserError("E-mail is invalid."));
        }
        return Result.Ok();
    }

    public static Result ValidatePassword(string password)
    {
        if (string.IsNullOrWhiteSpace(password)) {
            return Result.Fail(new InvalidUserError("Password not provided. Password is required and cannot be blank"));
        }
        if (password.Length < 3) {
            return Result.Fail(new InvalidUserError("Password is too short. Password must be at least 3 characters long."));
        }
        if (password.Length > 32) {
            return Result.Fail(new InvalidUserError("Password is too long. Password must less than 33 characters long."));
        }
        return Result.Ok();
    }

    public static Result<AuthToken> GetAuthToken(string? jwt, IAuthTokenService authTokenService)
    {
        if (string.IsNullOrWhiteSpace(jwt)) {
            return Result<AuthToken>.Fail(
                new InvalidTokenError("Authentication token not provided. Token is required and cannot be blank"));
        }
        Result<AuthToken> decodeResult = authTokenService.Decode(jwt);
        if (!decodeResult.IsSuccess) {
            return decodeResult;
        }
        return decodeResult;
    }

    public static Result ValidateAuthToken(AuthToken token)
    {
        if (token.UserId == 0 || token.Expiration == 0 || token.IssuedAt == 0) {
            return Result.Fail(new InvalidTokenError("Invalid token. Token doesnt have enough information in the payload. Or the information is incorrect."));
        }
        Result validationResult = ValidateId(token.UserId);
        if (!validationResult.IsSuccess) {
            return Result.Fail(new InvalidTokenError("Invalid token. Token userId is not valid"));
        }
        return Result.Ok();
    }

    public static async Task<Result> CheckEmailIsAvailable(UserFindByEmailQuery query, IUserQueryHandler handler)
    {
        try {
            UserDb? userFound = await handler.FindUserByEmail(query);
            if (userFound != null || userFound?.Id > 0) {
                return Result.Fail(new EmailAlreadyTakenError());
            }
            return Result.Ok();
        } catch (Exception e) {
            return Result.Fail("User:" + nameof(CheckEmailIsAvailable), "Failed to find user by e-mail: " + e.Message);
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

    public static async Task<Result> CreateUser(CreateUserCommand command, IUserCommandHandler handler)
    {
        try {
            await handler.CreateUser(command);
            return Result.Ok();
        } catch (Exception e) {
            return Result.Fail("User:" + nameof(CreateUser), "Error to create user: " + e.Message);
        }
    }

    public static async Task<Result<UserDb>> FindUserById(UserFindByIdQuery query, IUserQueryHandler handler)
    {
        try {
            UserDb? user = await handler.FindUserById(query);
            if (user == null) {
                return Result<UserDb>.Fail(new UserNotFoundError("User not found by id"));
            }
            return Result<UserDb>.Ok(user.Value);
        } catch (Exception e) {
            return Result<UserDb>.Fail("User:" + nameof(FindUserById), "Error to find user by id: " + e.Message);
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

    public static Result MatchPasswordAndHash(string password, string hash, IPasswordService passwordService)
    {
        try {
            bool isMatch = passwordService.CheckPassword(password, hash);
            if (!isMatch) {
                return Result.Fail(new PasswordMatchError());
            }
            return Result.Ok();
        } catch (Exception e) {
            return Result.Fail("User:" + nameof(MatchPasswordAndHash), "Error to match password: " + e.Message);
        }
    }

    public static Result<string> GenerateAuthToken(int userId, IAuthTokenService authTokenService)
    {
        try {
            return authTokenService.GenerateJWT(userId);
        } catch (Exception e) {
            return Result<string>.Fail("User:" + nameof(GenerateAuthToken), "Error to generate auth token: " + e.Message);
        }
    }

    public static async Task<Result> CheckUserExists(UserFindByIdQuery query, IUserQueryHandler userQueryHandler)
    {
        try {
            UserDb? userDb = await userQueryHandler.FindUserById(query);
            if (userDb == null) {
                return Result.Fail(new UserNotFoundError("User not found by id"));
            }
            return Result.Ok();
        } catch (Exception e) {
            return Result.Fail("User:" + nameof(CheckUserExists), "Error to check if user exists: " + e.Message);
        }
    }

    [Obsolete("GetUserFromToken is deprecated. Use GetAuthToken instead. You can also validate the token with ValidateAuthToken.")]
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
