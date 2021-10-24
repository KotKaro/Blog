import { AfterViewInit, Component, ElementRef, Input } from '@angular/core';
import { Post } from 'src/app/models/post.model';

@Component({
  selector: 'app-post-details-short',
  templateUrl: './post-details-short.component.html',
  styleUrls: ['./post-details-short.component.scss']
})
export class PostDetailsShortComponent {
  @Input() post: Post;

  showShowMoreButton: boolean;

  constructor(private element: ElementRef) {

  }

  isShowMoreButtonVisible(): boolean {
    const elements = this.element.nativeElement.querySelectorAll('.post-details');
    return elements[0].scrollHeight > 300;
  }
}
