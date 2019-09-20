import { Action } from '@ngrx/store';
import { CategoryHomeModel } from '../../models/home_model';

export enum HomeActionsTypes {
    LoadCategoryHomeModel = '[Home] Load HomeModel',
    LoadCategoryHomeModelSuccess = '[Home] Load HomeModel Success',
    LoadCategoryHomeModelError = '[Home] Load HomeModel Error'
}

export class LoadCategoryHomeModel implements Action {
    readonly type = HomeActionsTypes.LoadCategoryHomeModel;

    constructor(
        public payload: { rootCategoryId: number }
    ) {}
}

export class LoadCategoryHomeModelSuccess implements Action {
    readonly type = HomeActionsTypes.LoadCategoryHomeModelSuccess;

    constructor(
        public payload: { homeModel: CategoryHomeModel }
    ) {}
}

export class LoadCategoryHomeModelError implements Action {
    readonly type = HomeActionsTypes.LoadCategoryHomeModelError;

    constructor(
        public payload: { errors: string[] }
    ) {}
}
