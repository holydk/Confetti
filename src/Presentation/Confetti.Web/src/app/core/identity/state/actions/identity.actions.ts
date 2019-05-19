import { Action } from '@ngrx/store';

export enum IdentityActionsTypes {
  SignInRedirect = '[Identity] sign in redirect',
  SignInSilent = '[Identity] sign in silent',
  SignInError = '[Identity] sign in error',
  SignInSilentError = '[Identity] sign in silent error',
  SignInRedirectCallback = '[Identity] sign in redirect callback',

  GetUser = '[Identity] get user',
  GetUserSuccess = '[Identity] get user success'
}

//#region Sign In

export class SignInRedirect implements Action {
  readonly type = IdentityActionsTypes.SignInRedirect;
}

export class SignInSilent implements Action {
  readonly type = IdentityActionsTypes.SignInSilent;
}

export class SignInError implements Action {
  readonly type = IdentityActionsTypes.SignInError;

  constructor(
    public payload: { error: Error }
  ) {}
}

export class SignInSilentError implements Action {
  readonly type = IdentityActionsTypes.SignInSilentError;

  constructor(
    public payload: { error: Error }
  ) {}
}

export class SignInRedirectCallback implements Action {
  readonly type = IdentityActionsTypes.SignInRedirectCallback;
}

//#endregion

//#region User

export class GetUser implements Action {
  readonly type = IdentityActionsTypes.GetUser;
}

export class GetUserSuccess implements Action {
  readonly type = IdentityActionsTypes.GetUserSuccess;

  constructor(
    public payload: { user: any }
  ) {}
}

//#endregion
