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

    public async Task<Result<VerifyAuthTokenOutput>> Execute(VerifyAuthTokenInput input)
    {
        Result<VerifyAuthTokenOutput> result;

        // Validate input
        result = (Result<VerifyAuthTokenOutput>) UserEntity.ValidateEncodedToken(input.Token);
        if (!result.IsSuccess) return result;

        // Decode token
        Result<AuthToken> resultDecoded = UserEntity.DecodeToken(input.Token!, authTokenService);
        if (!resultDecoded.IsSuccess) return Result<VerifyAuthTokenOutput>.Failed(resultDecoded.Error!);

        // Validate token information
        int userId = resultDecoded.Payload.UserId;
        result = (Result<VerifyAuthTokenOutput>) UserEntity.ValidateId(userId);
        if (!result.IsSuccess) return result;

        // Check user exists by id
        var query = new UserFindByIdQuery { Id = userId };
        result = (Result<VerifyAuthTokenOutput>) await UserEntity.CheckUserExists(query, userQueryHandler);
        if (!result.IsSuccess) return result;

        var validOutput = new VerifyAuthTokenOutput { IsValid = true };
        return Result<VerifyAuthTokenOutput>.Successed(validOutput);
    }
}
