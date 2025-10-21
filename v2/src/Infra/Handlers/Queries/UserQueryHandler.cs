using Todos.Core.Db;
using Todos.Core.Queries;
using Todos.Core.Queries.Handlers;

namespace Todos.Infra.Handlers.Queries;

public class UserQueryHandler : IUserQueryHandler
{
    public Task<UserDb?> FindUserByEmail(UserFindByEmailQuery query)
    {
        throw new NotImplementedException();
    }
}
