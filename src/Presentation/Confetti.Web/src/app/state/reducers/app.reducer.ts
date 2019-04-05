import * as fromShared from '@shared/state/reducers/shared.reducer';
import { ActionReducerMap } from '@ngrx/store';
import { AppState } from './app.state';

export const appReducers: ActionReducerMap<AppState> = {
    shared: fromShared.sharedReducer
};
