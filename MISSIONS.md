# ToDos CSharp

## Code Missions (Features and Modifications to be made)

- [x] (06/10/2022) Change IEnumerable to IList in Find Tasks By UserId
- [x] (06/10/2022) [Feature] Task Update
- [x] (07/10/2022) [Feature] Create Todo
- [x] (08/10/2022) [Feature] Find Todos By Task Id
- [x] (08/10/2022) [Feature] Find Todo By Id
- [x] (10/10/2022) [Feature] Set Todo As Done
- [x] (10/10/2022) [Feature] Set Todo As NOT Done
- [x] (10/10/2022) [Feature] Update Todo
- [x] (10/10/2022) [Feature] Delete Todo
- [x] (10/10/2022) [Feature] Delete Done Todos
- [x] (17/10/2022) [Feature] Delete Done Todos By TaskId
- [x] (17/10/2022) Refactor Exception handler to return a 401 response for auth exceptions and
AuthMiddleware to throw InvalidRequestAuthException on Errors for ExceptionMiddleware to get
- [ ] (25/10/2022) [Change] Async all pipe up and down
- [ ] (25/10/2022) [Change] Implement ConnectionPool and test the speed diference
- [ ] (25/10/2022) [Auth] Create exeception AuthUserNotFoundException and replace UserNotFoundException
where the user not found is the one from authUserId (Auth related)


## Testing

- [ ] (25/10/2022) Create a test script that tests every end point of the application
- [ ] (25/10/2022) Test with a in memory database so the database can be erased and created without
any problems


## Future Nice to Have:

- [x] (06/10/2022) NotResourceOwnerExceptions when the userId from authToken is not the same as the
resource's userId
- [x] (07/10/2022) [Refactor] Change the names in IUserDataAccess and UserDataAccess
- [x] (10/10/2022) [Refactor] Change TodoDbDto, TaskDbDto and UserDbDto from string IDs to Guid so you
can use it in Query and not use dynamic objects with dapper anymore.
- [x] (13/10/2022) [Refactor] Use middleware to get authUserId.
- [x] (13/10/2022) [Refactor] Use middleware to get db connection.
- [x] (15/10/2022) [Refactor] Change the responseValue evaluations for response.Value method from the
DTO.
- [x] (15/10/2022) [TODO] Refactor the RequestPipeline class => replace comments with private methods
and remove extra private methods to center related code in the same place
- [x] (17/10/2022) [Refactor] Remove Id from UpdateTodoDto
- [x] (18/10/2022) [Refactor] Make the class and methods static for UsersWebIO, TasksWebIO and
TodosWebIO
- [x] (18/10/2022) [Refactor] Remove ! from WebIOs changing the useCases signatures
- [x] (21/10/2022) [Refactor] Use ExceptionHandler Middleware and remove try/catch from ApiControllers
- [x] (24/10/2022) [Fix] Change the 204 to 404 where cannot find for get
- [ ] (08/10/2022) [Fix] Discover how to make stupid csharp framework give 404 for route not found,
as any normal framework should, instead of 405 for some reason.


## DOCs

- [ ] (15/10/2022) Review home readme after all (base) Features are done.
