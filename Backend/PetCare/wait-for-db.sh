#!/bin/sh
# wait-for-db.sh
# Wait for PostgreSQL to be ready and apply EF Core migrations

set -e

HOST="$1"
PORT="$2"
shift 2

echo "Waiting for database $HOST:$PORT..."

# Чекаємо, поки PostgreSQL слухає порт
until nc -z "$HOST" "$PORT" 2>/dev/null; do
  echo "Waiting for DB..."
  sleep 1
done

echo "Database is ready."

# Перевірка наявності dotnet-ef і встановлення глобально, якщо нема
if ! command -v dotnet-ef >/dev/null 2>&1; then
  echo "Installing dotnet-ef tool..."
  dotnet tool install --global dotnet-ef
fi

# Додаємо шлях до глобальних інструментів у PATH
export PATH="$PATH:/root/.dotnet/tools"

# Запускаємо міграції з Infrastructure проекту
if [ -f "/app/PetCare.Infrastructure/PetCare.Infrastructure.csproj" ]; then
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
