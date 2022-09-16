using System.Data;

using Todos.Core.DataAccess;
using Todos.Core.Dtos;

namespace Todos.DataAccess.Users;

public class UserDataAccess : IUserDataAccess
{
    private readonly IDbConnection connection;

    public UserDataAccess(IDbConnection connection)
    {
        this.connection = connection;
    }

    public void CreateUser(CreateUserDto newUser)
    {
        throw new NotImplementedException();
    }

    public UserDbDto FindUserByEmail(string email)
    {
        throw new NotImplementedException();
    }
}
