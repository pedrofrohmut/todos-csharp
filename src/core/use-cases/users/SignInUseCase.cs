using Todos.Core.DataAccess;
using Todos.Core.Dtos;
using Todos.Core.Entities;
using Todos.Core.Exceptions;
using Todos.Core.Services;

namespace Todos.Core.UseCases.Users;

public class SignInUseCase : ISignInUseCase
{
    private readonly IUserDataAccess userDataAccess;
    private readonly IPasswordService passwordService;
    private readonly ITokenService tokenService;

    public SignInUseCase(
            IUserDataAccess userDataAccess,
            IPasswordService passwordService,
            ITokenService tokenService)
    {
        this.userDataAccess = userDataAccess;
        this.passwordService = passwordService;
        this.tokenService = tokenService;
    }

    public SignedUserDto Execute(UserCredentialsDto? credentials)
    {
        this.ValidateCredentials(credentials);
        var user = this.FindUser(credentials!.Email);
        this.VerifyPasswordMatch(credentials!.Password, user.PasswordHash);
        var token = this.GenerateToken(user.Id.ToString());
        return new SignedUserDto() {
            UserId = user.Id.ToString(),
            Name   = user.Name,
            Email  = user.Email,
            Token  = token
        };
    }

    private void ValidateCredentials(UserCredentialsDto? credentials)
    {
        if (credentials == null) {
            throw new InvalidUserException("Request Body is null");
        }
        User.ValidateEmail(credentials.Email);
        User.ValidatePassword(credentials.Password);
    }

    private UserDbDto FindUser(string email)
    {
        var user = this.userDataAccess.FindByEmail(email);
        if (user == null) {
            throw new UserNotFoundException();
        }
        return user;
    }

    private async Task<UserDbDto> FindUserAsync(string email)
    {
        var user = await this.userDataAccess.FindByEmailAsync(email);
        if (user == null) {
            throw new UserNotFoundException();
        }
        return user;
    }

    private void VerifyPasswordMatch(string password, string hash)
    {
        var isMatch = this.passwordService.MatchPasswordAndHash(password, hash);
        if (! isMatch) {
            throw new PasswordAndHashNotMatchException();
        }
    }

    private string GenerateToken(string userId) => this.tokenService.GenerateToken(userId);

    public async Task<SignedUserDto> ExecuteAsync(UserCredentialsDto? credentials)
    {
        this.ValidateCredentials(credentials);
        var user = await this.FindUserAsync(credentials!.Email);
        this.VerifyPasswordMatch(credentials!.Password, user.PasswordHash);
        var token = this.GenerateToken(user.Id.ToString());
        return new SignedUserDto() {
            UserId = user.Id.ToString(),
            Name   = user.Name,
            Email  = user.Email,
            Token  = token
        };
    }
}
