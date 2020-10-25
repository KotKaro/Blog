import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ToastrService } from 'ngx-toastr';

import { EntityService } from './entity.service';
import { Comment } from '../models/comment.model';

@Injectable()
export class CommentService extends EntityService<Comment> {
    constructor(protected httpClient: HttpClient, protected toastrService: ToastrService) {
        super('Comments', httpClient, toastrService);
    }
}
