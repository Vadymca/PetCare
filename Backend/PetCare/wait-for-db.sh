#!/bin/sh
# $1 = host, $2 = port, $3.. = команда

HOST=$1
PORT=$2
shift 2

echo "Waiting for database $HOST:$PORT..."

while ! nc -z $HOST $PORT; do
  sleep 2
done

echo "Database ready!"

# Виконуємо команду (наприклад, dotnet PetCare.Api.dll)
exec "$@"
