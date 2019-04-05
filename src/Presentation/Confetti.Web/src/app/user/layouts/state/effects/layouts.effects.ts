import { Actions, Effect, ofType } from '@ngrx/effects';
import { Injectable } from '@angular/core';
import { mergeMap, map, catchError } from 'rxjs/operators';
import { of } from 'rxjs';

import { LayoutService } from '@user/layouts/layout.service';
import {
    LayoutsActionsTypes,
    LoadHomeModelSuccess,
    LoadHomeModelError
} from '../actions/layouts.actions';

@Injectable()
export class LayoutsEffects {
    constructor(
        private actions$: Actions,
        private layoutService: LayoutService
    ) {}

    @Effect()
    loadLayoutModel$ = this.actions$.pipe(
        ofType(LayoutsActionsTypes.LoadLayoutModel),
        mergeMap(() => this.layoutService.getLayoutModel().pipe(
            map(res => {
                if (!res.successful) {
                    throw new Error(res.errors.join(','));
                }

                return new LoadHomeModelSuccess({ layoutModel: res.response });
            }),
            catchError(err => of(new LoadHomeModelError({ errors: err })))
        ))
    );
}
