import { Actions, Effect, ofType } from '@ngrx/effects';
import { Injectable } from '@angular/core';
import { map, catchError, switchMap } from 'rxjs/operators';
import { of } from 'rxjs';
import { AuthService } from '@app/core/services/auth.service';
import {
  IdentityActionsTypes,
  GetUserSuccess,
  SignInSilentError
} from '../actions/identity.actions';

@Injectable()
export class IdentityEffects {
  constructor(
    private actions$: Actions,
    private authService: AuthService
  ) {}

  @Effect({ dispatch: false })
  signInRedirect$ = this.actions$.pipe(
    ofType(IdentityActionsTypes.SignInRedirect),
    switchMap(() => this.authService.signInRedirect())
  );

  @Effect()
  signInSilent$ = this.actions$.pipe(
    ofType(IdentityActionsTypes.SignInSilent),
    switchMap(() => {
      return this.authService.signInSilent().pipe(
        map(user => new GetUserSuccess({ user: user })),
        catchError(error => {
          console.log(error);
          return of(new SignInSilentError(error));
        })
      );
    })
  );

  @Effect()
  signInRedirectCallback$ = this.actions$.pipe(
    ofType(IdentityActionsTypes.SignInRedirectCallback),
    switchMap(() => {
      return this.authService.signInRedirectCallback().pipe(
        map(user => new GetUserSuccess({ user: user })),
        catchError(error => {
          console.log(error);
          return of(new SignInSilentError(error));
        })
      );
    })
  );

  @Effect()
  getUser$ = this.actions$.pipe(
    ofType(IdentityActionsTypes.GetUser),
    switchMap(() => {
      return this.authService.getUser().pipe(
        map(user => new GetUserSuccess({ user: user }))
      );
    })
  );
}
