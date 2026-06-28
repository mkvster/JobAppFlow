import { Component, inject, signal } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';

import { HealthService } from '../../services/health.service';

type HealthState = 'loading' | 'healthy' | 'unhealthy';

@Component({
  selector: 'app-health-page',
  imports: [MatButtonModule, MatCardModule],
  templateUrl: './health.page.html',
  styleUrl: './health.page.scss'
})
export class HealthPage {
  private readonly healthService = inject(HealthService);

  protected readonly title = signal('JobAppFlow');
  protected readonly healthUrl = this.healthService.healthUrl;
  protected readonly status = signal<HealthState>('loading');
  protected readonly details = signal('Checking server...');
  protected readonly lastChecked = signal<string | null>(null);

  constructor() {
    this.refreshHealth();
  }

  protected refreshHealth(): void {
    this.status.set('loading');
    this.details.set('Checking server...');

    this.healthService.checkHealth().subscribe((healthy) => {
      this.status.set(healthy ? 'healthy' : 'unhealthy');
      this.details.set(
        healthy
          ? 'Server is online.'
          : 'Server is waking up or unavailable.'
      );
      this.lastChecked.set(new Date().toLocaleString());
    });
  }
}
