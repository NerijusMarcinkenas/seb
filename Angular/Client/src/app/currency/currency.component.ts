import { Component, computed, inject, signal } from '@angular/core';
import { toSignal } from '@angular/core/rxjs-interop';
import { CurrencyApiService } from '../services/currency.api.service';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzBadgeComponent } from 'ng-zorro-antd/badge';
import { NzOptionComponent, NzSelectComponent } from 'ng-zorro-antd/select';
import { NzTableComponent } from 'ng-zorro-antd/table';
import { NzInputNumberComponent } from 'ng-zorro-antd/input-number';
import { FormBuilder, FormsModule, ReactiveFormsModule } from '@angular/forms';

@Component({
  selector: 'app-currency',
  standalone: true,
  imports: [
    NzButtonModule,
    NzBadgeComponent,
    NzSelectComponent,
    NzOptionComponent,
    NzTableComponent,
    NzInputNumberComponent,
    FormsModule,
    ReactiveFormsModule,
  ],
  templateUrl: './currency.component.html',
})
export class CurrencyComponent {
  currencyApi = inject(CurrencyApiService);
  fb = inject(FormBuilder);

  currencies = toSignal(this.currencyApi.getLatestRates());
  currenciesComputed = computed(() => this.currencies());

  exchangeAmountFrom = signal<number | null>(null);
  exchangeAmountTo = signal<number | null>(null);
  currencyFrom = signal('');
  currencyTo = signal('');

  error = signal('');

  onFromChange(selectedCurrency: string, amount: number) {
    this.currencyFrom.set(selectedCurrency);
    this.exchangeAmountFrom.set(amount);
  }

  onToChange(selectedCurrency: string, amount: number) {
    this.currencyTo.set(selectedCurrency);
    this.exchangeAmountTo.set(amount);
  }

  onCalculate() {
    const fromCurrency = this.currencyFrom();
    const toCurrency = this.currencyTo();
    const amount = this.exchangeAmountFrom();
    if (!fromCurrency && !toCurrency) {
      this.error.set('Please select a currency to convert from and to');
    } else if (!fromCurrency && toCurrency) {
      this.error.set('Please select a currency to convert from');
    } else if (!toCurrency && fromCurrency) {
      this.error.set('Please select a currency to convert to');
    } else if (!amount) {
      this.error.set('Please enter an amount to convert');
    } else if (fromCurrency && toCurrency && amount) {
      this.error.set('');

      this.currencyApi
        .calculateExchangeRate(fromCurrency, toCurrency, amount)
        .subscribe({
          next: (rate) => {
            this.exchangeAmountTo.set(rate);
          },
        });
    }
  }
}
