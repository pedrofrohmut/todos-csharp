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

    public SignedUserDto Execute(UserCredentialsDto credentials)
    {
        this.ValidateCredentials(credentials);
        var user = this.GetUser(credentials.Email);
        this.VerifyPasswordMatch(credentials.Password, user.PasswordHash);
        var token = this.GenerateToken(user.Id);
        return new SignedUserDto() {
            UserId = user.Id,
            Name   = user.Name,
            Email  = user.Email,
            Token  = token
        };
    }

    private void ValidateCredentials(UserCredentialsDto credentials)
    {
        User.ValidateEmail(credentials.Email);
        User.ValidatePassword(credentials.Password);
    }

    private UserDbDto GetUser(string email)
    {
        var user = this.userDataAccess.FindUserByEmail(email);
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
}
