import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Comment } from 'src/app/models/comment.model';
import { Post } from 'src/app/models/post.model';
import { AuthService } from 'src/app/services/auth.service';
import { BlogRouterService } from 'src/app/services/blog-router.service';
import { ConfirmDialogService } from 'src/app/services/confirm-dialog.service';
import { PostService } from 'src/app/services/post.service';

@Component({
  selector: 'app-post-details',
  templateUrl: './post-details.component.html',
  styleUrls: ['./post-details.component.scss']
})
export class PostDetailsComponent {
  post: Post = { content: '', title: '', id: '', creationDate: new Date(), comments: [] };

  constructor(
    private route: ActivatedRoute,
    private blogRouter: BlogRouterService,
    public authService: AuthService,
    private postService: PostService,
    private confirmDialogService: ConfirmDialogService
  ) {
    this.post = route.snapshot.data.post;
  }

  goToEdit(): void {
    this.blogRouter.goToEditPost(this.getCurrentId());
  }

  getCurrentId(): string {
    return this.route.snapshot.paramMap.get('id');
  }

  isAuthenticated(): boolean {
    return this.authService.isAuthenticated();
  }

  showDialog(): void {
    this.confirmDialogService.confirmThis("Are you sure to delete?",
      () => {
        this.removePost();
      }, () => {
      });
  }

  removePost(): void {
    this.postService.remove(this.post.id)
      .subscribe(() => {
        this.blogRouter.goToBlog();
      });
  }

  onCommentCreated(comment: Comment): void {
    this.post.comments.push(comment);
  }

  onCommentRemoved(commentId: string): void {
    if (!commentId) {
      return;
    }

    this.post.comments = this.post.comments.filter(c => c.id !== commentId);
  }
}
