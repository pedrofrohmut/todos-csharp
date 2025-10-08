using Todos.Core.Exceptions;

namespace Todos.Core.Entities;

public class Task
{
    public static void ValidateId(string? id)
    {
        if (id == null) {
            throw new InvalidTaskException("Task id cannot be null");
        }
        var canBeParsed = Guid.TryParse(id, out var output);
        if (! canBeParsed) {
            throw new InvalidTaskException("Task id is not a valid Guid");
        }
    }
    
    public static void ValidateName(string name)
    {
        if (String.IsNullOrWhiteSpace(name)) {
            throw new InvalidTaskException("Task name is required and cannot be blank");
        }
        if (name.Length < 5 || name.Length > 120) {
            throw new InvalidTaskException("Name must be between 5 and 120 characters");
        }
    }

    public static void ValidateDescription(string description)
    {
        if (String.IsNullOrWhiteSpace(description)) {
            throw new InvalidTaskException("Task description is required and cannot be blank");
        }
        if (description.Length < 5 || description.Length > 255) {
            throw new InvalidTaskException("Description must be between 5 and 255 characters");
        }
    }
}
