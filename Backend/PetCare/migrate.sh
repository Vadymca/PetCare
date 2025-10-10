#!/bin/sh
# $1 = host, $2 = port

HOST=$1
PORT=$2

echo "Waiting for database $HOST:$PORT..."

while ! nc -z $HOST $PORT; do
  sleep 2
done

echo "Database ready! Applying migrations..."

# Виконуємо EF Core міграції в SDK stage
dotnet tool restore
dotnet ef database update \
  --project /src/PetCare.Infrastructure/PetCare.Infrastructure.csproj \
  --startup-project /src/PetCare.Api/PetCare.Api.csproj

echo "Migrations applied."
