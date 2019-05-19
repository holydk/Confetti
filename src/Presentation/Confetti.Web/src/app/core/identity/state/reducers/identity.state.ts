export interface IdentityErrorState {
  signInSilentError: any;
  signInError: any;
}

export interface IdentityState {
  user: any;
  errors: IdentityErrorState;
}
