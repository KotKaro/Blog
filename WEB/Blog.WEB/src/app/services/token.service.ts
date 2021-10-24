import {Injectable} from '@angular/core';
import {CookieService} from 'ngx-cookie-service';

@Injectable()
export class TokenService {

  private tokenCookieName = 'token';

  constructor(private cookieService: CookieService) {

  }

  setToken(token: string): void {
    this.cookieService.set(this.tokenCookieName, token);
  }

  getToken(): string {
    return this.cookieService.get(this.tokenCookieName);
  }
}
