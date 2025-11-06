# ===== SDK stage =====
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Встановлюємо netcat та postgresql-client
RUN apt-get update && \
    apt-get install -y --no-install-recommends netcat-traditional postgresql-client && \
    rm -rf /var/lib/apt/lists/*

# Встановлюємо dotnet-ef CLI
RUN dotnet tool install --global dotnet-ef --version 9.0.7
ENV PATH="$PATH:/root/.dotnet/tools"

# -------------------------------------------------------------
# 🔹 Копіюємо тільки csproj-файли (для кешування restore)
# -------------------------------------------------------------
COPY ["PetCare.Api/PetCare.Api.csproj", "PetCare.Api/"]
COPY ["PetCare.Application/PetCare.Application.csproj", "PetCare.Application/"]
COPY ["PetCare.Infrastructure/PetCare.Infrastructure.csproj", "PetCare.Infrastructure/"]
COPY ["PetCare.Domain/PetCare.Domain.csproj", "PetCare.Domain/"]

# Відновлюємо залежності (restore)
RUN dotnet restore "PetCare.Api/PetCare.Api.csproj"

# -------------------------------------------------------------
# 🔹 Копіюємо проекти окремо, щоб зміни у міграціях
#     у PetCare.Infrastructure завжди ламали кеш
# -------------------------------------------------------------
COPY PetCare.Infrastructure/ PetCare.Infrastructure/
COPY PetCare.Application/ PetCare.Application/
COPY PetCare.Domain/ PetCare.Domain/
COPY PetCare.Api/ PetCare.Api/

# -------------------------------------------------------------
# 🔹 Build усіх проектів
# -------------------------------------------------------------
RUN dotnet build PetCare.Infrastructure/PetCare.Infrastructure.csproj -c Release
RUN dotnet build PetCare.Api/PetCare.Api.csproj -c Release

# -------------------------------------------------------------
# 🔹 Публікуємо API разом з усіма залежностями і міграціями
# -------------------------------------------------------------
RUN dotnet publish "PetCare.Api/PetCare.Api.csproj" \
    -c Release \
    -o /app/publish \
    /p:UseAppHost=false \
    /p:CopyLocalLockFileAssemblies=true

    # 🔹 Додатково компілюємо PetCare.Infrastructure, щоб включити всі міграції в DLL
RUN dotnet build "PetCare.Infrastructure/PetCare.Infrastructure.csproj" \
    -c Release \
    -o /app/publish

# ===== Runtime stage =====
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app

# Встановлюємо netcat
RUN apt-get update && \
    apt-get install -y --no-install-recommends netcat-traditional && \
    rm -rf /var/lib/apt/lists/*

# Копіюємо dotnet-ef
COPY --from=build /root/.dotnet/tools /root/.dotnet/tools
ENV PATH="$PATH:/root/.dotnet/tools"

# Копіюємо публішені файли
COPY --from=build /app/publish ./

# 🔹 Додаємо копію вихідного коду інфраструктури (включно з Migrations)
COPY --from=build /src/PetCare.Infrastructure /app/PetCare.Infrastructure

# Копіюємо скрипт wait-for-db
COPY --from=build /src/PetCare.Api/wait-for-db.sh /wait-for-db.sh
RUN chmod +x /wait-for-db.sh

# Налаштування середовища
ENV ASPNETCORE_ENVIRONMENT=Development
ENV ASPNETCORE_URLS=http://+:5100

# EntryPoint: чекаємо базу та запускаємо API
ENTRYPOINT ["/wait-for-db.sh", "db", "5432", "dotnet", "/app/PetCare.Api.dll"]