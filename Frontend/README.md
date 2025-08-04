# PetcareFrontend

This project was generated using [Angular CLI](https://github.com/angular/angular-cli) version 19.0.3.

## Development server

To start a local development server, run:

```bash
ng serve
```

Once the server is running, open your browser and navigate to `http://localhost:4200/`. The application will automatically reload whenever you modify any of the source files.

## Code scaffolding

Angular CLI includes powerful code scaffolding tools. To generate a new component, run:

```bash
ng generate component component-name
```

For a complete list of available schematics (such as `components`, `directives`, or `pipes`), run:

```bash
ng generate --help
```

## Building

To build the project run:

```bash
ng build
```

This will compile your project and store the build artifacts in the `dist/` directory. By default, the production build optimizes your application for performance and speed.

## Running unit tests

To execute unit tests with the [Karma](https://karma-runner.github.io) test runner, use the following command:

```bash
ng test
```

## Running end-to-end tests

For end-to-end (e2e) testing, run:

```bash
ng e2e
```

Angular CLI does not come with an end-to-end testing framework by default. You can choose one that suits your needs.

## Additional Resources

For more information on using the Angular CLI, including detailed command references, visit the [Angular CLI Overview and Command Reference](https://angular.dev/tools/cli) page.

## Authentication API

### Мок-версія (Mock Backend)

1. POST /auth/login
   Опис: Логін користувача з емейлом і паролем.

Вхідні дані:

json
Copy
Edit
{
"email": "user1@example.com",
"password": "password"
}
Відповідь:

При коректних даних: 200 OK, повертає "2fa_required" (потрібно ввести 2FA код).

При некоректних — помилка 401 Unauthorized або Invalid credentials.

2. POST /auth/verify-2fa
   Опис: Верифікація 2FA коду.

Вхідні дані:

json
Copy
Edit
{
"code": "123456"
}
Відповідь:

При коректному коді: 200 OK, повертає об’єкт користувача з access токеном (mock-jwt-token).

При неправильному коді — помилка 401 Unauthorized або Invalid 2FA code.

3. POST /auth/logout
   Опис: Вихід з системи. Очищає локальні дані.

Вхідні дані: Порожній body.

Відповідь: 200 OK

4. POST /auth/refresh
   Опис: Оновлення access токена.

Вхідні дані: Порожній body.

Відповідь:

json
Copy
Edit
{
"accessToken": "mocked-access-token-12345",
"user": {
"id": "123",
"email": "test@example.com",
"firstName": "User",
"lastName": "Test",
"role": "Admin"
}
}

5.  Якщо залишити метод ось так, то це перевірка варіанту, коли в куках є актуальний refresh token
    refreshToken(): Observable<{ accessToken: string }> {
    // Імітація затримки відповіді, наприклад 500ms
    const fakeAccessToken = 'mocked-access-token-12345';

        return of({ accessToken: fakeAccessToken }).pipe(
          delay(500), // імітуємо мережеву затримку
          tap(response => {
            this.accessToken.set(response.accessToken);
            this.currentUser.set(this.mockUser);
          })
        );

    }

6.  Якщо закоментувати рядки, буде імітація, коли в куках немає актуального refresh token
    //this.accessToken.set(response.accessToken);
    //this.currentUser.set(this.mockUser);

### Реальний бекенд (Production)

1. POST /auth/login
   Опис: Логін користувача.

Вхідні дані:

json
Copy
Edit
{
"email": "user@example.com",
"password": "your_password"
}
Відповідь:

Успіх: HTTP 200 OK, може повернути 2fa_required якщо є двофакторна автентифікація.

Неуспіх: HTTP 401 Unauthorized.

2. POST /auth/verify-2fa
   Опис: Верифікація 2FA коду.

Вхідні дані:

json
Copy
Edit
{
"code": "xxxxxx"
}
Відповідь:

json
Copy
Edit
{
"accessToken": "eyJhbGciOiJIUzI1...",
"user": {
"id": "string",
"email": "user@example.com",
"firstName": "John",
"lastName": "Doe",
"role": "User"
}
}
Примітка: Refresh токен зберігається в HttpOnly cookie.

3. POST /auth/logout
   Опис: Логаут. Очищає сесію.

Вхідні дані: Порожній body.

Відповідь: HTTP 200 OK.

4. POST /auth/refresh
   Опис: Оновлення access токена (автоматично використовує refresh токен із куків).

Вхідні дані: Порожній body.

Відповідь:

json
Copy
Edit
{
"accessToken": "eyJhbGciOiJIUzI1...",
"user": {
"id": "string",
"email": "user@example.com",
"firstName": "John",
"lastName": "Doe",
"role": "User"
}
}
Куки: Refresh токен має бути HttpOnly.
