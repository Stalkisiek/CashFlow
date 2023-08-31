# 🌟 CashFlow Web Application 🌟

Welcome to the CashFlow Web Application! This application provides users with a streamlined web interface for efficient banking operations. It's built using the .NET Web API framework, Entity Framework for database management, and modern web development practices.

## Table of Contents

- ✨ [Features](#features)
- 📂 [Folder Structure](#folder-structure)
- 🚀 [Getting Started](#getting-started)
- 🔒 [Authentication](#authentication)
- 🔗 [Advanced Relationships](#advanced-relationships)
- 👤 [Getting User ID](#getting-user-id)

## Features

- 💼 **Account Management**: Users can create, update, and manage their bank accounts.
- 📝 **Request Handling**: Users can submit requests for actions like updating or deleting accounts.
- 🔐 **Authentication**: Secure user registration and login using JWT (JSON Web Tokens).
- 📚 **Swagger Documentation**: Explore the API endpoints using the Swagger UI.

## Folder Structure

The application follows a structured approach to organizing its components:

- 🎮 `Controllers`: Contains the API controllers that handle incoming HTTP requests.
- 📦 `Dtos`: Stores Data Transfer Objects for communication and data exchange.
- 📊 `Models`: Holds the core entity classes and database models.
- 🔧 `Services`: Contains business logic and services for various functionalities.
- 📁 `Data`: Includes the database context setup and migrations.
- 🌐 `AutoMapperProfile`: Configures object mappings for AutoMapper.

## Getting Started

To get started with the CashFlow Web Application, follow these steps:

1. 📥 Clone the repository: `git clone https://github.com/your-username/cashflow-web-app.git`
2. 📂 Navigate to the project directory: `cd cashflow-web-app`
3. 🛠 Install dependencies: `dotnet restore`
4. 🗝 Update your database connection string in `appsettings.json`.
5. 🚀 Apply migrations to the database: `dotnet ef database update`
6. ▶️ Run the application: `dotnet run`
7. 🌐 Access the API documentation: Open a browser and go to `https://localhost:5001/swagger`.

## Authentication

The application uses JWT (JSON Web Tokens) for authentication. Users can register, log in, and receive a token for API access. Token validation ensures secure access to authorized endpoints.

## Advanced Relationships

The application supports advanced relationships between entities, such as one-to-one and many-to-many relationships. These relationships are established in the database models and mapped using Entity Framework.

## Getting User ID

You can easily retrieve the logged-in user's ID for authorization and personalization purposes. The `HttpContext` is used to extract the user ID from the JWT token and validate access to specific functionalities.

Feel free to explore the codebase, experiment with API endpoints, and customize the application according to your requirements.

## Contributions and Feedback

Contributions and feedback are welcome! If you encounter any issues or have suggestions for improvements, please open an issue or create a pull request.

## License

This project is licensed under the [MIT License](LICENSE).
