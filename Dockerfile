# =========================
# Build stage
# =========================
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar csproj para aprovechar cache de restore
COPY RetoTienda.Api/RetoTienda.Api.csproj RetoTienda.Api/
COPY RetoTiendda.Application/RetoTienda.Application.csproj RetoTiendda.Application/
COPY RetoTiendda.Domain/RetoTienda.Domain.csproj RetoTiendda.Domain/
COPY RetoTiendda.Infrastructure/RetoTienda.Infrastructure.csproj RetoTiendda.Infrastructure/

RUN dotnet restore RetoTienda.Api/RetoTienda.Api.csproj

# Copiar el resto del código
COPY . .

# Publicar
RUN dotnet publish RetoTienda.Api/RetoTienda.Api.csproj -c Release -o /app/publish /p:UseAppHost=false

# =========================
# Runtime stage
# =========================
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "RetoTienda.Api.dll"]