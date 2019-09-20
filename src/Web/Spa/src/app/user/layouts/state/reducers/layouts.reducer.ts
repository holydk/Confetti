import { LayoutsState } from './layouts.state';
import { LayoutsActionsTypes } from '../actions/layouts.actions';

export const initialState: LayoutsState = {
  layoutModel: { headerModel: { topMenuModel: { categories: [] } } }
};

export function layoutsReducer(
    state = initialState,
    action: { type: LayoutsActionsTypes, payload: any }
): LayoutsState {
  console.log(action);
  switch (action.type) {
    case LayoutsActionsTypes.LoadLayoutModelSuccess:
      return {
        ...state,
        layoutModel: action.payload.layoutModel
      };

    default:
      return state;
  }
}
