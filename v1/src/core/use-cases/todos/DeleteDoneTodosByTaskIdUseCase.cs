using Todos.Core.DataAccess;
using Todos.Core.Dtos;
using Todos.Core.Entities;
using Todos.Core.Exceptions;
using Task = System.Threading.Tasks.Task;

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
        var taskDb = this.FindTask(validTaskId);
        this.CheckResourceOwnership(taskDb, validUserId);
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

    private void DeleteDoneTodosByTaskId(string taskId)
    {
        this.todoDataAccess.DeleteDoneByTaskId(taskId);
    }

    private Task DeleteDoneTodosByTaskIdAsync(string taskId)
    {
        return this.todoDataAccess.DeleteDoneByTaskIdAsync(taskId);
    }

    public async Task ExecuteAsync(string? taskId, string? authUserId)
    {
        var validTaskId = this.ValidateTaskId(taskId);
        var validUserId = this.ValidateUserId(authUserId);
        await this.CheckUserExistsAsync(validUserId);
        var taskDb = await this.FindTaskAsync(validTaskId);
        this.CheckResourceOwnership(taskDb, validUserId);
        await this.DeleteDoneTodosByTaskIdAsync(validTaskId);
    }
}
