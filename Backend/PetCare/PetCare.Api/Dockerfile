# ===== SDK stage (для build та міграцій) =====
FROM mcr.microsoft.com/dotnet/sdk:9.0-bookworm AS sdk
WORKDIR /src

# Встановлюємо PostgreSQL client та netcat
RUN apt-get update && \
    apt-get install -y --no-install-recommends netcat-traditional postgresql-client && \
    rm -rf /var/lib/apt/lists/*

COPY ["PetCare.Api/PetCare.Api.csproj", "PetCare.Api/"]
COPY ["PetCare.Application/PetCare.Application.csproj", "PetCare.Application/"]
COPY ["PetCare.Infrastructure/PetCare.Infrastructure.csproj", "PetCare.Infrastructure/"]
COPY ["PetCare.Domain/PetCare.Domain.csproj", "PetCare.Domain/"]
RUN dotnet restore "PetCare.Api/PetCare.Api.csproj"

# Копіюємо решту проекту
COPY . .

# Build та publish
RUN dotnet publish "PetCare.Api/PetCare.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false


# ===== Runtime stage =====
FROM mcr.microsoft.com/dotnet/aspnet:9.0-bookworm AS runtime
WORKDIR /app

# Встановлюємо netcat та PostgreSQL client
RUN apt-get update && \
    apt-get install -y --no-install-recommends netcat-traditional postgresql-client && \
    rm -rf /var/lib/apt/lists/*

# Копіюємо публішовані файли
COPY --from=sdk /app/publish ./

# Додаємо скрипт очікування БД
COPY wait-for-db.sh /wait-for-db.sh
RUN chmod +x /wait-for-db.sh

ENTRYPOINT ["/wait-for-db.sh", "db", "5432", "dotnet", "PetCare.Api.dll"]
