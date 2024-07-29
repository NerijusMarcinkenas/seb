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

  baseCurrencyAmount = signal<number | null>(null);
  targetCurrencyAmount = signal<number | null>(null);
  baseCurrency = signal('');
  targetCurrency = signal('');

  error = signal('');

  onCalculate() {
    const baseCurrency = this.baseCurrency();
    const targetCurrency = this.targetCurrency();
    const amount = this.baseCurrencyAmount();
    if (!baseCurrency && !targetCurrency) {
      this.error.set('Please select a currency to convert from and to');
    } else if (!baseCurrency && targetCurrency) {
      this.error.set('Please select a currency to convert from');
    } else if (!targetCurrency && baseCurrency) {
      this.error.set('Please select a currency to convert to');
    } else if (!amount || amount === 0) {
      this.error.set('Please enter an amount to convert');
    } else if (baseCurrency && targetCurrency && amount) {
      this.error.set('');

      this.currencyApi
        .calculateExchangeRate(baseCurrency, targetCurrency, amount)
        .subscribe({
          next: (calculatedAmount) => {
            this.targetCurrencyAmount.set(calculatedAmount);
          },
        });
    }
  }
}
