# ===== Base SDK image =====
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS base
WORKDIR /app
EXPOSE 80

# Install PostgreSQL client and netcat
RUN apt-get update && \
    apt-get install -y postgresql-client netcat-traditional && \
    rm -rf /var/lib/apt/lists/*

# ===== Build stage =====
FROM base AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy csproj files
COPY ["PetCare.Api/PetCare.Api.csproj", "PetCare.Api/"]
COPY ["PetCare.Application/PetCare.Application.csproj", "PetCare.Application/"]
COPY ["PetCare.Infrastructure/PetCare.Infrastructure.csproj", "PetCare.Infrastructure/"]
COPY ["PetCare.Domain/PetCare.Domain.csproj", "PetCare.Domain/"]
COPY ["PetCare.Api/stylecop.json", "PetCare.Api/"]

# Restore packages
RUN dotnet restore "./PetCare.Api/PetCare.Api.csproj"

# Copy all code
COPY . .

# Build project
WORKDIR "/src/PetCare.Api"
RUN dotnet build "./PetCare.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# ===== Apply migrations during build =====
RUN dotnet tool restore && \
    echo "Applying EF Core migrations..." && \
    dotnet ef database update \
        --project /src/PetCare.Infrastructure/PetCare.Infrastructure.csproj \
        --startup-project /src/PetCare.Api/PetCare.Api.csproj && \
    echo "Migrations applied."

# ===== Publish stage =====
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./PetCare.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# ===== Final image =====
FROM base AS final
WORKDIR /app

# Copy published files
COPY --from=publish /app/publish ./

# Copy wait-for-db script
COPY wait-for-db.sh /wait-for-db.sh
RUN chmod +x /wait-for-db.sh

# Start API after DB is ready
ENTRYPOINT ["/wait-for-db.sh", "db", "5432", "dotnet", "PetCare.Api.dll"]