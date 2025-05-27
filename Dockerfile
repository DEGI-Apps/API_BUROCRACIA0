# Etapa de construcción
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copia archivos de solución y proyecto para restore
COPY *.sln ./
COPY ./API_BASE.Infrastructure/*.csproj ./API_BASE.Infrastructure/
COPY ./API_BASE.Application/*.csproj ./API_BASE.Application/
COPY ./API_BASE.Domain/*.csproj ./API_BASE.Domain/
COPY ./API_BASE.API/*.csproj ./API_BASE.API/

# Restaura dependencias con configuración adicional
RUN dotnet restore --verbosity normal

# Copia el código fuente
COPY . .

# Publica la aplicación
RUN dotnet publish ./API_BASE.API/API_BASE.API.csproj -c Release -o /app/publish

# Etapa de ejecución
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "API_BASE.API.dll"]