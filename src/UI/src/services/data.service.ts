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
    // It's better to initiate data fetching from the component that needs it.
  }
  
  // Models
  getSolarSystemModels(): Observable<SolarSystemModel[]> {
    const params = { PageIndex: '0', pageSize: '10' };
    // The API nests the paginated result under a `systemModels` property.
    return this.http.get<{ systemModels: PaginatedResult<SolarSystemModel> }>(this.apiUrl, { params }).pipe(
      map(response => response?.systemModels?.data || []),
      tap(models => this._models.set(models || []))
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
    const payload = { SystemModel: modelData };
    return this.http.post<SolarSystemModel>(`${this.apiUrl}`, payload).pipe(
      tap(newModel => {
        this._models.update(models => [...models, newModel]);
      })
    );
  }

  updateSolarSystemModel(updatedModel: SolarSystemModel): Observable<SolarSystemModel> {
    const payload = { SystemModel: updatedModel };
    return this.http.put<SolarSystemModel>(`${this.apiUrl}`, payload).pipe(
      tap(result => {
        this._models.update(models => models.map(m => m.id === updatedModel.id ? result : m));
      })
    );
  }

  deleteSolarSystemModel(id: string): Observable<boolean> {
    return this.http.delete<any>(`${this.apiUrl}/${id}`, { observe: 'response' }).pipe(
      map(response => {
        const isSuccess = response.status >= 200 && response.status < 300;
        if (isSuccess) {
            this._models.update(models => models.filter(m => m.id !== id));
        }
        return isSuccess;
      }),
    );
  }

  // Quotes
  getQuotesByCustomerId(customerId: string): Observable<QuoteRequest[]> {
    // This should be a real API call in a real app
    return this.http.get<QuoteRequest[]>(`${environment.apiUrl}/quote-requests`).pipe(tap(quotes => this._quotes.set(quotes)));
  }

  getAllQuotes(): Observable<QuoteRequest[]> {
    return this.http.get<PaginatedResult<QuoteRequest>>(`${environment.apiUrl}/quote-requests`).pipe(
      map(response => response?.data || []),
      tap(quotes => this._quotes.set(quotes || []))
    );
  }

  getQuoteById(id: string): Observable<QuoteRequest | undefined> {
    // This should be a real API call in a real app
    return this.http.get<QuoteRequest>(`${environment.apiUrl}/quote-requests/${id}`);
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
    const payload = { solarSystemModelId: modelId, locationDetails: location, customConfig: config };
    return this.http.post<QuoteRequest>(`${environment.apiUrl}/quote-requests`, payload).pipe(
      tap(newQuote => this._quotes.update(quotes => [...quotes, newQuote]))
    );
  }

  updateQuoteStatus(quoteId: string, status: QuoteStatus) {
    // This would be a PUT/PATCH request in a real app
    this.http.put(`${environment.apiUrl}/quote-requests/${quoteId}/status`, { status }).subscribe(() => {
        this._quotes.update(quotes => quotes.map(q => q.id === quoteId ? { ...q, status } : q));
    });
  }
  
  // Orders
  getAllOrders(): Observable<Order[]> {
    return this.http.get<Order[]>(`${environment.apiUrl}/orders`).pipe(tap(orders => this._orders.set(orders)));
  }
  
  createOrder(quoteRequestId: string): Observable<Order> {
    return this.http.post<Order>(`${environment.apiUrl}/orders`, { quoteRequestId }).pipe(
      tap(newOrder => {
        this._orders.update(orders => [...orders, newOrder]);
      })
    );
  }

  updateOrderStatus(orderId: string, status: OrderStatus) {
    this.http.put(`${environment.apiUrl}/orders/${orderId}/status`, { status }).subscribe(() => {
      this._orders.update(orders => orders.map(o => o.id === orderId ? { ...o, status } : o));
    });
  }
}