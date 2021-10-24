import {Injectable, OnDestroy} from '@angular/core';
import {tap, catchError, takeWhile} from 'rxjs/operators';
import {interval, Observable, of, Subscription} from 'rxjs';
import {HttpClient, HttpHeaders} from '@angular/common/http';
import {TokenService} from './token.service';
import {flatMap} from 'rxjs/internal/operators';

@Injectable()
export class AuthService implements OnDestroy {
  currentUser;
  private refreshSubscription$: Subscription;

  constructor(private tokenService: TokenService, private http: HttpClient) {

  }

  login(userName: string, password: string): Observable<any> {
    const options = {
      headers: new HttpHeaders({'Content-Type': 'application/json'})
    };

    const loginInfo = {
      username: userName,
      password,
    };

    return this.http.post<any>('/api/auth/login', loginInfo, options)
      .pipe(
        tap(token => {
          this.tokenService.setToken(token.value);
          this.checkAuthenticationStatus();
        }),
        catchError(err => {
          return of(false);
        })
      );
  }

  checkAuthenticationStatus(): void {
    if (!!this.refreshSubscription$ && !this.refreshSubscription$.closed) {
      return;
    }

    if (!this.tokenService.getToken()) {
      return;
    }

    this.refreshSubscription$ = this.refreshToken()
      .pipe(
        flatMap(() => interval(60 * 1000 * 15)),
        takeWhile(() => !!this.tokenService.getToken()),
        flatMap(() => this.refreshToken())
      )
      .subscribe();
  }

  refreshToken(): Observable<string> {
    const options = {
      headers: new HttpHeaders({'Content-Type': 'application/json'})
    };

    const tokenInfo = {
      token: this.tokenService.getToken()
    };

    return this.http.post<{ value: string }>('/api/auth/refreshtoken', tokenInfo, options)
      .pipe(
        tap(result => this.tokenService.setToken(result.value)),
        catchError(() => {
          this.tokenService.setToken('');
          return of(null);
        }));
  }

  isAuthenticated(): boolean {
    return !!this.tokenService.getToken();
  }

  logout(): void {
    this.tokenService.setToken('');
  }

  ngOnDestroy(): void {
    this.refreshSubscription$.unsubscribe();
  }
}
