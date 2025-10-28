import { Injectable, signal } from '@angular/core';
import { Observable, of, throwError } from 'rxjs';
import { delay, tap } from 'rxjs/operators';
import { SolarSystemModel } from '../models/solar-system-model.model';
// Fix: Import LocationDetails from customer.model.ts as it is not exported from quote-request.model.ts
import { QuoteRequest, QuoteStatus, CustomConfig } from '../models/quote-request.model';
import { LocationDetails } from '../models/customer.model';
import { Order, OrderStatus } from '../models/order.model';

const MOCK_MODELS: SolarSystemModel[] = [
    { id: 'model-1', name: 'Starter SunKit', panelType: 'Monocrystalline', capacityKW: 5, basePrice: 12000, description: 'Ideal for small homes, providing essential power with high-efficiency panels.', imageUrl: 'https://picsum.photos/seed/model-1/600/400' },
    { id: 'model-2', name: 'EcoPower Plus', panelType: 'Polycrystalline', capacityKW: 8, basePrice: 18000, description: 'A balance of performance and value, suitable for medium-sized residences.', imageUrl: 'https://picsum.photos/seed/model-2/600/400' },
    { id: 'model-3', name: 'GridMaster Pro', panelType: 'Monocrystalline', capacityKW: 12, basePrice: 25000, description: 'Maximum power output for large homes or small businesses, with premium panels.', imageUrl: 'https://picsum.photos/seed/model-3/600/400' },
    { id: 'model-4', name: 'FlexiThin Solar', panelType: 'Thin-Film', capacityKW: 6, basePrice: 15000, description: 'Lightweight and flexible, perfect for non-traditional roofs or mobile applications.', imageUrl: 'https://picsum.photos/seed/model-4/600/400' },
];

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

  private readonly _models = signal<SolarSystemModel[]>([]);
  private readonly _quotes = signal<QuoteRequest[]>([]);
  private readonly _orders = signal<Order[]>([]);

  public readonly models = this._models.asReadonly();
  public readonly quotes = this._quotes.asReadonly();
  public readonly orders = this._orders.asReadonly();

  constructor() {
    this._models.set(MOCK_MODELS);
    this._quotes.set(MOCK_QUOTES);
    this._orders.set(MOCK_ORDERS);
  }
  
  // Models
  getSolarSystemModels(): Observable<SolarSystemModel[]> {
    return of(this.models()).pipe(delay(300), tap(models => this._models.set(models)));
  }

  getSolarSystemModelById(id: string): Observable<SolarSystemModel | undefined> {
    return of(this.models().find(m => m.id === id)).pipe(delay(200));
  }

  createSolarSystemModel(modelData: Omit<SolarSystemModel, 'id'>): Observable<SolarSystemModel> {
    const newModel: SolarSystemModel = {
        ...modelData,
        id: `model-${Date.now()}`,
    };
    this._models.update(models => [...models, newModel]);
    return of(newModel).pipe(delay(500));
  }

  updateSolarSystemModel(updatedModel: SolarSystemModel): Observable<SolarSystemModel> {
    this._models.update(models => models.map(m => m.id === updatedModel.id ? updatedModel : m));
    return of(updatedModel).pipe(delay(500));
  }

  deleteSolarSystemModel(id: string): Observable<boolean> {
    this._models.update(models => models.filter(m => m.id !== id));
    return of(true).pipe(delay(400));
  }

  // Quotes
  getQuotesByCustomerId(customerId: string): Observable<QuoteRequest[]> {
    return of(this.quotes().filter(q => q.customerId === customerId)).pipe(delay(400), tap(quotes => this._quotes.set(quotes)));
  }

  getAllQuotes(): Observable<QuoteRequest[]> {
    return of(this.quotes()).pipe(delay(500), tap(quotes => this._quotes.set(quotes)));
  }

  getQuoteById(id: string): Observable<QuoteRequest | undefined> {
    return of(this.quotes().find(q => q.id === id)).pipe(delay(200));
  }

  hasActiveQuote(customerId: string, modelId: string): Observable<boolean> {
     const hasQuote = this.quotes().some(q => 
        q.customerId === customerId && 
        q.solarSystemModelId === modelId && 
        (q.status === 'Pending' || q.status === 'Processing' || q.status === 'Ready')
    );
    return of(hasQuote).pipe(delay(100));
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
    return of(newQuote).pipe(delay(800));
  }

  updateQuoteStatus(quoteId: string, status: QuoteStatus) {
    this._quotes.update(quotes => quotes.map(q => 
        q.id === quoteId ? { ...q, status } : q
    ));
  }
  
  // Orders
  getAllOrders(): Observable<Order[]> {
    return of(this.orders()).pipe(delay(600), tap(orders => this._orders.set(orders)));
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
    return of(newOrder).pipe(delay(1000));
  }
  
  updateOrderStatus(orderId: string, status: OrderStatus) {
    this._orders.update(orders => orders.map(o => 
        o.id === orderId ? { ...o, status } : o
    ));
  }
}