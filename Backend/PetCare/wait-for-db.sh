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

>&2 echo "PostgreSQL is fully ready! Starting application..."
sleep 2
exec $cmd