
import { Component, ChangeDetectionStrategy, inject, signal, OnInit } from '@angular/core';
import { CommonModule, CurrencyPipe, DatePipe } from '@angular/common';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { switchMap, finalize, map, filter } from 'rxjs/operators';

import { DataService } from '../../services/data.service';
import { QuoteRequest } from '../../models/quote-request.model';
import { SolarSystemModel } from '../../models/solar-system-model.model';

interface OrderViewModel {
  quote: QuoteRequest;
  model: SolarSystemModel;
  totalPrice: number;
}

@Component({
  selector: 'app-order-placement',
  template: `
    <div class="container mx-auto px-4 py-8 max-w-3xl">
       @if (isLoading()) {
        <div class="flex justify-center items-center p-8">
          <div class="animate-spin rounded-full h-16 w-16 border-b-4 border-yellow-500"></div>
        </div>
      }

      @if (viewModel()) {
        <div class="bg-gray-800 rounded-lg shadow-lg p-8">
          <h1 class="text-3xl font-bold mb-4 text-gray-100">Confirm Your Order</h1>
          <p class="text-gray-400 mb-6">Review your quote details below and confirm to place your order.</p>

          <div class="border border-gray-700 rounded-lg p-6 space-y-4">
             <div class="flex justify-between items-center">
                <h2 class="text-xl font-semibold text-gray-100">{{ viewModel()?.model.name }}</h2>
                <span class="px-3 py-1 text-sm font-semibold rounded-full bg-green-900/70 text-green-300">
                    Quote Ready
                </span>
             </div>
             <p class="text-gray-400 text-sm">Quote created on {{ viewModel()?.quote.createdAt | date:'longDate' }}</p>
            <div class="pt-4 border-t border-gray-700">
              <h3 class="font-semibold text-gray-300">Installation Address:</h3>
              <p class="text-gray-400">{{ viewModel()?.quote.locationDetails.address }}, {{ viewModel()?.quote.locationDetails.city }}</p>
            </div>
             <div class="pt-4 border-t border-gray-700">
              <h3 class="font-semibold text-gray-300">Included Options:</h3>
              <ul class="list-disc list-inside text-gray-400 mt-2 text-sm">
                @if(viewModel()?.quote.customConfig.batteryStorage) { <li>Battery Storage</li> }
                @if(viewModel()?.quote.customConfig.evCharger) { <li>EV Charger</li> }
                @if(!viewModel()?.quote.customConfig.batteryStorage && !viewModel()?.quote.customConfig.evCharger) { <li>Standard Installation</li> }
              </ul>
            </div>
             <div class="pt-4 border-t border-gray-700 text-right">
                <p class="text-gray-400">Total Price</p>
                <p class="text-3xl font-bold text-green-400">{{ viewModel()?.totalPrice | currency:'USD':'symbol':'1.0-0' }}</p>
             </div>
          </div>
          
          <div class="mt-8 flex justify-between items-center">
              <a routerLink="/quotes" class="text-yellow-400 hover:underline">Cancel and go back</a>
              <button (click)="placeOrder()" [disabled]="isSubmitting()" class="inline-flex items-center px-8 py-3 border border-transparent text-base font-bold rounded-md shadow-sm text-gray-900 bg-yellow-500 hover:bg-yellow-600 disabled:bg-gray-600 disabled:cursor-not-allowed">
                 @if (isSubmitting()) {
                  <span class="animate-spin rounded-full h-5 w-5 border-b-2 border-gray-900 mr-2"></span>
                  <span>Placing Order...</span>
                } @else {
                  <span>Confirm and Place Order</span>
                }
              </button>
          </div>
        </div>
      } @else if (!isLoading() && !orderPlaced()) {
         <div class="text-center bg-gray-800 p-8 rounded-lg shadow-md">
            <h2 class="text-xl font-semibold text-gray-200">Quote not found or invalid.</h2>
            <p class="text-gray-400 mt-2">It might have expired or been processed already.</p>
            <a routerLink="/quotes" class="mt-4 inline-block bg-yellow-500 text-gray-900 font-bold py-2 px-4 rounded hover:bg-yellow-600">View My Quotes</a>
        </div>
      }

      @if (orderPlaced()) {
        <div class="bg-gray-800 rounded-lg shadow-lg p-8 text-center">
            <svg class="mx-auto h-16 w-16 text-green-400" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9 12l2 2 4-4m6 2a9 9 0 11-18 0 9 9 0 0118 0z" />
            </svg>
            <h1 class="text-3xl font-bold mt-4 text-gray-100">Order Placed Successfully!</h1>
            <p class="text-gray-400 mt-2">Your order is now being processed. We will contact you shortly with the next steps.</p>
            <p class="text-gray-500 text-sm mt-1">Your new order ID is: <span class="font-mono">{{ newOrderId() }}</span></p>
            <a routerLink="/quotes" class="mt-6 inline-block bg-yellow-500 text-gray-900 font-bold py-3 px-6 rounded-md hover:bg-yellow-600">Back to My Quotes</a>
        </div>
      }
    </div>
  `,
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [CommonModule, RouterLink, DatePipe, CurrencyPipe],
})
export class OrderPlacementComponent implements OnInit {
  private route = inject(ActivatedRoute);
  private dataService = inject(DataService);

  viewModel = signal<OrderViewModel | undefined>(undefined);
  isLoading = signal(true);
  isSubmitting = signal(false);
  orderPlaced = signal(false);
  newOrderId = signal('');

  ngOnInit(): void {
    this.route.paramMap.pipe(
      map(params => params.get('quoteId')),
      filter((quoteId): quoteId is string => !!quoteId),
      switchMap(quoteId => this.dataService.getQuoteById(quoteId)),
      filter((quote): quote is QuoteRequest => !!quote && quote.status === 'Ready'),
      switchMap(quote => {
        return this.dataService.getSolarSystemModelById(quote.solarSystemModelId).pipe(
          map(model => {
            if (!model) return undefined;
            let totalPrice = model.basePrice;
            if (quote.customConfig.batteryStorage) totalPrice += 8000;
            if (quote.customConfig.evCharger) totalPrice += 1500;
            return { quote, model, totalPrice };
          })
        );
      }),
      finalize(() => this.isLoading.set(false))
    ).subscribe(viewModel => {
      this.viewModel.set(viewModel);
    });
  }

  placeOrder() {
    const quote = this.viewModel()?.quote;
    if (!quote) return;

    this.isSubmitting.set(true);
    this.dataService.createOrder(quote.id)
      .pipe(finalize(() => this.isSubmitting.set(false)))
      .subscribe(order => {
        this.newOrderId.set(order.id);
        this.orderPlaced.set(true);
        this.viewModel.set(undefined); // Hide the form
      });
  }
}
