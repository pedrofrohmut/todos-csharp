using Todos.Core.Entities;
using Todos.Core.Queries;
using Todos.Core.Queries.Handlers;
using Todos.Core.Services;
using Todos.Core.Utils;

namespace Todos.Core.UseCases.Users;

public readonly struct VerifyAuthTokenInput
{
    public string? AuthToken { get; init; }
}

public readonly struct VerifyAuthTokenOutput
{
    public bool IsValid { get; init; }
}

public enum VerifyAuthTokenErrors
{
    InvalidToken,
    UserNotFound,
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

    public async Task<Result<VerifyAuthTokenOutput>> Execute(VerifyAuthTokenInput input)
    {
        // Get decoded token
        Result<AuthToken> tokenResult = UserEntity.GetAuthToken(input.AuthToken, this.authTokenService);
        if (!tokenResult.IsSuccess) {
            return tokenResult.ErrorCast<VerifyAuthTokenOutput>(VerifyAuthTokenErrors.InvalidToken);
        }
        AuthToken authToken = tokenResult.Payload;

        // Validate token
        Result validationResult = UserEntity.ValidateAuthToken(authToken);
        if (!validationResult.IsSuccess) {
            return validationResult.ErrorCast<VerifyAuthTokenOutput>(VerifyAuthTokenErrors.InvalidToken);
        }

        // Check user exists by id
        var query = new UserFindByIdQuery { Id = authToken.UserId };
        Result checkResult = await UserEntity.CheckUserExists(query, this.userQueryHandler);
        if (!checkResult.IsSuccess) {
            return checkResult.ErrorCast<VerifyAuthTokenOutput>(VerifyAuthTokenErrors.UserNotFound);
        }

        return Result<VerifyAuthTokenOutput>.Ok(new VerifyAuthTokenOutput { IsValid = true });
    }
}
