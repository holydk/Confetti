import { Action } from '@ngrx/store';
import { CategoryPublicModel } from '@user/models/catalog/category_public_model';

export enum CategoryActionsTypes {
    LoadCategoryPublicModel = '[Public] Load PublicModel',
    LoadCategoryPublicModelSuccess = '[Public] Load PublicModel Success',
    LoadCategoryPublicModelError = '[Public] Load PublicModel Error'
}

export class LoadCategoryPublicModel implements Action {
    readonly type = CategoryActionsTypes.LoadCategoryPublicModel;

    constructor(
        public payload: { categoryId: number }
    ) {}
}

export class LoadCategoryPublicModelSuccess implements Action {
    readonly type = CategoryActionsTypes.LoadCategoryPublicModelSuccess;

    constructor(
        public payload: { categoryPublicModel: CategoryPublicModel }
    ) {}
}

export class LoadCategoryPublicModelError implements Action {
    readonly type = CategoryActionsTypes.LoadCategoryPublicModelError;

    constructor(
        public payload: { errors: string[] }
    ) {}
}
