import { createSelector } from '@ngrx/store';
import { AppState } from '@app/state/reducers/app.state';

export const selectSharedState = (state: AppState) => state.shared;

export const getLastCookie = createSelector(
    selectSharedState,
    state => state.lastCookie);

export const getBreadcrumbPath = createSelector(
    selectSharedState,
    state => state.breadcrumbPath);
