#!/bin/sh
# wait-for-db.sh
# Script waits for a database to be ready before starting the application
# Usage: ./wait-for-db.sh <host> <port> <command...>

set -e

host="$1"
port="$2"
shift 2
cmd="$@"

echo "Waiting for database at $host:$port..."

# Чекаємо поки порт стане доступним
while ! nc -z "$host" "$port"; do
  echo "Database is unavailable - sleeping 1s..."
  sleep 1
done

echo "Database is up! Starting application..."

# Виконуємо команду, передану скрипту (наприклад, dotnet PetCare.Api.dll)
exec $cmd
