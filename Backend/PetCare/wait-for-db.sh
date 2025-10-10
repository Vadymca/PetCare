#!/bin/sh
# wait-for-db.sh

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

# Ensure dotnet-ef tool is available
if ! command -v dotnet-ef > /dev/null; then
  echo "Installing dotnet-ef global tool..."
  dotnet tool install --global dotnet-ef || true
  export PATH="$PATH:/root/.dotnet/tools"
fi

# Запускаємо міграції з Infrastructure проекту
if [ -d "/app/PetCare.Infrastructure" ] && [ -f "/app/PetCare.Api/PetCare.Api.csproj" ]; then
  echo "Applying EF Core migrations..."
  set +e
  dotnet ef database update \
    --no-build \
    --project /app/PetCare.Infrastructure/PetCare.Infrastructure.csproj \
    --startup-project /app/PetCare.Api/PetCare.Api.csproj
  if [ $? -ne 0 ]; then
    echo "EF Core migrations failed or already applied. Continuing..."
  fi
  set -e
fi

# Запускаємо основну команду
if [ "$#" -gt 0 ]; then
  echo "Starting application..."
  exec "$@"
fi
