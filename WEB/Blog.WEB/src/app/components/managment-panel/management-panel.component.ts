import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/services/auth.service';
import { BlogRouterService } from 'src/app/services/blog-router.service';

@Component({
  selector: 'app-management-panel',
  templateUrl: './management-panel.component.html',
  styleUrls: ['./management-panel.component.scss']
})

export class ManagementPanelComponent {
  constructor(private blogRouter: BlogRouterService, private authService: AuthService) {

  }

  navigateToPostCreate(): void {
    this.blogRouter.goToPostCreate();
  }

  logout(): void {
    this.authService.logout();
    this.blogRouter.goToBlog();
  }
}
