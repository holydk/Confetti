import { CategoryState } from './category.state';
import { CategoryActionsTypes } from '../actions/category.actions';

export const categoryStateKey = 'user-category-state';

export const initialState: CategoryState = {
  categoryPublicModel: null
};

export function categoryReducer(
  state = initialState,
  action: { type: CategoryActionsTypes, payload: any }
): CategoryState {
  switch (action.type) {
    case CategoryActionsTypes.LoadCategoryPublicModelSuccess:
      return {
        ...state,
        categoryPublicModel: action.payload.categoryPublicModel
      };

    default:
      return state;
  }
}
