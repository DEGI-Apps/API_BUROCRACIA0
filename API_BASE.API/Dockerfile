# Imagen base del SDK para compilar
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar los archivos del proyecto
COPY ["../API_BASE.Application/API_BASE.Application.csproj", "API_BASE.Application/"]
COPY ["../API_BASE.Domain/API_BASE.Domain.csproj", "API_BASE.Domain/"]
COPY ["../API_BASE.Infrastructure/API_BASE.Infrastructure.csproj", "API_BASE.Infrastructure/"]
COPY ["../API_BASE.API/API_BASE.API.csproj", "API_BASE.API/"]


# Copiar todo el código
COPY . .

# Publicar la app
WORKDIR "/src/API_BASE.API"
RUN dotnet publish -c Release -o /app/publish

# Imagen base para ejecutar la app
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .

# Puerto expuesto por la API (ej. 5000)
EXPOSE 5000
ENTRYPOINT ["dotnet", "API_BASE.API.dll"]
