import { EventEmitter, Injectable } from '@angular/core';

@Injectable()
export class LoadingService {
    httpCounter: number;

    public requestStartEvent = new EventEmitter<any>();
    public requestEndEvent = new EventEmitter<any>();

    incrementCounter(): void {
        this.httpCounter += 1;
        this.requestStartEvent.emit();
    }

    decrementCounter(): void {
        this.httpCounter -= 1;
        if (this.httpCounter === 0) {
            this.requestEndEvent.emit();
        }
    }
}

