import { Injectable, signal, computed, effect, inject } from '@angular/core';
import { Router } from '@angular/router';
import { User } from '../models/user.model';
import { QuoteRequest } from '../models/quote-request.model';
import { HttpClient } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from '../environments/environment';

const TOKEN_KEY = 'auth_token';
const CUSTOMER_ID_KEY = 'customer_id';
const USERNAME_KEY = 'username';


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
    this.loadSession();
    effect(() => {
      if (!this.isLoggedIn()) {
        this.router.navigate(['/login']);
      }
    });
  }

  private loadSession() {
    if (typeof window !== 'undefined') {
      const token = localStorage.getItem(TOKEN_KEY);
      const customerId = localStorage.getItem(CUSTOMER_ID_KEY);
      const username = localStorage.getItem(USERNAME_KEY);

      if (token && username) {
        try {
          const payload = JSON.parse(atob(token.split('.')[1]));
          // Map the JWT claims to our User model
          const user: User = {
            id: payload.sub,
            customerId: customerId ?? undefined,
            email: payload.email,
            name: username,
            role: payload['http://schemas.microsoft.com/ws/2008/06/identity/claims/role']
          };
          this._currentUser.set(user);
        } catch (e) {
          console.error('Invalid token', e);
          // Clear all session data on error
          this.clearSession();
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
    return this.http.post<{ succeeded: boolean, token: string, customerId: string, name: string }>(`${environment.apiUrl}/auth/login`, { username, password })
      .pipe(
        map(response => {
          this.saveSessionAndSetUser(response.token, response.customerId, response.name);
          const targetUrl = this.isAdmin() ? '/admin' : '/catalog';
          this.router.navigate([targetUrl]);
          return true;
        })
      );
  }

  private saveSessionAndSetUser(token: string, customerId: string | null, name: string) {
    if (typeof window !== 'undefined') {
      localStorage.setItem(TOKEN_KEY, token);
      localStorage.setItem(USERNAME_KEY, name);
      if (customerId) {
        localStorage.setItem(CUSTOMER_ID_KEY, customerId);
      }
    }
    this.loadSession(); // Reload user from the new token and stored data
  }

  private clearSession() {
    const keys = [TOKEN_KEY, CUSTOMER_ID_KEY, USERNAME_KEY];
    keys.forEach(k => localStorage.removeItem(k));
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
    this.clearSession();
    this._currentUser.set(null);
    this.router.navigate(['/login']);
  }

  getQuoteRequests(): Observable<QuoteRequest[]> {
    return this.http.get<QuoteRequest[]>(`${environment.apiUrl}/quote-requests`);
  }
}