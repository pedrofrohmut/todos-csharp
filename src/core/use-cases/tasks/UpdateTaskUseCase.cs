using Todos.Core.DataAccess;
using Todos.Core.Dtos;
using Todos.Core.Entities;
using Todos.Core.Exceptions;
using Task = System.Threading.Tasks.Task;

namespace Todos.Core.UseCases.Tasks;

public class UpdateTaskUseCase : IUpdateTaskUseCase
{
    private readonly IUserDataAccess userDataAccess;
    private readonly ITaskDataAccess taskDataAccess;

    public UpdateTaskUseCase(IUserDataAccess userDataAccess, ITaskDataAccess taskDataAccess)
    {
        this.userDataAccess = userDataAccess;
        this.taskDataAccess = taskDataAccess;
    }

    public void Execute(string? taskId, UpdateTaskDto? updatedTask, string? authUserId)
    {
        var validTaskId = this.ValidateTaskId(taskId);
        this.ValidateTask(updatedTask);
        var validUserId = this.ValidateUserId(authUserId);
        this.CheckUserExists(validUserId);
        var taskDb = this.FindTask(validTaskId);
        this.CheckResourceOwnership(taskDb, validUserId);
        this.UpdateTask(validTaskId, updatedTask!);
    }

    private string ValidateTaskId(string? taskId)
    {
        Entities.Task.ValidateId(taskId);
        return taskId!;
    }

    private void ValidateTask(UpdateTaskDto? updatedTask)
    {
        if (updatedTask == null) {
            throw new InvalidTaskException("Request Body is null");
        }
        Entities.Task.ValidateName(updatedTask.Name);
        Entities.Task.ValidateDescription(updatedTask.Description);
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

    private void UpdateTask(string taskId, UpdateTaskDto updatedTask)
    {
        this.taskDataAccess.Update(taskId, updatedTask);
    }

    private Task UpdateTaskAsync(string taskId, UpdateTaskDto updatedTask)
    {
        return this.taskDataAccess.UpdateAsync(taskId, updatedTask);
    }

    public async Task ExecuteAsync(string? taskId, UpdateTaskDto? updatedTask, string? authUserId)
    {
        var validTaskId = this.ValidateTaskId(taskId);
        this.ValidateTask(updatedTask);
        var validUserId = this.ValidateUserId(authUserId);
        await this.CheckUserExistsAsync(validUserId);
        var taskDb = await this.FindTaskAsync(validTaskId);
        this.CheckResourceOwnership(taskDb, validUserId);
        await this.UpdateTaskAsync(validTaskId, updatedTask!);
    }
}
