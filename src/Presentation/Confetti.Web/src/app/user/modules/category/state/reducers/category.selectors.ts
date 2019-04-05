import { createFeatureSelector, createSelector } from '@ngrx/store';
import { CategoryState } from './category.state';
import { categoryStateKey } from './category.reducer';

export const selectCategoryState = createFeatureSelector<CategoryState>(categoryStateKey);

export const selectCategoryPublicModel = createSelector(
    selectCategoryState,
    state => state.categoryPublicModel
);
