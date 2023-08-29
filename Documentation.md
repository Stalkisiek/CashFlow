# CashFlow Application Documentation

## Notes
When referring to "authorization" it signifies one of the following criteria:
1. Works if the id == id of the current user.
2. Current user has an authorization level greater than User.
3. Works only if the target has a lower authorization level.
4. Combination of the above criteria.

User refers to individuals registered with the bank. Account represents a single bank account registered to a user.

**PreviousRequests** serves to collect information about ongoing and completed requests, forming a history of executed requests.

## File and Folder Structure
```
- Controllers
  - AuthController.cs
  - BankAccountController.cs
  - RequestController.cs
  - UserController.cs
- Data
  - DataContext.cs
- Dtos
  - Authorization
    - LoginUserDto.cs
    - RegisterUserDto.cs
    - UpdateUserPasswordDto.cs
  - BankAccount
    - AddBankAccountDto.cs
    - GetBankAccountDto.cs
    - UpdateBankAccountDto.cs
  - Request
    - AddPreviousRequestDto.cs
    - AddRequestDto.cs
    - FulfillRequestDto.cs
    - GetPreviousRequestDto.cs
    - GetRequestDto.cs
  - User
    - GetUserDto.cs
    - UpdateUserAuthorizationLevelDto.cs
    - UpdateUserEmailDto.cs
    - UpdateUserNamesDto.cs
- Models
  - AuthorizationLevel.cs
  - BankAccount.cs
  - BankAccountType.cs
  - Entity.cs
  - PreviousRequest.cs
  - Request.cs
  - RequestAcceptMode.cs
  - RequestType.cs
  - ServiceResponse.cs
  - User.cs
- Services
  - AuthServices
    - AuthRepository.cs
    - IAuthRepository.cs
    - SyntaxChecker.cs
  - BankAccountServices
    - BankAccountService.cs
    - IBankAccountService.cs
  - RequestServices
    - IRequestService.cs
    - RequestService.cs
  - UserServices
    - IUserService.cs
    - UserService.cs
- AutoMapperProfile.cs
- Program.cs
```

## Detailed Description

### Models
- **User**: Holds Id, Name, Surname, Email, PH, PS, AuthLvl, CreatedAt, UpdatedAt.
- **AuthLvl**: Enumeration of User = 1, Admin = 2, SuperAdmin = 3.
- **ServiceResponse**: Primary return type. Contains Data, Success, StatusCode, and Message.
- **Request**: Contains Id, UserId, ReqType, and financial information.
- **ReqType**: Enumeration for DeleteUser = 1, DeleteAccount = 2, AddMoney = 3, AddCredit = 4, RemoveCredit = 5.
- **PreviousRequest**: Used to create a more user-friendly database. Its elements are displayed to users. Contains Id, RequestId (main request), ReqType, Status (ReqAccMode), UserId.
- **ReqAccMode**: Enumeration for Accepted = 1, Rejected = 2, Pending = 3, Deleted = 4.
- **BankAccounts**: Currently empty, to be populated later.

### Dtos
Used for communication between methods and HTTP requests, keeping information minimal.

### DataContext
Responsible for database creation and OnModelCreating method overrides.

## Services

### User
Methods declared in IUserService and implemented in UserService. Called in UserController.
Controller returns ActionResult with appropriate StatusCode (response.StatusCode) and response content.

Detailed method descriptions:
- Private **GetUserId**: Description not necessary.
- Private **GetUserAuthLvl**: Description not necessary.
- **GetAllUsers**: Returns a list of Dtos with all user data. Authorized method.
- **GetCurrentUser**: Returns Dto of the currently logged-in user.
- **GetUserById**: Returns Dto of the specified user by Id. Authorized method.
- **UpdateUserEmail**: Returns Dto of the user with updated data. This and subsequent methods (except password) can remotely update another user's data with proper authorization. Checks email validity using SyntaxChecker.cs (explained in authorization methods) and if it has been used before.
- **UpdateUserNames**: Returns Dto of the user with updated data. Checks validity of name and surname using SyntaxChecker.cs (separately for each).
- **UpdateUserAuthorizationLevel**: Returns Dto of the user with updated data. Highly authorized method. Validates the correctness of the new authorization level.
- **UpdateUserPassword**: Returns Dto of the user with updated data. Authorized method, checks validity of old password.
- **DeleteUserById**: Returns a list of Dtos with all user data. Highly authorized method, works the same way as DeleteCurrentUser for oneself (adjusted for different authorizations).
- **DeleteCurrentUser**: Essentially returns nothing (template set to ServiceResponse<string>). Authorized. Authorization level > User automatically allows self-deletion; others initiate a request using CreateRequest method in RequestService.

### AuthServices
Methods declared in IAuthRepository and implemented in AuthRepository. Called in AuthController.
Controller returns ActionResult with appropriate StatusCode (response.StatusCode) and response content.

**SyntaxChecker**: Contains two methods. One validates email using pattern {"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$"}, and the other validates name using pattern {"^[A-Za-z]{2,}$"}.

- **UserExists**: Checks if a user exists in the database based on their email.
- **Login**: Returns a JSON token. Checks user existence and correct password input.
- **Register**: Accepts name, surname, email, and password. Checks if email is already used and its validity, creates psHash and salt, adds user to the database.
- **CreatePasswordHash**: Description not necessary.
- **VerifyPasswordHash**: Description not necessary.
- **CreateToken**: Description not necessary.

### RequestServices
Methods declared in IRequestService and implemented in RequestService. Called in RequestController.
Controller returns ActionResult with appropriate StatusCode (response.StatusCode) and response content.
The application employs requests to generate proposals from non-privileged accounts, which can be evaluated by authorized individuals to execute/reject them.

- **GetUserId**: Description not necessary.
- **GetUserAuthLvl**: Description not necessary.
- **GetAll**: Returns a list with complete request data (GetRequestDto). Authorized method.
- **GetAllWithinUser**: Returns a list with simplified request data (GetPreviousRequestDto). Searches by Id. Everyone can see their own requests. Authorized method.
- **CreateRequest**: Returns this request. Checks type validity, prevents duplicate requests, adds the appropriate type request to the request and previousRequests Db. Helper method for other methods.
- **Fulfill**: Returns the id of the executed request. Authorized, removes the executed/rejected request, updates the status of this request in PreviousRequests, performs appropriate operations based on the type.

### Relationships
- **User** <-> **Requests**: One to many relationship.
- **User** <-> **PreviousRequests**: One to many relationship.
- **User** <-> **BankAccounts**: One to many relationship.
