#!/bin/sh
# wait-for-db.sh

host="$1"
port="$2"
shift 2
cmd="$@"

echo "Waiting for database $host:$port..."

while ! nc -z $host $port; do
  sleep 1
done

echo "Database is up - starting command: $cmd"
exec $cmd
