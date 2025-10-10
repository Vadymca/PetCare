#!/bin/bash
set -e

HOST=db
PORT=5432

echo "Waiting for database $HOST:$PORT..."

# Чекаємо поки БД буде доступна
while ! nc -z $HOST $PORT; do
  echo "Waiting for database..."
  sleep 2
done

echo "Database ready. Applying migrations..."

dotnet ef database update \
  --project /src/PetCare.Infrastructure/PetCare.Infrastructure.csproj \
  --startup-project /src/PetCare.Api/PetCare.Api.csproj

echo "Migrations applied."
