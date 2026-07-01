import { Component, computed, inject, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';

import { AuthStateService } from '../../services/auth-state.service';
import { HealthService } from '../../services/health.service';

type HealthState = 'checking' | 'online' | 'offline';

@Component({
  selector: 'app-login-page',
  imports: [
    ReactiveFormsModule,
    MatButtonModule,
    MatCardModule,
    MatFormFieldModule,
    MatIconModule,
    MatInputModule,
    MatProgressSpinnerModule
  ],
  templateUrl: './login.page.html',
  styleUrl: './login.page.scss'
})
export class LoginPage {
  private readonly fb = inject(FormBuilder);
  private readonly authState = inject(AuthStateService);
  private readonly healthService = inject(HealthService);
  private readonly router = inject(Router);

  protected readonly isSubmitting = signal(false);
  protected readonly isSubmittingDemo = signal(false);
  protected readonly errorMessage = signal<string | null>(null);
  protected readonly healthState = signal<HealthState>('checking');
  protected readonly healthMessage = computed(() => {
    switch (this.healthState()) {
      case 'online':
        return 'Server is online';
      case 'offline':
        return 'Server is waking up. You can still try logging in.';
      default:
        return 'Checking server...';
    }
  });
  protected readonly canSubmit = computed(() => this.healthState() !== 'checking');

  protected readonly form = this.fb.group({
    emailOrUsername: ['', [Validators.required]],
    password: ['', [Validators.required]]
  });

  constructor() {
    this.refreshHealth();
  }

  protected refreshHealth(): void {
    this.healthState.set('checking');

    this.healthService.checkHealth().subscribe((healthy) => {
      this.healthState.set(healthy ? 'online' : 'offline');
    });
  }

  protected async submit(): Promise<void> {
    if (this.form.invalid || this.isSubmitting() || this.isSubmittingDemo()) {
      this.form.markAllAsTouched();
      return;
    }

    this.isSubmitting.set(true);
    this.errorMessage.set(null);

    try {
      await this.authState.login({
        emailOrUsername: this.form.controls.emailOrUsername.value?.trim() ?? '',
        password: this.form.controls.password.value ?? ''
      });
      await this.router.navigate(['/home']);
    } catch {
      this.errorMessage.set('Login failed. Check your credentials and try again.');
    } finally {
      this.isSubmitting.set(false);
    }
  }

  protected async submitDemo(): Promise<void> {
    if (this.isSubmitting() || this.isSubmittingDemo()) {
      return;
    }

    this.isSubmittingDemo.set(true);
    this.errorMessage.set(null);

    try {
      await this.authState.loginDemo();
      await this.router.navigate(['/home']);
    } catch {
      this.errorMessage.set('Demo sign-in failed. Please try again.');
    } finally {
      this.isSubmittingDemo.set(false);
    }
  }
}
