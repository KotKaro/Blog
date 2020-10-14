import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Post } from 'src/app/models/post.model';
import { PostService } from 'src/app/services/post.service';

@Component({
  selector: 'app-post-create',
  templateUrl: './post-edit.component.html',
  styleUrls: ['./post-edit.component.scss']
})
export class PostEditComponent {
  post: Post;

  constructor(private route: ActivatedRoute, private postService: PostService, private router: Router) {
    this.post = route.snapshot.data.post;
  }

  update(): void {
    this.postService.update(this.post).subscribe(() => {
      this.router.navigate([`/post/${this.post.id}`]);
    });
  }

  isPostValid(): boolean {
    return !!this.post?.content?.trim() && !!this.post?.title?.trim();
  }
}
