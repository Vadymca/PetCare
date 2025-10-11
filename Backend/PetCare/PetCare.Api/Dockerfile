# ===== SDK stage =====
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS sdk
WORKDIR /src

# Встановлюємо netcat та postgresql-client
RUN apt-get update && \
    apt-get install -y --no-install-recommends netcat-traditional postgresql-client && \
    rm -rf /var/lib/apt/lists/*

# Встановлюємо dotnet-ef CLI
RUN dotnet tool install --global dotnet-ef --version 9.0.7
ENV PATH="$PATH:/root/.dotnet/tools"

# Копіюємо скрипт wait-for-db
COPY wait-for-db.sh /wait-for-db.sh
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

# Публікація API
RUN dotnet publish "PetCare.Api/PetCare.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# ===== Runtime stage =====
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app

# Встановлюємо netcat
RUN apt-get update && \
    apt-get install -y --no-install-recommends netcat-traditional && \
    rm -rf /var/lib/apt/lists/*

# Копіюємо dotnet-ef із SDK-етапу
COPY --from=sdk /root/.dotnet/tools /root/.dotnet/tools
ENV PATH="$PATH:/root/.dotnet/tools"

# Копіюємо публішені файли
COPY --from=sdk /app/publish ./

# Копіюємо скрипт wait-for-db
COPY --from=sdk /wait-for-db.sh /wait-for-db.sh
RUN chmod +x /wait-for-db.sh

# Налаштування середовища
ENV ASPNETCORE_ENVIRONMENT=Development
ENV ASPNETCORE_URLS=http://+:5100

# EntryPoint: чекаємо базу, застосовуємо міграції, запускаємо API
ENTRYPOINT ["/wait-for-db.sh", "db", "5432", "sh", "-c", "dotnet ef database update --project /src/PetCare.Infrastructure --startup-project /src/PetCare.Api && dotnet /app/PetCare.Api.dll"]