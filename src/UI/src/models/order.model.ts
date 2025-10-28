
export type OrderStatus = 'Processing' | 'Confirmed' | 'Cancelled';

export interface Order {
  id: string;
  quoteRequestId: string;
  status: OrderStatus;
  createdAt: Date;
}
