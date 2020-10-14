import { Component, Input, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AngularEditorConfig } from '@kolkov/angular-editor';
import { Post } from 'src/app/models/post.model';
import { PostService } from 'src/app/services/post.service';

@Component({
  selector: 'app-post-create',
  templateUrl: './post-create.component.html',
  styleUrls: ['./post-create.component.scss']
})
export class PostCreateComponent {
  post: Post;

  constructor(private fb: FormBuilder, private postService: PostService, private router: Router) {
  }

  createPost(): void {
    this.postService.create(this.post)
    .subscribe(data => {
      this.router.navigate(['/post', data['id']]);
    });
  }

  isPostValid() : boolean {
    return !!this.post?.content && !!this.post?.title;
  }
}
