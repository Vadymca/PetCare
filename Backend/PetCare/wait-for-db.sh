#!/bin/sh

# Параметри
HOST="$1"
PORT="$2"
shift 2
CMD="$@"

echo "Очікуємо запуск бази $HOST:$PORT..."
while ! nc -z $HOST $PORT; do
  sleep 1
done

echo "База доступна. Виконуємо міграції..."
dotnet tool restore # якщо використовується manifest
dotnet ef database update \
    --project PetCare.Infrastructure/PetCare.Infrastructure.csproj \
    --startup-project PetCare.Api/PetCare.Api.csproj \
    --configuration Release

echo "Запускаємо додаток..."
exec $CMD
