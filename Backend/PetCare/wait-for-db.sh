#!/bin/sh
# wait-for-db.sh
# Usage: wait-for-db.sh <host> <port> <command> [args...]

host="$1"
port="$2"
shift 2

echo "Waiting for database $host:$port..."

# простий .NET TCP loop
while ! nc -z "$host" "$port"; do
  echo "Waiting for DB..."
  sleep 3
done

echo "Database is ready. Starting application..."
exec "$@"
