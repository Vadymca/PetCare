#!/bin/bash
set -e

# -------------------------
# wait-for-db.sh
# -------------------------
# Usage: ./wait-for-db.sh <db_host> <db_port> <command> [args...]
# Example: ./wait-for-db.sh db 5432 dotnet PetCare.Api.dll
# -------------------------

DB_HOST=$1
DB_PORT=$2
shift 2

CMD="$@"

echo "Waiting for database $DB_HOST:$DB_PORT..."

# Чекаємо, поки база буде доступна
while ! nc -z $DB_HOST $DB_PORT; do
  echo "Database is unavailable - sleeping"
  sleep 2
done

echo "Database is up - running migrations"

# Виконуємо міграції EF Core
dotnet ef database update --project PetCare.Infrastructure --startup-project PetCare.Api

echo "Migrations applied - starting application"

# Запускаємо API
exec $CMD
