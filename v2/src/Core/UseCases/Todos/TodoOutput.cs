using Todos.Core.Db;

namespace Todos.Core.UseCases.Todos;

public readonly struct TodoOutput
{
    public int Id { get; init; }
    public string Name { get; init; }
    public string Description { get; init; }

    public TodoOutput(TodoDb todoDb)
    {
        this.Id = todoDb.Id;
        this.Name = todoDb.Name;
        this.Description = todoDb.Description;
    }
}
