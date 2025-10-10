Помилка, яку ви отримуєте, System.IO.FileNotFoundException: Could not find file '/app/obj/project.assets.json', вказує на те, що інструмент dotnet ef намагається знайти файл project.assets.json, який генерується під час відновлення залежностей (dotnet restore), але цей файл відсутній у фінальному Docker-образі. Це відбувається тому, що ви копіюєте лише .csproj файли та вихідні файли публікації (/app/publish), але не копіюєте папку obj, яка містить project.assets.json, необхідний для виконання EF Core міграцій.

Причина проблеми
Файл project.assets.json створюється в папці obj під час виконання dotnet restore і потрібен для інструменту dotnet ef, щоб визначити залежності проєкту. У вашому Dockerfile ви копіюєте лише:

Опубліковані файли з /app/publish.
.csproj файли та папки PetCare.Infrastructure, PetCare.Domain, PetCare.Application до /app.

Але папка obj (з project.assets.json) не копіюється до фінального образу. Під час виконання dotnet ef database update у wait-for-db.sh інструмент намагається знайти project.assets.json у /app/obj, але її там немає, що викликає помилку.

Виправлення
Щоб вирішити проблему, потрібно забезпечити, щоб project.assets.json був доступний у фінальному образі. Для цього необхідно виконати dotnet restore для проєктів у фінальному образі перед виконанням міграцій, а також скопіювати необхідні файли для відновлення залежностей. Ось як це зробити:


Виконайте dotnet restore у фінальному образі:
У Dockerfile додайте команду dotnet restore для PetCare.Api.csproj і PetCare.Infrastructure.csproj у секції final, щоб згенерувати project.assets.json.


Оновіть Dockerfile:
Оновіть секцію final, щоб скопіювати весь вихідний код проєктів (не лише .csproj) і виконати dotnet restore. Ось повна версія оновленого Dockerfile:
dockerfile# ===== Base SDK image =====
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS base
WORKDIR /app
EXPOSE 80

# Install PostgreSQL client and netcat
RUN apt-get update && \
    apt-get install -y postgresql-client netcat-traditional && \
    rm -rf /var/lib/apt/lists/*

# ===== Build stage =====
FROM base AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["PetCare.Api/PetCare.Api.csproj", "PetCare.Api/"]
COPY ["PetCare.Application/PetCare.Application.csproj", "PetCare.Application/"]
COPY ["PetCare.Infrastructure/PetCare.Infrastructure.csproj", "PetCare.Infrastructure/"]
COPY ["PetCare.Domain/PetCare.Domain.csproj", "PetCare.Domain/"]

RUN dotnet restore "./PetCare.Api/PetCare.Api.csproj"

COPY . .
WORKDIR "/src/PetCare.Api"
RUN dotnet build "./PetCare.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# ===== Publish stage =====
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./PetCare.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# ===== Final image (SDK for migrations) =====
FROM base AS final
WORKDIR /app

# Copy published files
COPY --from=publish /app/publish ./
# Copy source code for migrations
COPY ["PetCare.Api", "/app/PetCare.Api/"]
COPY ["PetCare.Infrastructure", "/app/PetCare.Infrastructure/"]
COPY ["PetCare.Domain", "/app/PetCare.Domain/"]
COPY ["PetCare.Application", "/app/PetCare.Application/"]

# Restore dependencies to generate project.assets.json
RUN dotnet restore "/app/PetCare.Api/PetCare.Api.csproj"
RUN dotnet restore "/app/PetCare.Infrastructure/PetCare.Infrastructure.csproj"

# Install dotnet-ef global tool
RUN dotnet tool install --global dotnet-ef --version 9.0.7

# Copy wait-for-db script
COPY wait-for-db.sh /wait-for-db.sh
RUN chmod +x /wait-for-db.sh

# Ensure dotnet global tools are in PATH (for dotnet-ef)
ENV PATH="$PATH:/root/.dotnet/tools"

# Run app only after DB is ready
ENTRYPOINT ["/wait-for-db.sh", "db", "5432", "dotnet", "PetCare.Api.dll"]