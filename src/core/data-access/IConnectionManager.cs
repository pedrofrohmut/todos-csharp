using System.Data;

namespace Todos.Core.DataAccess;

public interface IConnectionManager
{
    IDbConnection GetConnection();
    void OpenConnection(IDbConnection connection);
    void CloseConnection(IDbConnection connection);
}
