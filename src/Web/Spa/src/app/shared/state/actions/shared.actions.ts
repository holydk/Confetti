import { Action } from '@ngrx/store';

export enum SharedActionsTypes {
    SetCookie = '[App] Set cookie',
    GetCookieSuccess = '[App] Get cookie success',

    SetBreadcrumbPath = '[App] Set breadcrumb path'
}

export class SetCookie implements Action {
    readonly type = SharedActionsTypes.SetCookie;

    constructor(
        public payload: { key: string, value: string }
    ) {}
}

export class GetCookieSuccess implements Action {
    readonly type = SharedActionsTypes.GetCookieSuccess;

    constructor(
        public payload: { key: string, value: string }
    ) {}
}

export class SetBreadcrumbPath implements Action {
    readonly type = SharedActionsTypes.SetBreadcrumbPath;

    constructor(
        public payload: { path: string[] }
    ) {}
}
