# DevOps Project - .NET API

A simple REST API for product management built with .NET 10, demonstrating DevOps practices including logging, containerization, and CI/CD deployment.

## Live Demo

The application is automatically deployed via CI/CD pipeline:
- **Swagger UI**: [https://devopsprojectnetapirepo.onrender.com/swagger](https://devopsprojectnetapirepo.onrender.com/swagger)

## Technologies

- **.NET 10.0** - ASP.NET Core Minimal API
- **Serilog** - Structured logging
- **Swagger/OpenAPI** - API documentation
- **xUnit** - Unit testing
- **Docker** - Containerization
- **Docker Compose** - Container orchestration

## API Endpoints

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/products` | Get all products |
| GET | `/products/{id}` | Get product by ID |
| GET | `/products/search?name={name}` | Search products by name |
| POST | `/products` | Create new product |
| DELETE | `/products/{id}` | Delete product by ID |

## Running Locally

### Prerequisites
- .NET 10.0 SDK
- Docker Desktop (for containerization)

### Option 1: Run with .NET CLI

```bash
# Restore dependencies
dotnet restore

# Run the application
dotnet run --project Api/Api.csproj

# Run tests
dotnet test
```

The API will be available at `http://localhost:5000`

### Option 2: Run with Docker Compose

```bash
# Build and run
docker compose up --build

# Run in detached mode
docker compose up --build -d

# Stop containers
docker compose down
```

The API will be available at `http://localhost:5000`

### Option 3: Run with Docker

```bash
# Build image
docker build -t devops-api .

# Run container
docker run -p 5000:8080 devops-api
```

## 🧪 Testing

```bash
# Run all tests
dotnet test

# Run tests with coverage
dotnet test --collect:"XPlat Code Coverage"
```

## 📝 Logging

The application uses Serilog for structured logging:
- **Information** - API requests and responses
- **Warning** - Validation errors and not found resources
- **Error** - System errors and exceptions
- **Debug** - Detailed service layer operations

Logs are written to console and can be easily configured for different outputs.

## Docker

The project includes:
- **Dockerfile** - Multi-stage build for optimized image size
- **docker-compose.yml** - Easy container orchestration
- **.dockerignore** - Excludes unnecessary files from the image

## Project Structure

```
DevOpsProjectNetApiRepo/
├── Api/                          # Main API project
│   ├── Program.cs               # Application entry point
│   ├── models/                  # Data models
│   ├── services/                # Business logic
│   └── appsettings.json         # Configuration
├── Api.UnitTests/               # Unit tests
│   └── services/                # Service tests
├── Dockerfile                   # Container definition
├── docker-compose.yml           # Docker orchestration
└── README.md                    # This file
```

## 🔄 CI/CD

The application is automatically deployed to Render.com when changes are pushed to the repository. The deployment pipeline includes:
- Building the application
- Running tests
- Containerizing with Docker
- Deploying to production

## 📄 License

This project is for educational purposes.