import { Injectable } from '@angular/core';
import { tap, catchError } from 'rxjs/operators';
import { Observable, of } from 'rxjs';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { TokenService } from './token.service';

@Injectable()
export class AuthService {
    currentUser;

    constructor(private tokenService: TokenService, private http: HttpClient) {

    }

    login(userName: string, password: string): Observable<any> {
        const options = {
            headers: new HttpHeaders({ 'Content-Type': 'application/json' })
        };

        const loginInfo = {
            username: userName,
            password,
        };

        return this.http.post<any>('/api/auth/login', loginInfo, options).pipe(tap(token => {
            this.tokenService.setToken(token.value);
        }))
            .pipe(catchError(err => {
                return of(false);
            }));
    }

    checkAuthenticationStatus(): void {
        const options = {
            headers: new HttpHeaders({ 'Content-Type': 'application/json' })
        };

        const tokenInfo = {
            token: this.tokenService.getToken()
        };

        this.http.post<any>('api/auth/refreshtoken', tokenInfo, options)
            .pipe(tap(token => this.tokenService.setToken(token)))
            .pipe(catchError(err => {
                this.tokenService.setToken('');
                return of(false);
            }));
    }

    isAuthenticated(): boolean {
        return !!this.tokenService.getToken();
    }

    logout() {
        this.tokenService.setToken('');
    }
}
