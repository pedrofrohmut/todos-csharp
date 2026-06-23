using Todos.Core.Entities;
using Todos.Core.Queries;
using Todos.Core.Queries.Handlers;
using Todos.Core.Services;
using Todos.Core.Utils;

namespace Todos.Core.UseCases.Users;

public readonly struct VerifyAuthTokenInput
{
    public string? Token { get; init; }
}

public readonly struct VerifyAuthTokenOutput
{
    public bool IsValid { get; init; }
}

public class VerifyAuthTokenUseCase
{
    private readonly IAuthTokenService authTokenService;
    private readonly IUserQueryHandler userQueryHandler;

    public VerifyAuthTokenUseCase(IAuthTokenService authTokenService, IUserQueryHandler userQueryHandler)
    {
        this.authTokenService = authTokenService;
        this.userQueryHandler = userQueryHandler;
    }

    private Result<VerifyAuthTokenOutput> ErrorCast<T>(Result<T> result)
    {
        return Result<VerifyAuthTokenOutput>.Fail(result.Error);
    }

    public async Task<Result<VerifyAuthTokenOutput>> Execute(VerifyAuthTokenInput input)
    {
        // Validate input
        Result<bool> validationResult = UserEntity.ValidateEncodedToken(input.Token);
        if (!validationResult.IsSuccess || input.Token == null) {
            return ErrorCast(validationResult);
        }

        // Decode token
        Result<AuthToken> decodeResult = UserEntity.DecodeToken(input.Token, authTokenService);
        if (!decodeResult.IsSuccess) {
            return Result<VerifyAuthTokenOutput>.Fail(decodeResult.Error);
        }

        // Validate token information
        int userId = decodeResult.Payload.UserId;
        validationResult = UserEntity.ValidateId(userId);
        if (!validationResult.IsSuccess) {
            return ErrorCast(validationResult);
        }

        // Check user exists by id
        var query = new UserFindByIdQuery { Id = userId };
        Result<bool> checkResult = await UserEntity.CheckUserExists(query, userQueryHandler);
        if (!checkResult.IsSuccess) {
            return ErrorCast(checkResult);
        }

        var validOutput = new VerifyAuthTokenOutput { IsValid = true };
        return Result<VerifyAuthTokenOutput>.Ok(validOutput);
    }
}
