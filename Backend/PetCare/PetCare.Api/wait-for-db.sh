#!/bin/bash
set -e

host="$1"
port="$2"
shift 2
cmd="$@"

echo "=== Waiting for database $host:$port ==="

max_attempts=30
attempt=0

until nc -z "$host" "$port"; do
  attempt=$((attempt + 1))
  if [ $attempt -ge $max_attempts ]; then
    >&2 echo "ERROR: Database $host:$port did not become available after $max_attempts attempts"
    exit 1
  fi
  >&2 echo "Attempt $attempt/$max_attempts: Database is unavailable - sleeping..."
  sleep 2
done

>&2 echo "=== Database $host:$port is up ==="
>&2 echo "=== Starting application with command: $cmd ==="

exec $cmd