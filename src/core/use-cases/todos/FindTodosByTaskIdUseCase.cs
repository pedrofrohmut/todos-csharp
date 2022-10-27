using Todos.Core.DataAccess;
using Todos.Core.Dtos;
using Todos.Core.Entities;
using Todos.Core.Exceptions;
using Task = System.Threading.Tasks.Task;

namespace Todos.Core.UseCases.Todos;

public class FindTodosByTaskIdUseCase : IFindTodosByTaskIdUseCase
{
    private readonly IUserDataAccess userDataAccess;
    private readonly ITaskDataAccess taskDataAccess;
    private readonly ITodoDataAccess todoDataAccess;

    public FindTodosByTaskIdUseCase(IUserDataAccess userDataAccess,
                                    ITaskDataAccess taskDataAccess,
                                    ITodoDataAccess todoDataAccess)
    {
        this.userDataAccess = userDataAccess;
        this.taskDataAccess = taskDataAccess;
        this.todoDataAccess = todoDataAccess;
    }

    public List<TodoDto> Execute(string? taskId, string? authUserId)
    {
        var validTaskId = this.ValidateTaskId(taskId);
        var validUserId = this.ValidateAuthUserId(authUserId);
        this.CheckUserExists(validUserId);
        var taskDb = this.FindTask(validTaskId);
        this.CheckResourceOwnership(taskDb, validUserId);
        var todosDb = this.FindTodosByTaskId(validTaskId);
        var todos = this.MapTodosDbToTodos(todosDb);
        return todos;
    }

    private string ValidateTaskId(string? taskId)
    {
        Entities.Task.ValidateId(taskId);
        return taskId!;
    }

    private string ValidateAuthUserId(string? authUserId)
    {
        User.ValidateId(authUserId);
        return authUserId!;
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

    private TaskDbDto FindTask(string taskId)
    {
        var task = this.taskDataAccess.FindById(taskId);
        if (task == null) {
            throw new TaskNotFoundException();
        }
        return task;
    }

    private async Task<TaskDbDto> FindTaskAsync(string taskId)
    {
        var task = await this.taskDataAccess.FindByIdAsync(taskId);
        if (task == null) {
            throw new TaskNotFoundException();
        }
        return task;
    }

    private void CheckResourceOwnership(TaskDbDto taskDb, string authUserId)
    {
        if (taskDb.UserId.ToString() != authUserId) {
            throw new NotResourceOwnerException();
        }
    }

    private List<TodoDbDto> FindTodosByTaskId(string taskId)
    {
        var todos = this.todoDataAccess.FindByTaskId(taskId);
        if (todos == null) return new List<TodoDbDto>();
        return todos;
    }

    private async Task<List<TodoDbDto>> FindTodosByTaskIdAsync(string taskId)
    {
        var todos = await this.todoDataAccess.FindByTaskIdAsync(taskId);
        if (todos == null) return new List<TodoDbDto>();
        return todos;
    }

    private List<TodoDto> MapTodosDbToTodos(List<TodoDbDto> todosDb) =>
        todosDb
            .Select(todo => new TodoDto() {
                Id = todo.Id.ToString(),
                Name = todo.Name,
                Description = todo.Description,
                IsDone = todo.IsDone,
                TaskId = todo.TaskId.ToString(),
                UserId = todo.UserId.ToString()
            })
            .ToList();

    public async Task<List<TodoDto>> ExecuteAsync(string? taskId, string? authUserId)
    {
        var validTaskId = this.ValidateTaskId(taskId);
        var validUserId = this.ValidateAuthUserId(authUserId);
        await this.CheckUserExistsAsync(validUserId);
        var taskDb = await this.FindTaskAsync(validTaskId);
        this.CheckResourceOwnership(taskDb, validUserId);
        var todosDb = await this.FindTodosByTaskIdAsync(validTaskId);
        var todos = this.MapTodosDbToTodos(todosDb);
        return todos;
    }
}
