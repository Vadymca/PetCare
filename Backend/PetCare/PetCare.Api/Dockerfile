# ===== SDK stage =====
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

WORKDIR /src

# --- Встановлення утиліт ---
RUN apt-get update && \
    apt-get install -y --no-install-recommends netcat-traditional postgresql-client && \
    rm -rf /var/lib/apt/lists/*

# --- Копіюємо csproj для кешу ---
COPY ["PetCare.Api/PetCare.Api.csproj", "PetCare.Api/"]
COPY ["PetCare.Application/PetCare.Application.csproj", "PetCare.Application/"]
COPY ["PetCare.Infrastructure/PetCare.Infrastructure.csproj", "PetCare.Infrastructure/"]
COPY ["PetCare.Domain/PetCare.Domain.csproj", "PetCare.Domain/"]

RUN dotnet restore "PetCare.Api/PetCare.Api.csproj"

# --- Копіюємо весь код ---
COPY . .

# --- Будуємо API-проєкт ---
RUN dotnet build PetCare.Api/PetCare.Api.csproj -c Release

# --- Публікуємо API ---
RUN dotnet publish PetCare.Api/PetCare.Api.csproj \
    -c Release \
    -o /app/publish \
    /p:UseAppHost=false

# ===== Runtime stage =====
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime

WORKDIR /app

RUN apt-get update && \
    apt-get install -y --no-install-recommends netcat-traditional postgresql-client && \
    rm -rf /var/lib/apt/lists/*

# --- Копіюємо результати publish ---
COPY --from=build /app/publish .

# --- Копіюємо скрипт очікування бази ---
COPY --from=build /src/PetCare.Api/wait-for-db.sh /wait-for-db.sh
RUN chmod +x /wait-for-db.sh

# --- ENV налаштування ---
ENV ASPNETCORE_ENVIRONMENT=Development
ENV ASPNETCORE_URLS=http://+:5100

ENTRYPOINT ["/wait-for-db.sh", "db", "5432", "dotnet", "/app/PetCare.Api.dll"]