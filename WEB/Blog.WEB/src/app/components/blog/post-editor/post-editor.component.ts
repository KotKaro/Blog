import { Component, EventEmitter, Input, Output } from '@angular/core';
import { UntypedFormBuilder, UntypedFormControl, UntypedFormGroup, Validators } from '@angular/forms';
import { AngularEditorConfig } from '@kolkov/angular-editor';
import { Post } from 'src/app/models/post.model';

@Component({
  selector: 'app-post-editor',
  templateUrl: './post-editor.component.html',
  styleUrls: ['./post-editor.component.scss']
})
export class PostEditorComponent {
  private post: Post;

  @Output() postChange = new EventEmitter<Post>();
  postForm: UntypedFormGroup;

  @Input()
  set Post(val: Post) {
    this.post = val;
    this.postChange.emit(this.post);

    this.postForm.get('title').setValue(this.post?.title);
    this.postForm.get('content').setValue(this.post?.content);
    this.postChange.emit(this.post);
  }

  editorConfig: AngularEditorConfig = {
    editable: true,
    spellcheck: true,
    height: '100%',
    minHeight: '100%',
    maxHeight: '100%',
    width: 'auto',
    minWidth: '0',
    translate: 'yes',
    enableToolbar: true,
    showToolbar: true,
    placeholder: 'Enter text here...',
    defaultParagraphSeparator: '',
    defaultFontName: '',
    defaultFontSize: '',
    fonts: [
      { class: 'arial', name: 'Arial' },
      { class: 'times-new-roman', name: 'Times New Roman' },
      { class: 'calibri', name: 'Calibri' },
      { class: 'comic-sans-ms', name: 'Comic Sans MS' }
    ],
    uploadUrl: '/api/images',
    uploadWithCredentials: false,
    sanitize: true,
    toolbarPosition: 'top',
    toolbarHiddenButtons: [
      ['bold', 'italic'],
      ['fontSize']
    ]
  };
  constructor(private fb: UntypedFormBuilder) {
    this.postForm = fb.group({
      title: new UntypedFormControl(this.post?.title, [Validators.required]),
      content: new UntypedFormControl(this.post?.content, [Validators.required])
    });

    this.postForm.get('title').valueChanges.subscribe(title => {
      if (!this.post) {
        this.post = {} as Post;
      }

      this.post.title = title;
      this.postChange.emit(this.post);
    });

    this.postForm.get('content').valueChanges.subscribe(content => {
      if (!this.post) {
        this.post = {} as Post;
      }

      this.post.content = content;
      this.postChange.emit(this.post);
    });
  }
}
