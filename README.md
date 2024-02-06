![image](https://github.com/Stalkisiek/CashFlow/assets/117647150/cddec943-0c53-4217-861e-390d0eb691de)# 🌟 CashFlow Web Application 🌟

Welcome to the CashFlow Web Application! This application provides users with a streamlined web interface for efficient banking operations. It's built using the .NET Web API framework, Entity Framework for database management, and modern web development practices.

## Table of Contents

- 🎥 [Video Tutorial](#video-tutorial)
- ✨ [Features](#features)
- 📂 [Folder Structure](#folder-structure)
- 📂 [Frontend Folder Structure](#frontend-folder-structure)
- 🚀 [Getting Started](#getting-started)
- 🔒 [Authentication](#authentication)
- 🔗 [Advanced Relationships](#advanced-relationships)
- 👤 [Getting User ID](#getting-user-id)
- 📷 [Screenshots](#screenshots)
- 🚀 [Contributions and Feedback](#contributions-and-feedback)
- 📜 [License](#license)

## 🎥 Video Tutorial

Watch this comprehensive video tutorial to learn how to use all the features of the CashFlow Web Application:

[![CashFlow Web App Tutorial](https://imgur.com/a/VKsorQD)](https://www.youtube.com/watch?v=gdAbCZMJegM&t=333s&ab_channel=Stalkisiek)

## Features

- 💼 **Account Management**: Users can create, update, and manage their bank accounts.
- 📝 **Request Handling**: Users can submit requests for actions like updating or deleting accounts.
- 🔐 **Authentication**: Secure user registration and login using JWT (JSON Web Tokens).

## Folder Structure

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

## Getting Started

To get started with the CashFlow Web Application, follow these steps:

1. 📥 Clone the repository: `git clone https://github.com/your-username/cashflow-web-app.git`
2. 📂 Navigate to the project directory: `cd cashflow-web-app`
3. 🛠 Install dependencies: `npm install`
4. 🚀 Start the development server: `npm start`
5. 🌐 Access the application: Open a browser and go to `https://stalkisiek.com`

## Authentication 🔒

The application uses JWT (JSON Web Tokens) for authentication. Users can register, log in, and receive a token for API access. Token validation ensures secure access to authorized endpoints.

## Advanced Relationships 🔗

The application supports advanced relationships between entities, such as one-to-one and many-to-many relationships. These relationships are established in the database models and mapped using Entity Framework.

## Getting User ID 👤

You can easily retrieve the logged-in user's ID for authorization and personalization purposes. The application extracts the user ID from the JWT token and validates access to specific functionalities using the `HttpContext`.

## Screenshots 📷

**Login Page**
![Login Page](https://imgur.com/a/jdY3qZm)
- Login option

**Register Page**
![Register Page](https://imgur.com/a/rJxelfL)

**Main Page**
![Main Page](https://imgur.com/a/i5rBBaF)

**Bank Account**
![Bank Account](https://imgur.com/a/ShYejdO)
- Bank account details with ID in the top right corner

**User Panel**
![User Panel](https://imgur.com/a/C7tbFHf)
- Administrative panel for users with filtering option

**Bank Accounts Panel**
![Bank Accounts Panel](https://imgur.com/a/NL6gfTY)
- Administrative panel for bank accounts with filtering option

**Requests Panel**
![Requests Panel](https://imgur.com/a/dzKl83I)
- Administrative panel for requests with filtering option

## Contributions and Feedback 🚀

Contributions and feedback are welcome! If you encounter any issues or have suggestions for improvements, please open an issue or create a pull request.

## License 📜

This project is licensed under the [MIT License](LICENSE).
