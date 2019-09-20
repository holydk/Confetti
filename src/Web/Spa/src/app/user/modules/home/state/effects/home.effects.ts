import { Actions, Effect, ofType } from '@ngrx/effects';
import { Injectable } from '@angular/core';
import { mergeMap, map, catchError } from 'rxjs/operators';
import { of } from 'rxjs';

import {
    HomeActionsTypes,
    LoadCategoryHomeModelSuccess,
    LoadCategoryHomeModelError,
    LoadCategoryHomeModel} from '../actions/home.actions';
import { HomeService } from '../../home.service';

@Injectable()
export class HomeEffects {
    constructor(
        private actions$: Actions,
        private homeService: HomeService
    ) {}

    @Effect()
    loadCategoryHomeModel$ = this.actions$.pipe(
        ofType(HomeActionsTypes.LoadCategoryHomeModel),
        mergeMap((model: LoadCategoryHomeModel) => {
            return this.homeService.getCategoryHomeModel(model.payload.rootCategoryId).pipe(
                map(res => {
                    if (!res.successful) {
                        throw new Error(res.errors.join(','));
                    }

                    return new LoadCategoryHomeModelSuccess({ homeModel: res.response });
                }),
                catchError(err => of(new LoadCategoryHomeModelError({ errors: err })))
            );
        })
    );
}
