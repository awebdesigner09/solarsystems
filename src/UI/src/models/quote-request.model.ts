
export type QuoteStatus = 'Pending' | 'Processing' | 'Ready' | 'Expired';

export interface QuoteRequest {
  id: string;
  customerId: string;
  systemModelId: string; // This is correct for customer-facing quotes
  status: QuoteStatus;
  createdAt?: Date; // Make optional as it's not in the admin list
  customConfig: string | null;
  // locationDetails is not part of the quote request from the API
}

export interface AdminQuoteRequest {
  id: string;
  customerId: string;
  systemModelId: string; // Mismatch: 'systemModelId' from admin endpoint
  status: QuoteStatus; // Mismatch: number from admin endpoint
  customConfig: string | null; // Mismatch: string from admin endpoint
}
