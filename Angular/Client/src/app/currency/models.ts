export interface Currency {
  id: number;
  name: string;
  code: string;
  rate: number;
  dateStamp: Date;
}

export interface CalculatedRates {
  fromCurrency: string;
  toCurrency: string;
  amount: number;
  rate: number;
}
