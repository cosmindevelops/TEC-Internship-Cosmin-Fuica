# Use the official ASP.NET Core runtime as a parent image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

# Install curl
RUN apt-get update && apt-get install -y curl

# Use the SDK image for building the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["WebApp/WebApp.csproj", "WebApp/"]
RUN dotnet restore "WebApp/WebApp.csproj"
COPY . .
WORKDIR "/src/WebApp"
RUN dotnet build "WebApp.csproj" -c Release -o /app/build

# Remove conflicting files
RUN rm -f /src/WebApp/appsettings.Development.json
RUN rm -f /src/WebApp/appsettings.json
RUN rm -f /src/WebApp/config/appsettings.Docker.json

FROM build AS publish
WORKDIR /src/WebApp
RUN dotnet publish "WebApp.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY WebApp/config/appsettings.Docker.json appsettings.json
ENTRYPOINT ["dotnet", "WebApp.dll"]
