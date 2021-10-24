import {Injectable} from '@angular/core';
import {Router} from '@angular/router';

@Injectable()
export class BlogRouterService {

  constructor(private router: Router) {
  }

  public Routes = {
    aboutMe: '/about-me'
  };
  currentUser;

  goToBlog(): void {
    this.router.navigate(['/blog']);
  }

  goToAboutMe(): void {
    this.router.navigate([this.Routes.aboutMe]);
  }

  goToPostDetails(id: string): void {
    if (!id) {
      this.goToBlog();
    }

    this.router.navigate(['/post', id]);
  }

  goToPostCreate(): void {
    this.router.navigate(['/post-create']);
  }

  goToEditPost(id: string): void {
    if (!id) {
      return this.goToBlog();
    }

    this.router.navigate(['/post/edit/', id]);
  }

  goToLogin(): void {
    this.router.navigate(['/login']);
  }
}
