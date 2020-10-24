import { Component, OnInit, ViewChild } from '@angular/core';
import { Location } from "@angular/common";
import { NavigationStart, Router } from '@angular/router';
import { AuthService } from './services/auth.service';
import { filter } from 'rxjs/operators';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit{
  title = 'Karol Kot';
  @ViewChild('pageHeader') header;

  constructor(private router: Router, private authService: AuthService) {
    router.events
    .pipe(filter(event => event instanceof NavigationStart))
    .subscribe((data: NavigationStart) => {
      const url = data?.url;

      if (!url) {
        return;
      }

      if (url.endsWith('/about-me')) {
        this.header.nativeElement.textContent = 'About me';
      } else {
        this.header.nativeElement.textContent = 'Blog';
      }
    });
  }

  ngOnInit(): void {
    this.authService.checkAuthenticationStatus();
  }
}
