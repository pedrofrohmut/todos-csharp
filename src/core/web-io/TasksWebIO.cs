using Todos.Core.Dtos;
using Todos.Core.Exceptions;
using Todos.Core.UseCases.Tasks;

namespace Todos.Core.WebIO; 

public class TasksWebIO
{
    public WebResponseDto Create(ICreateTaskUseCase createTaskUseCase, WebRequestDto request)
    {
        try {
            createTaskUseCase.Execute((CreateTaskDto) request.Body!, request.AuthUserId!);
            return new WebResponseDto() { Message = "Task Created", Status = 201 };
        } catch (Exception e) {
            if (e is InvalidUserException ||
                e is InvalidTaskException ||
                e is UserNotFoundException)
            {
                return new WebResponseDto () { Message = e.Message, Status = 400 };
            }
            throw;
        }
    }

    public WebResponseDto Delete(IDeleteTaskUseCase deleteTaskUseCase, WebRequestDto request)
    {
        try {
            deleteTaskUseCase.Execute(request.Param!, request.AuthUserId!);
            return new WebResponseDto() { Message = "", Status = 204 };
        } catch (Exception e) {
            if (e is InvalidUserException ||
                e is InvalidTaskException ||
                e is UserNotFoundException)
            {
                return new WebResponseDto() { Message = e.Message, Status = 400 };
            }
            throw;
        }
    }

    // TODO: check what to do when task not found
    public WebResponseDto FindById(IFindTaskByIdUseCase findTaskByIdUseCase, WebRequestDto request)
    {
        try {
            var foundTask = findTaskByIdUseCase.Execute(request.Param!, request.AuthUserId!);
            return new WebResponseDto() { Body = foundTask, Status = 200 };
        } catch (Exception e) {
            if (e is InvalidUserException ||
                e is InvalidTaskException ||
                e is UserNotFoundException)
            {
                return new WebResponseDto() { Message = e.Message, Status = 400 };
            }
            if (e is TaskNotFoundException) {
                return new WebResponseDto() { Message = "", Status = 204 };
            }
            throw;
        }
    }

    public WebResponseDto FindByUserId(IFindTasksByUserIdUseCase findTasksByUserIdUseCase, WebRequestDto request)
    {
        try {
            var tasks = findTasksByUserIdUseCase.Execute(request.AuthUserId!);
            if (tasks.Count == 0) {
                return new WebResponseDto() { Message = "",  Status = 204 };
            }
            return new WebResponseDto() { Body = tasks,  Status = 200 };
        } catch (Exception e) {
            if (e is InvalidUserException ||
                e is InvalidTaskException ||
                e is UserNotFoundException)
            {
                return new WebResponseDto() { Message = e.Message, Status = 400 };
            }
            throw;
        }
    }

    public WebResponseDto Update(IUpdateTaskUseCase updateTaskUseCase, WebRequestDto request)
    {
        try {
            var body = (UpdateTaskDto) request.Body!;
            var updatedTask = new UpdateTaskDto() {
                Id = request.Param!,
                Name = body.Name,
                Description = body.Description
            };
            updateTaskUseCase.Execute(updatedTask, request.AuthUserId!);
            return new WebResponseDto() { Message = "", Status = 204 };
        } catch (Exception e) {
            if (e is InvalidUserException ||
                e is InvalidTaskException ||
                e is UserNotFoundException ||
                e is TaskNotFoundException)
            {
                return new WebResponseDto() { Message = e.Message, Status = 400 };
            }
            throw;
        }
    }
}
