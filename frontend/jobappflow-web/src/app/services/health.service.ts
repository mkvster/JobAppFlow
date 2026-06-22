import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { catchError, map, of, Observable } from 'rxjs';

import { environment } from '../../environments/environment';

@Injectable({ providedIn: 'root' })
export class HealthService {
  private readonly http = inject(HttpClient);

  readonly healthUrl = `${environment.backendBaseUrl.replace(/\/$/, '')}/health`;

  checkHealth(): Observable<boolean> {
    return this.http.get(this.healthUrl, { responseType: 'text' }).pipe(
      map((response) => response.trim().length > 0),
      catchError(() => of(false))
    );
  }
}
