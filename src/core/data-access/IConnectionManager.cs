using System.Data;
using Microsoft.Extensions.Configuration;

namespace Todos.Core.DataAccess;

public interface IConnectionManager
{
    IDbConnection GetConnection(IConfiguration configuration);
    void OpenConnection(IDbConnection? connection);
    void CloseConnection(IDbConnection? connection);
}
