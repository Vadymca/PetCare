export type UserRole = 'Admin' | 'User' | 'Moderator' | string;
export interface User {
  id: string;
  email: string;
  firstName: string;
  lastName: string;
  phone: string;
  role: UserRole;
  postalCode: string;
  address?: string;
  password?: string;
  language?: string;
  createdAt?: string;
  updatedAt?: string;
  points?: number;
  lastLogin?: string;
  profilePhoto?: string;
  emailConfirmed?: boolean;
}
