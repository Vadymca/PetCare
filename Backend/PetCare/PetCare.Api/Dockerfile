# ===== SDK stage =====
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

RUN apt-get update && \
    apt-get install -y --no-install-recommends netcat-traditional postgresql-client && \
    rm -rf /var/lib/apt/lists/*

RUN dotnet tool install --global dotnet-ef --version 9.0.7
ENV PATH="$PATH:/root/.dotnet/tools"

# --- Копіюємо csproj для кешу ---
COPY ["PetCare.Api/PetCare.Api.csproj", "PetCare.Api/"]
COPY ["PetCare.Application/PetCare.Application.csproj", "PetCare.Application/"]
COPY ["PetCare.Infrastructure/PetCare.Infrastructure.csproj", "PetCare.Infrastructure/"]
COPY ["PetCare.Domain/PetCare.Domain.csproj", "PetCare.Domain/"]
RUN dotnet restore "PetCare.Api/PetCare.Api.csproj"

# --- Копіюємо весь код ---
COPY . .

# --- Будуємо рішення (вибери одне з двох) ---
# Якщо є PetCare.sln:
RUN dotnet build PetCare.sln -c Release
# Якщо рішення немає — використовуй замість попереднього рядка:
# RUN dotnet build PetCare.Api/PetCare.Api.csproj -c Release

# --- Публікуємо Infrastructure окремо (щоб міграції компілювались у DLL) ---
RUN dotnet publish PetCare.Infrastructure/PetCare.Infrastructure.csproj \
    -c Release \
    -o /app/infrastructure \
    /p:UseAppHost=false

# --- Публікуємо API ---
RUN dotnet publish PetCare.Api/PetCare.Api.csproj \
    -c Release \
    -o /app/publish \
    /p:UseAppHost=false

# ===== Runtime stage =====
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app

RUN apt-get update && \
    apt-get install -y --no-install-recommends netcat-traditional && \
    rm -rf /var/lib/apt/lists/*

COPY --from=build /root/.dotnet/tools /root/.dotnet/tools
ENV PATH="$PATH:/root/.dotnet/tools"

COPY --from=build /app/publish .
COPY --from=build /app/infrastructure ./PetCare.Infrastructure

COPY --from=build /src/PetCare.Api/wait-for-db.sh /wait-for-db.sh
RUN chmod +x /wait-for-db.sh

ENV ASPNETCORE_ENVIRONMENT=Development
ENV ASPNETCORE_URLS=http://+:5100

ENTRYPOINT ["/wait-for-db.sh", "db", "5432", "dotnet", "/app/PetCare.Api.dll"]
