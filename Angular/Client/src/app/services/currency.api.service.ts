import { HttpClient, HttpParams } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { Currency } from '../currency/models';

@Injectable({
  providedIn: 'root',
})
export class CurrencyApiService {
  client = inject(HttpClient);

  getLatestRates() {
    return this.client.get<Currency[]>(`${environment.apiUrl}/currency/latest`);
  }

  calculateExchangeRate(
    fromCurrency: string,
    toCurrency: string,
    amount: number,
  ) {
    const params = new HttpParams();
    params.append('fromCurrency', fromCurrency);
    params.append('toCurrency', toCurrency);
    params.append('amount', amount);

    return this.client.get<number>(
      `${environment.apiUrl}/currency/exchange-rate`,
      { params },
    );
  }
}
