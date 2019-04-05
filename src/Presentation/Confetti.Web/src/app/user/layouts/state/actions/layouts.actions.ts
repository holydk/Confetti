import { Action } from '@ngrx/store';
import { LayoutModel } from '@app/user/models/common/layout_model';

export enum LayoutsActionsTypes {
    LoadLayoutModel = '[Layouts] Load LayoutModel',
    LoadLayoutModelSuccess = '[Layouts] Load LayoutModel Success',
    LoadLayoutModelError = '[Layouts] Load HomeModel Error'
}

export class LoadLayoutModel implements Action {
    readonly type = LayoutsActionsTypes.LoadLayoutModel;
}

export class LoadHomeModelSuccess implements Action {
    readonly type = LayoutsActionsTypes.LoadLayoutModelSuccess;

    constructor(
        public payload: { layoutModel: LayoutModel }
    ) {}
}

export class LoadHomeModelError implements Action {
    readonly type = LayoutsActionsTypes.LoadLayoutModelError;

    constructor(
        public payload: { errors: string[] }
    ) {}
}
