export interface UserPreferences {
  language: string;
}

export type UserRole = 'Admin' | 'User' | 'Moderator' | string;
export interface User {
  id: string;
  email: string;
  passwordHash: string;
  firstName: string;
  lastName: string;
  phone: string;
  role: UserRole;
  preferences?: UserPreferences;
}
