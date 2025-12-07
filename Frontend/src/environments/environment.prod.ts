// export const environment = {
//   production: true,
//   apiUrl: 'http://json-server:3000' // URL для Docker
// };

// export const environment = {
//   production: true,
//   apiUrl:
//     process.env['API_BASE_URL'] ||
//     'https://api-dobrodiy.kn314-uz.keenetic.pro/',
// };

export const environment = {
  production: true,
  // Fallback: Vite може не побачити env під час білда, тому використовуємо дефолт (твій бекенд)
  // На Render env перезапишеться runtime, якщо вона є
  apiUrl:
    import.meta.env.VITE_API_URL ||
    'https://api-dobrodiy.kn314-uz.keenetic.pro',
};
