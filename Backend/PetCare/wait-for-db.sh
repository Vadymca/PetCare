#!/bin/sh
# Usage: /wait-for-db.sh host port [command args...]

set -e

host="$1"
port="$2"
shift 2

echo "Waiting for database $host:$port..."

# Цикл очікування бази
while ! nc -z "$host" "$port"; do
  echo "Database is unavailable - sleeping 2s..."
  sleep 2
done

echo "Database ready!"

# Виконуємо передану команду
exec "$@"
