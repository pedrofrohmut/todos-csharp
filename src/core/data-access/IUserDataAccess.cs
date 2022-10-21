using Todos.Core.Dtos;

namespace Todos.Core.DataAccess;

public interface IUserDataAccess
{
    void Create(CreateUserDto newUser, string passwordHash);
    UserDbDto? FindByEmail(string email);
    UserDbDto? FindById(string userId);
}
