import { Component, OnInit, OnDestroy, Input } from '@angular/core';
import { Subject, Observable } from 'rxjs';
import { Store } from '@ngrx/store';
import { AppState } from '@app/state/reducers/app.state';
import { selectBreadcrumbPath } from '@shared/state/reducers/shared.selectors';
import { takeUntil, filter, tap } from 'rxjs/operators';
import { CategorySimpleModel } from '@user/models/catalog/category_simple_model';

@Component({
  selector: 'app-categories-dropdown',
  templateUrl: './categories-dropdown.component.html',
  styleUrls: ['./categories-dropdown.component.scss']
})
export class CategoriesDropdownComponent implements OnInit, OnDestroy {
  //#region Fields

  private unsubscribed$ = new Subject();

  //#endregion

  //#region Properties

  @Input()
  public categories: CategorySimpleModel[];
  public childCategories: CategorySimpleModel[];

  //#endregion

  constructor(
    private store: Store<AppState>
  ) { }

  ngOnInit(): void {
    this.getBreadcrumbPath().subscribe();
  }

  //#region Utilities

  private getBreadcrumbPath(): Observable<string[]> {
    return this.store.select(selectBreadcrumbPath).pipe(
      takeUntil(this.unsubscribed$),
      filter(path => path !== null && path !== undefined && path.length > 0),
      tap(path => {
        this.setChildCategories(path[0]);
      })
    );
  }

  private setChildCategories(rootCategoryName: string): void {
    console.log(`setChildCategories ${rootCategoryName}`);
    if (rootCategoryName && this.categories) {
      const category = this.categories.find(c => c.title === rootCategoryName);
      if (category) {
        this.childCategories = category.subCategories;
      }
    }
  }

  //#endregion

  ngOnDestroy(): void {
    this.unsubscribed$.next();
    this.unsubscribed$.complete();
  }
}
