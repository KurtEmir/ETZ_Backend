# Runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
# Render, container'ın $PORT'unda dinlemeni bekler
ENV ASPNETCORE_URLS=http://0.0.0.0:${PORT}
EXPOSE 8080

# Build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ./src/ETZ.Api/ETZ.Api.csproj ./src/ETZ.Api/
COPY ./src/ETZ.Persistence/ETZ.Persistence.csproj ./src/ETZ.Persistence/
COPY ./src/ETZ.Application/ETZ.Application.csproj ./src/ETZ.Application/
COPY ./src/ETZ.Domain/ETZ.Domain.csproj ./src/ETZ.Domain/
RUN dotnet restore ./src/ETZ.Api/ETZ.Api.csproj
COPY . .
RUN dotnet publish ./src/ETZ.Api/ETZ.Api.csproj -c Release -o /app/publish

# Final
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "ETZ.Api.dll"]