import { SharedState } from './shared.state';
import { SharedActionsTypes } from '../actions/shared.actions';

export const initialState: SharedState = {
    lastCookie: { key: null, value: null },
    breadcrumbPath: []
};

export function sharedReducer(
    state = initialState,
    action: { type: SharedActionsTypes, payload: any}
): SharedState {
    switch (action.type) {
        case SharedActionsTypes.GetCookieSuccess:
            return { ...state, lastCookie: action.payload };

        case SharedActionsTypes.SetBreadcrumbPath:
            return { ...state, breadcrumbPath: action.payload.path };

        default:
            return state;
    }
}
