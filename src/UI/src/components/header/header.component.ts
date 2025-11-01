// Fix: Replaced invalid file content with a proper Angular component.
import { Component, ChangeDetectionStrategy, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink, RouterLinkActive } from '@angular/router';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-header',
  template: `
    <header class="bg-gray-800 shadow-md">
      <nav class="container mx-auto px-4 py-3 flex justify-between items-center">
        <a routerLink="/" class="flex items-center space-x-2">
          <svg xmlns="http://www.w3.org/2000/svg" width="42" height="42" fill="currentColor" class="bi bi-brightness-low-fill text-yellow-500" viewBox="0 0 16 16">
            <path d="M12 8a4 4 0 1 1-8 0 4 4 0 0 1 8 0M8.5 2.5a.5.5 0 1 1-1 0 .5.5 0 0 1 1 0m0 11a.5.5 0 1 1-1 0 .5.5 0 0 1 1 0m5-5a.5.5 0 1 1 0-1 .5.5 0 0 1 0 1m-11 0a.5.5 0 1 1 0-1 .5.5 0 0 1 0 1m9.743-4.036a.5.5 0 1 1-.707-.707.5.5 0 0 1 .707.707m-7.779 7.779a.5.5 0 1 1-.707-.707.5.5 0 0 1 .707.707m7.072 0a.5.5 0 1 1 .707-.707.5.5 0 0 1-.707.707M3.757 4.464a.5.5 0 1 1 .707-.707.5.5 0 0 1-.707.707"/>
          </svg>
          
          <span class="text-xl font-bold text-gray-100">Solar Systems</span>
        </a>

        <div class="flex items-center space-x-4">
          @if (authService.isLoggedIn()) {
            <div class="flex items-center space-x-4">
              <!-- Customer links -->
              @if (!authService.isAdmin()) {
                <a routerLink="/catalog" routerLinkActive="text-yellow-400" [routerLinkActiveOptions]="{exact: true}" class="text-gray-300 hover:text-yellow-400">Catalog</a>
                <a routerLink="/quotes" routerLinkActive="text-yellow-400" class="text-gray-300 hover:text-yellow-400">My Quotes</a>
              }
              <!-- Admin links -->
              @if (authService.isAdmin()) {
                <a routerLink="/catalog" routerLinkActive="text-yellow-400" class="text-gray-300 hover:text-yellow-400">Catalog</a>
                <a routerLink="/admin" routerLinkActive="text-yellow-400" class="text-gray-300 hover:text-yellow-400">Dashboard</a>
              }
              
              <span class="text-gray-400">|</span>

              <span class="text-gray-300">Welcome, {{ authService.currentUser()?.name }}</span>
              <button (click)="logout()" class="px-3 py-1.5 text-sm font-semibold rounded-md bg-yellow-500 text-gray-900 hover:bg-yellow-600">
                Logout
              </button>
            </div>
          } @else {
            <div class="flex items-center space-x-2">
                <a routerLink="/login" class="px-4 py-2 text-sm font-medium rounded-md text-gray-300 hover:bg-gray-700">Login</a>
                <a routerLink="/register" class="px-4 py-2 text-sm font-medium rounded-md bg-yellow-500 text-gray-900 hover:bg-yellow-600">Register</a>
            </div>
          }
        </div>
      </nav>
    </header>
  `,
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [CommonModule, RouterLink, RouterLinkActive],
})
export class HeaderComponent {
  authService = inject(AuthService);

  logout() {
    this.authService.logout();
  }
}
