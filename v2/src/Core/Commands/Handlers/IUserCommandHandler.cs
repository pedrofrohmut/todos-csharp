namespace Todos.Core.Commands.Handlers;

public interface IUserCommandHandler
{
    Task CreateUser(CreateUserCommand command);
}
