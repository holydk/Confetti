import { Component, OnInit, OnDestroy } from '@angular/core';
import { Store } from '@ngrx/store';
import { Router, ActivatedRoute, NavigationEnd } from '@angular/router';

import { LayoutModel } from '@app/user/models/common/layout_model';
import { LoadLayoutModel } from '../../state/actions/layouts.actions';
import { selectLayoutModel } from '../../state/reducers/layouts.selectors';
import { AppState } from '@app/state/reducers/app.state';
import { Subject, Observable } from 'rxjs';
import { filter, takeUntil, tap } from 'rxjs/operators';

@Component({
  selector: 'app-wrapper',
  templateUrl: './wrapper.component.html',
})
export class WrapperComponent implements OnInit, OnDestroy {
  private unsubscribed$ = new Subject();

  public layoutModel: LayoutModel;

  constructor(
    private store: Store<AppState>,
    private router: Router,
    private route: ActivatedRoute,
  ) {}

  ngOnInit(): void {
    this.store.dispatch(new LoadLayoutModel());
    this.getLayoutModel().subscribe(model => {
      this.layoutModel = model;

      // redirect to first root category
      const categories = model.headerModel.topMenuModel.categories;
      if (this.router.url === '/' && categories.length > 0) {
        this.router.navigate(['/', categories[0].route]);
      }
    });
  }

  private getLayoutModel(): Observable<LayoutModel> {
    return this.store.select(selectLayoutModel).pipe(
      takeUntil(this.unsubscribed$)
    );
  }

  ngOnDestroy(): void {
    this.unsubscribed$.next();
    this.unsubscribed$.complete();
  }
}
