using Todos.Core.DataAccess;
using Todos.Core.Dtos;
using Todos.Core.Entities;
using Todos.Core.Exceptions;

namespace Todos.Core.UseCases.Tasks;

public class DeleteTaskUseCase : IDeleteTaskUseCase
{
    private readonly IUserDataAccess userDataAccess;
    private readonly ITaskDataAccess taskDataAccess;

    public DeleteTaskUseCase(IUserDataAccess userDataAccess, ITaskDataAccess taskDataAccess)
    {
        this.userDataAccess = userDataAccess;
        this.taskDataAccess = taskDataAccess;
    }

    public void Execute(string? taskId, string? authUserId)
    {
        var validTaskId = this.ValidateTaskId(taskId);
        var validUserId = this.ValidateUserId(authUserId);
        this.CheckUserExists(validUserId);
        var taskDb = this.FindTask(validTaskId);
        this.CheckResourceOwnership(taskDb, validUserId);
        this.DeleteTask(validTaskId);
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
        var user = this.userDataAccess.FindById(authUserId);
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

    private void CheckResourceOwnership(TaskDbDto taskDb, string authUserId)
    {
        if (taskDb.UserId != authUserId) {
            throw new NotResourceOwnerException();
        }
    }

    private void DeleteTask(string taskId)
    {
        this.taskDataAccess.Delete(taskId);
    }
}
