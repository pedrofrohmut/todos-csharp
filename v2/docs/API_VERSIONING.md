# API v2.0 Documentation

## Why v2.0?

This document explains the rationale behind breaking changes and the architectural decisions made for API version 2.0.

### Motivation

_Describe why you are creating v2.0. Examples:_

- _Major architectural refactoring_
- _Breaking changes that cannot be done backward-compatibly_
- _Shift in domain model or business logic_
- _Technology stack changes_
- _Performance improvements requiring schema changes_

### Key Changes from v1

| Aspect | v1 | v2 |
|--------|----|----|
| _Add as needed_ | | |

### Migration Guide

_Describe how consumers can migrate from v1 to v2_

### Deprecation Notice

_If applicable, document what is deprecated and when v1 will be removed_

___________________________________________________________________________________________________

# 1.
In the Use Cases, changed when business errors happens from throwing and Exception to return a
Result.Error. Because Exceptions are expensive in dotnet and to use the result pattern instead.

# 2.
Using CQRS instead of just DataAccess to manipulate the persistence. Also using 2 databases one
for reading and one for writing. This way also having a backup. Maybe just having a backup db
would have the same effect, but there is no need to change it now.

# 3.
Having a UseCasesFactory to abstract away from controllers the instantiation of usecases but keeping
dbConnection in the controller to close the connection in finally block and passing only the
readDbConnection when no writing is needed

# 4.
Removed the WebIO layer the status code and the body will be decided in the controller based on
the result returned by the use case.

# 5.
All the usecases now will have an input and output only for them so changes in one usecase will
not affect others and the input and output can be less generic and have only what is needed.

# 6.
Check if needed: Make API error responses more complex in the format
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

# 7.
Check out if there something that makes the read db faster to read then the write db. So there
is a better reason for the CQRS then just using a second db as backup (that could be done with
data access too).
