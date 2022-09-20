using Todos.Core.Dtos;
using Todos.Core.Exceptions;
using Todos.Core.UseCases.Users;

namespace Todos.Core.WebIO; 

public class UsersWebIO
{
    public WebResponseDto SignUpUser(ISignUpUseCase signUpUseCase, WebRequestDto request)
    {
        try {
            signUpUseCase.Execute((CreateUserDto) request.Body!);
            return new WebResponseDto() { Message = "User created", Status = 201 };
        } catch (InvalidUserException e) {
            return new WebResponseDto() { Message = e.Message, Status = 400 };
        } catch (EmailAlreadyTakenException e) {
            return new WebResponseDto() { Message = e.Message, Status = 400 };
        }
    }

    public WebResponseDto SignInUser(WebRequestDto req)
    {
        return new WebResponseDto() {
            Body = new SignedUserDto(),
            Status = 200
        };
    }

    public WebResponseDto Verify(WebRequestDto req)
    {
        return new WebResponseDto() {
            Body = true,
            Status = 200
        };
    }
}
