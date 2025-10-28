import { Injectable, inject, OnDestroy } from '@angular/core';
import { DataService } from './data.service';
import { QuoteStatus } from '../models/quote-request.model';
import { OrderStatus } from '../models/order.model';
import { ToastService } from './toast.service';

@Injectable({
  providedIn: 'root'
})
export class NotificationService implements OnDestroy {
  private dataService = inject(DataService);
  private toastService = inject(ToastService);
  private intervalId: any;

  constructor() {
    this.startSimulation();
  }

  private startSimulation() {
    if (typeof window !== 'undefined') {
        this.intervalId = setInterval(() => {
        this.simulateStatusChange();
        }, 5000); // Simulate an update every 5 seconds
    }
  }

  private simulateStatusChange() {
    const quotes = this.dataService.quotes();
    const models = this.dataService.models();

    const pendingQuote = quotes.find(q => q.status === 'Pending');
    if (pendingQuote && Math.random() > 0.5) {
      const modelName = models.find(m => m.id === pendingQuote.solarSystemModelId)?.name || 'a system';
      this.dataService.updateQuoteStatus(pendingQuote.id, 'Processing');
      this.toastService.show(`Quote for ${modelName} is now Processing.`, 'info');
      return;
    }

    const processingQuote = quotes.find(q => q.status === 'Processing');
    if (processingQuote && Math.random() > 0.5) {
      const modelName = models.find(m => m.id === processingQuote.solarSystemModelId)?.name || 'a system';
      this.dataService.updateQuoteStatus(processingQuote.id, 'Ready');
      this.toastService.show(`Your quote for ${modelName} is now Ready!`, 'success');
      return;
    }
    
    const orders = this.dataService.orders();
    const processingOrder = orders.find(o => o.status === 'Processing');
    if (processingOrder && Math.random() > 0.6) {
        this.dataService.updateOrderStatus(processingOrder.id, 'Confirmed');
        this.toastService.show(`Order #${processingOrder.id.slice(-6)} has been Confirmed.`, 'success');
    }
  }

  ngOnDestroy() {
    if (this.intervalId) {
      clearInterval(this.intervalId);
    }
  }
}
