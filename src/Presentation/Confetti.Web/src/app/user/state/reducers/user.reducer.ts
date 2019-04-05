import { ActionReducerMap } from '@ngrx/store';
import { UserState } from './user.state';
import { layoutsReducer } from '@user/layouts/state/reducers/layouts.reducer';

export const userStateKey = 'user-state';

export const userReducers: ActionReducerMap<UserState> = {
    layouts: layoutsReducer
};
