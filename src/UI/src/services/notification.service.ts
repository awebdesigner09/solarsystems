import { Injectable, inject, OnDestroy, effect } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { DataService } from './data.service';
import { ToastService } from './toast.service';
import { AuthService } from './auth.service';
import { environment } from '../environments/environment';

// Define interfaces for type safety based on backend DTOs
interface QuoteRequestStatusChangedPayload {
  quoteRequestId: string;
  customerId: string;
  newStatus: string;
  oldStatus: string;
}

interface RealtimeNotification<T> {
  EventType: string;
  Payload: T;
}

@Injectable({
  providedIn: 'root'
})
export class NotificationService implements OnDestroy {
  private dataService = inject(DataService);
  private toastService = inject(ToastService);
  private authService = inject(AuthService);

  private hubConnection?: signalR.HubConnection;

  constructor() {
    // --- FOR TESTING: Connect as a specific customer to test SignalR notifications --- (REMOVED)
    // // This will be replaced by the effect below once login is fully implemented.
    // // The customerId should be the GUID only, not the full Redis channel name.
    // const testCustomerId = "5cd5bd3c-2b96-4e6d-8d5b-53dd00cd6d56";
    // this.startConnection(testCustomerId);

    // --- PRODUCTION CODE: React to user login/logout ---
    effect(() => {
      const user = this.authService.currentUser();
      if (user && user.role === 'customer') {
        this.startConnection(user.id);
      } else {
        this.stopConnection();
      }
    });
  }

  private startConnection(customerId: string): void {
    if (this.hubConnection) {
      return; // Connection already exists
    }

    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl(`${environment.apiUrl}/notificationsHub`)
      .withAutomaticReconnect()
      .build();

    this.hubConnection.start()
      .then(() => {
        console.log('SignalR connection started.');
        this.hubConnection?.invoke('JoinCustomerGroup', customerId)
          .catch(err => console.error('Error joining customer group: ', err));
        this.addEventListeners();
      })
      .catch(err => console.error('Error while starting SignalR connection: ', err));
  }

  private addEventListeners(): void {
    this.hubConnection?.on('QuoteRequestEvent', (message: string) => {
      const notification: RealtimeNotification<QuoteRequestStatusChangedPayload> = JSON.parse(message);

      if (notification.EventType === 'QuoteRequestStatusChanged') {
        const payload = notification.Payload;
        
        // First, update the local state so our data is fresh
        this.dataService.updateQuoteStatus(payload.quoteRequestId, payload.newStatus as any);

        // Now, create a more descriptive message for the toast notification
        const quotes = this.dataService.quotes();
        const models = this.dataService.models();

        const updatedQuote = quotes.find(q => q.id === payload.quoteRequestId);
        const model = updatedQuote ? models.find(m => m.id === updatedQuote.systemModelId) : undefined;

        const modelName = model?.name || 'your system';
        const toastMessage = `Your quote for '${modelName}' is now ${payload.newStatus}.`;
        this.toastService.show(toastMessage, 'info', 5000, '/quotes');
      }
    });
  }

  ngOnDestroy() {
    this.stopConnection();
  }

  private stopConnection(): void {
    this.hubConnection?.stop().then(() => console.log('SignalR connection stopped.'));
    this.hubConnection = undefined;
  }
}
