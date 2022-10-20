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
- [ ] (17/10/2022) [Feature] Delete Done Todos By TaskId
- [x] (17/10/2022) Refactor Exception handler to return a 401 response for auth exceptions and 
AuthMiddleware to throw InvalidRequestAuthException on Errors for ExceptionMiddleware to get


## Future Nice to Have:

- [ ] (06/10/2022) NotResourceOwnerExceptions when the userId from authToken is not the same as the 
resource's userId
- [ ] (07/10/2022) [Refactor] Change the names in IUserDataAccess and UserDataAccess
- [ ] (08/10/2022) [Fix] Discover how to make stupid csharp framework give 404 for route not found, 
as any normal framework should, instead of 405 for some reason.
- [ ] (10/10/2022) [Refactor] Change TodoDbDto, TaskDbDto and UserDbDto from string IDs to Guid so you
can use it in Query and not use dynamic objects with dapper anymore.
- [ ] (13/10/2022) [Refactor] Use middleware to get authUserId.
- [ ] (13/10/2022) [Refactor] Use middleware to get db connection.
- [ ] (15/10/2022) [Refactor] Change the responseValue evaluations for response.Value method from the 
DTO.
- [ ] (15/10/2022) [TODO] Refactor the RequestPipeline class => replace comments with private methods 
and remove extra private methods to center related code in the same place
- [ ] (17/10/2022) [Refactor] Remove Id from UpdateTodoDto
- [ ] (18/10/2022) [Refactor] Make the class and methods static for UsersWebIO, TasksWebIO and 
TodosWebIO
- [ ] (18/10/2022) [Refactor] Remove ! from WebIOs changing the useCases signatures


## DOCs

- [ ] (15/10/2022) Review home readme after all (base) Features are done.
