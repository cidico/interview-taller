# CI/CD Status
[![CI/CD (.NET Core 9)](https://github.com/cidico/interview-taller/actions/workflows/backend-ci.yml/badge.svg)](https://github.com/cidico/interview-taller/actions/workflows/backend-ci.yml)

# Backend .NET Core 9 Challenge
This repository contains a backend developed in .NET Core 9, designed for a job challenge. The backend includes a RESTful API with in-memory storage, an event-driven pattern, and Docker containerization support. Below are the instructions for building, testing, and running, with and without Docker.

## Prerequisites
- .NET SDK 9.0 installed.
- Git installed (to clone the repository).
- Docker installed (optional, for container use).
- A terminal (PowerShell, CMD, or Bash).

## Project Structure
- src/backend/: Contains the API source code and Dockerfile.
- src/frontend/: Contains the Angular source code.
- .github/workflows/backend-ci.yaml: CI/CD pipeline using GitHub Actions.
- .gitignore: Files ignored by Git.

## Instructions without Docker

### Build

1. Navigate to the backend directory:
```bash
cd src/backend   
```
2. Restore dependencies and build the project:
```bash
dotnet restore
dotnet build --configuration Release
```

- This creates binaries in src/backend/bin/Release/net9.0/.

### Test
1. Ensure the test project (src/backend.Tests) is configured.
2. Run unit tests:   
```bash
cd src/backend
dotnet test --configuration Release
```
- Results will be displayed in the terminal, using xUnit and NSubstitute.

### Run
1. Set environment variables (optional, to configure environment and port):   
```bash
$env:ASPNETCORE_ENVIRONMENT="Test"
$env:PORT="9090"
```

- Or use the dotnet command with arguments:     
```bash
dotnet run --environment Test --urls http://0.0.0.0:9090
```

2. Start the application:   
```bash
cd src/backend
dotnet run
```

- The API will be available at http://localhost:9090 (or the defined port).
3. Test an endpoint (example):   
```bash
curl http://localhost:9090/api/sample/testKey
```
## Instructions with Docker

### Build
1. Navigate to the backend directory:   
```bash
cd src/backend
```
2. Build the Docker image with environment arguments:   
```bash
docker build -t backend-service-dotnet:latest --build-arg ASPNETCORE_ENVIRONMENT=Test --build-arg PORT=9090 .
```

- This creates an image with the specified environment and port.

### Tests
Tests are run during the project build (via dotnet test in the pipeline or locally before Docker). To test inside the container, you would need to mount the test project, but this is optional. For simplicity, run tests before building the image (see "Test" without Docker).

### Run
1. Run the container, mapping the port and setting environment variables:
```bash
docker run -p 9090:9090 -e ASPNETCORE_ENVIRONMENT=Test -e PORT=9090 backend-service-dotnet:latest
```
- The -p 9090:9090 maps port 9090 from the host to the container.   
- The API will be available at http://localhost:9090.

2. Test an endpoint (example):   
```bash
curl http://localhost:9090/api/sample/testKey
```
3. To stop the container, use Ctrl+C or list and remove:   
```bash
docker ps
docker stop <container_id>
```

## Additional Notes
- The CI/CD pipeline in GitHub Actions will trigger automatically on push or pull_request to the main branch, or manually via workflow_dispatch with inputs for ASPNETCORE_ENVIRONMENT and PORT.

