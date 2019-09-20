import { Component, OnInit, OnDestroy, ChangeDetectionStrategy } from '@angular/core';
import { Subject, Observable } from 'rxjs';
import { Store } from '@ngrx/store';
import { AppState } from '@app/state/reducers/app.state';
import { MetaService } from '@ngx-meta/core';
import { ActivatedRoute, Router } from '@angular/router';
import { takeUntil, map, filter, tap, switchMap } from 'rxjs/operators';
import { LoadCategoryPublicModel, LoadCategoryPublicModelSuccess } from '../../state/actions/category.actions';
import { selectCategoryPublicModel } from '../../state/reducers/category.selectors';
import { SetBreadcrumbPath } from '@shared/state/actions/shared.actions';
import { CategoryPublicModel } from '@user/models/catalog/category_public_model';
import { selectBreadcrumbPath } from '@shared/state/reducers/shared.selectors';

export interface Food {
  value: string;
  viewValue: string;
}

@Component({
  selector: 'app-category',
  templateUrl: './category.component.html',
  styleUrls: ['./category.component.scss']
})
export class CategoryComponent implements OnInit, OnDestroy {
  //#region Fields

  private unsubscribed$ = new Subject();

  //#endregion

  //#region Properties

  public category$: Observable<CategoryPublicModel>;
  public breadcrumbPath$: Observable<string[]>;
  public foods: Food[] = [
    {value: 'steak-0', viewValue: 'Steak'},
    {value: 'pizza-1', viewValue: 'Pizza'},
    {value: 'tacos-2', viewValue: 'Tacos'}
  ];
  public tiles: any = [
    {text: '', cols: 1, rows: 1},
    {text: '', cols: 1, rows: 1},
    {text: '', cols: 1, rows: 1},
    {text: '', cols: 1, rows: 1},
    {text: '', cols: 1, rows: 1},
    {text: '', cols: 1, rows: 1},
    {text: '', cols: 1, rows: 1},
    {text: '', cols: 1, rows: 1},
    {text: '', cols: 1, rows: 1},
    {text: '', cols: 1, rows: 1},
    {text: '', cols: 1, rows: 1},
    {text: '', cols: 1, rows: 1}
  ];

  //#endregion

  constructor(
    private store: Store<AppState>,
    private route: ActivatedRoute,
    private meta: MetaService,
    private router: Router
  ) { }

  ngOnInit(): void {
    this.loadCategoryPublicModel();
    this.category$ = this.getCategoryPublicModel();
    this.breadcrumbPath$ = this.getBreadcrumbPath();
    // this.category$ = this.route.data.pipe(
    //   takeUntil(this.unsubscribed$),
    //   map(({ category }) => {
    //     console.log(category);
    //     return category;
    //   }),
    //   filter(category => category !== null),
    //   tap(category => {
    //     console.log(category);
    //     this.setBreadcrumb(category);
    //     this.setMetaTags(category);
    //   })
    // );
  }

  //#region Utilities

  private getBreadcrumbPath(): Observable<string[]> {
    return this.store.select(selectBreadcrumbPath).pipe(
      takeUntil(this.unsubscribed$)
    );
  }

  private loadCategoryPublicModel(): void {
    this.route.params.pipe(
      takeUntil(this.unsubscribed$),
      map(params => params['slug']),
      map(slug => Number(slug)),
      filter(slug => slug !== null && !isNaN(slug)),
      tap(slug => {
        console.log('TEST');
        // todo: set setBreadcrumb by slug
        this.store.dispatch(
          new LoadCategoryPublicModel(
          {
            categoryId: slug
          })
        );
      })
    ).subscribe();
  }

  private getCategoryPublicModel(): Observable<CategoryPublicModel> {
    return this.store.select(selectCategoryPublicModel).pipe(
      takeUntil(this.unsubscribed$),
      filter(category => category !== null),
      tap(category => {
        console.log(category);
        this.setBreadcrumb(category);
        this.setMetaTags(category);
      })
    );
  }

  private setBreadcrumb(model: CategoryPublicModel) {
    if (model.categoryBreadcrumb && model.categoryBreadcrumb.length > 0) {
      this.store.dispatch(
        new SetBreadcrumbPath(
          {
            path: model.categoryBreadcrumb.map(c => c.title)
          }
        )
      );
    }
  }

  private setMetaTags(model: CategoryPublicModel) {
    if (model.metaTitle) {
      this.meta.setTitle(model.metaTitle);
    }
  }

  //#region

  ngOnDestroy(): void {
    this.unsubscribed$.next();
    this.unsubscribed$.complete();
    this.store.dispatch(
      new LoadCategoryPublicModelSuccess(
      {
        categoryPublicModel: null
      })
    );
  }
}
