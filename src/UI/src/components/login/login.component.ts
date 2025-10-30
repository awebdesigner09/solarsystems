
import { Component, ChangeDetectionStrategy, inject, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { CommonModule } from '@angular/common';
import { HttpErrorResponse } from '@angular/common/http';
import { AuthService } from '../../services/auth.service';
import { finalize } from 'rxjs/operators';

@Component({
  selector: 'app-login',
  template: `
    <div class="flex items-center justify-center min-h-screen bg-gray-900">
      <div class="w-full max-w-md p-8 space-y-6 bg-gray-800 rounded-lg shadow-md">
        <h2 class="text-2xl font-bold text-center text-gray-100">Login to SolarQuote</h2>
        <form [formGroup]="loginForm" (ngSubmit)="onSubmit()">
          
          <div class="mb-4">
            <label for="username" class="block text-sm font-medium text-gray-400">Username</label>
            <input id="username" type="text" formControlName="username" class="mt-1 block w-full px-3 py-2 bg-gray-700 border border-gray-600 rounded-md shadow-sm text-gray-200 placeholder-gray-400 focus:outline-none focus:ring-yellow-500 focus:border-yellow-500 sm:text-sm" placeholder="your_username">
            @if (loginForm.get('username')?.invalid && (loginForm.get('username')?.dirty || loginForm.get('username')?.touched)) {
              <div class="text-red-400 text-sm mt-1">
                @if (loginForm.get('username')?.errors?.['required']) {
                  <span>Username is required.</span>
                }
              </div>
            }
          </div>

          <div class="mb-6">
             <label for="password" class="block text-sm font-medium text-gray-400">Password</label>
            <input id="password" type="password" formControlName="password" class="mt-1 block w-full px-3 py-2 bg-gray-700 border border-gray-600 rounded-md shadow-sm text-gray-200 placeholder-gray-400 focus:outline-none focus:ring-yellow-500 focus:border-yellow-500 sm:text-sm" placeholder="••••••••">
             @if (loginForm.get('password')?.invalid && (loginForm.get('password')?.dirty || loginForm.get('password')?.touched)) {
              <div class="text-red-400 text-sm mt-1">
                @if (loginForm.get('password')?.errors?.['required']) {
                  <span>Password is required.</span>
                }
              </div>
            }
          </div>

          @if (errorMessage()) {
            <div class="mb-4 p-3 bg-red-900/50 text-red-300 rounded-md">
              {{ errorMessage() }}
            </div>
          }

          <div>
            <button type="submit" [disabled]="loginForm.invalid || isLoading()" class="w-full flex justify-center py-2 px-4 border border-transparent rounded-md shadow-sm text-sm font-bold text-gray-900 bg-yellow-500 hover:bg-yellow-600 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-yellow-500 disabled:bg-yellow-800 disabled:cursor-not-allowed">
              @if (isLoading()) {
                <span class="animate-spin rounded-full h-5 w-5 border-b-2 border-gray-900"></span>
              } @else {
                <span>Login</span>
              }
            </button>
          </div>
        </form>
        <p class="mt-4 text-center text-sm text-gray-400">
          Don't have an account? <a routerLink="/register" class="font-medium text-yellow-400 hover:text-yellow-300">Register here</a>
        </p>
      </div>
    </div>
  `,
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [ReactiveFormsModule, RouterLink, CommonModule],
})
export class LoginComponent {
  private fb = inject(FormBuilder);
  private authService = inject(AuthService);
  private router = inject(Router);

  isLoading = signal(false);
  errorMessage = signal<string | null>(null);

  loginForm = this.fb.group({
    username: ['', [Validators.required]],
    password: ['', Validators.required],
  });

  onSubmit() {
    if (this.loginForm.invalid) {
      return;
    }
    this.isLoading.set(true);
    this.errorMessage.set(null);
    const { username, password } = this.loginForm.value;

    this.authService.login(username!, password!)
      .pipe(
        finalize(() => this.isLoading.set(false))
      )
      .subscribe({
        next: () => {
          // Navigation is now handled inside the authService
        },
        error: (err: HttpErrorResponse) => {
          if (err.status === 401) {
            this.errorMessage.set('Invalid username or password. Please try again.');
          } else {
            this.errorMessage.set('An unexpected error occurred. Please try again later.');
          }
        }
      });
  }
}
