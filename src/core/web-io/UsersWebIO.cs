using Todos.Core.Dtos;
using Todos.Core.UseCases.Users;

namespace Todos.Core.WebIO; 

public class UsersWebIO
{
    public WebResponseDto SignUpUser(ISignUpUseCase signUpUseCase, WebRequestDto request)
    {
        return new WebResponseDto() {
            Message = "User created",
            Status = 201
        };
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
