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

# ===== Final image (SDK for migrations) =====
FROM base AS final
WORKDIR /app

# Copy published files
COPY --from=publish /app/publish ./

# Copy all projects for EF Core migrations
COPY ["PetCare.Infrastructure", "/app/PetCare.Infrastructure"]
COPY ["PetCare.Domain", "/app/PetCare.Domain"]
COPY ["PetCare.Application", "/app/PetCare.Application"]
COPY ["PetCare.Api/PetCare.Api.csproj", "/app/PetCare.Api/PetCare.Api.csproj"]

# Copy wait-for-db script
COPY wait-for-db.sh /wait-for-db.sh
RUN chmod +x /wait-for-db.sh

# Ensure dotnet global tools are in PATH (for dotnet-ef)
ENV PATH="$PATH:/root/.dotnet/tools"

# Run app only after DB is ready
ENTRYPOINT ["/wait-for-db.sh", "db", "5432", "dotnet", "PetCare.Api.dll"]
