
import { Component, ChangeDetectionStrategy, inject, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { CommonModule } from '@angular/common';
import { HttpErrorResponse } from '@angular/common/http';
import { AuthService } from '../../services/auth.service';
import { finalize } from 'rxjs/operators';
import { ToastService } from '../../services/toast.service';

@Component({
  selector: 'app-registration',
  template: `
<div class="flex items-center justify-center min-h-screen bg-gray-900">
  <div class="w-full max-w-lg p-8 space-y-6 bg-gray-800 rounded-lg shadow-md">
    <h2 class="text-2xl font-bold text-center text-gray-100">Create your SolarQuote Account</h2>
    <form [formGroup]="registerForm" (ngSubmit)="onSubmit()">

      <div class="grid grid-cols-1 md:grid-cols-2 gap-4 mb-4">
        <div>
          <label for="username" class="block text-sm font-medium text-gray-400">Username</label>
          <input id="username" type="text" formControlName="username" class="mt-1 block w-full px-3 py-2 bg-gray-700 border border-gray-600 rounded-md shadow-sm text-gray-200 placeholder-gray-400 focus:outline-none focus:ring-yellow-500 focus:border-yellow-500 sm:text-sm" placeholder="johndoe">
          @if (registerForm.get('username')?.invalid && registerForm.get('username')?.touched) {
            <div class="text-red-400 text-sm mt-1">
              <span>Username is required.</span>
            </div>
          }
        </div>
        <div>
          <label for="fullName" class="block text-sm font-medium text-gray-400">Full Name</label>
          <input id="fullName" type="text" formControlName="fullName" class="mt-1 block w-full px-3 py-2 bg-gray-700 border border-gray-600 rounded-md shadow-sm text-gray-200 placeholder-gray-400 focus:outline-none focus:ring-yellow-500 focus:border-yellow-500 sm:text-sm" placeholder="John Doe">
          @if (registerForm.get('fullName')?.invalid && registerForm.get('fullName')?.touched) {
            <div class="text-red-400 text-sm mt-1">
              <span>Full Name is required.</span>
            </div>
          }
        </div>
        <div>
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
      </div>

      <div class="mb-4">
        <label for="password" class="block text-sm font-medium text-gray-400">Password</label>
        <input id="password" type="password" formControlName="password" class="mt-1 block w-full px-3 py-2 bg-gray-700 border border-gray-600 rounded-md shadow-sm text-gray-200 placeholder-gray-400 focus:outline-none focus:ring-yellow-500 focus:border-yellow-500 sm:text-sm" placeholder="••••••••">
         @if (registerForm.get('password')?.invalid && registerForm.get('password')?.touched) {
          <div class="text-red-400 text-sm mt-1">
            @if (registerForm.get('password')?.errors?.['required']) {
              <span>Password is required.</span>
            }
            @if (registerForm.get('password')?.errors?.['minlength']) {
              <span>Password must be at least 8 characters long.</span>
            }
          </div>
        }
      </div>

      <hr class="border-gray-600 my-6">
      <h3 class="text-lg font-medium text-gray-200 mb-4">Shipping Address</h3>

      <div class="mb-4">
        <label for="addressLine1" class="block text-sm font-medium text-gray-400">Address Line 1</label>
        <input id="addressLine1" type="text" formControlName="addressLine1" class="mt-1 block w-full px-3 py-2 bg-gray-700 border border-gray-600 rounded-md shadow-sm text-gray-200 placeholder-gray-400 focus:outline-none focus:ring-yellow-500 focus:border-yellow-500 sm:text-sm">
        @if (registerForm.get('addressLine1')?.invalid && registerForm.get('addressLine1')?.touched) {
          <div class="text-red-400 text-sm mt-1"><span>Address is required.</span></div>
        }
      </div>

      <div class="mb-4">
        <label for="addressLine2" class="block text-sm font-medium text-gray-400">Address Line 2 (Optional)</label>
        <input id="addressLine2" type="text" formControlName="addressLine2" class="mt-1 block w-full px-3 py-2 bg-gray-700 border border-gray-600 rounded-md shadow-sm text-gray-200 placeholder-gray-400 focus:outline-none focus:ring-yellow-500 focus:border-yellow-500 sm:text-sm">
      </div>

      <div class="grid grid-cols-1 md:grid-cols-2 gap-4 mb-4">
        <div>
          <label for="city" class="block text-sm font-medium text-gray-400">City</label>
          <input id="city" type="text" formControlName="city" class="mt-1 block w-full px-3 py-2 bg-gray-700 border border-gray-600 rounded-md shadow-sm text-gray-200 placeholder-gray-400 focus:outline-none focus:ring-yellow-500 focus:border-yellow-500 sm:text-sm">
          @if (registerForm.get('city')?.invalid && registerForm.get('city')?.touched) {
            <div class="text-red-400 text-sm mt-1"><span>City is required.</span></div>
          }
        </div>
        <div>
          <label for="state" class="block text-sm font-medium text-gray-400">State / Province</label>
          <input id="state" type="text" formControlName="state" class="mt-1 block w-full px-3 py-2 bg-gray-700 border border-gray-600 rounded-md shadow-sm text-gray-200 placeholder-gray-400 focus:outline-none focus:ring-yellow-500 focus:border-yellow-500 sm:text-sm">
          @if (registerForm.get('state')?.invalid && registerForm.get('state')?.touched) {
            <div class="text-red-400 text-sm mt-1"><span>State is required.</span></div>
          }
        </div>
      </div>

      <div class="grid grid-cols-1 md:grid-cols-2 gap-4 mb-6">
        <div>
          <label for="postalCode" class="block text-sm font-medium text-gray-400">Postal Code</label>
          <input id="postalCode" type="text" formControlName="postalCode" class="mt-1 block w-full px-3 py-2 bg-gray-700 border border-gray-600 rounded-md shadow-sm text-gray-200 placeholder-gray-400 focus:outline-none focus:ring-yellow-500 focus:border-yellow-500 sm:text-sm">
          @if (registerForm.get('postalCode')?.invalid && registerForm.get('postalCode')?.touched) {
            <div class="text-red-400 text-sm mt-1"><span>Postal Code is required.</span></div>
          }
        </div>
        <div>
          <label for="country" class="block text-sm font-medium text-gray-400">Country</label>
          <input id="country" type="text" formControlName="country" class="mt-1 block w-full px-3 py-2 bg-gray-700 border border-gray-600 rounded-md shadow-sm text-gray-200 placeholder-gray-400 focus:outline-none focus:ring-yellow-500 focus:border-yellow-500 sm:text-sm">
        </div>
      </div>

      @if (errorMessages().length > 0) {
        <div class="mb-4 p-3 bg-red-900/50 text-red-300 rounded-md">
          @for(error of errorMessages(); track error) {
            <p>{{error}}</p>
          }
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
  private toastService = inject(ToastService);
  private router = inject(Router);

  isLoading = signal(false);
  errorMessages = signal<string[]>([]);

  registerForm = this.fb.group({
    username: ['', Validators.required],
    fullName: ['', Validators.required],
    email: ['', [Validators.required, Validators.email]],
    password: ['', [Validators.required, Validators.minLength(8)]],
    addressLine1: ['', Validators.required],
    addressLine2: [''],
    city: ['', Validators.required],
    state: ['', Validators.required],
    postalCode: ['', Validators.required],
    country: ['USA']
  });

  onSubmit() {
    if (this.registerForm.invalid) {
      return;
    }
    this.isLoading.set(true);
    this.errorMessages.set([]);
    const { 
      username,
      fullName,
      email, 
      password,
      addressLine1,
      addressLine2,
      city,
      state,
      postalCode,
      country
    } = this.registerForm.value;

    this.authService.register(
      username!, fullName!, email!, password!, 
      addressLine1!, addressLine2, city!, state!, postalCode!, country
    )
      .pipe(
        finalize(() => this.isLoading.set(false))
      )
      .subscribe({
        next: () => {
          this.toastService.show('Registration successful! Please log in.', 'success');
          this.router.navigate(['/login']);
        },
        error: (err: HttpErrorResponse) => {
          if (err.status === 400 && err.error?.errors) {
            // Handle validation errors from the backend
            this.errorMessages.set(err.error.errors);
          } else if (err.status === 400 && typeof err.error === 'object') {
            // Handle FluentValidation default error format
            const validationErrors = Object.values(err.error.errors).flatMap(e => e as string[]);
            this.errorMessages.set(validationErrors);
          }
          else {
            this.errorMessages.set(['An unexpected error occurred. Please try again.']);
          }
        }
      });
  }
}
