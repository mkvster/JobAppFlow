import { Component, computed, inject } from '@angular/core';
import { Router } from '@angular/router';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { MatMenuModule } from '@angular/material/menu';
import { MatToolbarModule } from '@angular/material/toolbar';

import { AuthStateService } from '../../services/auth-state.service';

@Component({
  selector: 'app-home-page',
  imports: [
    MatButtonModule,
    MatCardModule,
    MatIconModule,
    MatMenuModule,
    MatToolbarModule
  ],
  templateUrl: './home.page.html',
  styleUrl: './home.page.scss'
})
export class HomePage {
  private readonly authState = inject(AuthStateService);
  private readonly router = inject(Router);

  protected readonly user = this.authState.user;
  protected readonly isDemo = this.authState.isDemo;
  protected readonly handle = computed(() => {
    const user = this.authState.user();
    if (!user) {
      return '';
    }

    const emailLocalPart = user.email?.split('@')[0]?.trim();
    return emailLocalPart || user.userName || 'account';
  });

  protected async logout(): Promise<void> {
    await this.authState.logout();
    await this.router.navigate(['/login']);
  }
}
