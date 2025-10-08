using Todos.Core.Dtos;
using Todos.Core.Exceptions;
using Todos.Core.UseCases.Tasks;

namespace Todos.Core.WebIO;

public static class TasksWebIO
{
    public static WebResponseDto Create(ICreateTaskUseCase createTaskUseCase, WebRequestDto request)
    {
        try {
            createTaskUseCase.Execute((CreateTaskDto?) request.Body, request.AuthUserId);
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

    public async static Task<WebResponseDto> CreateAsync(ICreateTaskUseCase createTaskUseCase,
                                                         WebRequestDto request)
    {
        try {
            await createTaskUseCase.ExecuteAsync((CreateTaskDto?) request.Body, request.AuthUserId);
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

    public static WebResponseDto Delete(IDeleteTaskUseCase deleteTaskUseCase, WebRequestDto request)
    {
        try {
            deleteTaskUseCase.Execute(request.Param, request.AuthUserId);
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

    public async static Task<WebResponseDto> DeleteAsync(IDeleteTaskUseCase deleteTaskUseCase,
                                                         WebRequestDto request)
    {
        try {
            await deleteTaskUseCase.ExecuteAsync(request.Param, request.AuthUserId);
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

    public static WebResponseDto FindById(IFindTaskByIdUseCase findTaskByIdUseCase, WebRequestDto request)
    {
        try {
            var foundTask = findTaskByIdUseCase.Execute(request.Param, request.AuthUserId);
            return new WebResponseDto() { Body = foundTask, Status = 200 };
        } catch (Exception e) {
            if (e is InvalidUserException ||
                e is InvalidTaskException ||
                e is UserNotFoundException)
            {
                return new WebResponseDto() { Message = e.Message, Status = 400 };
            }
            if (e is TaskNotFoundException) {
                return new WebResponseDto() { Message = "Task Not Found", Status = 404 };
            }
            throw;
        }
    }

    public async static Task<WebResponseDto> FindByIdAsync(IFindTaskByIdUseCase findTaskByIdUseCase,
                                                           WebRequestDto request)
    {
        try {
            var foundTask = await findTaskByIdUseCase.ExecuteAsync(request.Param, request.AuthUserId);
            return new WebResponseDto() { Body = foundTask, Status = 200 };
        } catch (Exception e) {
            if (e is InvalidUserException ||
                e is InvalidTaskException ||
                e is UserNotFoundException)
            {
                return new WebResponseDto() { Message = e.Message, Status = 400 };
            }
            if (e is TaskNotFoundException) {
                return new WebResponseDto() { Message = "Task Not Found", Status = 404 };
            }
            throw;
        }
    }

    public static WebResponseDto FindByUserId(IFindTasksByUserIdUseCase findTasksByUserIdUseCase, WebRequestDto request)
    {
        try {
            var tasks = findTasksByUserIdUseCase.Execute(request.AuthUserId);
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

    public async static Task<WebResponseDto> FindByUserIdAsync(
        IFindTasksByUserIdUseCase findTasksByUserIdUseCase, WebRequestDto request)
    {
        try {
            var tasks = await findTasksByUserIdUseCase.ExecuteAsync(request.AuthUserId);
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

    public static WebResponseDto Update(IUpdateTaskUseCase updateTaskUseCase, WebRequestDto request)
    {
        try {
            updateTaskUseCase.Execute(request.Param, (UpdateTaskDto?) request.Body, request.AuthUserId);
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

    public async static Task<WebResponseDto> UpdateAsync(IUpdateTaskUseCase updateTaskUseCase,
                                                         WebRequestDto request)
    {
        try {
            await updateTaskUseCase.ExecuteAsync(request.Param,
                                            (UpdateTaskDto?) request.Body,
                                            request.AuthUserId);
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
