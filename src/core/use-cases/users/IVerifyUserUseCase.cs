namespace Todos.Core.UseCases.Users;

public interface IVerifyUserUseCase
{
    void Execute(string? authUserId);
    Task ExecuteAsync(string? authUserId);
}
