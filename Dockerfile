# Use the official .NET 8 SDK image to build the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy the solution file and restore dependencies
COPY emporium/*.sln ./
COPY emporium/app/*.csproj ./app/
COPY emporium/Data/*.csproj ./Data/
RUN dotnet restore

# Copy the rest of the application code
COPY emporium/app/. ./app/
COPY emporium/Data/. ./Data/
WORKDIR /src/app
RUN dotnet build -c Release -o /app/build

# Publish the application
RUN dotnet publish -c Release -o /app/publish

# Use the official ASP.NET Core runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

# Expose the port the app runs on
EXPOSE 8080

# Run the application
ENTRYPOINT ["dotnet", "App.dll"]
