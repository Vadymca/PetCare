# ===== SDK stage =====
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Встановлюємо netcat та postgresql-client
RUN apt-get update && \
    apt-get install -y --no-install-recommends netcat-traditional postgresql-client && \
    rm -rf /var/lib/apt/lists/*

# Копіюємо csproj і робимо restore
COPY ["PetCare.Api/PetCare.Api.csproj", "PetCare.Api/"]
COPY ["PetCare.Application/PetCare.Application.csproj", "PetCare.Application/"]
COPY ["PetCare.Infrastructure/PetCare.Infrastructure.csproj", "PetCare.Infrastructure/"]
COPY ["PetCare.Domain/PetCare.Domain.csproj", "PetCare.Domain/"]
RUN dotnet restore "PetCare.Api/PetCare.Api.csproj"

# Копіюємо весь код
COPY . .

# Build і publish API
RUN dotnet publish "PetCare.Api/PetCare.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# ===== Runtime stage =====
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app

# Встановлюємо netcat
RUN apt-get update && \
    apt-get install -y --no-install-recommends netcat-traditional && \
    rm -rf /var/lib/apt/lists/*

# Копіюємо публішені файли з build stage
COPY --from=build /app/publish ./ 

# Копіюємо скрипт wait-for-db
COPY PetCare.Api/wait-for-db.sh /wait-for-db.sh
RUN chmod +x /wait-for-db.sh

ENV ASPNETCORE_ENVIRONMENT=Development
ENV ASPNETCORE_URLS=http://+:5100

# EntryPoint: чекаємо БД, потім запускаємо додаток (міграції + seed через Program.cs)
ENTRYPOINT ["/wait-for-db.sh", "db", "5432", "dotnet", "PetCare.Api.dll"]