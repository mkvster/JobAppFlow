import { Routes } from '@angular/router';
import { HealthPage } from '../pages/health/health.page';

export const routes: Routes = [
  {
    path: '',
    pathMatch: 'full',
    redirectTo: 'health'
  },
  {
    path: 'health',
    component: HealthPage
  },
  {
    path: '**',
    redirectTo: 'health'
  }
];
