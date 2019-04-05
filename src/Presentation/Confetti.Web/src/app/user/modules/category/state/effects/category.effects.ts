import { Actions, Effect, ofType } from '@ngrx/effects';
import { Injectable } from '@angular/core';
import { mergeMap, map, catchError } from 'rxjs/operators';
import { of } from 'rxjs';

import { CategoryService } from '../../category.service';
import {
    CategoryActionsTypes,
    LoadCategoryPublicModel,
    LoadCategoryPublicModelSuccess,
    LoadCategoryPublicModelError
} from '../actions/category.actions';

@Injectable()
export class CategoryEffects {
    constructor(
        private actions$: Actions,
        private categoryService: CategoryService
    ) {}

    @Effect()
    loadCategoryPublicModel$ = this.actions$.pipe(
        ofType(CategoryActionsTypes.LoadCategoryPublicModel),
        mergeMap((model: LoadCategoryPublicModel) => {
            return this.categoryService.getCategoryPublicModel(model.payload.categoryId).pipe(
                map(res => {
                    if (!res.successful) {
                        throw new Error(res.errors.join(','));
                    }

                    return new LoadCategoryPublicModelSuccess({ categoryPublicModel: res.response });
                }),
                catchError(err => {
                    return of(new LoadCategoryPublicModelError({ errors: err }));
                })
            );
        })
    );
}
