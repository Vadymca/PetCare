#!/bin/sh
# wait-for-db.sh
# Waits for a TCP service (Postgres) to become available, then executes the given command.

set -e

host="$1"
port="$2"
shift 2
cmd="$@"

# Check that host and port are provided
if [ -z "$host" ] || [ -z "$port" ]; then
  echo "Usage: $0 <host> <port> <command>"
  exit 1
fi

echo "Waiting for database $host:$port..."

# Loop until the database is available
while ! nc -z "$host" "$port"; do
  echo "Database is unavailable - sleeping 2s..."
  sleep 2
done

echo "Database is available! Running command..."
exec $cmd
