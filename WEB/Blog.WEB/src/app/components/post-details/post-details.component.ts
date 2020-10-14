import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Post } from 'src/app/models/post.model';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'app-post-details',
  templateUrl: './post-details.component.html',
  styleUrls: ['./post-details.component.scss']
})
export class PostDetailsComponent {
  post: Post = { content: '', title: '', id: '' };

  constructor(private route: ActivatedRoute, private router: Router, public authService: AuthService) {
    this.post = route.snapshot.data.post;
  }

  goToEdit(): void {
    this.router.navigate(['/post/edit/' + this.getCurrentId()]);
  }

  getCurrentId(): string {
    return this.route.snapshot.paramMap.get('id');
  }

  isAuthenticated(): boolean {
    return this.authService.isAuthenticated();
  }
}
