# --------------------------
# Base image for runtime
# --------------------------
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80

# --------------------------
# Build stage
# --------------------------
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Копіюємо лише csproj для restore (швидше)
COPY ["PetCare.Api/PetCare.Api.csproj", "PetCare.Api/"]
COPY ["PetCare.Application/PetCare.Application.csproj", "PetCare.Application/"]
COPY ["PetCare.Infrastructure/PetCare.Infrastructure.csproj", "PetCare.Infrastructure/"]
COPY ["PetCare.Domain/PetCare.Domain.csproj", "PetCare.Domain/"]

RUN dotnet restore "PetCare.Api/PetCare.Api.csproj"

# Копіюємо весь код після restore
COPY . .

WORKDIR "/src/PetCare.Api"

# Build без appsettings.json
RUN dotnet build "PetCare.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# --------------------------
# Publish stage
# --------------------------
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "PetCare.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# --------------------------
# Final stage
# --------------------------
FROM base AS final
WORKDIR /app

# Копіюємо опубліковані файли
COPY --from=publish /app/publish .

# Використовуємо ENTRYPOINT
ENTRYPOINT ["sh", "-c", "dotnet ef database update --project PetCare.Infrastructure --startup-project PetCare.Api && dotnet PetCare.Api.dll"]