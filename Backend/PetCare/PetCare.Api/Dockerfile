# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# ===== Base runtime image =====
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80

# Install PostgreSQL client and netcat
RUN apt-get update && \
    apt-get install -y postgresql-client netcat-traditional && \
    rm -rf /var/lib/apt/lists/*

# Install PostgreSQL client for pg_isready
RUN apt-get update && apt-get install -y postgresql-client && rm -rf /var/lib/apt/lists/*

# ===== Build stage =====
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["PetCare.Api/PetCare.Api.csproj", "PetCare.Api/"]
COPY ["PetCare.Application/PetCare.Application.csproj", "PetCare.Application/"]
COPY ["PetCare.Infrastructure/PetCare.Infrastructure.csproj", "PetCare.Infrastructure/"]
COPY ["PetCare.Domain/PetCare.Domain.csproj", "PetCare.Domain/"]
RUN dotnet restore "./PetCare.Api/PetCare.Api.csproj"
COPY . .
WORKDIR "/src/PetCare.Api"
RUN dotnet build "./PetCare.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# ===== Publish stage =====
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./PetCare.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# ===== Final image =====
FROM base AS final
WORKDIR /app

# Copy published files
COPY --from=publish /app/publish .

# Copy wait-for-db script (розташований поряд із docker-compose.yml)
COPY wait-for-db.sh /wait-for-db.sh
RUN chmod +x /wait-for-db.sh

# Run app only after DB is ready
ENTRYPOINT ["/wait-for-db.sh", "db", "5432", "--", "dotnet", "PetCare.Api.dll"]