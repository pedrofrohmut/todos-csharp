using Todos.Core.Dtos;
using Todos.Core.Entities;
using Todos.Core.Exceptions;
using Todos.Core.DataAccess;

namespace Todos.Core.UseCases.Tasks;

public class
    CreateTaskUseCase : ICreateTaskUseCase
{
    private readonly IUserDataAccess userDataAccess;
    private readonly ITaskDataAccess taskDataAccess;

    public CreateTaskUseCase(IUserDataAccess userDataAccess, ITaskDataAccess taskDataAccess)
    {
        this.userDataAccess = userDataAccess;
        this.taskDataAccess = taskDataAccess;
    }

    public void Execute(CreateTaskDto? newTask, string? authUserId)
    {
        var validTask = this.ValidateNewTask(newTask);
        var validUserId = this.ValidateAuthUserId(authUserId);
        this.CheckUserExists(validUserId);
        this.CreateTask(validTask, validUserId);
    }

    private CreateTaskDto ValidateNewTask(CreateTaskDto? newTask)
    {
        if (newTask == null) {
            throw new InvalidTaskException("Request Body is null");
        }
        Entities.Task.ValidateName(newTask.Name);
        Entities.Task.ValidateDescription(newTask.Description);
        return newTask!;
    }

    private string ValidateAuthUserId(string? authUserId)
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

    private void CreateTask(CreateTaskDto newTask, string authUserId)
    {
        this.taskDataAccess.Create(newTask, authUserId);
    }
}
