# Use the official ASP.NET Core runtime as a parent image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

# Install curl
RUN apt-get update && apt-get install -y curl

# Use the SDK image for building the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["ApiApp/ApiApp.csproj", "ApiApp/"]
RUN dotnet restore "ApiApp/ApiApp.csproj"
COPY . .
WORKDIR "/src/ApiApp"
RUN dotnet build "ApiApp.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ApiApp.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY ApiApp/config/appsettings.Docker.json appsettings.json
ENTRYPOINT ["dotnet", "ApiApp.dll"]