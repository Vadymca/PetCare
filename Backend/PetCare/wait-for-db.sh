#!/bin/sh
# wait-for-db.sh
# ---------------------------
# Waits for a PostgreSQL database to become available
# Usage: ./wait-for-db.sh host port [cmd args...]

set -e

HOST="$1"
PORT="$2"
shift 2

echo "Waiting for database $HOST:$PORT..."

# Чекаємо, поки база буде доступна
until nc -z "$HOST" "$PORT"; do
  echo "Database is unavailable - sleeping 2s..."
  sleep 2
done

echo "Database ready!"

# Виконуємо передану команду (наприклад, dotnet PetCare.Api.dll або dotnet ef database update)
exec "$@"
