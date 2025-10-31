
import { Component, ChangeDetectionStrategy, inject, signal, OnInit, computed, effect } from '@angular/core';
import { CommonModule, DatePipe, CurrencyPipe } from '@angular/common';
import { RouterLink } from '@angular/router';
import { combineLatest } from 'rxjs';
import { finalize } from 'rxjs/operators';

import { DataService } from '../../services/data.service';
import { AuthService } from '../../services/auth.service';
import { QuoteRequest } from '../../models/quote-request.model';
import { SolarSystemModel } from '../../models/solar-system-model.model';

interface QuoteViewModel extends QuoteRequest {
  model?: SolarSystemModel;
  totalPrice: number;
}

@Component({
  selector: 'app-customer-quotes',
  template: `
    <div class="container mx-auto px-4 py-8">
      <h1 class="text-3xl font-bold mb-6 text-gray-100">My Quotes</h1>

      @if (isLoading()) {
        <div class="flex justify-center items-center p-8">
          <div class="animate-spin rounded-full h-16 w-16 border-b-4 border-yellow-500"></div>
        </div>
      }

      @if (quotesWithModels().length > 0) {
        <div class="space-y-6">
          @for (quote of quotesWithModels(); track quote.id) {
            <div class="bg-gray-800 rounded-lg shadow-md overflow-hidden">
              <div [class]="'p-6 border-l-8 ' + statusBorderColor(quote.status)">
                <div class="flex flex-wrap justify-between items-start">
                  <div>
                    <h2 class="text-2xl font-bold text-gray-100">{{ quote.model?.name }}</h2>
                    <p class="text-sm text-gray-400">Quote requested on: {{ quote.createdAt | date:'longDate' }}</p>
                  </div>
                  <div class="text-right">
                    <span [class]="'px-3 py-1 text-sm font-semibold rounded-full ' + statusBadgeColor(quote.status)">
                      {{ quote.status }}
                    </span>
                    <p class="text-2xl font-light text-green-400 mt-2">{{ quote.totalPrice | currency:'USD':'symbol':'1.0-0' }}</p>
                  </div>
                </div>
                @if(quote.customConfig) {
                <div class="mt-4 pt-4 border-t border-gray-700">
                  <h3 class="font-semibold text-gray-300">Custom Configuration:</h3>
                  <p class="text-gray-400 mt-2 text-sm italic">
                    {{ quote.customConfig }}
                  </p>
                </div>
                }

                @if (quote.status === 'Ready') {
                  <div class="mt-4 text-right">
                    <a [routerLink]="['/order', quote.id]" class="inline-flex items-center px-6 py-2 border border-transparent text-base font-bold rounded-md shadow-sm text-gray-900 bg-yellow-500 hover:bg-yellow-600">
                      Place Order
                      <svg class="ml-2 -mr-1 h-5 w-5" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 20 20" fill="currentColor" aria-hidden="true">
                        <path fill-rule="evenodd" d="M10.293 3.293a1 1 0 011.414 0l6 6a1 1 0 010 1.414l-6 6a1 1 0 01-1.414-1.414L14.586 11H3a1 1 0 110-2h11.586l-4.293-4.293a1 1 0 010-1.414z" clip-rule="evenodd" />
                      </svg>
                    </a>
                  </div>
                }
              </div>
            </div>
          }
        </div>
      } @else if (!isLoading()) {
        <div class="text-center bg-gray-800 p-8 rounded-lg shadow-md">
            <h2 class="text-xl font-semibold text-gray-200">No quotes found.</h2>
            <p class="text-gray-400 mt-2">Ready to go green? Get started by exploring our solar systems.</p>
            <a routerLink="/catalog" class="mt-4 inline-block bg-yellow-500 text-gray-900 font-bold py-2 px-4 rounded hover:bg-yellow-600">Browse Catalog</a>
        </div>
      }
    </div>
  `,
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [CommonModule, RouterLink, DatePipe, CurrencyPipe],
})
export class CustomerQuotesComponent implements OnInit {
  private dataService = inject(DataService);
  private authService = inject(AuthService);

  quotesWithModels = signal<QuoteViewModel[]>([]);
  isLoading = signal(true);
  
  customerQuotes = computed(() => {
      const currentUser = this.authService.currentUser();
      if (!currentUser) return [];
      return this.dataService.quotes().filter(q => q.customerId === currentUser.id);
  });
  
  models = this.dataService.models;

  constructor() {
    effect(() => {
      const quotes = this.customerQuotes();
      const allModels = this.models();
      const viewModels = quotes.map(quote => {
        const model = allModels.find(m => m.id === quote.solarSystemModelId);
        let totalPrice = model?.basePrice || 0;
        // Price calculation based on customConfig is removed as it's now a string.
        // This should be handled by the backend.
        return { ...quote, model, totalPrice };
      });
    });
  }

  ngOnInit(): void {
    this.isLoading.set(true);
    const currentUser = this.authService.currentUser();
    if (currentUser) {
        combineLatest([
            this.dataService.getQuotesByCustomerId(currentUser.id),
            this.dataService.getSolarSystemModels()
        ]).pipe(
            finalize(() => this.isLoading.set(false))
        ).subscribe();
    } else {
        this.isLoading.set(false);
    }
  }

  statusBadgeColor(status: string): string {
    switch (status) {
      case 'Pending': return 'bg-yellow-900/70 text-yellow-300';
      case 'Processing': return 'bg-blue-900/70 text-blue-300';
      case 'Ready': return 'bg-green-900/70 text-green-300';
      case 'Expired': return 'bg-gray-700 text-gray-300';
      default: return 'bg-gray-700 text-gray-300';
    }
  }

  statusBorderColor(status: string): string {
    switch (status) {
      case 'Pending': return 'border-yellow-500';
      case 'Processing': return 'border-blue-500';
      case 'Ready': return 'border-green-500';
      case 'Expired': return 'border-gray-600';
      default: return 'border-gray-600';
    }
  }
}
