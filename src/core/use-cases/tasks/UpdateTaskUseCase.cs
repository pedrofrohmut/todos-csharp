using Todos.Core.DataAccess;
using Todos.Core.Dtos;
using Todos.Core.Entities;
using Todos.Core.Exceptions;

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

    public void Execute(UpdateTaskDto updatedTask, string authUserId)
    {
        this.ValidateTask(updatedTask);
        this.ValidateUserId(authUserId);
        this.CheckUserExists(authUserId);
        var taskDb = this.FindTask(updatedTask.Id);
        this.CheckResourceOwnership(taskDb, authUserId);
        this.UpdateTask(updatedTask);
    }

    private void ValidateTask(UpdateTaskDto updatedTask)
    {
        Entities.Task.ValidateId(updatedTask.Id);
        Entities.Task.ValidateName(updatedTask.Name);
        Entities.Task.ValidateDescription(updatedTask.Description);
    }

    private void ValidateUserId(string authUserId)
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

    private void UpdateTask(UpdateTaskDto updatedTask)
    {
        this.taskDataAccess.Update(updatedTask);
    }
}
