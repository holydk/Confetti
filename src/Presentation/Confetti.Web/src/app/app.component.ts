import { Component, OnInit } from '@angular/core';
import { AppState } from './state/reducers/app.state';
import { Store } from '@ngrx/store';
import { GetUser } from './core/identity/state/actions/identity.actions';

@Component({
  selector: 'app-root',
  template: '<router-outlet></router-outlet>'
})
export class AppComponent implements OnInit {
  constructor(
    private store: Store<AppState>
  ) {}

  ngOnInit(): void {
    this.store.dispatch(new GetUser());
  }
}
