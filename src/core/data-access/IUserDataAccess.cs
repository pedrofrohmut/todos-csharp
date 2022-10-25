using Todos.Core.Dtos;

namespace Todos.Core.DataAccess;

public interface IUserDataAccess
{
    void Create(CreateUserDto newUser, string passwordHash);
    Task CreateAsync(CreateUserDto newUser, string passwordHash);
    UserDbDto? FindByEmail(string email);
    Task<UserDbDto?> FindByEmailAsync(string email);
    UserDbDto? FindById(string userId);
    Task<UserDbDto?> FindByIdAsync(string userId);
}
