
import { LocationDetails } from './customer.model';

export type QuoteStatus = 'Pending' | 'Processing' | 'Ready' | 'Expired';

export interface CustomConfig {
  batteryStorage: boolean;
  evCharger: boolean;
  notes?: string;
}

export interface QuoteRequest {
  id: string;
  customerId: string;
  solarSystemModelId: string;
  status: QuoteStatus;
  createdAt: Date;
  customConfig: CustomConfig;
  locationDetails: LocationDetails;
}
