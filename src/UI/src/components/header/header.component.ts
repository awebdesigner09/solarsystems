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
          <svg class="w-8 h-8 text-yellow-500" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke-width="1.5" stroke="currentColor">
            <path stroke-linecap="round" stroke-linejoin="round" d="M12 3v2.25m6.364.386l-1.591 1.591M21 12h-2.25m-.386 6.364l-1.591-1.591M12 18.75V21m-6.364-.386l1.591-1.591M3 12h2.25m.386-6.364l1.591 1.591" />
          </svg>
          <span class="text-xl font-bold text-gray-100">SolarQuote</span>
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
