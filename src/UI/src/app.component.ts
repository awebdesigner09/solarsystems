
import { Component, ChangeDetectionStrategy, inject } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { HeaderComponent } from './components/header/header.component';
import { NotificationService } from './services/notification.service';
import { ToastComponent } from './components/toast/toast.component';

@Component({
  selector: 'app-root',
  
  template: `
    <app-header></app-header>
    <main class="bg-gray-900 text-gray-200 min-h-screen">
      <router-outlet></router-outlet>
    </main>
    <app-toast></app-toast>
  `,
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [RouterOutlet, HeaderComponent, ToastComponent],
})
export class AppComponent {
  // Inject NotificationService to initialize the real-time simulation
  private notificationService = inject(NotificationService);
}