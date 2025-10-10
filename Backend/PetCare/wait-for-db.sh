#!/bin/sh
set -e

host="$1"
port="$2"
shift 2
cmd="$@"

echo "Waiting for database $host:$port..."

while ! nc -z $host $port; do
  echo "Database is unavailable - sleeping"
  sleep 2
done

echo "Database ready! Applying EF Core migrations..."

# Виконати міграції у SDK (тут /app вже містить лише dll, тому робимо через dotnet run у SDK)
dotnet ef database update \
    --project /src/PetCare.Infrastructure/PetCare.Infrastructure.csproj \
    --startup-project /src/PetCare.Api/PetCare.Api.csproj

echo "Migrations applied. Starting app..."
exec $cmd
