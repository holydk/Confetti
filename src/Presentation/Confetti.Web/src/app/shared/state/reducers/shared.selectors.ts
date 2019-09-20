import { createSelector } from '@ngrx/store';
import { AppState } from '@app/state/reducers/app.state';

export const selectSharedState = (state: AppState) => state.shared;

export const selectLastCookie = createSelector(
  selectSharedState,
  state => state.lastCookie
);

export const selectBreadcrumbPath = createSelector(
  selectSharedState,
  state => state.breadcrumbPath
);
