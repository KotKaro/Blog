import { Component, OnDestroy } from '@angular/core';
import { Subscription } from 'rxjs';
import { timeout } from 'rxjs/operators';
import { LoadingService } from 'src/app/services/loading.service';

@Component({
  selector: 'app-loading',
  templateUrl: './loading.component.html',
  styleUrls: ['./loading.component.scss']
})

export class LoadingComponent implements OnDestroy {
  visible: boolean;
  startSubscription: Subscription;
  endSubscription: Subscription;

  constructor(private loadingService: LoadingService) {
    this.startSubscription = loadingService.requestStartEvent.subscribe(() => {
      setTimeout(() => {
        this.visible = true;
      }, 1);
    });

    this.endSubscription = loadingService.requestEndEvent.subscribe(() => {
      this.visible = false;
    });
  }

  ngOnDestroy(): void {
    this.startSubscription.unsubscribe();
    this.endSubscription.unsubscribe();
  }
}
