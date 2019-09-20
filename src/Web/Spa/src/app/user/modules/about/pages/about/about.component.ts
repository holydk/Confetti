import { Component, OnInit, OnDestroy } from '@angular/core';
import { Store } from '@ngrx/store';
import { AppState } from '@app/state/reducers/app.state';
import { SetBreadcrumbPath } from '@shared/state/actions/shared.actions';

@Component({
  selector: 'app-about',
  templateUrl: './about.component.html'
})
export class AboutComponent implements OnInit, OnDestroy {
  ngOnDestroy(): void {
    console.log('Destroy');
  }

  constructor(
    private store: Store<AppState>
  ) { }

  ngOnInit() {
    console.log('Init');
    this.store.dispatch(new SetBreadcrumbPath(
      {
          path: [ 'Детям' ]
      }
  ));
  }
}
