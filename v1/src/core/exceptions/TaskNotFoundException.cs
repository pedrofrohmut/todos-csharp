namespace Todos.Core.Exceptions;

public class TaskNotFoundException : Exception
{
    public TaskNotFoundException() : base("Task not found") {}

    public TaskNotFoundException(string message) : base(message) {}
}
