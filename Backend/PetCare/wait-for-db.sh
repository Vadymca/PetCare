#!/bin/sh
# Simple wait-for-db script
host="$1"
port="$2"
shift 2
cmd="$@"

until nc -z "$host" "$port"; do
  echo "Database is unavailable - sleeping 2s..."
  sleep 2
done

echo "Database ready!"
exec $cmd
