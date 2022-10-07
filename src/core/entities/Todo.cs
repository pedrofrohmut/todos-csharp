using Todos.Core.Exceptions;

namespace Todos.Core.Entities;

public class Todo
{
    
    public static void ValidateName(string name)
    {
        if (String.IsNullOrWhiteSpace(name)) {
            throw new InvalidTodoException("Todo name is required and cannot be blank");
        }
        if (name.Length < 5 || name.Length > 120) {
            throw new InvalidTodoException("Name must be between 5 and 120 characters");
        }
    }

    public static void ValidateDescription(string description)
    {
        if (String.IsNullOrWhiteSpace(description)) {
            throw new InvalidTodoException("Todo description is required and cannot be blank");
        }
        if (description.Length < 5 || description.Length > 255) {
            throw new InvalidTodoException("Description must be between 5 and 255 characters");
        }
    }
}
