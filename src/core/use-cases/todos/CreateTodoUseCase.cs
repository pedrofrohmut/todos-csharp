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

    public void Execute(CreateTodoDto newTodo, string authUserId)
    {
        var taskId = newTodo.TaskId;
        this.ValidateUserId(authUserId);
        this.ValidateTaskId(taskId);
        this.ValidateTodo(newTodo);
        this.CheckUserExists(authUserId);
        this.CheckTaskExists(taskId);
        this.CreateTodo(newTodo, authUserId);
    }

    private void ValidateUserId(string authUserId)
    {
        User.ValidateId(authUserId);
    }

    private void ValidateTaskId(string taskId)
    {
        Entities.Task.ValidateId(taskId);
    }

    private void ValidateTodo(CreateTodoDto newTodo)
    {
        Todo.ValidateName(newTodo.Name);
        Todo.ValidateDescription(newTodo.Description);
    }

    private void CheckUserExists(string authUserId)
    {
        var user = this.userDataAccess.FindUserById(authUserId);
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
