export interface Currency {
  id: number;
  name: string;
  code: string;
  rate: number;
  dateStamp: Date;
}

export interface CalculatedRates {
  baseCurrency: CurrencyAmount;
  targetCurrency: CurrencyAmount;
}

export interface CurrencyAmount {
  currency: string;
  amount: number;
}
