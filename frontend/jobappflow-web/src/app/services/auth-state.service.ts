import { computed, inject, Injectable, signal } from '@angular/core';
import { catchError, firstValueFrom, of } from 'rxjs';

import { AuthApiService } from './auth-api.service';
import { AuthSession, AuthUser, LoginRequest } from '../models/auth.models';

@Injectable({ providedIn: 'root' })
export class AuthStateService {
  private readonly api = inject(AuthApiService);

  private readonly userSignal = signal<AuthUser | null>(null);
  private readonly accessTokenSignal = signal<string | null>(null);
  private readonly initializedSignal = signal(false);
  private readonly initializingSignal = signal(false);
  private readonly loggingOutSignal = signal(false);

  private initializationPromise: Promise<AuthSession | null> | null = null;

  readonly user = this.userSignal.asReadonly();
  readonly accessToken = this.accessTokenSignal.asReadonly();
  readonly isAuthenticated = computed(() => this.userSignal() !== null && this.accessTokenSignal() !== null);
  readonly isDemo = computed(() => this.userSignal()?.roles.includes('Demo') ?? false);
  readonly isInitialized = this.initializedSignal.asReadonly();
  readonly isInitializing = this.initializingSignal.asReadonly();
  readonly isLoggingOut = this.loggingOutSignal.asReadonly();

  async initialize(): Promise<void> {
    if (this.initializedSignal()) {
      return;
    }

    await this.refreshSession();
    this.initializedSignal.set(true);
  }

  async login(request: LoginRequest): Promise<AuthSession> {
    const session = await firstValueFrom(this.api.login(request));
    this.setSession(session);
    this.initializedSignal.set(true);
    return session;
  }

  async loginDemo(): Promise<AuthSession> {
    const session = await firstValueFrom(this.api.demo());
    this.setSession(session);
    this.initializedSignal.set(true);
    return session;
  }

  async refreshSession(): Promise<AuthSession | null> {
    if (this.loggingOutSignal()) {
      return null;
    }

    if (this.initializationPromise) {
      return this.initializationPromise;
    }

    this.initializingSignal.set(true);
    this.initializationPromise = firstValueFrom(
      this.api.refresh().pipe(
        catchError(() => of(null))
      )
    ).then((session) => {
      if (session) {
        this.setSession(session);
      } else {
        this.clearSession();
      }

      return session;
    }).finally(() => {
      this.initializingSignal.set(false);
      this.initializationPromise = null;
    });

    return this.initializationPromise;
  }

  async logout(): Promise<void> {
    this.loggingOutSignal.set(true);

    try {
      await firstValueFrom(
        this.api.logout().pipe(
          catchError(() => of(void 0))
        )
      );
    } finally {
      this.clearSession();
      this.initializedSignal.set(true);
      this.loggingOutSignal.set(false);
    }
  }

  private setSession(session: AuthSession): void {
    this.userSignal.set(session.user);
    this.accessTokenSignal.set(session.accessToken);
  }

  private clearSession(): void {
    this.userSignal.set(null);
    this.accessTokenSignal.set(null);
  }
}
