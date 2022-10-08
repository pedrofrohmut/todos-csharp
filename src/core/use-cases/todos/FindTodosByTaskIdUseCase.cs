using Todos.Core.DataAccess;
using Todos.Core.Dtos;
using Todos.Core.Entities;
using Todos.Core.Exceptions;

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

    public List<TodoDto> Execute(string taskId, string authUserId)
    {
        this.ValidateTaskId(taskId);
        this.ValidateAuthUserId(authUserId);
        this.CheckUserExists(authUserId);
        var todosDb = this.FindTodosByTaskId(taskId);
        var todos = this.MapTodosDbToTodos(todosDb);
        return todos;
    }

    private void ValidateTaskId(string taskId)
    {
        Entities.Task.ValidateId(taskId);
    }

    private void ValidateAuthUserId(string authUserId)
    {
        User.ValidateId(authUserId);
    }

    private void CheckUserExists(string authUserId)
    {
        var user = this.userDataAccess.FindUserById(authUserId);
        if (user == null) {
            throw new UserNotFoundException();
        }
    }

    private List<TodoDbDto> FindTodosByTaskId(string taskId)
    {
        var todos = this.todoDataAccess.FindByTaskId(taskId).ToList();
        return todos;
    }

    private List<TodoDto> MapTodosDbToTodos(List<TodoDbDto> todosDb) =>
        todosDb
            .Select(todo => new TodoDto() {
                Id = todo.Id,
                Name = todo.Name,
                Description = todo.Description,
                IsDone = todo.IsDone,
                TaskId = todo.TaskId,
                UserId = todo.UserId
            })
            .ToList();
}
