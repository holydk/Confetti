import { Component, OnInit, OnDestroy } from '@angular/core';
import { Store } from '@ngrx/store';
import { AppState } from '@app/state/reducers/app.state';
import { SignInRedirect } from '@app/core/identity/state/actions/identity.actions';
import { UnsubscribeSubject } from '@app/core/unsubscribe.subject';
import { Observable, Subject } from 'rxjs';
import { filter, switchMap, takeUntil } from 'rxjs/operators';
import { selectUser, selectIsLoggedIn } from '@app/core/identity/state/reducers/identity.selectors';

@Component({
  selector: 'app-account-dropdown',
  templateUrl: './account-dropdown.component.html',
  styleUrls: ['./account-dropdown.component.scss']
})
export class AccountDropdownComponent implements OnInit, OnDestroy {
  //#region Fields

  private unsubscribed$ = new Subject();

  //#endregion

  public isAuthenticated$: Observable<boolean>;
  public user$: Observable<any>;

  constructor(
    private store: Store<AppState>
  ) { }

  ngOnInit() {
    this.user$ = this.getUser();
    this.isAuthenticated$ = this.isLoggedIn();
  }

  /**
   * login
   */
  public login() {
    this.store.dispatch(new SignInRedirect());
  }

  //#region Utilities

  private getUser(): Observable<any> {
    return this.isLoggedIn().pipe(
      filter(loggedIn => loggedIn),
      switchMap(() => this.store.select(selectUser))
    );
  }

  private isLoggedIn(): Observable<boolean> {
    return this.store.select(selectIsLoggedIn).pipe(
      takeUntil(this.unsubscribed$)
    );
  }

  //#endregion

  ngOnDestroy() {
    this.unsubscribed$.next();
    this.unsubscribed$.complete();
  }
}
