using Todos.Core.Dtos;
using Todos.Core.UseCases.Users;

namespace Todos.Core.Controllers; 

public class UsersController
{
    public ControllerResponseDto SignUpUser(ISignUpUseCase signUpUseCase, AdaptedRequest request)
    {
        return new ControllerResponseDto() {
            Message = "User created",
            Status = 201
        };
    }

    public ControllerResponseDto SignInUser(AdaptedRequest req)
    {
        return new ControllerResponseDto() {
            Body = new SignedUserDto(),
            Status = 200
        };
    }

    public ControllerResponseDto Verify(AdaptedRequest req)
    {
        return new ControllerResponseDto() {
            Body = true,
            Status = 200
        };
    }
}
