using Todos.Core.Dtos;

namespace Todos.Core.UseCases.Users;

public interface ISignUpUseCase
{
    void Execute(CreateUserDto newUser);
}
