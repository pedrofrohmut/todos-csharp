using Todos.Core.Dtos;
using Todos.Core.Exceptions;
using Todos.Core.UseCases.Todos;

namespace Todos.Core.WebIO;

public static class TodosWebIO
{
    public static WebResponseDto Create(ICreateTodoUseCase createTodoUseCase, WebRequestDto request)
    {
        try {
            createTodoUseCase.Execute((CreateTodoDto?) request.Body, request.AuthUserId);
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

    public async static Task<WebResponseDto> CreateAsync(ICreateTodoUseCase createTodoUseCase, WebRequestDto request)
    {
        try {
            await createTodoUseCase.ExecuteAsync((CreateTodoDto?) request.Body, request.AuthUserId);
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

    public static WebResponseDto FindByTaskId(IFindTodosByTaskIdUseCase findTodosByTaskIdUseCase,
                                       WebRequestDto request)
    {
        try {
            var todos = findTodosByTaskIdUseCase.Execute(request.Param, request.AuthUserId);
            return new WebResponseDto() { Body = todos, Status = 200 };
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

    public async static Task<WebResponseDto> FindByTaskIdAsync(
        IFindTodosByTaskIdUseCase findTodosByTaskIdUseCase, WebRequestDto request)
    {
        try {
            var todos = await findTodosByTaskIdUseCase.ExecuteAsync(request.Param, request.AuthUserId);
            return new WebResponseDto() { Body = todos, Status = 200 };
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

    public static WebResponseDto FindById(IFindTodoByIdUseCase findTodoByIdUseCase, WebRequestDto request)
    {
        try {
            var todo = findTodoByIdUseCase.Execute(request.Param, request.AuthUserId);
            return new WebResponseDto() { Body = todo, Status = 200 };
        } catch (Exception e) {
            if (e is TodoNotFoundException) {
                return new WebResponseDto() { Message = "Todo Not Found", Status = 404 };
            }
            if (e is InvalidUserException ||
                e is InvalidTaskException ||
                e is InvalidTodoException ||
                e is UserNotFoundException)
            {
                return new WebResponseDto() { Message = e.Message, Status = 400 };
            }
            throw;
        }
    }

    public async static Task<WebResponseDto> FindByIdAsync(IFindTodoByIdUseCase findTodoByIdUseCase,
                                                           WebRequestDto request)
    {
        try {
            var todo = await findTodoByIdUseCase.ExecuteAsync(request.Param, request.AuthUserId);
            return new WebResponseDto() { Body = todo, Status = 200 };
        } catch (Exception e) {
            if (e is TodoNotFoundException) {
                return new WebResponseDto() { Message = "Todo Not Found", Status = 404 };
            }
            if (e is InvalidUserException ||
                e is InvalidTaskException ||
                e is InvalidTodoException ||
                e is UserNotFoundException)
            {
                return new WebResponseDto() { Message = e.Message, Status = 400 };
            }
            throw;
        }
    }

    public static WebResponseDto SetDone(ISetTodoDoneUseCase setTodoDoneUseCase, WebRequestDto request)
    {
        try {
            setTodoDoneUseCase.Execute(request.Param, request.AuthUserId);
            return new WebResponseDto() { Message = "", Status = 204 };
        } catch (Exception e) {
            if (e is InvalidTodoException ||
                e is InvalidUserException ||
                e is UserNotFoundException ||
                e is TodoNotFoundException)
            {
                return new WebResponseDto() { Message = e.Message, Status = 400 };
            }
            throw;
        }
    }

    public async static Task<WebResponseDto> SetDoneAsync(ISetTodoDoneUseCase setTodoDoneUseCase, WebRequestDto request)
    {
        try {
            await setTodoDoneUseCase.ExecuteAsync(request.Param, request.AuthUserId);
            return new WebResponseDto() { Message = "", Status = 204 };
        } catch (Exception e) {
            if (e is InvalidTodoException ||
                e is InvalidUserException ||
                e is UserNotFoundException ||
                e is TodoNotFoundException)
            {
                return new WebResponseDto() { Message = e.Message, Status = 400 };
            }
            throw;
        }
    }

    public static WebResponseDto SetNotDone(ISetTodoNotDoneUseCase setTodoNotDoneUseCase, WebRequestDto request)
    {
        try {
            setTodoNotDoneUseCase.Execute(request.Param, request.AuthUserId);
            return new WebResponseDto() { Message = "", Status = 204 };
        } catch (Exception e) {
            if (e is InvalidTodoException ||
                e is InvalidUserException ||
                e is UserNotFoundException ||
                e is TodoNotFoundException)
            {
                return new WebResponseDto() { Message = e.Message, Status = 400 };
            }
            throw;
        }
    }

    public async static Task<WebResponseDto> SetNotDoneAsync(
        ISetTodoNotDoneUseCase setTodoNotDoneUseCase, WebRequestDto request)
    {
        try {
            await setTodoNotDoneUseCase.ExecuteAsync(request.Param, request.AuthUserId);
            return new WebResponseDto() { Message = "", Status = 204 };
        } catch (Exception e) {
            if (e is InvalidTodoException ||
                e is InvalidUserException ||
                e is UserNotFoundException ||
                e is TodoNotFoundException)
            {
                return new WebResponseDto() { Message = e.Message, Status = 400 };
            }
            throw;
        }
    }

    public static WebResponseDto Update(IUpdateTodoUseCase updateTodoUseCase, WebRequestDto request)
    {
        try {
            updateTodoUseCase.Execute(request.Param, (UpdateTodoDto?) request.Body, request.AuthUserId);
            return new WebResponseDto() { Message = "", Status = 204 };
        } catch (Exception e) {
            if (e is InvalidUserException ||
                e is InvalidTodoException ||
                e is UserNotFoundException ||
                e is TodoNotFoundException)
            {
                return new WebResponseDto() { Message = e.Message, Status = 400 };
            }
            throw;
        }
    }

    public async static Task<WebResponseDto> UpdateAsync(IUpdateTodoUseCase updateTodoUseCase, WebRequestDto request)
    {
        try {
            await updateTodoUseCase.ExecuteAsync(request.Param, (UpdateTodoDto?)
                                                 request.Body,
                                                 request.AuthUserId);
            return new WebResponseDto() { Message = "", Status = 204 };
        } catch (Exception e) {
            if (e is InvalidUserException ||
                e is InvalidTodoException ||
                e is UserNotFoundException ||
                e is TodoNotFoundException)
            {
                return new WebResponseDto() { Message = e.Message, Status = 400 };
            }
            throw;
        }
    }

    public static WebResponseDto Delete(IDeleteTodoUseCase deleteTodoUseCase, WebRequestDto request)
    {
        try {
            deleteTodoUseCase.Execute(request.Param, request.AuthUserId);
            return new WebResponseDto() { Message = "", Status = 204 };
        } catch (Exception e) {
            if (e is InvalidUserException ||
                e is InvalidTodoException ||
                e is UserNotFoundException ||
                e is TodoNotFoundException)
            {
                return new WebResponseDto() { Message = e.Message, Status = 400 };
            }
            throw;
        }
    }

    public async static Task<WebResponseDto> DeleteAsync(IDeleteTodoUseCase deleteTodoUseCase,
                                                         WebRequestDto request)
    {
        try {
            await deleteTodoUseCase.ExecuteAsync(request.Param, request.AuthUserId);
            return new WebResponseDto() { Message = "", Status = 204 };
        } catch (Exception e) {
            if (e is InvalidUserException ||
                e is InvalidTodoException ||
                e is UserNotFoundException ||
                e is TodoNotFoundException)
            {
                return new WebResponseDto() { Message = e.Message, Status = 400 };
            }
            throw;
        }
    }


    public static WebResponseDto DeleteDone(IDeleteDoneTodosUseCase deleteDoneTodosUseCase,
                                          WebRequestDto request)
    {
        try {
            deleteDoneTodosUseCase.Execute(request.AuthUserId);
            return new WebResponseDto() { Message = "", Status = 204 };
        } catch (Exception e) {
            if (e is InvalidUserException || e is UserNotFoundException) {
                return new WebResponseDto() { Message = e.Message, Status = 400 };
            }
            throw;
        }
    }

    public async static Task<WebResponseDto> DeleteDoneAsync(IDeleteDoneTodosUseCase deleteDoneTodosUseCase,
                                          WebRequestDto request)
    {
        try {
            await deleteDoneTodosUseCase.ExecuteAsync(request.AuthUserId);
            return new WebResponseDto() { Message = "", Status = 204 };
        } catch (Exception e) {
            if (e is InvalidUserException || e is UserNotFoundException) {
                return new WebResponseDto() { Message = e.Message, Status = 400 };
            }
            throw;
        }
    }

    public static WebResponseDto DeleteDoneByTaskId(
        IDeleteDoneTodosByTaskIdUseCase deleteDoneTodosByTaskIdUseCase,
        WebRequestDto request)
    {
        try {
            deleteDoneTodosByTaskIdUseCase.Execute(request.Param, request.AuthUserId);
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

    public async static Task<WebResponseDto> DeleteDoneByTaskIdAsync(
        IDeleteDoneTodosByTaskIdUseCase deleteDoneTodosByTaskIdUseCase, WebRequestDto request)
    {
        try {
            await deleteDoneTodosByTaskIdUseCase.ExecuteAsync(request.Param, request.AuthUserId);
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
