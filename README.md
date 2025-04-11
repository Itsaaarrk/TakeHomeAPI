# TakeHomeAPI

This project is a RESTful API built with ASP.NET Core and Microsoft SQL Server. It includes JWT-based authentication, API versioning (v1/v2), structured logging using Serilog, and Swagger documentation.

## Features

- **Authentication**: 
  - `POST /api/v1/auth/register` – Register a new user.
  - `POST /api/v1/auth/login` – Login and obtain a JWT token.
- **Products Endpoints**:
  - **v1**: `GET /api/v1/products` returns basic product details.
  - **v2**: `GET /api/v2/products` returns products with nested packaging details.
- **Structured Logging**: Uses Serilog to log key events (log folder).
- **API Versioning**: Easily switch between v1 and v2 endpoints.
- **Swagger/OpenAPI**: Automatically generated API documentation.

## Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/9.0)
- Microsoft SQL Server
- Docker (optional for containerized deployment)

## Setup Instructions

1. Clone the Repository:
   ```bash
   git clone https://github.com/itsaaarrk/Takehomeapi.git
   cd TakeHomeAPI

2. Configure the Database:
   - Update the `DefaultConnection` string in `appsettings.json` with your SQL Server connection details.

3. Apply Database Migrations:
   dotnet ef database update

4. Run the Application:
   dotnet run or select "https" and hit run button or F5 in Visual Studio
   
5. Access the API:
   - Swagger UI: [https://localhost:5000/swagger](or the port specified in `launchSettings.json`).

## Additional Notes

- **Rate Limiting**: The API is rate-limited to 100 requests per minute globally.
- **HTTPS**: Ensure you trust the development certificate by running:
  dotnet dev-certs https --trust

  - **Logging**: Logs are written to the console and `Logs/log-.txt`.

Feel free to explore the API using the Swagger UI or any API client like Postman.



## Notes for takehome 1 - database.zip
- This zip file are scripts for creating tables, indexing, inserting sample data and design of ERD using SSMS.