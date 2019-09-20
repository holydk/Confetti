import { Component, OnInit } from '@angular/core';
import { Store } from '@ngrx/store';
import { AppState } from '@app/state/reducers/app.state';
import { SignInRedirectCallback } from '@app/core/identity/state/actions/identity.actions';

@Component({
  selector: 'app-signin-oidc',
  template: ''
})
export class SignInOidcComponent implements OnInit {

  constructor(
    private store: Store<AppState>
  ) { }

  ngOnInit() {
    this.store.dispatch(new SignInRedirectCallback());
  }
}
