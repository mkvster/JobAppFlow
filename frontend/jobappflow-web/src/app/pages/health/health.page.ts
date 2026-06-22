import { NgIf } from '@angular/common';
import { Component, inject, signal } from '@angular/core';

import { environment } from '../../../environments/environment';
import { HealthService } from '../../services/health.service';

type HealthState = 'loading' | 'healthy' | 'unhealthy';

@Component({
  selector: 'app-health-page',
  imports: [NgIf],
  templateUrl: './health.page.html',
  styleUrl: './health.page.scss'
})
export class HealthPage {
  private readonly healthService = inject(HealthService);

  protected readonly title = signal('JobAppFlow');
  protected readonly healthUrl = this.healthService.healthUrl;
  protected readonly status = signal<HealthState>('loading');
  protected readonly details = signal('Checking backend connection...');
  protected readonly lastChecked = signal<string | null>(null);

  constructor() {
    this.refreshHealth();
  }

  protected refreshHealth(): void {
    this.status.set('loading');
    this.details.set(`Checking ${this.healthUrl} ...`);

    this.healthService.checkHealth().subscribe((healthy) => {
      this.status.set(healthy ? 'healthy' : 'unhealthy');
      this.details.set(
        healthy
          ? `Backend is reachable.`
          : 'Backend health check failed. Check the API URL and confirm the server is running.'
      );
      this.lastChecked.set(new Date().toLocaleString());
    });
  }
}
