import * as fromShared from '@shared/state/reducers/shared.reducer';
import * as fromIdentity from '@app/core/identity/state/reducers/identity.reducer';
import { ActionReducerMap } from '@ngrx/store';
import { AppState } from './app.state';

export const appReducers: ActionReducerMap<AppState> = {
    shared: fromShared.sharedReducer,
    identity: fromIdentity.identityReducer
};
