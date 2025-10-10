# ===== SDK image for building & migrations =====
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS sdk
WORKDIR /src

# Install PostgreSQL client and netcat
RUN apt-get update && \
    apt-get install -y postgresql-client netcat-traditional && \
    rm -rf /var/lib/apt/lists/*

# Install EF Core CLI globally
RUN dotnet tool install --global dotnet-ef --version 9.0.7
ENV PATH="$PATH:/root/.dotnet/tools"

# Copy csproj and restore
COPY ["PetCare.Api/PetCare.Api.csproj", "PetCare.Api/"]
COPY ["PetCare.Application/PetCare.Application.csproj", "PetCare.Application/"]
COPY ["PetCare.Infrastructure/PetCare.Infrastructure.csproj", "PetCare.Infrastructure/"]
COPY ["PetCare.Domain/PetCare.Domain.csproj", "PetCare.Domain/"]
COPY ["PetCare.Api/stylecop.json", "PetCare.Api/"]
RUN dotnet restore "PetCare.Api/PetCare.Api.csproj"

# Copy everything else
COPY . .

# Build & publish
RUN dotnet publish "PetCare.Api/PetCare.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# ===== Runtime image =====
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app

# Copy published app
COPY --from=sdk /app/publish ./

# Install PostgreSQL client and netcat for wait-for-db
RUN apt-get update && \
    apt-get install -y postgresql-client netcat-traditional && \
    rm -rf /var/lib/apt/lists/*

# Copy wait-for-db script
COPY wait-for-db.sh /wait-for-db.sh
RUN chmod +x /wait-for-db.sh

ENTRYPOINT ["/wait-for-db.sh", "db", "5432", "dotnet", "PetCare.Api.dll"]