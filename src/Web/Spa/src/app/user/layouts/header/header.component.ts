import { Component, OnInit, OnDestroy } from '@angular/core';
import { Store } from '@ngrx/store';
import { selectBreadcrumbPath } from '@shared/state/reducers/shared.selectors';
import { AppState } from '@app/state/reducers/app.state';
import { takeUntil } from 'rxjs/operators';
import { Observable, Subject } from 'rxjs';
import { CategorySimpleModel } from '@user/models/catalog/category_simple_model';
import { selectMenuCategories } from '../state/reducers/layouts.selectors';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss']
})
export class HeaderComponent implements OnInit, OnDestroy {
  //#region Fields

  private unsubscribed$ = new Subject();

  //#endregion

  //#region Properties

  public categories$: Observable<CategorySimpleModel[]>;
  public breadcrumbPath$: Observable<string[]>;

  //#endregion

  constructor(
    private store: Store<AppState>
  ) { }

  ngOnInit(): void {
    this.categories$ = this.getMenuCategories();
    this.breadcrumbPath$ = this.getBreadcrumbPath();
  }

  //#region Utilities

  private getBreadcrumbPath(): Observable<string[]> {
    return this.store.select(selectBreadcrumbPath).pipe(
      takeUntil(this.unsubscribed$)
    );
  }

  private getMenuCategories(): Observable<CategorySimpleModel[]> {
    return this.store.select(selectMenuCategories).pipe(
      takeUntil(this.unsubscribed$)
    );
  }

  //#endregion

  ngOnDestroy(): void {
    this.unsubscribed$.next();
    this.unsubscribed$.complete();
  }
}
