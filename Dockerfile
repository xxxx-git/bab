FROM mcr.microsoft.com/dotnet/core/sdk:2.2 AS build-test
WORKDIR /app

# Copy everything else and build
COPY service/. ./service/
COPY test/. ./test/
RUN dotnet publish test/test.csproj -c Release

# Build runtime image
FROM mcr.microsoft.com/dotnet/core/aspnet:2.2 AS test
WORKDIR /app
COPY --from=build-test /app/test/bin/Release/netcoreapp2.2 .
RUN dotnet vstest test.dll
ENTRYPOINT ["dotnet", "vstest test.dll"]




FROM mcr.microsoft.com/dotnet/core/sdk:2.2 AS build-prod
WORKDIR /app

# Copy everything else and build
COPY service/. .
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/core/aspnet:2.2 AS prod
WORKDIR /app
COPY --from=build-prod /app/service/bin/Release/netcoreapp2.2 .
ENTRYPOINT ["dotnet", "service.dll"]