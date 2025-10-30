import { Injectable, signal, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, of, throwError, tap, map } from 'rxjs';
import { SolarSystemModel } from '../models/solar-system-model.model';
// Fix: Import LocationDetails from customer.model.ts as it is not exported from quote-request.model.ts
import { QuoteRequest, QuoteStatus, CustomConfig } from '../models/quote-request.model';
import { LocationDetails } from '../models/customer.model';
import { Order, OrderStatus } from '../models/order.model';
import { environment } from '../environments/environment';

interface PaginatedResult<T> {
  pageIndex: number;
  pageSize: number;
  count: number;
  data: T[];
}

interface GetSystemModelsResponse {
  systemModels: PaginatedResult<SolarSystemModel>;
}

const MOCK_QUOTES: QuoteRequest[] = [
    {
        id: 'quote-1', customerId: 'cust-123', solarSystemModelId: 'model-1', status: 'Ready', createdAt: new Date(Date.now() - 2 * 24 * 60 * 60 * 1000),
        locationDetails: { address: '123 Sun St', city: 'Solaris', state: 'CA', zipCode: '90210' },
        customConfig: { batteryStorage: true, evCharger: false }
    },
    {
        id: 'quote-2', customerId: 'cust-123', solarSystemModelId: 'model-3', status: 'Processing', createdAt: new Date(Date.now() - 1 * 24 * 60 * 60 * 1000),
        locationDetails: { address: '456 Bright Ave', city: 'Luminara', state: 'NV', zipCode: '89101' },
        customConfig: { batteryStorage: true, evCharger: true, notes: 'Interested in financing options.' }
    },
];

const MOCK_ORDERS: Order[] = [
    { id: 'order-1', quoteRequestId: 'quote-old-1', status: 'Confirmed', createdAt: new Date(Date.now() - 10 * 24 * 60 * 60 * 1000) }
];


@Injectable({
  providedIn: 'root'
})
export class DataService {
  private http = inject(HttpClient);
  private apiUrl = `${environment.apiUrl}/system-models`;

  private readonly _models = signal<SolarSystemModel[]>([]);
  private readonly _quotes = signal<QuoteRequest[]>([]);
  private readonly _orders = signal<Order[]>([]);

  public readonly models = this._models.asReadonly();
  public readonly quotes = this._quotes.asReadonly();
  public readonly orders = this._orders.asReadonly();

  constructor() {
    this._quotes.set(MOCK_QUOTES);
    this._orders.set(MOCK_ORDERS);
    this.getSolarSystemModels().subscribe(); // Fetch models on service initialization
  }
  
  // Models
  getSolarSystemModels(): Observable<SolarSystemModel[]> {
    return this.http.get<GetSystemModelsResponse>(this.apiUrl).pipe(
      // The API returns a paginated result, so we need to extract the data array.
      map(response => response.systemModels.data),
      tap(models => this._models.set(models))
    );
  }

  getSolarSystemModelById(id: string): Observable<SolarSystemModel | undefined> {
    // First check if we have it in the signal
    const existing = this.models().find(m => m.id === id);
    if (existing) {
      return of(existing);
    }
    // Otherwise, fetch from the API
    return this.http.get<SolarSystemModel>(`${this.apiUrl}/${id}`);
  }

  createSolarSystemModel(modelData: Omit<SolarSystemModel, 'id'>): Observable<SolarSystemModel> {
    return this.http.post<SolarSystemModel>(`${this.apiUrl}`, modelData).pipe(
      tap(newModel => {
        this._models.update(models => [...models, newModel]);
      })
    );
  }

  updateSolarSystemModel(updatedModel: SolarSystemModel): Observable<SolarSystemModel> {
    return this.http.put<SolarSystemModel>(`${this.apiUrl}/${updatedModel.id}`, updatedModel).pipe(
      tap(result => {
        this._models.update(models => models.map(m => m.id === updatedModel.id ? result : m));
      })
    );
  }

  deleteSolarSystemModel(id: string): Observable<boolean> {
    return this.http.delete<{ isSuccess: boolean }>(`${this.apiUrl}/${id}`).pipe(
      tap(response => {
        if (response.isSuccess) {
          this._models.update(models => models.filter(m => m.id !== id));
        }
      }),
      map(response => response.isSuccess)
    );
  }

  // Quotes
  getQuotesByCustomerId(customerId: string): Observable<QuoteRequest[]> {
    return of(this.quotes().filter(q => q.customerId === customerId)).pipe(tap(quotes => this._quotes.set(quotes)));
  }

  getAllQuotes(): Observable<QuoteRequest[]> {
    return of(this.quotes()).pipe(tap(quotes => this._quotes.set(quotes)));
  }

  getQuoteById(id: string): Observable<QuoteRequest | undefined> {
    return of(this.quotes().find(q => q.id === id));
  }

  hasActiveQuote(customerId: string, modelId: string): Observable<boolean> {
     const hasQuote = this.quotes().some(q => 
        q.customerId === customerId && 
        q.solarSystemModelId === modelId && 
        (q.status === 'Pending' || q.status === 'Processing' || q.status === 'Ready')
    );
    return of(hasQuote);
  }

  createQuote(customerId: string, modelId: string, location: LocationDetails, config: CustomConfig): Observable<QuoteRequest> {
    const newQuote: QuoteRequest = {
        id: `quote-${Date.now()}`,
        customerId,
        solarSystemModelId: modelId,
        status: 'Pending',
        createdAt: new Date(),
        locationDetails: location,
        customConfig: config,
    };
    this._quotes.update(quotes => [...quotes, newQuote]);
    return of(newQuote);
  }

  updateQuoteStatus(quoteId: string, status: QuoteStatus) {
    this._quotes.update(quotes => quotes.map(q => 
        q.id === quoteId ? { ...q, status } : q
    ));
  }
  
  // Orders
  getAllOrders(): Observable<Order[]> {
    return of(this.orders()).pipe(tap(orders => this._orders.set(orders)));
  }

  createOrder(quoteRequestId: string): Observable<Order> {
    const quote = this.quotes().find(q => q.id === quoteRequestId);
    if (!quote || quote.status !== 'Ready') {
      return throwError(() => new Error('Quote is not ready for ordering.'));
    }
    
    this.updateQuoteStatus(quoteRequestId, 'Expired'); // Mark quote as used

    const newOrder: Order = {
        id: `order-${Date.now()}`,
        quoteRequestId,
        status: 'Processing',
        createdAt: new Date(),
    };
    this._orders.update(orders => [...orders, newOrder]);
    return of(newOrder);
  }
  
  updateOrderStatus(orderId: string, status: OrderStatus) {
    this._orders.update(orders => orders.map(o => 
        o.id === orderId ? { ...o, status } : o
    ));
  }
}