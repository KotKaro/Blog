import { Component, EventEmitter, Input, Output } from '@angular/core';
import { UntypedFormBuilder, UntypedFormControl, UntypedFormGroup, Validators } from '@angular/forms';
import { Comment } from 'src/app/models/comment.model';
import { CommentService } from 'src/app/services/comment.service';

@Component({
  selector: 'app-comment-create',
  templateUrl: './comment-create.component.html',
  styleUrls: ['./comment-create.component.scss']
})
export class CommentCreateComponent {
  @Input() postId: string;
  @Output() commentCreatedEventEmitter = new EventEmitter<Comment>();

  commentCreateForm: UntypedFormGroup;
  comment: Comment = { id: '', content: '', creator: '', postId: '' };

  constructor(fb: UntypedFormBuilder, private commentService: CommentService) {
    this.commentCreateForm = fb.group({
      creator: new UntypedFormControl(this.comment?.creator, [Validators.required]),
      content: new UntypedFormControl(this.comment?.content, [Validators.required])
    });
  }

  addComment(): void {
    const comment = {
      id: '',
      postId: this.postId,
      content: this.commentCreateForm.get('content').value,
      creator: this.commentCreateForm.get('creator').value,
    };

    this.commentService.create(comment).subscribe(() => {
      this.commentCreatedEventEmitter.emit(comment);
    });
  }
}
