using Todos.Core.DataAccess;
using Todos.Core.Entities;
using Todos.Core.Exceptions;

namespace Todos.Core.UseCases.Todos;

public class DeleteDoneTodosByTaskIdUseCase : IDeleteDoneTodosByTaskIdUseCase
{
    private readonly IUserDataAccess userDataAccess;
    private readonly ITaskDataAccess taskDataAccess;
    private readonly ITodoDataAccess todoDataAccess;

    public DeleteDoneTodosByTaskIdUseCase(IUserDataAccess userDataAccess,
                                          ITaskDataAccess taskDataAccess,
                                          ITodoDataAccess todoDataAccess)
    {
        this.userDataAccess = userDataAccess;
        this.taskDataAccess = taskDataAccess;
        this.todoDataAccess = todoDataAccess;
    }
    
    public void Execute(string? taskId, string? authUserId)
    {
        var validTaskId = this.ValidateTaskId(taskId);
        var validUserId = this.ValidateUserId(authUserId);
        this.CheckUserExists(validUserId);
        this.CheckTaskExists(validTaskId);
        this.DeleteDoneTodosByTaskId(validTaskId);
    }

    private string ValidateTaskId(string? taskId)
    {
        Entities.Task.ValidateId(taskId);
        return taskId!;
    }

    private string ValidateUserId(string? authUserId)
    {
        User.ValidateId(authUserId);
        return authUserId!;
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

    private void DeleteDoneTodosByTaskId(string taskId)
    {
        this.todoDataAccess.DeleteDoneByTaskId(taskId);
    }
}
