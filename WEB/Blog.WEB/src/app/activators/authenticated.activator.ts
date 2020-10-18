import { Injectable } from '@angular/core';
import { CanActivate } from '@angular/router';
import { AuthService } from '../services/auth.service';
import { BlogRouterService } from '../services/blog-router.service';

@Injectable()
export class AuthenticatedActivator implements CanActivate {
  constructor(private authService: AuthService, private blogRouter: BlogRouterService) { }

  canActivate(): boolean {
    const result = this.authService.isAuthenticated();
    if (!result) {
      this.blogRouter.goToBlog();
    }

    return result;
  }
}
