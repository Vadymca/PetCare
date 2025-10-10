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
  echo "Applying EF Core migrations..."
  dotnet ef database update \
      --no-build \
      --project /app/PetCare.Infrastructure/PetCare.Infrastructure.csproj \
      --startup-project /app/PetCare.Api.csproj
fi

# Запускаємо основну команду
if [ "$#" -gt 0 ]; then
  echo "Starting application..."
  exec "$@"
fi