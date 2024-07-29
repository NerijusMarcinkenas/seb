import { HttpClient, HttpParams } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { CalculatedRates, Currency } from '../currency/models';

@Injectable({
  providedIn: 'root',
})
export class CurrencyApiService {
  client = inject(HttpClient);

  getLatestRates() {
    return this.client.get<Currency[]>(`${environment.apiUrl}/currency/latest`);
  }

  calculateExchangeRate(
    baseCurrency: string,
    targetCurrency: string,
    baseAmount: number,
  ) {
    const params = new HttpParams();
    params.append('baseCurrency', baseCurrency);
    params.append('targetCurrency', targetCurrency);
    params.append('amount', baseAmount);

    return this.client.get<number>(
      `${environment.apiUrl}/currency/exchange-rate?baseCurrency=${baseCurrency}&targetCurrency=${targetCurrency}&amount=${baseAmount}`,
    );
  }
}
