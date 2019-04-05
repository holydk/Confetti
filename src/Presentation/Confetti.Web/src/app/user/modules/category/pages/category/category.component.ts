import { Component, OnInit, OnDestroy } from '@angular/core';
import { Subject } from 'rxjs';
import { Store } from '@ngrx/store';
import { AppState } from '@app/state/reducers/app.state';
import { MetaService } from '@ngx-meta/core';
import { ActivatedRoute } from '@angular/router';
import { takeUntil, map } from 'rxjs/operators';
import { LoadCategoryPublicModel } from '../../state/actions/category.actions';
import { selectCategoryPublicModel } from '../../state/reducers/category.selectors';
import { SetBreadcrumbPath } from '@shared/state/actions/shared.actions';
import { CategoryPublicModel } from '@user/models/catalog/category_public_model';

@Component({
  selector: 'app-category',
  templateUrl: './category.component.html',
  styleUrls: ['./category.component.scss']
})
export class CategoryComponent implements OnInit, OnDestroy {
  private unsubscribed$ = new Subject();

  constructor(
    private store: Store<AppState>,
    private route: ActivatedRoute,
    private meta: MetaService
  ) { }

  ngOnInit(): void {
    this.route.params.pipe(
      takeUntil(this.unsubscribed$),
      map(params => params['slug'])
    ).subscribe(value => {
        const id = value as number;
        if (id) {
          this.loadCategoryPublicModel(id);
        }
    });

    this.store.select(selectCategoryPublicModel).pipe(
      takeUntil(this.unsubscribed$)
    ).subscribe(model => {
        if (model) {
          this.setMetaTags(model);
          this.setBreadcrumb(model);
        }
        console.log(model);
    });
  }

  private setMetaTags(model: CategoryPublicModel) {
    if (model.metaTitle) {
      this.meta.setTitle(model.metaTitle);
    }
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

  private loadCategoryPublicModel(categoryId: number) {
    this.store.dispatch(
      new LoadCategoryPublicModel(
      {
        categoryId: categoryId
      })
    );
  }

  ngOnDestroy(): void {
    this.unsubscribed$.next();
    this.unsubscribed$.complete();
  }
}
