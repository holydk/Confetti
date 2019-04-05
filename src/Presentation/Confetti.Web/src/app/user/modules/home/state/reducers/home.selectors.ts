import { createFeatureSelector, createSelector } from '@ngrx/store';
import { HomeState } from './home.state';
import { homeStateKey } from './home.reducer';

export const selectHomeState = createFeatureSelector<HomeState>(homeStateKey);

export const selectCategoryHomeModel = createSelector(
    selectHomeState,
    state => state.categoryHomeModel
);
