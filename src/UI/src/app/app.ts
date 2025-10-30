import { Component, signal, inject } from '@angular/core';
import { AuthService } from '../services/auth.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.html',
  standalone: false,
  styleUrl: './app.css',
})
export class App {
  protected readonly title = signal('UI');
  
  // Inject AuthService to get user information
  protected authService = inject(AuthService);
}
