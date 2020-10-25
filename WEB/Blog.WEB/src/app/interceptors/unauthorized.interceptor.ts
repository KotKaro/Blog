import { HttpErrorResponse, HttpEvent, HttpHandler, HttpInterceptor, HttpRequest, HttpResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';

import { Observable, of } from 'rxjs';

import { TokenService } from '../services/token.service';
import { catchError, filter, tap } from 'rxjs/operators';
import { BlogRouterService } from '../services/blog-router.service';


@Injectable()
export class UnauthorizedInterceptor implements HttpInterceptor {
    constructor(private tokenService: TokenService, private blogRouter: BlogRouterService) { }
    intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        return next.handle(request)
            .pipe(catchError(r => {
                const error = r as HttpErrorResponse;
                if (error.status === 401 || error.status === 403) {
                    this.tokenService.setToken('');
                    this.blogRouter.goToLogin();
                    return of(null);
                }

                return of(r);
            }));
    }
}
