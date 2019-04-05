import { HomeState } from './home.state';
import { HomeActionsTypes } from '../actions/home.actions';

export const homeStateKey = 'user-home-state';

export const initialState: HomeState = {
    categoryHomeModel: {
        title: '',
        description: '',
        metaTitle: '',
        metaDescription: '',
        metaKeywords: '',
        categories: []
    }
};

export function homeReducer(
    state = initialState,
    action: { type: HomeActionsTypes, payload: any }
): HomeState {
    switch (action.type) {
        case HomeActionsTypes.LoadCategoryHomeModelSuccess:
            return { ...state, categoryHomeModel: action.payload.homeModel };

        default:
            return state;
    }
}
