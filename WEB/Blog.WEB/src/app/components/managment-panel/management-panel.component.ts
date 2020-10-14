import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'app-management-panel',
  templateUrl: './management-panel.component.html',
  styleUrls: ['./management-panel.component.scss']
})

export class ManagementPanelComponent {
  constructor(private router: Router, private authService: AuthService) {

  }

  navigateToPostCreate(): void {
    this.router.navigate(['/post-create']);
  }

  logout(): void {
    this.authService.logout();
    this.router.navigate(['/blog']);
  }
}
