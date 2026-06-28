import { HttpClient } from '@angular/common/http';
import { HttpContext } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';

import { environment } from '../../environments/environment';
import { AuthSession, LoginRequest } from '../models/auth.models';
import { SKIP_AUTH_INTERCEPTOR } from './auth.interceptor';

@Injectable({ providedIn: 'root' })
export class AuthApiService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = environment.backendBaseUrl.replace(/\/$/, '');
  private readonly authUrl = `${this.baseUrl}/api/v1/auth`;

  login(request: LoginRequest) {
    return this.http.post<AuthSession>(`${this.authUrl}/login`, request, {
      withCredentials: true,
      context: new HttpContext().set(SKIP_AUTH_INTERCEPTOR, true)
    });
  }

  refresh() {
    return this.http.post<AuthSession>(`${this.authUrl}/refresh`, null, {
      withCredentials: true,
      context: new HttpContext().set(SKIP_AUTH_INTERCEPTOR, true)
    });
  }

  logout() {
    return this.http.post<void>(`${this.authUrl}/logout`, null, {
      withCredentials: true,
      context: new HttpContext().set(SKIP_AUTH_INTERCEPTOR, true)
    });
  }
}
