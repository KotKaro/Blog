import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { AngularEditorConfig } from '@kolkov/angular-editor';
import { Post } from 'src/app/models/post.model';

@Component({
  selector: 'app-post-editor',
  templateUrl: './post-editor.component.html',
  styleUrls: ['./post-editor.component.scss']
})
export class PostEditorComponent {
  private _post: Post;

  @Output() postChange = new EventEmitter<Post>();
  postForm: FormGroup;

  @Input()
  set post(val: Post) {
    this._post = val;
    this.postChange.emit(this._post);

    this.postForm.get('title').setValue(this._post?.title);
    this.postForm.get('content').setValue(this._post?.content);
    this.postChange.emit(this._post);
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
  constructor(private fb: FormBuilder) {
    this.postForm = fb.group({
      title: new FormControl(this.post?.title, [Validators.required]),
      content: new FormControl(this.post?.content, [Validators.required])
    });

    this.postForm.get('title').valueChanges.subscribe(title => {
      if (!this._post) {
        this._post = {} as Post;
      }

      this._post.title = title;
      this.postChange.emit(this._post);
    });

    this.postForm.get('content').valueChanges.subscribe(content => {
      if (!this._post) {
        this._post = {} as Post;
      }

      this._post.content = content;
      this.postChange.emit(this._post);
    });
  }
}
