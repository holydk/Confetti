import { Actions, Effect, ofType } from '@ngrx/effects';
import { Injectable } from '@angular/core';
import { map } from 'rxjs/operators';

import { UniversalStorage } from '@shared/storage/universal.storage';
import { SharedActionsTypes, SetCookie, GetCookieSuccess } from '../actions/shared.actions';

@Injectable()
export class SharedEffects {
    constructor(
        private actions$: Actions,
        private storage: UniversalStorage
    ) {}

    @Effect()
    setCookie$ = this.actions$.pipe(
        ofType(SharedActionsTypes.SetCookie),
        map((action: SetCookie) => {
            const key = action.payload.key;
            const value = action.payload.value;
            this.storage.setItem(key, value);

            return new GetCookieSuccess({ key, value });
        })
    );
}
