#!/bin/sh

# Parameters
HOST="$1"
PORT="$2"
shift 2
CMD="$@"

echo "Очікуємо запуск бази $HOST:$PORT..."
while ! nc -z $HOST $PORT; do
  sleep 1
done

echo "База доступна. Запускаємо додаток..."
exec $CMD
