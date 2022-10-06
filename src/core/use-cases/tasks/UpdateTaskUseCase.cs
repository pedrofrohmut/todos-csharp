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
        this.CheckTaskExists(updatedTask.Id);
        this.UpdateTask(updatedTask);
    }

    private void ValidateTask(UpdateTaskDto updatedTask)
    {
        Todos.Core.Entities.Task.ValidateId(updatedTask.Id);
        Todos.Core.Entities.Task.ValidateName(updatedTask.Name);
        Todos.Core.Entities.Task.ValidateDescription(updatedTask.Description);
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

    private void CheckTaskExists(string taskId)
    {
        var task = this.taskDataAccess.FindById(taskId);
        if (task == null) {
            throw new TaskNotFoundException();
        }
    }

    private void UpdateTask(UpdateTaskDto updatedTask)
    {
        this.taskDataAccess.Update(updatedTask);
    }
}
