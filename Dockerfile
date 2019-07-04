FROM mcr.microsoft.com/dotnet/core/sdk:2.2 AS build-prod
WORKDIR /app

# Copy everything else and build
COPY service/. .
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/core/aspnet:2.2 AS prod
WORKDIR /app
COPY --from=build-prod /app/test/out/ .
ENTRYPOINT ["dotnet", "service.dll"]