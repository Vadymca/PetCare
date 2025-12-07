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

import { GLOBAL_ENV } from '../app/global-env';

export const environment = {
  production: true,
  // Чисто, типобезпечно і без any
  apiUrl: GLOBAL_ENV.API_BASE_URL,
};
