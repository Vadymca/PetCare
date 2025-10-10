#!/bin/sh
# wait-for-db.sh

set -e

HOST="$1"
PORT="$2"
shift 2

echo "Waiting for database $HOST:$PORT..."

# „екаЇмо на в≥дкритт€ порту (netcat)
while ! nc -z "$HOST" "$PORT"; do
  sleep 1
done

echo "Database is ready. Starting application..."

# якщо залишилис€ аргументи Ч виконуЇмо њх
if [ "$#" -gt 0 ]; then
  exec "$@"
fi
