import { Entity } from './entity';

export class Comment extends Entity {
    postId: string;
    creator: string;
    content: string;
}
