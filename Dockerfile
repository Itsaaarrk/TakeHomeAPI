# Use the .NET SDK image for building the project
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy the project file and restore dependencies
COPY ["TakeHomeAPI.csproj", "./"]
RUN dotnet restore "./TakeHomeAPI.csproj"

# Copy all source code and publish the application
COPY . .
RUN dotnet publish "TakeHomeAPI.csproj" -c Release -o /app/publish

# Use the official ASP.NET Core runtime image
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 80
ENTRYPOINT ["dotnet", "TakeHomeAPI.dll"]
