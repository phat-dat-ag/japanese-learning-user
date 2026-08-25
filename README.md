# Japanese Learning - User Service

User Service for the Japanese Learning application.

This service is responsible for user-related functionality and is built with ASP.NET Core Web API following a layered architecture.

---

## Tech Stack

- .NET 9
- ASP.NET Core Web API
- C#
- SQL Server
- Dapper
- MediatR
- FluentValidation
- Swagger / OpenAPI
- Health Checks
- xUnit

---

## Project Structure

```text
japanese-learning-user/
│
├── src/
│   ├── JapaneseLearning.User.Api/
│   │   ├── Common/
│   │   ├── Properties/
│   │   └── Program.cs
│   │
│   ├── JapaneseLearning.User.Application/
│   │   ├── Common/
│   │   │   └── Behaviors/
│   │   └── DependencyInjection.cs
│   │
│   ├── JapaneseLearning.User.Domain/
│   │
│   └── JapaneseLearning.User.Infrastructure/
│       ├── Database/
│       ├── HealthChecks/
│       └── DependencyInjection.cs
│
├── tests/
│   └── JapaneseLearning.User.UnitTests/
│
├── JapaneseLearning.User.sln
└── README.md
```

---

## Prerequisites

Before running the project, make sure the following are installed:

- Git
- .NET 9 SDK
- SQL Server

Verify .NET:

```bash
dotnet --version
```

The project targets:

```text
net9.0
```

---

## Getting Started

### 1. Clone the Repository

```bash
git clone <repository-url>
cd japanese-learning-user
```

Replace `<repository-url>` with the repository URL.

---

## Database Configuration

The application requires a SQL Server connection string.

The configuration key is:

```text
Database:ConnectionString
```

Example:

```text
Server=localhost,1433;Database=JapaneseLearningUser;User Id=sa;Password=YOUR_PASSWORD;TrustServerCertificate=True
```

Replace `YOUR_PASSWORD` with the actual SQL Server password.

**Do not commit database credentials or passwords to the repository.**

### Windows Environment Variable

You can configure the connection string using:

```cmd
setx Database__ConnectionString "Server=localhost,1433;Database=JapaneseLearningUser;User Id=sa;Password=YOUR_PASSWORD;TrustServerCertificate=True"
```

After running `setx`, open a new terminal.

Verify:

```cmd
echo %Database__ConnectionString%
```

---

## Database Requirements

The application expects the following database:

```text
JapaneseLearningUser
```

Make sure:

- SQL Server is running.
- `JapaneseLearningUser` exists.
- The configured user has access to the database.
- The connection string is correct.
- SQL Server is accessible from the application.

---

## Build

Restore dependencies:

```bash
dotnet restore
```

Build the solution:

```bash
dotnet build JapaneseLearning.User.sln
```

Expected result:

```text
Build succeeded.
```

---

## Run

Start the API:

```bash
dotnet run --project src/JapaneseLearning.User.Api
```

The default development endpoint is:

```text
http://localhost:5116
```

---

## Verify

### Swagger

Open:

http://localhost:5116/swagger

Swagger should load successfully.

### Health Check

Open:

http://localhost:5116/health

The health check should succeed when the application can connect to SQL Server.

---

## Architecture

The project follows a layered architecture:

```text
API
 │
 ▼
Application
 │
 ▼
Domain

Infrastructure
 ├── Database
 ├── Health Checks
 └── External Implementations
```

### API

Responsible for:

- HTTP endpoints
- API responses
- Exception handling
- Swagger
- Health checks
- HTTP-specific concerns

### Application

Responsible for:

- Application use cases
- MediatR
- Validation
- Pipeline behaviors
- Application services

Current MediatR pipeline:

```text
Request
   ↓
LoggingBehavior
   ↓
ValidationBehavior
   ↓
Handler
```

### Domain

Contains:

- Domain entities
- Business rules
- Core domain concepts

The Domain layer should remain independent from Infrastructure and framework-specific implementation details.

### Infrastructure

Responsible for:

- SQL Server
- Dapper
- Database connections
- Health checks
- Infrastructure configuration

---

## Error Handling

The API uses a global exception handler and a consistent response format.

Example:

```json
{
  "success": false,
  "data": null,
  "error": {
    "code": "VALIDATION_ERROR",
    "message": "One or more validation errors occurred.",
    "details": []
  },
  "traceId": "0H..."
}
```

The `traceId` can be used to correlate an API error with application logs.

Unexpected exceptions are handled centrally and should not expose sensitive implementation details to clients in production.

---

## API Response Format

Successful responses use the common API response structure:

```json
{
  "success": true,
  "data": {},
  "error": null,
  "traceId": "0H..."
}
```

Error responses use:

```json
{
  "success": false,
  "data": null,
  "error": {
    "code": "ERROR_CODE",
    "message": "Error message",
    "details": null
  },
  "traceId": "0H..."
}
```

---

## Logging

Application requests are logged through the MediatR logging pipeline.

The logging behavior records:

- Request name
- Execution time
- Successful requests
- Failed requests

Example:

```text
Handling CreateUserCommand
Handled CreateUserCommand in 25ms
```

Failed requests include the exception and execution time in the logs.

---

## Validation

Request validation is implemented using FluentValidation.

Validation runs through the MediatR pipeline before the request handler.

```text
API Request
    ↓
MediatR
    ↓
ValidationBehavior
    ↓
Validator
    ↓
Handler
```

If validation fails, the request handler is not executed.

---

## Testing

Run all tests:

```bash
dotnet test JapaneseLearning.User.sln
```

Unit tests are located in:

```text
tests/JapaneseLearning.User.UnitTests/
```

Tests should be added for meaningful business and application behavior.

Temporary test controllers or endpoints should be removed after development verification.

---

## Development Workflow

Create a feature branch from `develop`:

```bash
git switch develop
git pull
git switch -c feature/<feature-name>
```

Example:

```bash
git switch -c feature/user-registration
```

Before creating a Pull Request:

```bash
dotnet build JapaneseLearning.User.sln
dotnet test JapaneseLearning.User.sln
```

Review changes:

```bash
git status
git diff
```

Commit changes:

```bash
git add .
git commit -m "feat: implement user registration"
```

Push the feature branch:

```bash
git push -u origin feature/user-registration
```

Create the Pull Request with:

```text
Base: develop
Compare: feature/user-registration
```

---

## Development Rules

- Do not commit secrets or database passwords.
- Do not hard-code connection strings.
- Keep business logic out of controllers.
- Keep the Domain layer independent from Infrastructure.
- Put use-case logic in Application.
- Put database implementation in Infrastructure.
- Use the common API response format.
- Use the global exception handler.
- Add tests for meaningful application behavior.
- Remove temporary development code after verification.

---

## Common Commands

Restore dependencies:

```bash
dotnet restore
```

Build solution:

```bash
dotnet build JapaneseLearning.User.sln
```

Run tests:

```bash
dotnet test JapaneseLearning.User.sln
```

Run API:

```bash
dotnet run --project src/JapaneseLearning.User.Api
```

Check Git status:

```bash
git status
```

---

## Current Status

The project foundation is set up and ready for feature development.

Currently configured:

- Layered architecture
- SQL Server connection
- Dapper
- Database connection factory
- SQL Server health check
- Configuration Options
- MediatR
- MediatR validation pipeline
- MediatR logging pipeline
- Global exception handling
- Common API response
- Swagger / OpenAPI
- Development environment configuration
- Unit test project