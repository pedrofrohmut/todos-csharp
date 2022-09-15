using Todos.Core.Dtos;

namespace Todos.Core.Controllers; 

public class UsersController
{
    public ControllerResponseDto<string> SignUpUser(AdaptedRequest<CreateUserDto> req)
    {
        return new ControllerResponseDto<string>() {
            Msg = "User created",
            Status = 201
        };
    }

    public ControllerResponseDto<SignedUserDto> SignInUser(AdaptedRequest<UserCredentialsDto> req)
    {
        return new ControllerResponseDto<SignedUserDto>() {
            Body = new SignedUserDto(),
            Status = 200
        };
    }

    public ControllerResponseDto<bool> Verify(AdaptedRequest<object> req)
    {
        return new ControllerResponseDto<bool>() {
            Body = true,
            Status = 200
        };
    }
}
