# ===== SDK stage =====
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Встановлюємо утиліти
RUN apt-get update && \
    apt-get install -y --no-install-recommends netcat-traditional postgresql-client && \
    rm -rf /var/lib/apt/lists/*

# Встановлюємо dotnet-ef CLI
RUN dotnet tool install --global dotnet-ef --version 9.0.7
ENV PATH="$PATH:/root/.dotnet/tools"

# 🔹 Копіюємо тільки csproj-файли для кешування restore
COPY ["PetCare.Api/PetCare.Api.csproj", "PetCare.Api/"]
COPY ["PetCare.Application/PetCare.Application.csproj", "PetCare.Application/"]
COPY ["PetCare.Infrastructure/PetCare.Infrastructure.csproj", "PetCare.Infrastructure/"]
COPY ["PetCare.Domain/PetCare.Domain.csproj", "PetCare.Domain/"]

RUN dotnet restore "PetCare.Api/PetCare.Api.csproj"

# 🔹 Копіюємо повний код
COPY . .

# 🔹 Build усіх проектів (включно з Infrastructure)
RUN dotnet build PetCare.Infrastructure/PetCare.Infrastructure.csproj -c Release
RUN dotnet build PetCare.Api/PetCare.Api.csproj -c Release

# 🔹 Публікуємо API разом із DLL міграцій
RUN dotnet publish PetCare.Infrastructure/PetCare.Infrastructure.csproj -c Release -o /app/publish
RUN dotnet publish PetCare.Api/PetCare.Api.csproj -c Release -o /app/publish /p:UseAppHost=false /p:CopyLocalLockFileAssemblies=true

# ===== Runtime stage =====
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app

# Встановлюємо netcat
RUN apt-get update && \
    apt-get install -y --no-install-recommends netcat-traditional && \
    rm -rf /var/lib/apt/lists/*

# Копіюємо опубліковані файли
COPY --from=build /app/publish ./

# 🔹 Копіюємо також вихідні файли міграцій, щоб EF міг знайти атрибути
COPY --from=build /src/PetCare.Infrastructure/Migrations /app/PetCare.Infrastructure/Migrations

# Копіюємо скрипт очікування бази
COPY --from=build /src/PetCare.Api/wait-for-db.sh /wait-for-db.sh
RUN chmod +x /wait-for-db.sh

ENV ASPNETCORE_ENVIRONMENT=Development
ENV ASPNETCORE_URLS=http://+:5100

ENTRYPOINT ["/wait-for-db.sh", "db", "5432", "dotnet", "/app/PetCare.Api.dll"]
