import { CommonModule } from '@angular/common';
import { Component, effect, inject } from '@angular/core';
import { Router, RouterLink, RouterModule } from '@angular/router';
import { NzConfigService } from 'ng-zorro-antd/core/config';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterModule, RouterLink, CommonModule],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss',
})
export class AppComponent {
  router = inject(Router);

  navigateEffect = effect(() => {
    this.router.navigate(['/currency']);
  });
}
