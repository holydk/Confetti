import { Component, OnInit, OnDestroy } from '@angular/core';
import { Store } from '@ngrx/store';
import { ActivatedRoute, Router } from '@angular/router';
import { MetaService } from '@ngx-meta/core';

import { CategorySimpleModel } from '@app/user/models/catalog/category_simple_model';
import { AppState } from '@app/state/reducers/app.state';
import { Subject } from 'rxjs';
import { takeUntil, map } from 'rxjs/operators';
import { LoadCategoryHomeModel } from '../state/actions/home.actions';
import { selectCategoryHomeModel } from '../state/reducers/home.selectors';
import { CategoryHomeModel } from '../models/home_model';
import { SetBreadcrumbPath } from '@shared/state/actions/shared.actions';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html'
})
export class HomeComponent implements OnInit, OnDestroy {
  private unsubscribed$ = new Subject();

  constructor(
    private store: Store<AppState>,
    private route: ActivatedRoute,
    private meta: MetaService
  ) {}

  ngOnInit(): void {
    this.route.params.pipe(
      takeUntil(this.unsubscribed$),
      map(params => params['slug'])
    ).subscribe(value => {
      const id = value as number;
      if (id) {
        this.loadCategoryHomeModel(id);
      }
    });

    this.store.select(selectCategoryHomeModel).pipe(
      takeUntil(this.unsubscribed$)
    ).subscribe(model => {
      if (model) {
        this.setMetaTags(model);
      }
    });
  }

  private loadCategoryHomeModel(id: number): any {
    this.store.dispatch(
      new LoadCategoryHomeModel(
        {
          rootCategoryId: id
        }
      )
    );
  }

  private setMetaTags(model: CategoryHomeModel) {
    if (model.metaTitle) {
      this.meta.setTitle(model.metaTitle);
    }
  }

  private setBreadcrumb(model: CategoryHomeModel) {
    if (model.title) {
      this.store.dispatch(
        new SetBreadcrumbPath(
          {
            path: [ model.title ]
          }
        )
      );
    }
  }

  ngOnDestroy(): void {
    this.unsubscribed$.next();
    this.unsubscribed$.complete();
  }
}

