import {Component} from '@angular/core';
import {Post} from 'src/app/models/post.model';
import {BlogRouterService} from 'src/app/services/blog-router.service';
import {PostService} from 'src/app/services/post.service';

@Component({
  selector: 'app-post-create',
  templateUrl: './post-create.component.html',
  styleUrls: ['./post-create.component.scss']
})
export class PostCreateComponent {
  post: Post = {} as Post;

  constructor(private postService: PostService, private blogRouter: BlogRouterService) {
  }

  createPost(): void {
    this.postService.create(this.post)
      .subscribe((post: Post) => {
        this.blogRouter.goToPostDetails(post.id);
      });
  }

  isPostValid(): boolean {
    return !!this.post?.content && !!this.post?.title;
  }
}
