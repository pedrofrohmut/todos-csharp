# Todos AspNetCore 6

Todos in csharp using TDD and AspNetCore 6.0.100

_Web Framework = AspNetCore 6_

_Database = PostgreSQL_

## Required Routes

### For users

* Sign Up => void
(Public/Guest)
[POST] api/users
Create users by newTodoBody and Hashes the password

* Sign In => signedUser
(Public/Guest)
[POST] api/users/signin
Verify user credentials and send a token and user info

* Verify => boolean
(Private/User)
[GET] api/users/verify
Checks if the token is valid by token in request headers

### For Tasks

* Create Task => void
(Private/User)
[POST] api/tasks
Creates a task for an user with taskBody and userId

* DeleteTask => void
(Private/User)
[DELETE] api/tasks/{taskId}
Deletes a task by its id

* FindById => Task
(Private/User)
[GET] api/tasks/{taskId}
Finds a task by its id

* FindByUserId => List(Task)
(Private/User)
[GET] api/tasks/user/{userId}
Find all tasks of an user by his/her id

* UpdateTask => void
(Private/User)
[PUT] api/tasks/{taskId}
Update a task by its id and updatedBody

### For Todos

* ClearCompletedTodos => void
(Private/User)
[DELETE] api/todos/clearCompleted
Delete all todos set as isCompleted to true of the authenticated user

* CreateTodo => void
(Private/User)
[POST] api/todos
Creates a todo for a task with todoBody and taskId

* DeleteTodo => void
(Private/User)
[DELETE] api/todos/{todoId}
Delete a todo by its id

* FindTodoById => Todo
(Private/User)
[GET] api/todos/{todoId}
Find a todo by its id

* FindTodosByTaskId => List(Todo)
(Private/User)
[GET] api/todos/task/{taskId}
Find all todos from a task by taskId

* SetTodoAsDone => void
(Private/User)
[PATCH] api/todos/complete/{todoId}
Set todo as complete by its id

* SetTodoAsNotDone => void
(Private/User)
[PATCH] api/todos/notcomplete/{todoId}
Set todo as NOT complete by its id

* UpdateTodo => void
(Private/User)
[PUT] api/todos/{todoId}
Update a todo by its id and updatedBody
