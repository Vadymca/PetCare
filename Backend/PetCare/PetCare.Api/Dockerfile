# ===== Base runtime image =====
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80

# Install PostgreSQL client and netcat
RUN apt-get update && \
    apt-get install -y postgresql-client netcat-traditional && \
    rm -rf /var/lib/apt/lists/*

# ===== Build stage =====
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Копіюємо проєкти
COPY ["PetCare.Api/PetCare.Api.csproj", "PetCare.Api/"]
COPY ["PetCare.Application/PetCare.Application.csproj", "PetCare.Application/"]
COPY ["PetCare.Infrastructure/PetCare.Infrastructure.csproj", "PetCare.Infrastructure/"]
COPY ["PetCare.Domain/PetCare.Domain.csproj", "PetCare.Domain/"]

# Відновлюємо пакети
RUN dotnet restore "./PetCare.Api/PetCare.Api.csproj"

# Копіюємо решту коду
COPY . .

WORKDIR "/src/PetCare.Api"

# Збираємо проєкт
RUN dotnet build "./PetCare.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Публікуємо проєкт
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./PetCare.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# ===== Final runtime image =====
FROM base AS final
WORKDIR /app

# Копіюємо публікацію
COPY --from=publish /app/publish .

# Копіюємо Infrastructure для міграцій
COPY --from=build /src/PetCare.Infrastructure /app/PetCare.Infrastructure
COPY --from=build /src/PetCare.Api /app/PetCare.Api

# Копіюємо скрипт для чекання БД
COPY wait-for-db.sh /wait-for-db.sh
RUN chmod +x /wait-for-db.sh

# Запускаємо скрипт
ENTRYPOINT ["/wait-for-db.sh", "db", "5432", "dotnet", "PetCare.Api.dll"]
