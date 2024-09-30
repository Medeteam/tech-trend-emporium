# tech-trend-emporium
E-Commerce App using FAKESTORE API

## Actions' pipeline status

**Main branch status**

![main branch status](https://github.com/Medeteam/tech-trend-emporium/actions/workflows/pipeline.yml/badge.svg)

## Rulest
This repository uses the trunk-based strategy, so a Pull request is required with 2 aprovals before any merge, no force pushes allowed.

## Clone project
For cloning the project, use `git clone` command. Then in the local repository execute the instruction:

```dotnet restore ./emporium/emporium.sln```

> **Note:** If you don't have the emporium.sln file in that route, change the route in the command before runing it.

## Pipeline CI/CD
The pipeline contains integration with SonarCloud to scan the code in order to check the code coverage for tests.
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