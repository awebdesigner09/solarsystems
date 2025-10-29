import { Component, ChangeDetectionStrategy, inject, signal, OnInit, computed } from '@angular/core';
import { CommonModule, DatePipe, CurrencyPipe } from '@angular/common';
import { RouterLink } from '@angular/router';
import { combineLatest } from 'rxjs';
import { finalize } from 'rxjs/operators';

import { DataService } from '../../services/data.service';
import { QuoteRequest } from '../../models/quote-request.model';
import { Order } from '../../models/order.model';
import { SolarSystemModel } from '../../models/solar-system-model.model';

interface QuoteViewModel extends QuoteRequest {
  modelName?: string;
}

interface OrderViewModel extends Order {
    modelName?: string;
    customerId: string;
}

@Component({
  selector: 'app-admin-dashboard',
  imports: [CommonModule, RouterLink, DatePipe],
  template: `
    <div class="container mx-auto px-4 py-8">
      <div class="flex justify-between items-center mb-6">
        <h1 class="text-3xl font-bold text-gray-100">Admin Dashboard</h1>
        <a routerLink="/admin/model/new" class="inline-flex items-center px-4 py-2 border border-transparent text-sm font-bold rounded-md shadow-sm text-gray-900 bg-yellow-500 hover:bg-yellow-600">
          <svg class="w-5 h-5 mr-2" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 20 20" fill="currentColor">
            <path fill-rule="evenodd" d="M10 3a1 1 0 011 1v5h5a1 1 0 110 2h-5v5a1 1 0 11-2 0v-5H4a1 1 0 110-2h5V4a1 1 0 011-1z" clip-rule="evenodd" />
          </svg>
          Add New Solar Model
        </a>
      </div>

      @if (isLoading()) {
        <div class="flex justify-center items-center p-8">
          <div class="animate-spin rounded-full h-16 w-16 border-b-4 border-yellow-500"></div>
        </div>
      }

      <div class="grid grid-cols-1 lg:grid-cols-2 gap-8">
        <!-- Quotes Section -->
        <div class="bg-gray-800 rounded-lg shadow-md p-6">
          <h2 class="text-2xl font-semibold mb-4 text-gray-200">Recent Quotes</h2>
          @if (quoteViewModels().length > 0) {
            <div class="overflow-x-auto">
              <table class="min-w-full divide-y divide-gray-700">
                <thead class="bg-gray-700/50">
                  <tr>
                    <th scope="col" class="px-4 py-3 text-left text-xs font-medium text-gray-300 uppercase tracking-wider">Model</th>
                    <th scope="col" class="px-4 py-3 text-left text-xs font-medium text-gray-300 uppercase tracking-wider">Customer ID</th>
                    <th scope="col" class="px-4 py-3 text-left text-xs font-medium text-gray-300 uppercase tracking-wider">Status</th>
                    <th scope="col" class="px-4 py-3 text-left text-xs font-medium text-gray-300 uppercase tracking-wider">Date</th>
                  </tr>
                </thead>
                <tbody class="bg-gray-800 divide-y divide-gray-700">
                  @for (quote of quoteViewModels(); track quote.id) {
                    <tr>
                      <td class="px-4 py-4 whitespace-nowrap text-sm font-medium text-gray-100">{{ quote.modelName }}</td>
                      <td class="px-4 py-4 whitespace-nowrap text-sm text-gray-400 font-mono">{{ quote.customerId }}</td>
                      <td class="px-4 py-4 whitespace-nowrap text-sm">
                        <span [class]="'px-2 inline-flex text-xs leading-5 font-semibold rounded-full ' + statusBadgeColor(quote.status)">
                          {{ quote.status }}
                        </span>
                      </td>
                      <td class="px-4 py-4 whitespace-nowrap text-sm text-gray-400">{{ quote.createdAt | date:'short' }}</td>
                    </tr>
                  }
                </tbody>
              </table>
            </div>
          } @else {
            <p class="text-gray-400">No quotes found.</p>
          }
        </div>

        <!-- Orders Section -->
        <div class="bg-gray-800 rounded-lg shadow-md p-6">
          <h2 class="text-2xl font-semibold mb-4 text-gray-200">Active Orders</h2>
          @if (orderViewModels().length > 0) {
            <div class="overflow-x-auto">
              <table class="min-w-full divide-y divide-gray-700">
                <thead class="bg-gray-700/50">
                  <tr>
                    <th scope="col" class="px-4 py-3 text-left text-xs font-medium text-gray-300 uppercase tracking-wider">Order ID</th>
                    <th scope="col" class="px-4 py-3 text-left text-xs font-medium text-gray-300 uppercase tracking-wider">Model</th>
                    <th scope="col" class="px-4 py-3 text-left text-xs font-medium text-gray-300 uppercase tracking-wider">Status</th>
                    <th scope="col" class="px-4 py-3 text-left text-xs font-medium text-gray-300 uppercase tracking-wider">Date</th>
                  </tr>
                </thead>
                <tbody class="bg-gray-800 divide-y divide-gray-700">
                  @for (order of orderViewModels(); track order.id) {
                    <tr>
                      <td class="px-4 py-4 whitespace-nowrap text-sm text-gray-400 font-mono">{{ order.id }}</td>
                      <td class="px-4 py-4 whitespace-nowrap text-sm font-medium text-gray-100">{{ order.modelName }}</td>
                      <td class="px-4 py-4 whitespace-nowrap text-sm">
                         <span [class]="'px-2 inline-flex text-xs leading-5 font-semibold rounded-full ' + statusBadgeColor(order.status)">
                          {{ order.status }}
                        </span>
                      </td>
                      <td class="px-4 py-4 whitespace-nowrap text-sm text-gray-400">{{ order.createdAt | date:'short' }}</td>
                    </tr>
                  }
                </tbody>
              </table>
            </div>
          } @else {
            <p class="text-gray-400">No orders found.</p>
          }
        </div>
      </div>
    </div>
  `,
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class AdminDashboardComponent implements OnInit {
  private dataService = inject(DataService);
  
  isLoading = signal(true);
  
  // Signals from service
  private quotes = this.dataService.quotes;
  private orders = this.dataService.orders;
  private models = this.dataService.models;

  quoteViewModels = computed<QuoteViewModel[]>(() => {
    const allQuotes = this.quotes();
    const allModels = this.models();
    return allQuotes
        .map(quote => ({
            ...quote,
            modelName: allModels.find(m => m.id === quote.solarSystemModelId)?.name || 'Unknown',
        }))
        .sort((a, b) => b.createdAt.getTime() - a.createdAt.getTime());
  });
  
  orderViewModels = computed<OrderViewModel[]>(() => {
      const allOrders = this.orders();
      const allQuotes = this.quotes();
      const allModels = this.models();
      return allOrders
        .map(order => {
            const quote = allQuotes.find(q => q.id === order.quoteRequestId);
            const model = quote ? allModels.find(m => m.id === quote.solarSystemModelId) : undefined;
            return {
                ...order,
                modelName: model?.name || 'Unknown',
                customerId: quote?.customerId || 'N/A'
            };
        })
        .sort((a, b) => b.createdAt.getTime() - a.createdAt.getTime());
  });

  ngOnInit(): void {
    this.isLoading.set(true);
    combineLatest([
        this.dataService.getAllQuotes(),
        this.dataService.getAllOrders(),
        this.dataService.getSolarSystemModels()
    ]).pipe(
        finalize(() => this.isLoading.set(false))
    ).subscribe();
  }

  statusBadgeColor(status: string): string {
    switch (status) {
      // Quote statuses
      case 'Pending': return 'bg-yellow-900/70 text-yellow-300';
      case 'Processing': return 'bg-blue-900/70 text-blue-300';
      case 'Ready': return 'bg-green-900/70 text-green-300';
      case 'Expired': return 'bg-gray-700 text-gray-300';
      // Order statuses
      case 'Confirmed': return 'bg-green-800/70 text-green-200';
      case 'Cancelled': return 'bg-red-900/70 text-red-300';
      default: return 'bg-gray-700 text-gray-300';
    }
  }
}
