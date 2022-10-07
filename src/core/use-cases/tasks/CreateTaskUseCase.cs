using Todos.Core.Dtos;
using Todos.Core.Entities;
using Todos.Core.Exceptions;
using Todos.Core.DataAccess;

namespace Todos.Core.UseCases.Tasks;

public class CreateTaskUseCase : ICreateTaskUseCase
{
    private readonly IUserDataAccess userDataAccess;
    private readonly ITaskDataAccess taskDataAccess;

    public CreateTaskUseCase(IUserDataAccess userDataAccess, ITaskDataAccess taskDataAccess)
    {
        this.userDataAccess = userDataAccess;
        this.taskDataAccess = taskDataAccess;
    }

    public void Execute(CreateTaskDto newTask, string authUserId)
    {
        this.ValidateNewTask(newTask);
        this.ValidateAuthUserId(authUserId);
        this.CheckUserExists(authUserId);
        this.CreateTask(newTask, authUserId);
    }

    private void ValidateNewTask(CreateTaskDto newTask)
    {
        Entities.Task.ValidateName(newTask.Name);
        Entities.Task.ValidateDescription(newTask.Description);
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

    private void CreateTask(CreateTaskDto newTask, string authUserId)
    {
        this.taskDataAccess.Create(newTask, authUserId);
    }
}
