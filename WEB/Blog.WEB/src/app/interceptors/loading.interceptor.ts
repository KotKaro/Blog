import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from '@angular/common/http';
import { Injectable } from '@angular/core';

import { Observable } from 'rxjs';

import { TokenService } from '../services/token.service';
import { environment } from '../../environments/environment';


@Injectable()
export class LoadingInterceptor implements HttpInterceptor {
    constructor(public tokenService: TokenService) { }
    intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {

        if (!environment.production) {
            return next.handle(request);
        }

        const redirectRequest = request.clone({ url: request.url.replace('/api/', environment.apiUrl), method: request.method});
        console.log(redirectRequest.url);

        return next.handle(redirectRequest);
    }
}
