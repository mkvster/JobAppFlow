import { Routes } from '@angular/router';
import { authGuard, guestGuard } from './auth.guard';

export const routes: Routes = [
  {
    path: '',
    pathMatch: 'full',
    redirectTo: 'home'
  },
  {
    path: 'login',
    loadComponent: () => import('../pages/login/login.page').then((m) => m.LoginPage),
    canActivate: [guestGuard]
  },
  {
    path: 'home',
    loadComponent: () => import('../pages/home/home.page').then((m) => m.HomePage),
    canActivate: [authGuard]
  },
  {
    path: 'health',
    loadComponent: () => import('../pages/health/health.page').then((m) => m.HealthPage)
  },
  {
    path: '**',
    redirectTo: 'home'
  }
];
