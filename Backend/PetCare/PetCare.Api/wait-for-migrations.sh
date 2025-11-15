#!/bin/bash
host="$1"
port="$2"
shift 2
cmd="$@"

echo "Waiting for database $host:$port to be ready..."

# Wait for database to be reachable
until nc -z "$host" "$port"; do
  >&2 echo "Database is not ready yet. Waiting..."
  sleep 2
done

echo "Database is ready. Starting application..."

# The application (PetCare.Api) handles migrations automatically on startup via:
# await dbContext.Database.MigrateAsync(); in Program.cs (lines 501)
# This ensures migrations are applied before the application starts serving requests

echo "Starting application: $cmd"
echo "Note: Application will automatically apply migrations and create the __EFMigrationsHistory table if needed"

exec $cmd