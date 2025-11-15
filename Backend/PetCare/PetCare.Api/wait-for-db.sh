#!/bin/bash

host="$1"
port="$2"
shift 2
cmd="$@"

>&2 echo "Waiting for database connection at $host:$port..."

# Чекаємо на відкритий порт
until nc -z "$host" "$port"; do
  >&2 echo "Database port not ready yet..."
  sleep 1
done

>&2 echo "Database port is open, waiting for PostgreSQL to be ready..."

# Додатково чекаємо на готовність PostgreSQL
until PGPASSWORD=$POSTGRES_PASSWORD psql -h "$host" -U "$POSTGRES_USER" -d "$POSTGRES_DB" -c "SELECT 1" > /dev/null 2>&1; do
  >&2 echo "PostgreSQL is not ready yet..."
  sleep 2
done

>&2 echo "PostgreSQL is fully ready!"

# Застосовуємо міграції через migrations bundle
>&2 echo "Applying EF Core migrations using bundle..."

# Виконуємо migrations bundle
/app/efbundle --connection "$ConnectionStrings__DefaultConnection" --verbose

if [ $? -eq 0 ]; then
  >&2 echo "✅ Migrations applied successfully!"
else
  >&2 echo "❌ Failed to apply migrations. Exiting..."
  exit 1
fi

>&2 echo "Starting application..."
exec $cmd
