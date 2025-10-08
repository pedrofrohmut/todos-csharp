using Todos.Core.Dtos;
using Todos.Core.Exceptions;
using Todos.Core.UseCases.Users;

namespace Todos.Core.WebIO;

public static class UsersWebIO
{
    public static WebResponseDto SignUp(ISignUpUseCase signUpUseCase, WebRequestDto request)
    {
        try {
            signUpUseCase.Execute((CreateUserDto?) request.Body);
            return new WebResponseDto() { Message = "User created", Status = 201 };
        } catch (Exception e) {
            if (e is InvalidUserException || e is EmailAlreadyTakenException) {
                return new WebResponseDto() { Message = e.Message, Status = 400 };
            }
            throw;
        }
    }

    public async static Task<WebResponseDto> SignUpAsync(ISignUpUseCase signUpUseCase, WebRequestDto request)
    {
        try {
            await signUpUseCase.ExecuteAsync((CreateUserDto?) request.Body);
            return new WebResponseDto() { Message = "User created", Status = 201 };
        } catch (Exception e) {
            if (e is InvalidUserException || e is EmailAlreadyTakenException) {
                return new WebResponseDto() { Message = e.Message, Status = 400 };
            }
            throw;
        }
    }

    public static WebResponseDto SignIn(ISignInUseCase signInUseCase, WebRequestDto req)
    {
        try {
            var signedUser = signInUseCase.Execute((UserCredentialsDto?) req.Body);
            return new WebResponseDto() { Body = signedUser, Status = 200 };
        } catch (Exception e) {
            if (e is InvalidUserException ||
                e is UserNotFoundException ||
                e is PasswordAndHashNotMatchException)
            {
                return new WebResponseDto() { Message = e.Message, Status = 400 };
            }
            throw;
        }
    }

    public async static Task<WebResponseDto> SignInAsync(ISignInUseCase signInUseCase, WebRequestDto req)
    {
        try {
            var signedUser = await signInUseCase.ExecuteAsync((UserCredentialsDto?) req.Body);
            return new WebResponseDto() { Body = signedUser, Status = 200 };
        } catch (Exception e) {
            if (e is InvalidUserException ||
                e is UserNotFoundException ||
                e is PasswordAndHashNotMatchException)
            {
                return new WebResponseDto() { Message = e.Message, Status = 400 };
            }
            throw;
        }
    }

    public static WebResponseDto Verify(IVerifyUserUseCase verifyUserUseCase, WebRequestDto req)
    {
        try {
            verifyUserUseCase.Execute(req.AuthUserId);
            return new WebResponseDto() { Body = true, Status = 200 };
        } catch (Exception e) {
            if (e is InvalidUserException || e is UserNotFoundException) {
                return new WebResponseDto() { Message = e.Message, Status = 400 };
            }
            throw;
        }
    }

    public async static Task<WebResponseDto> VerifyAsync(IVerifyUserUseCase verifyUserUseCase, WebRequestDto req)
    {
        try {
            await verifyUserUseCase.ExecuteAsync(req.AuthUserId);
            return new WebResponseDto() { Body = true, Status = 200 };
        } catch (Exception e) {
            if (e is InvalidUserException || e is UserNotFoundException) {
                return new WebResponseDto() { Message = e.Message, Status = 400 };
            }
            throw;
        }
    }
}
