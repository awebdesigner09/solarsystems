export interface Quote {
    id: string;
    quoteRequestId: string;
    systemModelId: string;
    totalPrice: number;
    customConfiguration: string;
    expirationDate: Date;
    createdAt: Date;
}