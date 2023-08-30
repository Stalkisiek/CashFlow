# CashFlow Application Documentation

## Table of Contents

- [Introduction](#introduction)
- [File and Folder Structure](#file-and-folder-structure)
- [Models](#models)
- [DTOs (Data Transfer Objects)](#dtos-data-transfer-objects)
- [DataContext](#datacontext)
- [Services](#services)
- - [AuthService](#authservice)
- - [RequestService](#requestservice)
- - [BankAccountService](#bankaccountservice)
- - [UserService](#userservice)

## Introduction

The CashFlow application documentation provides an overview of the application's architecture, models, data flow, and services. This documentation is written in Markdown format to ensure clarity and organization.

## File and Folder Structure

```plaintext
CashFlowApp
|-- Controllers
|   |-- AuthController.cs
|   |-- BankAccountController.cs
|   |-- RequestController.cs
|   |-- UpdateController.cs
|   |-- UserController.cs
|-- Data
|   |-- DataContext.cs
|-- Dtos
|   |-- Authorization
|   |   |-- LoginUserDto.cs
|   |   |-- RegisterUserDto.cs
|   |   |-- UpdateUserPasswordDto.cs
|   |-- BankAccount
|   |   |-- AddBankAccountDto.cs
|   |   |-- GetBankAccountDto.cs
|   |   |-- UpdateBankAccountDto.cs
|   |-- Request
|   |   |-- AddPreviousRequestDto.cs
|   |   |-- AddRequestDto.cs
|   |   |-- FulfillRequestDto.cs
|   |   |-- GetPreviousRequestDto.cs
|   |   |-- GetRequestDto.cs
|   |-- User
|   |   |-- GetUserDto.cs
|   |   |-- UpdateUserAuthorizationLevelDto.cs
|   |   |-- UpdateUserEmailDto.cs
|   |   |-- UpdateUserNamesDto.cs
|-- Models
|   |-- AuthorizationLevel.cs
|   |-- BankAccount.cs
|   |-- BankAccountType.cs
|   |-- Entity.cs
|   |-- PreviousRequest.cs
|   |-- Request.cs
|   |-- RequestAcceptMode.cs
|   |-- RequestType.cs
|   |-- ServiceResponse.cs
|   |-- User.cs
|-- Services
|   |-- AuthServices
|   |   |-- AuthRepository.cs
|   |   |-- IAuthRepository.cs
|   |   |-- SyntaxChecker.cs
|   |-- BankAccountServices
|   |   |-- BankAccountService.cs
|   |   |-- IBankAccountService.cs
|   |-- RequestServices
|   |   |-- IRequestService.cs
|   |   |-- RequestService.cs
|   |-- UpdateServices.cs
|   |   |-- IUpdateService.cs
|   |   |-- UpdateService.cs
|   |-- UserServices
|   |   |-- IUserService.cs
|   |   |-- UserService.cs
|-- AutoMapperProfile.cs
|-- Program.cs
```

## Models

### User

- **Id:** User's unique identifier
- **Name:** User's first name
- **Surname:** User's last name
- **Email:** User's email address
- **PH:** PasswordHash
- **PS:** PasswordSalt
- **AuthLvl:** User's authorization level (enum: User = 1, Admin = 2, SuperAdmin = 3)
- **CreatedAt:** Timestamp of user creation
- **UpdatedAt:** Timestamp of user data update

### AuthLvl (Authorization Level)

- **User:** Regular user with limited permissions (enum value: 1)
- **Admin:** Administrator with higher privileges (enum value: 2)
- **SuperAdmin:** Highest level of administrative privileges (enum value: 3)

### ServiceResponse

- **Data:** Main return type for service responses
- **Success:** Indicates whether the operation was successful
- **StatusCode:** HTTP status code
- **Message:** Message describing the operation's result

### Request

- **Id:** Request's unique identifier
- **UserId:** Id of the user who initiated the request
- **AccountId:** Id of the associated bank account
- **ReqType:** Type of the request (enum: DeleteUser = 1, DeleteAccount = 2, AddMoney = 3, AddCredit = 4)
- **Balance:** Balance before doing request
- **BalanceAmmount:** Amount for request
- **BalanceFinally:** Balance after request
- **Credit:** Placeholder Credit before request
- **CreditAmmount:** Amount for request
- **CreditFinally:** Credit after request

### ReqType (Request Type)

- **DeleteUser:** Request to delete a user (enum value: 1)
- **DeleteAccount:** Request to delete an account (enum value: 2)
- **AddMoney:** Request to add money to an account (enum value: 3)
- **AddCredit:** Request to add credit to an account (enum value: 4)

### PreviousRequest

- **Id:** Request's unique identifier (used for creating a more user-friendly database)
- **RequestId:** Id of the main request
- **ReqType:** Type of the request (enum: Accepted = 1, Rejected = 2, Pending = 3, Deleted = 4)
- **Status:** Status of the request (enum: Accepted, Rejected, Pending, Deleted)
- **UserId:** Id of the user who initiated the request
- **Message:** Message to tell something about admin deccision 

### ReqAccMode (Request Accept Mode)

- **Accepted:** Request has been accepted (enum value: 1)
- **Rejected:** Request has been rejected (enum value: 2)
- **Pending:** Request is pending (enum value: 3)
- **Deleted:** Request has been deleted (enum value: 4)

### BankAccount

- **Id:** Bank account's unique identifier
- **BankAcType:** Type of bank account (enum: Savings = 1, Credit = 2)
- **Name:** Name of the bank account (combination of name, surname, and type)
- **Balance:** Current balance of the bank account
- **Credit:** Current credit ballance of the bank account
- **CreatedAt:** Timestamp of bank account creation
- **UpdatedAt:** Timestamp of bank account data update
- **UserId:** Id of the user who owns the bank account

### BankAcType (Bank Account Type)

- **Savings:** Savings bank account (enum value: 1)
- **Credit:** Credit bank account (enum value: 2)

## DTOs (Data Transfer Objects)

### Authorization DTOs

#### LoginUserDto

- **Email:** User's email address
- **Password:** User's password

#### RegisterUserDto

- **Name:** User's first name
- **Surname:** User's last name
- **Email:** User's email address
- **Password:** User's password

#### UpdateUserPasswordDto

- **CurrentPassword:** User's current password
- **NewPassword:** User's new password

### BankAccount DTOs

#### AddBankAccountDto

- **Type:** Type of bank account to add (enum: Savings = 1, Credit = 2)



#### GetBankAccountDto

- **Id:** Bank account's unique identifier
- **Type:** Type of the bank account
- **Name:** Name of the bank account
- **Balance:** Current balance of the bank account
- **Credit:** Current credit balance of the bank account
- **CreatedAt:** Timestamp of bank account creation
- **UpdatedAt:** Timestamp of bank account data update

#### UpdateBankAccountDto

- **Id:** Bank account's unique identifier
- **Type:** Type of the bank account
- **Balance:** New balance of the bank account
- **CreditBalance:** Current credit balance of the bank account

### Request DTOs

#### AddRequestDto

- **Type:** Type of the request (enum: DeleteUser = 1, DeleteAccount = 2, AddMoney = 3, AddCredit = 4)
- **AccountId:** Id of the associated bank account
- **ChangeBalance:** Amount to change balance
- **ChangeCredit:** Amount to change credit

#### GetRequestDto

- **Id:** Request's unique identifier
- **UserId:** Id of the user who initiated the request
- **Type:** Type of the request
- **Balance:** Balance before doing request
- **BalanceAmmount:** Amount for request
- **BalanceFinally:** Balance after request
- **Credit:** Placeholder Credit before request
- **CreditAmmount:** Amount for request
- **CreditFinally:** Credit after request

#### FulfillRequestDto

- **Id:** Request's unique identifier
- **Accepted:** Is request accepted
- **Message:** Message from admin

#### AddPreviousRequestDto

- **Id:** Request's unique identifier
- **UserId:** Id of the user who initiated the request
- **Typ:** Type of the request
- **Status:** Status of the request (enum: Accepted = 1, Rejected = 2, Pending = 3, Deleted = 4)

#### GetPreviousRequestDto

- **Id:** Request's unique identifier
- **UserId:** Id of the user who initiated the request
- **RequestId:** Id of the main request
- **Type:** Type of the request
- **Status:** Status of the request
- **Message:** Message from admin

### User DTOs

#### GetUserDto

- **Id:** User's unique identifier
- **Name:** User's first name
- **Surname:** User's last name
- **Email:** User's email address
- **AuthLvl:** User's authorization level
- **CreatedAt:** Timestamp of user creation
- **UpdatedAt:** Timestamp of user data update

#### UpdateUserAuthorizationLevelDto

- **Id:** User's unique identifier
- **lvl:** New authorization level for the user

#### UpdateUserEmailDto

- **Id:** User's unique identifier
- **Email:** New email address for the user

#### UpdateUserNamesDto

- **Id:** User's unique identifier
- **Name:** New first name for the user
- **Surname:** New last name for the user

## DataContext

The DataContext class is used for creating database contexts and overriding the OnModelCreating method. It creates the following database tables:

- Users
- BankAccounts
- Requests
- PreviousRequests

### User

**BasicInfo:**
Methods declared in IUserService and implemented in UserService. Called in UserController. The controller uses ActionResult and returns StatusCode(response.StatusCode, response).

**Declared Methods and Descriptions:**

**Private GetUserId ->** 
- Input: None
- Checks: None
- Output: Currently logged-in user
- AdditionalInfo: Description not needed 

**Private GetUserAuthLvl ->** 
- Input: None
- Checks: None
- Output: Authorization level of currently logged-in user
- AdditionalInfo: Description not needed

**GetAllUsers ->** 
- Input: None
- Checks: Authorization level
- Output: List of user data DTOs
- AdditionalInfo: Description not needed

**GetCurrentUser ->** 
- Input: None
- Checks: User exists
- Output: DTO with user data
- AdditionalInfo: Description not needed

**GetUserById ->** 
- Input: User ID
- Checks: User exists, authorization level
- Output: DTO with user data
- AdditionalInfo: Works for any user when their ID is provided, and for any user by an administrator

**UpdateUserEmail ->** 
- Input: DTO with ID and new email
- Checks: User exists, authorization level, valid email (using SyntaxChecker.cs), email uniqueness in the database
- Output: DTO with user data
- AdditionalInfo: Logged-in user can change their own email; authorized user can overwrite other users' data. Email must be unique.

**UpdateUserNames ->** 
- Input: DTO with ID, first name, and last name
- Checks: User exists, authorization level, valid name (using SyntaxChecker.cs)
- Output: DTO with user data
- AdditionalInfo: Logged-in user can change their own name; authorized user can overwrite other users' data. Email must be unique.

**UpdateUserAuthorizationLevel ->** 
- Input: DTO with ID and authorization level
- Checks: User exists, authorization level, valid authorization level
- Output: DTO with user data
- AdditionalInfo: Description not needed

**UpdateUserPassword ->** 
- Input: DTO with old and new passwords
- Checks: User exists, valid old password, difference between old and new password
- Output: DTO with user data
- AdditionalInfo: Can only be used by logged-in user

**DeleteUserById ->** 
For admin:
- Input: User ID
- Checks: User exists, authorization level
- Output: List of user data DTOs
- AdditionalInfo: Checks if the logged-in user has higher authorization level than the target user
For user:
- Input: User ID
- Checks: User exists, account ownership
- Output: None
- AdditionalInfo: Creates a request using CreateRequest

**DeleteUserById ->** 
For admin:
- Input: User ID
- Checks: User exists, authorization level
- Output: None
- AdditionalInfo: Checks if the logged-in user has higher authorization level than the target user
For user:
- Input: User ID
- Checks: User exists
- Output: None
- AdditionalInfo: Creates a request using CreateRequest

### AuthServices

**BasicInfo:**
Methods declared in IAuthRepository and implemented in AuthRepository. Called in AuthController. The controller uses ActionResult and returns StatusCode(response.StatusCode, response). SyntaxChecker is also used internally.

**SyntaxChecker:**

**IsValidEmail ->** 
- Input: Email
- Checks: Email pattern @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$"
- Output: bool
- AdditionalInfo: Description not needed

**IsValidName ->**
- Input: Name
- Checks: Name pattern @"^[A-Za-z]{2,}$"
- Output: bool
- AdditionalInfo: Description not needed

**AuthRepository:**

**Private GetUserId ->** 
- Input: None
- Checks: None
- Output: Currently logged-in user
- AdditionalInfo: Description not needed 

**UserExists ->** 
- Input: Email
- Checks: None
- Output: bool
- AdditionalInfo: Description not needed

**Login ->** 
- Input: DTO with Email and Password
- Checks: User exists, valid password (VerifyPasswordHash)
- Output: JSON token
- AdditionalInfo: Description not needed

**Register ->**  
- Input: DTO with Name, Surname, Email, and Password
- Checks: User doesn't exist, valid email, valid names
- Output: ID of the created user
- AdditionalInfo: Description not needed

**CreatePasswordHash ->** 
- Input: Password, out PasswordHash, out PasswordSalt
- Checks: None 
- Output: None
- AdditionalInfo: Generates and saves data to the provided arrays

**VerifyPasswordHash ->** 
- Input: Password, PasswordHash, PasswordSalt
- Checks: None
- Output: bool
- AdditionalInfo: Description not needed

**CreateToken ->**  
- Input: User
- Checks: None
- Output: JSON token
- AdditionalInfo: Description not needed

### RequestServices

**BasicInfo:**
Methods declared in IRequestService and implemented in RequestService. Called in RequestController. The controller uses ActionResult and returns StatusCode(response.StatusCode, response). The application uses requests to assign tasks from non-privileged accounts to be reviewed and executed/rejected by users with appropriate authorization levels.

**Private GetUserId ->** 
- Input: None
- Checks: None
- Output: Currently logged-in user
- AdditionalInfo: Description not needed
 
**Private GetUserAuthLvl ->** 
- Input: None
- Checks: None
- Output: Authorization level of currently logged-in user
- AdditionalInfo: Description not needed

**GetAll ->** 
- Input: None
- Checks: Authorization level of currently logged-in user
- Output: List of DTOs with detailed data from the Requests table
- AdditionalInfo: Full data returned

**GetAllWithinUser ->** 
- Input: User ID, bool showOnlyPending
- Checks: Admin or query about a specific user
- Output: List with simplified information, along with the status of each request
- AdditionalInfo: showOnlyPending allows filtering and returning only unresolved requests

**CreateRequest ->** 
- Input: Request DTO with Type, AccountId, ChangeBalance, ChangeCredit
- Checks: Validity of the request type
- Output: DTO with data about the created request
- AdditionalInfo: Adds a PreviousRequest object with a Pending status to the appropriate array

**Fulfill ->** 
- Input: Request ID, Accepted flag, Message
- Checks: Authorization level, request existence in the database, request existence in PreviousRequests, User existence
- Output: ID of the executed request
- AdditionalInfo:
   - For DeleteUser:
     - Input: ID, Accepted, Message
     - Output: ID of the executed request
     - AdditionalInfo: Upon user deletion, all connected requests are also removed
   - For DeleteAccount:
     - Input: ID, Accepted, Message
     - Output: ID of the executed request
     - AdditionalInfo: None
   - For AddMoney:
     - Input: ID, Accepted, Message
     - Output: ID of the executed request
     - AdditionalInfo: Adds to balance


   - For AddCredit:
     - Input: ID, Accepted, Message
     - Output: ID of the executed request
     - AdditionalInfo: Adds to balance and credit

### BankAccountServices

Methods declared in IBankAccountService and implemented in BankAccountService. Called in BankAccountController. The controller uses ActionResult and returns StatusCode(response.StatusCode, response).

**CreateBankAccount ->** 
- Input: DTO with account type to create
- Checks: User exists in the database, valid and declared account type, user doesn't have an account of this type (can have only one of each)
- Output: DTO with bank account information
- AdditionalInfo: Account name is set as name_surname_type; theoretically, names can repeat but they are not unique.

**GetBankAccountById ->** 
- Input: Bank account ID
- Checks: Bank account exists, permissions (user's account or permission level > user)
- Output: DTO with bank account information

**GetAll ->
- Input: None
- Checks: Permissions (level > user)
- Output: List of DTOs with bank account information

**GetAllWithinUser ->** 
- Input: None
- Checks: User exists in the database
- Output: List of DTOs with bank account information

**UpdateBankAccount ->** 
- Input: DTO with target account ID, type, balance, credit balance
- Checks: Permissions (permission level > user), bank account exists, type is valid, type is not repeated (only one of each type allowed)
- Output: DTO with bank account information
- AdditionalInfo: Helper method for remotely overwriting account information.

**DeleteBankAccount ->** 
- Input: Bank account ID
- Checks: Bank account exists, permissions
- Output: List of DTOs with bank account information
- AdditionalInfo: This method has two different functionalities. If the user has the necessary permissions, the account is deleted. However, if the user doesn't have the permissions but performs the action on their own account, a request is created using CreateRequest in IRequestService.

**AddBalance ->** 
- Input: Bank account ID, amount
- Checks: Bank account exists, permission level, account ownership, amount > 0
- Output: DTO with bank account information
- AdditionalInfo: This method has two different functionalities. If the user has the necessary permissions, the balance is added to the account. However, if the user doesn't have the permissions but performs the action on their own account, a request is created using CreateRequest in IRequestService.

**SubtractBalance ->** 
- Input: Bank account ID, amount
- Checks: Bank account exists, amount > 0, authorization level, feasibility of operation (to avoid going into the negative)
- Output: DTO with bank account information
- AdditionalInfo: This method has two different functionalities. If the user has the necessary permissions, the amount is subtracted from the account balance (checking if it's feasible). However, if the user doesn't have the permissions but performs the action on their own account, they can also perform this operation.

**TransferBalance ->** 
- Input: User's bank account ID, target bank account ID, amount
- Checks: Both bank accounts exist, the user's bank account is valid, the user's bank account has enough funds
- Output: DTO with bank account information
- AdditionalInfo: Only works as a transfer from the currently logged-in user's account. No need to add administration functionality.

**AddCredit ->** 
- Input: Bank account ID, amount
- Checks: Bank account exists, authorization (authorized users can add without a request), current credit value compared to maximum, account ownership
- Output: DTO with bank account information
- AdditionalInfo: One credit request at a time; constants define the maximum credit value.

**PayCredit ->** 
- Input: Bank account ID, amount
- Checks: Bank account exists, authorization or account ownership, sufficient funds
- Output: DTO with bank account information
- AdditionalInfo: If the user attempts to pay too much for the credit, it will be considered, and only the required amount will be deducted.

**Relationships:**
User <-> Requests -> one to many
User <-> PreviousRequests -> one to many
User <-> BankAccounts -> one to many
