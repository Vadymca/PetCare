# PetCareApp

Android-додаток для управління інформацією про тварин.

## Вимоги
- Android Studio (остання версія)
- JDK 11
- Підключення до JSON-сервера[](http://json-server:3000)

## Налаштування
1. Відкрий проєкт у Android Studio.
2. Синхронізуй Gradle (File -> Sync Project with Gradle Files).
3. Переконайся, що JSON-сервер запущено на http://json-server:3000.Запуск JSON-сервера: `json-server --watch db.json --port 3000`
4. Запусти додаток на емуляторі або пристрої (Run -> Run 'app').

## Залежності
- Dagger 2, Retrofit, Room, Glide тощо (див. build.gradle.kts).