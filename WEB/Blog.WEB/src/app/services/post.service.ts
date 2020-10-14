import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ToastrService } from 'ngx-toastr';

import { Post } from '../models/post.model';
import { EntityService } from './entity.service';

@Injectable()
export class PostService extends EntityService<Post> {
    constructor(protected httpClient: HttpClient, protected toastrService: ToastrService) {
        super('posts', httpClient, toastrService);
    }
}

