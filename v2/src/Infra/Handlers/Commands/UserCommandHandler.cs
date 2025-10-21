using Todos.Core.Commands;
using Todos.Core.Commands.Handlers;

namespace Todos.Infra.Handlers.Commands;

public class UserCommandHandler : IUserCommandHandler
{
    public Task CreateUser(CreateUserCommand command)
    {
        throw new NotImplementedException();
    }
}
