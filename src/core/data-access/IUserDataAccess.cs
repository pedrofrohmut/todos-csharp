using Todos.Core.Dtos;

namespace Todos.Core.DataAccess;

public interface IUserDataAccess
{
    // Sync
    void Create(CreateUserDto newUser, string passwordHash);
    UserDbDto? FindByEmail(string email);
    UserDbDto? FindById(string userId);
    // Async
    Task CreateAsync(CreateUserDto newUser, string passwordHash);
    Task<UserDbDto?> FindByEmailAsync(string email);
    Task<UserDbDto?> FindByIdAsync(string userId);
}
