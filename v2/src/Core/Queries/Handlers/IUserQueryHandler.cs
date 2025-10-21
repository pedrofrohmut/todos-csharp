using Todos.Core.Db;

namespace Todos.Core.Queries.Handlers;

public interface IUserQueryHandler
{
    Task<UserDb?> FindUserByEmail(UserFindByEmailQuery query);
}
