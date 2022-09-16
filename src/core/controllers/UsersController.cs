using Todos.Core.Dtos;

namespace Todos.Core.Controllers; 

public class UsersController
{
    public ControllerResponseDto SignUpUser(AdaptedRequest req)
    {
        return new ControllerResponseDto() {
            Msg = "User created",
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
