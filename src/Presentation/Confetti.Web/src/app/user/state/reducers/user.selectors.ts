import { createFeatureSelector } from '@ngrx/store';
import { UserState } from '@user/state/reducers/user.state';
import { userStateKey } from './user.reducer';

export const selectUserState = createFeatureSelector<UserState>(userStateKey);
