# Todos CSharp

Todos in csharp using TDD and AspNetCore 6.0.100

_Web Framework = AspNetCore 6_

_Database = PostgreSQL_

## Required Routes

for async route just add '/async' in the end of route

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

        [GET] api/tasks

        Find all tasks of an user by his/her id

        (id is acquired from the auth token, not needed in the url)

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

* DeleteDone => void

        (Private/User)

        [DELETE] api/todos/done

        Delete done todos by userId

        (userId comes from auth token)

* DeleteDoneByTaskId => void

        (Private/User)

        [DELETE] api/todos/done/task/{taskId}

        Delete done todos for a task

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

        [PATCH] api/todos/setdone/{todoId}

        Set todo as complete by its id

* SetTodoAsNotDone => void

        (Private/User)

        [PATCH] api/todos/setnotdone/{todoId}

        Set todo as NOT complete by its id

* UpdateTodo => void

        (Private/User)

        [PUT] api/todos/{todoId}

        Update a todo by its id and updatedBody
