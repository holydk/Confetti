import { SharedState } from './shared.state';
import { SharedActionsTypes } from '../actions/shared.actions';

export const initialState: SharedState = {
  lastCookie: null,
  breadcrumbPath: null
};

export function sharedReducer(
  state = initialState,
  action: { type: SharedActionsTypes, payload: any}
): SharedState {
  switch (action.type) {
    case SharedActionsTypes.GetCookieSuccess:
      return { ...state, lastCookie: action.payload };

    case SharedActionsTypes.SetBreadcrumbPath:
      const path = action.payload.path as string[];
      if (path && path.length > 0) {
        if (state.breadcrumbPath !== null && path.length === state.breadcrumbPath.length) {
          console.log(`${state.breadcrumbPath} ${action.payload.path}`);
          const missingPath = path.filter(p => state.breadcrumbPath.indexOf(p) < 0);
          if (missingPath === null && missingPath.length === 0) {
            console.log(`Missing path ${missingPath}`);
            return state;
          }
        }

        return {
          ...state,
          breadcrumbPath: action.payload.path
        };
      }
      return state;

    default:
      return state;
  }
}
