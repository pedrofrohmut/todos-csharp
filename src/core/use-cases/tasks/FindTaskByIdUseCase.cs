using Todos.Core.DataAccess;
using Todos.Core.Dtos;
using Todos.Core.Entities;
using Todos.Core.Exceptions;

namespace Todos.Core.UseCases.Tasks;

public class FindTaskByIdUseCase : IFindTaskByIdUseCase
{
    private readonly IUserDataAccess userDataAccess;
    private readonly ITaskDataAccess taskDataAccess;

    public FindTaskByIdUseCase(IUserDataAccess userDataAccess, ITaskDataAccess taskDataAccess)
    {
        this.userDataAccess = userDataAccess;
        this.taskDataAccess = taskDataAccess;
    }

    public TaskDto Execute(string? taskId, string? authUserId)
    {
        var validTaskId = this.ValidateTaskId(taskId);
        var validUserId = this.ValidateUserId(authUserId);
        this.CheckUserExists(validUserId);
        var taskDb = this.FindTask(validTaskId);
        this.CheckResourceOwnership(taskDb, validUserId);
        var task = this.MapTaskDbToTask(taskDb);
        return task;
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
        var foundTask = this.taskDataAccess.FindById(taskId);
        if (foundTask == null) {
            throw new TaskNotFoundException();
        }
        return foundTask;
    }

    private void CheckResourceOwnership(TaskDbDto taskDb, string authUserId)
    {
        if (taskDb.UserId.ToString() != authUserId) {
            throw new NotResourceOwnerException();
        }
    }

    private TaskDto MapTaskDbToTask(TaskDbDto task)
    {
        return new TaskDto() {
            Id = task.Id.ToString(),
            Name = task.Name,
            Description = task.Description,
            UserId = task.UserId.ToString()
        };
    }
}
