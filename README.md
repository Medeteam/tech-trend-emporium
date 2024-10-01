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

## Paquetes Utilizados

Este proyecto utiliza varios paquetes de NuGet para añadir funcionalidad clave a la aplicación. A continuación se enumeran los paquetes utilizados y una breve descripción de cada uno:

### 1. **DotNetEnv (3.1.1)**
   - Este paquete permite la carga de variables de entorno desde un archivo `.env` en aplicaciones .NET. Es útil para almacenar configuraciones como cadenas de conexión de base de datos y claves API de forma segura fuera del código fuente.

### 2. **Microsoft.AspNetCore.Authentication.JwtBearer (8.0.8)**
   - Implementa la autenticación basada en tokens JWT (JSON Web Token) en aplicaciones ASP.NET Core, lo que facilita la autenticación de usuarios en aplicaciones distribuidas y APIs.

### 3. **Microsoft.AspNetCore.Authorization (8.0.8)**
   - Facilita la implementación de políticas de autorización para controlar el acceso a diferentes partes de la aplicación en función de roles y permisos de los usuarios.

### 4. **Microsoft.AspNetCore.Mvc.NewtonsoftJson (8.0.8)**
   - Integra `Newtonsoft.Json` en ASP.NET Core para manejar la serialización y deserialización de objetos JSON. Es utilizado principalmente para el intercambio de datos entre el cliente y el servidor.

### 5. **Microsoft.EntityFrameworkCore (8.0.8)**
   - Proporciona herramientas para interactuar con bases de datos utilizando el patrón ORM (Mapeo Relacional de Objetos) en .NET, facilitando la manipulación de datos sin necesidad de escribir consultas SQL directamente.

### 6. **Microsoft.EntityFrameworkCore.Design (8.0.8)**
   - Contiene herramientas necesarias para realizar migraciones y scaffolding de bases de datos cuando se trabaja con Entity Framework Core.

### 7. **Microsoft.EntityFrameworkCore.SqlServer (8.0.8)**
   - Proveedor de Entity Framework Core para SQL Server, permite trabajar con bases de datos SQL Server a través de EF Core.

### 8. **Microsoft.Extensions.Http (8.0.0)**
   - Proporciona mejoras para la gestión de peticiones HTTP, incluido el manejo de errores y la resiliencia a través de políticas de reintentos.

### 9. **Microsoft.Extensions.Http.Polly (8.0.8)**
   - Extiende `HttpClient` con capacidades de resiliencia a través de la integración con Polly, una biblioteca que ofrece funcionalidades como reintentos automáticos y manejo de fallos en solicitudes HTTP.

### 10. **Microsoft.Extensions.Identity.Core (8.0.8)**
   - Proporciona las funcionalidades centrales para la autenticación y gestión de usuarios, incluyendo la configuración de roles y permisos.

### 11. **Swashbuckle.AspNetCore (6.7.3)**
   - Herramienta que genera automáticamente una interfaz Swagger para la documentación de tus APIs ASP.NET Core, facilitando la exploración e interacción con las APIs desde un navegador.


## Herramientas de Pruebas y Análisis

### 1. **Bogus (35.6.1)**
   - Generador de datos falsos que se utiliza para crear datos de prueba realistas, útil para pruebas unitarias y de integración.

### 2. **coverlet.collector (6.0.0)**
   - Herramienta para medir la cobertura de código durante las pruebas en .NET, ayudando a garantizar que el código esté adecuadamente probado.

### 3. **Microsoft.EntityFrameworkCore.InMemory (8.0.8)**
   - Proveedor de EF Core que permite simular una base de datos en memoria para realizar pruebas unitarias sin necesidad de una base de datos real.

### 4. **xunit (2.5.3)**
   - Marco de pruebas unitarias utilizado para escribir y ejecutar pruebas en aplicaciones .NET.

### 5. **xunit.runner.visualstudio (2.5.3)**
   - Integra xUnit con Visual Studio, permitiendo ejecutar y visualizar pruebas desde el entorno de desarrollo.

### 6. **Microsoft.NET.Test.Sdk (17.8.0)**
   - Herramienta que proporciona infraestructura para ejecutar pruebas en proyectos .NET, integrándose con varios marcos de pruebas como xUnit, NUnit, y MSTest.

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
- Access the API at [http://localhost:5000](http://localhost:5000)

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