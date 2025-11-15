# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Install build dependencies
RUN apt-get update && \
    apt-get install -y --no-install-recommends \
        netcat-traditional \
        postgresql-client && \
    rm -rf /var/lib/apt/lists/*

# Restore and build
COPY PetCare.Api/PetCare.Api.csproj PetCare.Api/
COPY PetCare.Application/PetCare.Application.csproj PetCare.Application/
COPY PetCare.Infrastructure/PetCare.Infrastructure.csproj PetCare.Infrastructure/
COPY PetCare.Domain/PetCare.Domain.csproj PetCare.Domain/
RUN dotnet restore PetCare.Api/PetCare.Api.csproj

COPY . .
RUN dotnet build PetCare.Api/PetCare.Api.csproj -c Release
RUN dotnet publish PetCare.Api/PetCare.Api.csproj -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app

# Install runtime dependencies
RUN apt-get update && \
    apt-get install -y --no-install-recommends \
        netcat-traditional \
        postgresql-client && \
    rm -rf /var/lib/apt/lists/*

# Copy published application
COPY --from=build /app/publish .

# Copy wait script
COPY --from=build /src/PetCare.Api/wait-for-migrations.sh /wait-for-migrations.sh
RUN chmod +x /wait-for-migrations.sh

# Health check
HEALTHCHECK --interval=30s --timeout=3s --start-period=5s --retries=3 \
    CMD curl -f http://localhost:5100/ || exit 1

ENTRYPOINT ["/wait-for-migrations.sh", "db", "5432", "dotnet", "/app/PetCare.Api.dll"]
