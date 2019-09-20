import { Subject, Observable } from 'rxjs';
import { takeUntil } from 'rxjs/operators';

export class UnsubscribeSubject {
  private unsubscribed$ = new Subject();

  protected withUnsubscribe<T>(observable: Observable<T>): Observable<T> {
    return observable.pipe(
      takeUntil(this.unsubscribed$)
    );
  }

  protected unsubscribe() {
    this.unsubscribed$.next();
    this.unsubscribed$.complete();
  }
}
