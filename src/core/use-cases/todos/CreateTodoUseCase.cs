using Todos.Core.DataAccess;
using Todos.Core.Dtos;
using Todos.Core.Entities;
using Todos.Core.Exceptions;

namespace Todos.Core.UseCases.Todos;

public class CreateTodoUseCase : ICreateTodoUseCase
{
    private readonly IUserDataAccess userDataAccess;
    private readonly ITaskDataAccess taskDataAccess;
    private readonly ITodoDataAccess todoDataAccess;

    public CreateTodoUseCase(IUserDataAccess userDataAccess,
                             ITaskDataAccess taskDataAccess,
                             ITodoDataAccess todoDataAccess)
    {
        this.userDataAccess = userDataAccess;
        this.taskDataAccess = taskDataAccess;
        this.todoDataAccess = todoDataAccess;
    }

    public void Execute(CreateTodoDto? newTodo, string? authUserId)
    {
        var validUserId = this.ValidateUserId(authUserId);
        var validNewTodo = this.ValidateTodo(newTodo);
        var validTaskId = this.ValidateTaskId(validNewTodo.taskId);
        this.CheckUserExists(validUserId);
        this.CheckTaskExists(validTaskId);
        this.CreateTodo(validNewTodo, validUserId);
    }

    private string ValidateUserId(string? authUserId)
    {
        User.ValidateId(authUserId);
        return authUserId!;
    }

    private string ValidateTaskId(string? taskId)
    {
        Entities.Task.ValidateId(taskId);
        return taskId!;
    }

    private CreateTodoDto ValidateTodo(CreateTodoDto? newTodo)
    {
        if (newTodo == null) {
            throw new InvalidTodoException("Request Body is null");
        }
        Todo.ValidateName(newTodo.Name);
        Todo.ValidateDescription(newTodo.Description);
        return newTodo;
    }

    private void CheckUserExists(string authUserId)
    {
        var user = this.userDataAccess.FindById(authUserId);
        if (user == null) {
            throw new UserNotFoundException();
        }
    }

    private void CheckTaskExists(string taskId)
    {
        var task = this.taskDataAccess.FindById(taskId);
        if (task == null) {
            throw new TaskNotFoundException();
        }
    }

    private void CreateTodo(CreateTodoDto newTodo, string authUserId)
    {
        this.todoDataAccess.Create(newTodo, authUserId);
    }
}
