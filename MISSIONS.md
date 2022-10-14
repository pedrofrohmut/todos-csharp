# ToDos CSharp

## Code Missions (Features and Modifications to be made)

- [x] (06/10/2022) Change IEnumerable to IList in Find Tasks By UserId
- [x] (06/10/2022) Add feature for task update
- [x] (07/10/2022) Feature for create todo
- [x] (08/10/2022) Feature Find Todos By Task Id
- [x] (08/10/2022) Feature Find Todo By Id
- [x] (10/10/2022) Feature Set Todo As Done
- [x] (10/10/2022) Feature Set Todo As NOT Done
- [ ] (10/10/2022) Feature Update Todo
- [ ] (10/10/2022) Feature Delete Todo
- [ ] (10/10/2022) Feature Clear Done Todos


## Future Nice to Have:

- [ ] (06/10/2022) NotResourceOwnerExceptions when the userId from authToken is not the same as the 
resource's userId
- [ ] (07/10/2022) [Refactor] Change the names in IUserDataAccess and UserDataAccess
- [ ] (08/10/2022) [Fix] Discover how to make stupid csharp framework give 404 for route not found, 
as any normal framework should, instead of 405 for some reason.
- [ ] (10/10/2022) [Refactor] Change TodoDbDto, TaskDbDto and UserDbDto from string IDs to Guid so you
can use it in Query and not use dynamic objects with dapper anymore.
- [ ] (13/10/2022) [Refactor] Use middleware to get authUserId
- [ ] (13/10/2022) [Refactor] Use middleware to get db connection
