using Todos.Core.DataAccess;
using Todos.Core.Dtos;
using Todos.Core.Entities;
using Todos.Core.Exceptions;
using Task = System.Threading.Tasks.Task;

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
        var validTaskId = this.ValidateTaskId(validNewTodo.TaskId);
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

    private async Task CheckUserExistsAsync(string authUserId)
    {
        var user = await this.userDataAccess.FindByIdAsync(authUserId);
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

    private async Task CheckTaskExistsAsync(string taskId)
    {
        var task = await this.taskDataAccess.FindByIdAsync(taskId);
        if (task == null) {
            throw new TaskNotFoundException();
        }
    }

    private void CreateTodo(CreateTodoDto newTodo, string authUserId)
    {
        this.todoDataAccess.Create(newTodo, authUserId);
    }

    private Task CreateTodoAsync(CreateTodoDto newTodo, string authUserId)
    {
        return this.todoDataAccess.CreateAsync(newTodo, authUserId);
    }

    public async Task ExecuteAsync(CreateTodoDto? newTodo, string? authUserId)
    {
        var validUserId = this.ValidateUserId(authUserId);
        var validNewTodo = this.ValidateTodo(newTodo);
        var validTaskId = this.ValidateTaskId(validNewTodo.TaskId);
        await this.CheckUserExistsAsync(validUserId);
        await this.CheckTaskExistsAsync(validTaskId);
        await this.CreateTodoAsync(validNewTodo, validUserId);
    }
}
