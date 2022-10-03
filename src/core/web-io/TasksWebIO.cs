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
}
