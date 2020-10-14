import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, Resolve } from '@angular/router';

import { Observable } from 'rxjs';

import { Post } from '../models/post.model';
import { PostService } from '../services/post.service';

@Injectable()
export class PostResolver implements Resolve<Post> {
    constructor(private postService: PostService) {

    }

    resolve(route: ActivatedRouteSnapshot): Observable<Post> {
        return this.postService.getById(route.paramMap.get('id'));
    }
}