# ===== Base SDK image =====
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS base
WORKDIR /app
EXPOSE 80
RUN apt-get update && \
    apt-get install -y postgresql-client netcat-traditional && \
    rm -rf /var/lib/apt/lists/*

# ===== Build stage (SDK) =====
FROM base AS build
WORKDIR /src
COPY ["PetCare.Api/PetCare.Api.csproj", "PetCare.Api/"]
COPY ["PetCare.Application/PetCare.Application.csproj", "PetCare.Application/"]
COPY ["PetCare.Infrastructure/PetCare.Infrastructure.csproj", "PetCare.Infrastructure/"]
COPY ["PetCare.Domain/PetCare.Domain.csproj", "PetCare.Domain/"]
COPY ["PetCare.Api/stylecop.json", "PetCare.Api/"]
RUN dotnet restore "./PetCare.Api/PetCare.Api.csproj"
COPY . .
WORKDIR "/src/PetCare.Api"
RUN dotnet build "./PetCare.Api.csproj" -c Release -o /app/build
RUN dotnet publish "./PetCare.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# ===== Final image (runtime) =====
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish ./
COPY wait-for-db.sh /wait-for-db.sh
RUN chmod +x /wait-for-db.sh
ENTRYPOINT ["/wait-for-db.sh", "db", "5432", "dotnet", "PetCare.Api.dll"]