FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY RetoTienda.Api/RetoTienda.Api.csproj RetoTienda.Api/
COPY RetoTiendda.Application/RetoTienda.Application.csproj RetoTiendda.Application/
COPY RetoTiendda.Domain/RetoTienda.Domain.csproj RetoTiendda.Domain/
COPY RetoTiendda.Infrastructure/RetoTienda.Infrastructure.csproj RetoTiendda.Infrastructure/
COPY RetoTienda.Contracts/RetoTienda.Contracts.csproj RetoTienda.Contracts/

RUN dotnet restore RetoTienda.Api/RetoTienda.Api.csproj
COPY . .
RUN dotnet publish RetoTienda.Api/RetoTienda.Api.csproj -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "RetoTienda.Api.dll"]