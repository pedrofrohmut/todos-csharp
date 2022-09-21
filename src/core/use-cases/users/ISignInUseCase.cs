using Todos.Core.Dtos;

namespace Todos.Core.UseCases.Users;

public interface ISignInUseCase
{
    SignedUserDto Execute(UserCredentialsDto credentials);
}
