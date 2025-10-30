import { Injectable, signal, computed, effect, inject } from '@angular/core';
import { Router } from '@angular/router';
import { User } from '../models/user.model';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from '../environments/environment';

const TOKEN_KEY = 'auth_token';


@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private router = inject(Router);
  private _currentUser = signal<User | null>(null);
  private http = inject(HttpClient);

  public currentUser = this._currentUser.asReadonly();
  public isLoggedIn = computed(() => this._currentUser() !== null);
  // The role claim from the backend is a URL, so we check for 'Admin' at the end.
  public isAdmin = computed(() => 
    this._currentUser()?.role.endsWith('Admin') ?? false
  );

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
          // Map the JWT claims to our User model
          const user: User = {
            id: payload.sub,
            email: payload.email,
            name: payload.name,
            role: payload['http://schemas.microsoft.com/ws/2008/06/identity/claims/role']
          };
          this._currentUser.set(user);
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

  login(username: string, password: string): Observable<boolean> {
    return this.http.post<{ token:string }>(`${environment.apiUrl}/auth/login`, { username, password })
      .pipe(
        map(response => {
          this.saveTokenAndSetUser(response.token);
          const targetUrl = this.isAdmin() ? '/admin' : '/catalog';
          this.router.navigate([targetUrl]);
          return true;
        })
      );
  }

  private saveTokenAndSetUser(token: string) {
    if (typeof window !== 'undefined') {
      localStorage.setItem(TOKEN_KEY, token);
    }
    this.loadToken(); // Reload user from the new token
  }

  register(
    username: string,
    fullName: string,
    email: string, 
    password: string,
    addressLine1: string,
    addressLine2: string | null,
    city: string,
    state: string,
    postalCode: string,
    country: string | null
  ): Observable<any> {
    const registerPayload = {
      username: username,
      fullName: fullName,
      email,
      password,
      addressLine1,
      addressLine2,
      city,
      state,
      postalCode,
      country
    };
    return this.http.post(`${environment.apiUrl}/auth/register`, registerPayload);
  }
  
  logout() {
    if (typeof window !== 'undefined') {
      localStorage.removeItem(TOKEN_KEY);
    }
    this._currentUser.set(null);
    this.router.navigate(['/login']);
  }
}