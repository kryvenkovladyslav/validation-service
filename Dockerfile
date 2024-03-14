
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/WebApi/WebApi.csproj", "src/WebApi/"]
COPY ["tests/BusinessLayerTests/BusinessLayerTests.csproj", "tests/BusinessLayerTests/"]
COPY ["src/BusinessLayer/BusinessLayer.csproj", "src/BusinessLayer/"]
COPY ["src/AzureBlobStorage/AzureBlobStorage.csproj", "src/AzureBlobStorage/"]
COPY ["src/Domain/Domain.csproj", "src/Domain/"]
COPY ["src/DependencyInjection/DependencyInjection.csproj", "src/DependencyInjection/"]
RUN dotnet restore "./src/WebApi/./WebApi.csproj"
COPY . .
WORKDIR "/src/src/WebApi"
RUN dotnet build "./WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WebApi.dll"]