#!/bin/sh
# Usage: ./wait-for-db.sh <host> <port> <cmd> <args...>

HOST=$1
PORT=$2
shift 2
CMD="$@"

echo "Waiting for database $HOST:$PORT..."

until nc -z "$HOST" "$PORT"; do
  echo "Database is not ready, sleeping..."
  sleep 2
done

echo "Database ready!"
exec $CMD
