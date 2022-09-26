using Todos.Core.Dtos;

namespace Todos.Core.DataAccess;

public interface IUserDataAccess
{
    void CreateUser(CreateUserDto newUser, string passwordHash);
    UserDbDto? FindUserByEmail(string email);
    UserDbDto? FindUserById(string userId);
}
