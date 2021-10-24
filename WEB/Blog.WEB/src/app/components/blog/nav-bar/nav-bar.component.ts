import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'app-nav-bar',
  templateUrl: './nav-bar.component.html',
  styleUrls: ['./nav-bar.component.scss']
})
export class NavBarComponent {
  constructor(private router: Router, private authService: AuthService) {
  }

  isLoggedIn(): boolean {
    return this.authService.isAuthenticated();
  }

  isAtLoginComponent(): boolean {
    return this.router.url.endsWith('login');
  }

  isAtManagmentComponent(): boolean {
    return this.router.url.endsWith('management');
  }

  getCorrectRoute(): string {
    if (this.authService.isAuthenticated() && !this.isAtManagmentComponent()) {
      return 'management';
    }

    if (this.isAtLoginComponent() || this.isAtManagmentComponent()) {
      return 'blog';
    }

    return 'login';
  }
}
