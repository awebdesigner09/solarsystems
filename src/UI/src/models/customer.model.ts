
export interface LocationDetails {
  address: string;
  city: string;
  state: string;
  zipCode: string;
}

export interface Customer {
  id: string;
  name: string;
  email: string;
  locationDetails: LocationDetails;
}
