// src/env.d.ts
interface ImportMetaEnv {
  readonly NG_APP_API_URL?: string;
  // якщо в майбутньому додаш ще змінні — пиши їх тут
}

interface ImportMeta {
  readonly env: ImportMetaEnv;
}
