using Todos.Core.DataAccess;
using Todos.Core.Dtos;
using Todos.Core.Entities;
using Todos.Core.Exceptions;
using Todos.Core.Services;
using Task = System.Threading.Tasks.Task;

namespace Todos.Core.UseCases.Users;

public class SignUpUseCase : ISignUpUseCase
{
    private readonly IUserDataAccess userDataAccess;
    private readonly IPasswordService passwordService;

    public SignUpUseCase(IUserDataAccess userDataAccess, IPasswordService passwordService)
    {
        this.userDataAccess = userDataAccess;
        this.passwordService = passwordService;
    }

    public void Execute(CreateUserDto? newUser)
    {
        this.ValidateNewUser(newUser);
        this.CheckEmailAlreadyTaken(newUser!.Email);
        var passwordHash = this.GetPasswordHash(newUser.Password);
        this.CreateUser(newUser, passwordHash);
    }

    private void ValidateNewUser(CreateUserDto? newUser)
    {
        if (newUser == null) {
            throw new InvalidUserException("Request Body is null");
        }
        User.ValidateName(newUser.Name);
        User.ValidateEmail(newUser.Email);
        User.ValidatePassword(newUser.Password);
    }

    private void CheckEmailAlreadyTaken(string email)
    {
        var user = this.userDataAccess.FindByEmail(email);
        if (user != null) {
            throw new EmailAlreadyTakenException();
        }
    }

    private async Task CheckEmailAlreadyTakenAsync(string email)
    {
        var user = await this.userDataAccess.FindByEmailAsync(email);
        if (user != null) {
            throw new EmailAlreadyTakenException();
        }
    }

    private string GetPasswordHash(string password) =>
        this.passwordService.HashPassword(password);

    private void CreateUser(CreateUserDto newUser, string passwordHash) =>
        this.userDataAccess.Create(newUser, passwordHash);

    private async Task CreateUserAsync(CreateUserDto newUser, string passwordHash) =>
        await this.userDataAccess.CreateAsync(newUser, passwordHash);

    public async Task ExecuteAsync(CreateUserDto? newUser)
    {
        this.ValidateNewUser(newUser);
        await this.CheckEmailAlreadyTakenAsync(newUser!.Email);
        var passwordHash = this.GetPasswordHash(newUser.Password);
        await this.CreateUserAsync(newUser, passwordHash);
    }
}
