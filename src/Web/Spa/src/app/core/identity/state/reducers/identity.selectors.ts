import { createSelector } from '@ngrx/store';
import { AppState } from '@app/state/reducers/app.state';

export const selectIdentityState = (state: AppState) => state.identity;

export const selectUser = createSelector(
  selectIdentityState,
  state => state.user
);

export const selectIsLoggedIn = createSelector(
  selectUser,
  user => user != null && user.expired !== true,
);
