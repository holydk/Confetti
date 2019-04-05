import { createSelector } from '@ngrx/store';
import { selectUserState } from '@user/state/reducers/user.selectors';

export const selectLayoutsState = createSelector(
    selectUserState,
    state => state.layouts
);

export const selectLayoutModel = createSelector(
    selectLayoutsState,
    state => state.layoutModel
);
