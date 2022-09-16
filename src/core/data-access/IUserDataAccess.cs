using Todos.Core.Dtos;

namespace Todos.Core.DataAccess;

public interface IUserDataAccess
{
    void CreateUser(CreateUserDto newUser);
    UserDbDto FindUserByEmail(string email);
}
