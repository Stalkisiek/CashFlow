# 🌟 CashFlow Web Application 🌟

Welcome to the CashFlow Web Application! This application provides users with a streamlined web interface for efficient banking operations. It's built using the .NET Web API framework, Entity Framework for database management, and modern web development practices.

## Table of Contents

- 🚀 [Getting Started](#getting-started)
  - 🔧 [Database Configuration](#database-configuration)
  - 🌐 [Backend Setup](#backend-setup)
  - 🌟 [Frontend Setup](#frontend-setup)
- 🎥 [Video Tutorial](#video-tutorial)
- ✨ [Features](#features)
- 📂 [Folder Structure](#folder-structure)
- 📂 [Frontend Folder Structure](#frontend-folder-structure)
- 🔒 [Authentication](#authentication)
- 🔗 [Advanced Relationships](#advanced-relationships)
- 👤 [Getting User ID](#getting-user-id)
- 📷 [Screenshots](#screenshots)
- 🚀 [Contributions and Feedback](#contributions-and-feedback)
- 📜 [License](#license)

## Getting Started

### Database Configuration 🔧

Before getting started, make sure to configure your database settings:

1. Change the database connection string in the backend project to match your database server.

### Backend Setup 🌐

To set up the backend of the CashFlow Web Application:

1. Clone the repository: `git clone https://github.com/your-username/cashflow-web-app.git`
2. Navigate to the project directory: `cd cashflow-web-app`
3. Apply database migrations: `dotnet ef database update`
4. Start the backend server: `dotnet run` in the `./Backend` directory.

### Frontend Setup 🌟

To set up the frontend of the CashFlow Web Application:

1. Navigate to the frontend directory: `cd ./Frontend/cashflow`
2. Install dependencies: `npm install`
3. Start the development server: `npm start`

Now, you have both the backend and frontend up and running!

## 🎥 Video Tutorial

Watch this comprehensive video tutorial to learn how to use all the features of the CashFlow Web Application:
[<img src="https://github.com/Stalkisiek/CashFlow/assets/117647150/05988d2c-d412-43b7-937f-08fa5672c1c5" alt="CashFlow Web App Tutorial" width="800" />](https://www.youtube.com/watch?v=gdAbCZMJegM&t=333s&ab_channel=Stalkisiek)

## Features

- 💼 **Account Management**: Users can create, update, and manage their bank accounts.
- 📝 **Request Handling**: Users can submit requests for actions like updating or deleting accounts.
- 🔐 **Authentication**: Secure user registration and login using JWT (JSON Web Tokens).

## Backend Folder Structure

The application follows a structured approach to organizing its components:

- 🎮 `Controllers`: Contains the API controllers that handle incoming HTTP requests.
- 📦 `Dtos`: Stores Data Transfer Objects for communication and data exchange.
- 📊 `Models`: Holds the core entity classes and database models.
- 🔧 `Services`: Contains business logic and services for various functionalities.
- 📁 `Data`: Includes the database context setup and migrations.
- 🌐 `AutoMapperProfile`: Configures object mappings for AutoMapper.

## Frontend Folder Structure

The frontend follows a structured approach to organizing its components:

- 🌟 `features`: Contains modules with various functionalities.
- 🖋 `fonts`: Stores custom fonts used in the application.
- ⚙️ `hooks`: Custom React hooks.
- 📄 `pages`: Page layout and routing configuration.
- 🖼️ `pictures`: Includes all the images used in the application.
- 💼 `types`: Definition of custom types and interfaces.

## Authentication 🔒

The application uses JWT (JSON Web Tokens) for authentication. Users can register, log in, and receive a token for API access. Token validation ensures secure access to authorized endpoints.

## Advanced Relationships 🔗

The application supports advanced relationships between entities, such as one-to-one and many-to-many relationships. These relationships are established in the database models and mapped using Entity Framework.

## Getting User ID 👤

You can easily retrieve the logged-in user's ID for authorization and personalization purposes. The application extracts the user ID from the JWT token and validates access to specific functionalities using the `HttpContext`.

## Screenshots 📷

To create a new line in a Markdown (.md) file after titles, you can use either two spaces at the end of the title line or insert a blank line between the title and the content. Here's how your provided content with new lines after titles might look:

**Login Page**  
<img src="https://github.com/Stalkisiek/CashFlow/assets/117647150/2d3acf5c-9ae8-480d-be5e-d9854fe35c0f" alt="Login Page" width="800" />

**Register Page**  
<img src="https://github.com/Stalkisiek/CashFlow/assets/117647150/c24cc544-5d13-463a-bfb9-6bc2a25eea8b" alt="Register Page" width="800" />

**Main Page**  
<img src="https://github.com/Stalkisiek/CashFlow/assets/117647150/582a718e-28cb-4659-833d-0999763d20d5" alt="Main Page" width="800" />

**Bank Account**  
<img src="https://github.com/Stalkisiek/CashFlow/assets/117647150/f581ab7b-644f-454e-98a9-83c224f61a7b" alt="Bank Account" width="800" />
- Bank account details with ID in the top right corner

**User Panel**  
<img src="https://github.com/Stalkisiek/CashFlow/assets/117647150/383d463a-d51b-497e-96cf-8a3b1a6ce360" alt="User Panel" width="800" />
- Administrative panel for users with filtering option

**Bank Accounts Panel**  
<img src="https://github.com/Stalkisiek/CashFlow/assets/117647150/822e2709-6abc-4b05-adfb-c78501f05838" alt="Bank Accounts Panel" width="800" />
- Administrative panel for bank accounts with filtering option

**Requests Panel**  
<img src="https://github.com/Stalkisiek/CashFlow/assets/117647150/058c1abc-12fd-4716-87c1-a23b51073e03" alt="Requests Panel" width="800" />
- Administrative panel for requests with filtering option

This format will create a new line after each title, making the content more readable in Markdown.

## Contributions and Feedback 🚀

Contributions and feedback are welcome! If you encounter any issues or have suggestions for improvements, please open an issue or create a pull request.

## License 📜

This project is licensed under the [MIT License](LICENSE).
