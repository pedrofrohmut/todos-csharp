using Todos.Core.Db;

namespace Todos.Core.UseCases.Items;

public readonly struct ItemOutput
{
    public int Id { get; init; }
    public string Name { get; init; }
    public string Description { get; init; }
    public bool IsDone { get; init; }

    public ItemOutput(ItemDb itemDb)
    {
        this.Id = itemDb.Id;
        this.Name = itemDb.Name;
        this.Description = itemDb.Description;
        this.IsDone = itemDb.IsDone;
    }
}
