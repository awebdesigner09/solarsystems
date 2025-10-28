import { Injectable, signal, computed, effect, inject } from '@angular/core';
import { Router } from '@angular/router';
import { User } from '../models/user.model';
import { of, throwError } from 'rxjs';
import { delay, switchMap } from 'rxjs/operators';

const TOKEN_KEY = 'auth_token';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private router = inject(Router);
  private _currentUser = signal<User | null>(null);

  public currentUser = this._currentUser.asReadonly();
  public isLoggedIn = computed(() => this._currentUser() !== null);
  public isAdmin = computed(() => this._currentUser()?.role === 'admin');

  constructor() {
    this.loadToken();
    effect(() => {
      if (!this.isLoggedIn()) {
        this.router.navigate(['/login']);
      }
    });
  }

  private loadToken() {
    if (typeof window !== 'undefined') {
      const token = localStorage.getItem(TOKEN_KEY);
      if (token) {
        try {
          const payload = JSON.parse(atob(token.split('.')[1]));
          this._currentUser.set(payload);
        } catch (e) {
          console.error('Invalid token', e);
          localStorage.removeItem(TOKEN_KEY);
        }
      }
    }
  }

  getToken(): string | null {
    if (typeof window !== 'undefined') {
      return localStorage.getItem(TOKEN_KEY);
    }
    return null;
  }

  login(email: string) {
    // Mock API call
    const isAdmin = email.includes('admin');
    const user: User = {
      id: isAdmin ? 'admin-1' : 'cust-123',
      email,
      name: isAdmin ? 'Admin User' : 'Customer Joe',
      role: isAdmin ? 'admin' : 'customer',
    };

    // Simulate JWT: header.payload.signature
    const mockToken = `header.${btoa(JSON.stringify(user))}.signature`;
    
    if (typeof window !== 'undefined') {
      localStorage.setItem(TOKEN_KEY, mockToken);
    }
    this._currentUser.set(user);
    const targetUrl = isAdmin ? '/admin' : '/catalog';
    return of(true).pipe(delay(500)); // Simulate network delay
  }

  register(name: string, email: string, password: string) {
    // Mock user creation. In a real app, this would be an API call
    // that would also handle duplicate email checks.
    if (email.includes('admin')) {
      return of(false).pipe(
          delay(500), 
          switchMap(() => throwError(() => new Error('Cannot register with an admin email.')))
      );
    }
    
    const newUser: User = {
      id: `cust-${Date.now()}`,
      email,
      name,
      role: 'customer',
    };

    // Simulate successful registration and automatically log in
    const mockToken = `header.${btoa(JSON.stringify(newUser))}.signature`;
    
    if (typeof window !== 'undefined') {
      localStorage.setItem(TOKEN_KEY, mockToken);
    }
    this._currentUser.set(newUser);
    
    return of(true).pipe(delay(800)); // Simulate network delay
  }

  logout() {
    if (typeof window !== 'undefined') {
      localStorage.removeItem(TOKEN_KEY);
    }
    this._currentUser.set(null);
    this.router.navigate(['/login']);
  }
}