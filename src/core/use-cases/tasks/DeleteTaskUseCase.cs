using Todos.Core.DataAccess;
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

    public void Execute(string taskId, string authUserId)
    {
        this.ValidateTaskId(taskId);
        this.ValidateUserId(authUserId);
        this.CheckUserExists(authUserId);
        this.DeleteTask(taskId);
    }

    private void ValidateTaskId(string taskId)
    {
        Entities.Task.ValidateId(taskId);
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

    private void DeleteTask(string taskId)
    {
        this.taskDataAccess.Delete(taskId);
    }
}
