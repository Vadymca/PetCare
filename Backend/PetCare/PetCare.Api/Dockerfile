# ===== SDK stage =====
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS sdk
WORKDIR /src

# Встановлюємо netcat та postgresql-client
RUN apt-get update && \
    apt-get install -y --no-install-recommends netcat-traditional postgresql-client && \
    rm -rf /var/lib/apt/lists/*

# Копіюємо скрипт wait-for-db
COPY PetCare.Api/wait-for-db.sh /wait-for-db.sh
RUN chmod +x /wait-for-db.sh

# Копіюємо csproj файли і робимо restore
COPY ["PetCare.Api/PetCare.Api.csproj", "PetCare.Api/"]
COPY ["PetCare.Application/PetCare.Application.csproj", "PetCare.Application/"]
COPY ["PetCare.Infrastructure/PetCare.Infrastructure.csproj", "PetCare.Infrastructure/"]
COPY ["PetCare.Domain/PetCare.Domain.csproj", "PetCare.Domain/"]
RUN dotnet restore "PetCare.Api/PetCare.Api.csproj"

# Копіюємо весь код
COPY . .

# Build всі проєкти
RUN dotnet build PetCare.Infrastructure/PetCare.Infrastructure.csproj -c Release
RUN dotnet build PetCare.Api/PetCare.Api.csproj -c Release

# ===== Apply EF Core migrations at build time =====
RUN dotnet ef database update \
    --project PetCare.Infrastructure/PetCare.Infrastructure.csproj \
    --startup-project PetCare.Api/PetCare.Api.csproj \
    --configuration Release

# Публікація API
RUN dotnet publish "PetCare.Api/PetCare.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# ===== Runtime stage =====
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app

# Встановлюємо netcat
RUN apt-get update && \
    apt-get install -y --no-install-recommends netcat-traditional && \
    rm -rf /var/lib/apt/lists/*

# Копіюємо публішені файли
COPY --from=sdk /app/publish ./ 

# Копіюємо скрипт wait-for-db
COPY PetCare.Api/wait-for-db.sh /wait-for-db.sh
RUN chmod +x /wait-for-db.sh

# Налаштування середовища
ENV ASPNETCORE_ENVIRONMENT=Development
ENV ASPNETCORE_URLS=http://+:5100

# EntryPoint
ENTRYPOINT ["/wait-for-db.sh", "db", "5432", "dotnet", "PetCare.Api.dll"]
