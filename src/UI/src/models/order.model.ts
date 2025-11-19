
export type OrderStatus = 'Processing' | 'Confirmed' | 'Cancelled';

export interface Order {
  id: string;
  quoteId: string;
  status: OrderStatus;
  createdAt: Date;
}

export interface OrderSummary {
  id: string;
  quoteId: string;
  systemModelName: string;
  city: string;
  state: string;
  totalPrice: number;
  orderDate: string;
  orderStatus: OrderStatus;
  statusDate: string;
}
