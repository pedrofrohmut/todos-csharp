# Version 2
Make this changes to be version 2 and save old stuff as version 1 so you dont have
to be conservative and can go all in

# Updates on Controllers

1. Calls Helper getDBConnection to get db_connection
1. Get use case instance from UseCasesFactory using the db_connection
1. Make FooUseCase.Input from request data (params, body and headers)
1. Calls Execute from the use case instance passing the input
1. Gets the response from use case in format Result<FooUseCase.Output>
1. In case of Result.IsSuccess the returns the output in the body with the
appropriate status code
1. In case of !Result.IsSuccess returns an error response with the title, and a
list of the errors found in the format 'type Error = { code, description }'. Also
set the status code depending on the type of error found

## Todos
1. Controllers no longer need the WebIO layer. UseCases will have Input and Output
 - Input  -> FooUseCase.Input
 - Output -> Result<FooUseCase.Output>

# Results instead of exceptions

## MilanJovanovic

1. Add Results instead of exceptions to UseCases. Exceptions are expensive and should
only be used for except cases in csharp + dotnet. Make something like rust result
to replace the exceptions + happy path

```txt
    Result { error, value }
    Error { code, message }

    Result<User> =>
        bool  Result.IsSuccess (has no errors),
        Error Result.Error     (returns the error)
        User  Result.Value     (returns the success value),

    Result.Error.Code == "User.NotFound"
    Result.Error.Message == "User not found by id"
```

Videos:
- [Get Rid of Exceptions in Your Code With the Result Pattern](https://www.youtube.com/watch?v=WCCkEe_Hy2Y)
- [2 Best Practices for Returning API Errors in ASP.NET Core](https://www.youtube.com/watch?v=YBK93gkGRj8)

2. Make the API response on error to be more than a message with a status code with
a message. Make it a object + status code.

With this format you can have more than one error per response

```json
Error Response: {
    "httpStatus": 400,
    "title": "Sign up has validation errors",
    "errors": [
        {
            "code": "Validation.Email",
            "description": "E-mail format is not valid"
        },
        {
            "code": "Validation.Password",
            "description": "Password is too short. Must be at least 6 characters"
        }
    ]
}
```

# CQRS

- all the cqrs should be inside the infra layer and hidden behind the data access
layers

- Impl cqrs with 2 databases (1 for reads and 1 for writes) no external libs

## Caching Queries

The query database is a sqlite + caching
[.NET Caching](https://learn.microsoft.com/en-us/dotnet/core/extensions/caching)
