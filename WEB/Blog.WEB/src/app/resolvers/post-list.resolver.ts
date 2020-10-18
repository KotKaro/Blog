import { Injectable } from '@angular/core';
import { Resolve } from '@angular/router';

import { Observable } from 'rxjs';

import { Post } from '../models/post.model';
import { PostService } from '../services/post.service';

@Injectable()
export class PostListResolver implements Resolve<Post[]> {
    constructor(private postService: PostService) {

    }

    resolve(): Observable<Post[]> {
        return this.postService.getAll();
    }
}