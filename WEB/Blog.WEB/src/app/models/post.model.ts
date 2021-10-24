import {Comment} from './comment.model';
import {Entity} from './entity';

export class Post extends Entity {
  title: string;
  content: string;
  creationDate: Date;
  comments: Comment[];
}
