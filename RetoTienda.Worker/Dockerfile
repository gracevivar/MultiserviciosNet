FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY RetoTienda.Worker/RetoTienda.Worker.csproj RetoTienda.Worker/
COPY RetoTienda.Contracts/RetoTienda.Contracts.csproj RetoTienda.Contracts/
RUN dotnet restore RetoTienda.Worker/RetoTienda.Worker.csproj
COPY . .
RUN dotnet publish RetoTienda.Worker/RetoTienda.Worker.csproj -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/runtime:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "RetoTienda.Worker.dll"]