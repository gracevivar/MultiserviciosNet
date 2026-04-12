FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY RetoTienda.Gateway/RetoTienda.Gateway.csproj RetoTienda.Gateway/
RUN dotnet restore RetoTienda.Gateway/RetoTienda.Gateway.csproj
COPY . .
RUN dotnet publish RetoTienda.Gateway/RetoTienda.Gateway.csproj -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "RetoTienda.Gateway.dll"]