
export interface User {
  id: string;
  customerId?: string;
  email: string;
  name: string;
  role: 'customer' | 'admin';
}
