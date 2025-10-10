#!/bin/sh
set -e

HOST="$1"
PORT="$2"
shift 2

echo "Waiting for database $HOST:$PORT..."

until nc -z "$HOST" "$PORT" 2>/dev/null; do
  echo "Waiting for DB..."
  sleep 1
done

echo "Database is ready."

# Виконуємо міграції через SDK
if [ -d "/app/PetCare.Infrastructure" ]; then
  echo "Checking for dotnet-ef..."
  if ! command -v dotnet-ef >/dev/null 2>&1; then
    echo "Error: dotnet-ef is not installed or not found in PATH"
    exit 1
  fi
  echo "Checking for project.assets.json..."
  ls -la /app/PetCare.Api/obj/project.assets.json 2>/dev/null || echo "Warning: /app/PetCare.Api/obj/project.assets.json not found"
  ls -la /app/PetCare.Infrastructure/obj/project.assets.json 2>/dev/null || echo "Warning: /app/PetCare.Infrastructure/obj/project.assets.json not found"
  echo "Applying EF Core migrations..."
  dotnet ef database update \
      --no-build \
      --project /app/PetCare.Infrastructure/PetCare.Infrastructure.csproj \
      --startup-project /app/PetCare.Api/PetCare.Api.csproj
fi

# Запускаємо основну команду
if [ "$#" -gt 0 ]; then
  echo "Starting application..."
  exec "$@"
fi