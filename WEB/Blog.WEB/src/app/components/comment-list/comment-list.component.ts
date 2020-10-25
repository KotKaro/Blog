import { Component, EventEmitter, Input, Output } from '@angular/core';
import { Comment } from 'src/app/models/comment.model';
import { AuthService } from 'src/app/services/auth.service';
import { CommentService } from 'src/app/services/comment.service';

@Component({
  selector: 'app-comment-list',
  templateUrl: './comment-list.component.html',
  styleUrls: ['./comment-list.component.scss']
})
export class CommentListComponent {
  @Input() comments: Comment[];
  @Output() commentRemoveEventEmitter = new EventEmitter<string>();

  constructor(private authService: AuthService, private commentService: CommentService) {

  }

  isAuthenticated(): boolean {
    return this.authService.isAuthenticated();
  }

  removeComment(id: string): void {
    if (!id) {
      return;
    }

    this.commentService.remove(id).subscribe(() => {
      this.commentRemoveEventEmitter.emit(id);
    });
  }
}
