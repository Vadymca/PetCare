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

echo "Applying EF Core migrations..."
dotnet tool restore
dotnet ef database update \
    --project /app/PetCare.Infrastructure/PetCare.Infrastructure.csproj \
    --startup-project /app/PetCare.Api.dll
echo "Migrations applied."

# Start the API
exec "$@"
