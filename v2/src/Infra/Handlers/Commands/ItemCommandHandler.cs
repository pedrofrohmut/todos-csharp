using System.Data;
using Todos.Core.Commands;
using Todos.Core.Commands.Handlers;

namespace Todos.Infra.Handlers.Commands;

public class ItemCommandHandler : IItemCommandHandler
{
    private readonly IDbConnection writeConnection;
    private readonly IDbConnection readConnection;

    public ItemCommandHandler(IDbConnection writeConnection, IDbConnection readConnection)
    {
        this.writeConnection = writeConnection;
        this.readConnection = readConnection;
    }

    public async Task CreateItem(ItemCreateCommand command)
    {
        throw new NotImplementedException();
    }

    public async Task UpdateItem(ItemUpdateCommand command)
    {
        throw new NotImplementedException();
    }

    public async Task DeleteItem(ItemDeleteCommand command)
    {
        throw new NotImplementedException();
    }
}
