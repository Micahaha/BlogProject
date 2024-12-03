# Use the official .NET SDK image to build and publish the app
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80/tcp
EXPOSE 443/tcp

# Use the .NET SDK image to build the app
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

# Copy the csproj file and restore any dependencies
COPY ["BlogProject.csproj", "./"]
RUN dotnet restore

# Copy the rest of the application source code
COPY . ./

# Build the application
RUN dotnet publish -c Release -o /app/publish

# Final stage to run the application
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
COPY appsettings.json ./

# Run the application
ENTRYPOINT ["dotnet", "BlogProject.dll"]
