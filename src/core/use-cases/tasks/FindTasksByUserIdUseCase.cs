using Todos.Core.DataAccess;
using Todos.Core.Dtos;
using Todos.Core.Entities;
using Todos.Core.Exceptions;
using Task = System.Threading.Tasks.Task;

namespace Todos.Core.UseCases.Tasks;

public class FindTasksByUserIdUseCase : IFindTasksByUserIdUseCase
{
    private readonly IUserDataAccess userDataAccess;
    private readonly ITaskDataAccess taskDataAccess;

    public FindTasksByUserIdUseCase(IUserDataAccess userDataAccess, ITaskDataAccess taskDataAccess)
    {
        this.userDataAccess = userDataAccess;
        this.taskDataAccess = taskDataAccess;
    }

    public List<TaskDto> Execute(string? authUserId)
    {
        var validUserId = this.ValidateUserId(authUserId);
        this.CheckUserExists(validUserId);
        var tasksDb = this.FindTasksByUserId(validUserId);
        this.CheckResourceOwnership(tasksDb, validUserId);
        var tasks = this.MapTasksDbToTasks(tasksDb);
        return tasks;
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

    private List<TaskDbDto> FindTasksByUserId(string authUserId) =>
        this.taskDataAccess.FindByUserId(authUserId);

    private async Task<List<TaskDbDto>> FindTasksByUserIdAsync(string authUserId) =>
        await this.taskDataAccess.FindByUserIdAsync(authUserId);

    private void CheckResourceOwnership(List<TaskDbDto> tasksDb, string authUserId)
    {
        tasksDb.ForEach(taskDb => {
            if (taskDb.UserId.ToString() != authUserId) {
                throw new NotResourceOwnerException();
            }
        });
    }

    private List<TaskDto> MapTasksDbToTasks(List<TaskDbDto> tasksDb) =>
        tasksDb
            .Select(task => new TaskDto() {
                Id = task.Id.ToString(),
                Name = task.Name,
                Description = task.Description,
                UserId = task.UserId.ToString()
            })
            .ToList();

    public async Task<List<TaskDto>> ExecuteAsync(string? authUserId)
    {
        var validUserId = this.ValidateUserId(authUserId);
        await this.CheckUserExistsAsync(validUserId);
        var tasksDb = await this.FindTasksByUserIdAsync(validUserId);
        this.CheckResourceOwnership(tasksDb, validUserId);
        var tasks = this.MapTasksDbToTasks(tasksDb);
        return tasks;
    }
}
