# Tech Trend Emporium

**Tech Trend Emporium** is an e-commerce application using the [FAKESTORE API](https://fakestoreapi.com/). This project is designed to provide a scalable and modular e-commerce solution while incorporating best practices in development and CI/CD.

## Features

- **Integrated API**: Utilizes the [FAKESTORE API](https://fakestoreapi.com/) for managing products, categories, and users.
- **Modular Structure**: Clear separation of concerns into different modules for scalability and maintainability.
- **CI/CD with SonarCloud**: Includes a CI/CD pipeline that checks code quality and test coverage.
- **Authentication and Authorization**: Handles user roles for controlled access to different sections of the system.

## Installation

1. Clone the repository:
   ```bash
   git clone https://github.com/Medeteam/tech-trend-emporium.git

2. Restore the necessary NuGet packages
   ```bash
   git clone https://github.com/Medeteam/tech-trend-emporium.git
   ```

## Used Packages

This project uses several NuGet packages to add key functionality to the application. Listed below are the packages used and a brief description of each:

### 1. **DotNetEnv (3.1.1)**
   - This package allows loading environment variables from a `.env` file in .NET applications. It is useful for storing settings such as database connection strings and API keys safely outside the source code.

### 2. **Microsoft.AspNetCore.Authentication.JwtBearer (8.0.8)**
   - Implements JWT (JSON Web Token) token-based authentication in ASP.NET Core applications, facilitating user authentication in distributed applications and APIs.

### 3. **Microsoft.AspNetCore.Authorization (8.0.8)**
   - Facilitates the implementation of authorization policies to control access to different parts of the application based on user roles and permissions.

### 4. **Microsoft.AspNetCore.Mvc.NewtonsoftJson (8.0.8)**
   - Integrates `Newtonsoft.Json` into ASP.NET Core to handle serialization and deserialization of JSON objects. It is mainly used for data exchange between client and server.

### 5. **Microsoft.EntityFrameworkCore (8.0.8)**
   - It provides tools to interact with databases using the ORM (Object Relational Mapping) pattern in .NET, facilitating data manipulation without the need to write SQL queries directly.

### 6. **Microsoft.EntityFrameworkCore.Design (8.0.8)**
   - It contains the necessary tools to perform database migrations and scaffolding when working with Entity Framework Core.

### 7. **Microsoft.EntityFrameworkCore.SqlServer (8.0.8)**
   - Provider of Entity Framework Core for SQL Server, allows working with SQL Server databases through EF Core.

### 8. **Microsoft.Extensions.Http (8.0.0)**
   - Provides enhancements to HTTP request handling, including error handling and resiliency through retry policies.

### 9. **Microsoft.Extensions.Http.Polly (8.0.8)**
   - Extends `HttpClient` with resiliency capabilities through integration with Polly, a library that provides features such as automatic retries and HTTP request failure handling.

### 10. **Microsoft.Extensions.Identity.Core (8.0.8)**
   - Provides core functionality for authentication and user management, including role and permission settings.

### 11. **Swashbuckle.AspNetCore (6.7.3)**
   - Tool that automatically generates a Swagger interface for the documentation of your ASP.NET Core APIs, facilitating the exploration and interaction with the APIs from a browser.


## Testing and Analysis Tools

### 1. **Bogus (35.6.1)**
   - Dummy data generator used to create realistic test data, useful for unit and integration testing.

### 2. **coverlet.collector (6.0.0)**
   - Tool to measure code coverage during .NET testing, helping to ensure that code is properly tested.

### 3. **Microsoft.EntityFrameworkCore.InMemory (8.0.8)**
   - EF Core provider that allows simulating an in-memory database for unit testing without the need for a real database.

### 4. **xunit (2.5.3)**
   - Unit testing framework used to write and run tests on .NET applications.

### 5. **xunit.runner.visualstudio (2.5.3)**
   - Integrates xUnit with Visual Studio, allowing to run and visualize tests from the development environment.

### 6. **Microsoft.NET.Test.Sdk (17.8.0)**
   - Tool that provides infrastructure to run tests in .NET projects, integrating with several testing frameworks such as xUnit, NUnit, and MSTest.

# USAGE
**To run the application locally, follow these steps:**

1. Build the solution
   ```bash
   dotnet build ./emporium/emporium.sln
   ```

2. Run the project
   ```bash
     dotnet run --project ./emporium/Emporium.Api
   ```

3. Access the Api
- Access the API at [http://localhost:5161/swagger/index.html](http://localhost:5161/swagger/index.html)

4. Data Base creation
- Install a DBMS of your liking that supports MySQL
- Create a database
- Create a dotenv file in the project
- Update the .env file in the project, especifically the SQLSERVER_CONECTION_STRING so it reflects your server conection and the database name. example: SQLSERVER_CONNECTION_STRING="Server=yourservername;Database=yourdatabasename;Integrated Security=True;TrustServerCertificate=True;"
- You need to update the .env with a property called JWT_KEY, it needs to be a string of 25 up to 40 characters
- You need to update the .env file with a property called JWT_ISSUER, this property need to have a value that is a string containing localhost and the port that the app is running in in this case 5161
- Using visual studio go to: Tools > Nuget package manager > Package console
- Using the console that displayed type in the following command: add-migration migrationname
- Wait till it finishes creating the migration
- Use the following command on the same console: update-database
- At this point you have the database created on your DBMS, you will need to use postman or swagger to populate the default Roles, Categories, Products and shopping statuses
- If you have any problem with authorizations for the routes you can comment the "Authorize" decorator on any route necessary so you can bypass the required policy

# Development Strategy
This project follows the trunk-based development strategy with the following rules:

- All contributions must be made via pull requests.
- Each pull request requires two approvals before being merged.
- Force pushes are not allowed on the main branch.

## Pipeline CI/CD
The CI/CD pipeline integrates with [SonarCloud](https://www.sonarsource.com/products/sonarcloud/) to:

- Scan the code and check for quality.
- Verify test coverage.
- Ensure no security vulnerabilities are introduced.

## Actions' pipeline status

**Main branch status**

![main branch status](https://github.com/Medeteam/tech-trend-emporium/actions/workflows/CI-CD.yml/badge.svg)


## Contribute
If you would like to contribute to this project:

- Frotk this repository.
- Create a new branch
```bash
    git checkout -b Feature/my-feature
```
- Commit you changes
```bash
    git commit -m "Add new feature"
```
- Push you Branch
```bash
    git push origin my-feature-branch
```
- Open a pull request on GitHub