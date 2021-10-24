import {Component, OnInit, ViewChild} from '@angular/core';
import {NavigationStart, Router} from '@angular/router';
import {filter} from 'rxjs/operators';
import {BlogRouterService, AuthService} from './services';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
  private navigateFn;
  title = 'Karol Kot';
  @ViewChild('pageHeader') header;

  constructor(private router: Router, private authService: AuthService, blogRouterService: BlogRouterService) {
    router.events
      .pipe(filter(event => event instanceof NavigationStart))
      .subscribe((data: NavigationStart) => {
        const url = data?.url;

        if (!url) {
          return;
        }

        if (url.endsWith(blogRouterService.Routes.aboutMe)) {
          this.navigateFn = blogRouterService.goToAboutMe;
          this.header.nativeElement.textContent = 'About me';
        } else {
          this.navigateFn = blogRouterService.goToBlog;
          this.header.nativeElement.textContent = 'Blog';
        }
      });
  }

  ngOnInit(): void {
    this.authService.checkAuthenticationStatus();
  }

  onHeaderClick(): void {
    this.navigateFn();
  }
}
