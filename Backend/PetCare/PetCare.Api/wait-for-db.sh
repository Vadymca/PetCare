#!/bin/bash
host="$1"
port="$2"
shift 2
cmd="$@"

until nc -z "$host" "$port"; do
  >&2 echo "Waiting for database $host:$port..."
  sleep 1
done

>&2 echo "Database is up - starting command: $cmd"
exec $cmd