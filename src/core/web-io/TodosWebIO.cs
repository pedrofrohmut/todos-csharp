using Todos.Core.Dtos;
using Todos.Core.Exceptions;
using Todos.Core.UseCases.Todos;

namespace Todos.Core.WebIO; 

public class TodosWebIO
{
    public WebResponseDto Create(ICreateTodoUseCase createTodoUseCase, WebRequestDto request)
    {
        try {
            createTodoUseCase.Execute((CreateTodoDto) request.Body!, request.AuthUserId!);
            return new WebResponseDto() { Message = "Todo Created", Status = 201 };
        } catch (Exception e) {
            if (e is InvalidUserException ||
                e is InvalidTaskException ||
                e is InvalidTodoException ||
                e is UserNotFoundException ||
                e is TaskNotFoundException)
            {
                return new WebResponseDto() { Message = e.Message, Status = 400 };
            }
            throw;
        }
    }
}
