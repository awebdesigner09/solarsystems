
import { Component, ChangeDetectionStrategy, inject, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { CommonModule } from '@angular/common';
import { AuthService } from '../../services/auth.service';
import { finalize } from 'rxjs/operators';

@Component({
  selector: 'app-registration',
  template: `
    <div class="flex items-center justify-center min-h-screen bg-gray-900">
      <div class="w-full max-w-md p-8 space-y-6 bg-gray-800 rounded-lg shadow-md">
        <h2 class="text-2xl font-bold text-center text-gray-100">Create your SolarQuote Account</h2>
        <form [formGroup]="registerForm" (ngSubmit)="onSubmit()">

          <div class="mb-4">
            <label for="name" class="block text-sm font-medium text-gray-400">Full Name</label>
            <input id="name" type="text" formControlName="name" class="mt-1 block w-full px-3 py-2 bg-gray-700 border border-gray-600 rounded-md shadow-sm text-gray-200 placeholder-gray-400 focus:outline-none focus:ring-yellow-500 focus:border-yellow-500 sm:text-sm" placeholder="John Doe">
            @if (registerForm.get('name')?.invalid && registerForm.get('name')?.touched) {
              <div class="text-red-400 text-sm mt-1">
                <span>Name is required.</span>
              </div>
            }
          </div>
          
          <div class="mb-4">
            <label for="email" class="block text-sm font-medium text-gray-400">Email Address</label>
            <input id="email" type="email" formControlName="email" class="mt-1 block w-full px-3 py-2 bg-gray-700 border border-gray-600 rounded-md shadow-sm text-gray-200 placeholder-gray-400 focus:outline-none focus:ring-yellow-500 focus:border-yellow-500 sm:text-sm" placeholder="you@example.com">
             @if (registerForm.get('email')?.invalid && registerForm.get('email')?.touched) {
              <div class="text-red-400 text-sm mt-1">
                @if (registerForm.get('email')?.errors?.['required']) {
                  <span>Email is required.</span>
                }
                @if (registerForm.get('email')?.errors?.['email']) {
                  <span>Please enter a valid email.</span>
                }
              </div>
            }
          </div>

          <div class="mb-6">
            <label for="password" class="block text-sm font-medium text-gray-400">Password</label>
            <input id="password" type="password" formControlName="password" class="mt-1 block w-full px-3 py-2 bg-gray-700 border border-gray-600 rounded-md shadow-sm text-gray-200 placeholder-gray-400 focus:outline-none focus:ring-yellow-500 focus:border-yellow-500 sm:text-sm" placeholder="••••••••">
             @if (registerForm.get('password')?.invalid && registerForm.get('password')?.touched) {
              <div class="text-red-400 text-sm mt-1">
                @if (registerForm.get('password')?.errors?.['required']) {
                  <span>Password is required.</span>
                }
                @if (registerForm.get('password')?.errors?.['minlength']) {
                  <span>Password must be at least 6 characters long.</span>
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
            <button type="submit" [disabled]="registerForm.invalid || isLoading()" class="w-full flex justify-center py-2 px-4 border border-transparent rounded-md shadow-sm text-sm font-bold text-gray-900 bg-yellow-500 hover:bg-yellow-600 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-yellow-500 disabled:bg-yellow-800 disabled:cursor-not-allowed">
              @if (isLoading()) {
                <span class="animate-spin rounded-full h-5 w-5 border-b-2 border-gray-900"></span>
              } @else {
                <span>Register</span>
              }
            </button>
          </div>
        </form>
        <p class="mt-4 text-center text-sm text-gray-400">
          Already have an account? <a routerLink="/login" class="font-medium text-yellow-400 hover:text-yellow-300">Login here</a>
        </p>
      </div>
    </div>
  `,
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [ReactiveFormsModule, RouterLink, CommonModule],
})
export class RegistrationComponent {
  private fb = inject(FormBuilder);
  private authService = inject(AuthService);
  private router = inject(Router);

  isLoading = signal(false);
  errorMessage = signal<string | null>(null);

  registerForm = this.fb.group({
    name: ['', Validators.required],
    email: ['', [Validators.required, Validators.email]],
    password: ['', [Validators.required, Validators.minLength(6)]],
  });

  onSubmit() {
    if (this.registerForm.invalid) {
      return;
    }
    this.isLoading.set(true);
    this.errorMessage.set(null);
    const { name, email, password } = this.registerForm.value;

    this.authService.register(name!, email!, password!)
      .pipe(
        finalize(() => this.isLoading.set(false))
      )
      .subscribe({
        next: () => {
          this.router.navigate(['/catalog']);
        },
        error: (err) => {
          this.errorMessage.set(err.message || 'Registration failed. Please try again.');
        }
      });
  }
}
