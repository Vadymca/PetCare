export type UserRole = 'Admin' | 'User' | 'Moderator' | string;
export interface User {
  id: string;
  email: string;
  passwordHash?: string;
  password?: string;
  firstName: string;
  lastName: string;
  phone: string;
  role: UserRole;
  points?: number;
  lastLogin?: string;
  profilePhoto?: string;
  createdAt?: string;
  updatedAt?: string;
}
