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
  apiUrl: import.meta.env.VITE_API_URL ?? '/api',
};
