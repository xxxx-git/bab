FROM mcr.microsoft.com/dotnet/core/sdk:2.2 AS build-test
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY *.sln .
COPY service/*.csproj ./service/
COPY test/*.csproj ./test/
RUN dotnet restore

# Copy everything else and build
COPY service/. ./service/
COPY test/. ./test/
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/core/aspnet:2.2 AS test
WORKDIR /app
COPY --from=build-test /app/test/out .
RUN dotnet vstest test.dll
ENTRYPOINT ["dotnet", "vstest test.dll"]





FROM mcr.microsoft.com/dotnet/core/sdk:2.2 AS build-prod
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY service/*.csproj .
RUN dotnet restore

# Copy everything else and build
COPY service/. .
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/core/aspnet:2.2
WORKDIR /app
COPY --from=build-prod /app/service/out .
ENTRYPOINT ["dotnet", "service.dll"]