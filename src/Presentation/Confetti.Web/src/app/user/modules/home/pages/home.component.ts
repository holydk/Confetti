import { Component, OnInit, OnDestroy, ChangeDetectionStrategy, ChangeDetectorRef } from '@angular/core';
import { Store } from '@ngrx/store';
import { ActivatedRoute } from '@angular/router';
import { MetaService } from '@ngx-meta/core';

import { AppState } from '@app/state/reducers/app.state';
import { Subject, Observable, interval, of } from 'rxjs';
import { takeUntil, map, filter, tap, switchMap, startWith, take } from 'rxjs/operators';
import { LoadCategoryHomeModel, LoadCategoryHomeModelSuccess } from '../state/actions/home.actions';
import { selectCategoryHomeModel } from '../state/reducers/home.selectors';
import { CategoryHomeModel } from '../models/home_model';
import { SetBreadcrumbPath } from '@shared/state/actions/shared.actions';
import { NguCarouselConfig } from '@ngu/carousel';
import { defaultCarouselTileConfig } from '@shared/carousel/defaultCarouselTileConfig';
import { defaultSlider } from '@shared/carousel/default-slide.animation';

export interface Tile {
  color: string;
  cols: number;
  rows: number;
  text: string;
}

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss'],
  animations: [defaultSlider],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class HomeComponent implements OnInit, OnDestroy {
  //#region Fields

  private unsubscribed$ = new Subject();

  //#endregion

  //#region Properties

  public category$: Observable<CategoryHomeModel>;
  public tiles: Tile[] = [
    {text: '', cols: 3, rows: 2, color: 'lightblue'},
    {text: '', cols: 2, rows: 1, color: 'lightgreen'},
    {text: '', cols: 2, rows: 1, color: 'lightpink'}
  ];
  public tiles2: Tile[] = [
    {text: '', cols: 2, rows: 1, color: 'lightpink'},
    {text: '', cols: 3, rows: 2, color: 'lightgreen'},
    {text: '', cols: 2, rows: 1, color: 'lightpink'}
  ];

  public carouselTileItems$: Observable<number[]>;
  public carouselTileConfig: NguCarouselConfig = defaultCarouselTileConfig;
  tempData: any[];

  //#endregion

  constructor(
    private store: Store<AppState>,
    private route: ActivatedRoute,
    private meta: MetaService,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    this.loadCategoryHomeModel();
    this.category$ = this.getCategoryHomeModel();
    this.tempData = [];
    this.carouselTileItems$ = of([1, 2, 3, 4, 5, 6, 7, 8, 9, 10]);
  }

  //#region Utilities

  private loadCategoryHomeModel(): void {
    this.route.params.pipe(
      takeUntil(this.unsubscribed$),
      map(params => params['slug']),
      map(slug => Number(slug)),
      filter(slug => slug !== null && !isNaN(slug)),
      tap(slug => {
        console.log('TEST');
        // todo: set setBreadcrumb by slug
        this.store.dispatch(
          new LoadCategoryHomeModel(
            {
              rootCategoryId: slug
            }
          )
        );
      })
    ).subscribe();
  }

  private getCategoryHomeModel(): Observable<CategoryHomeModel> {
    return this.store.select(selectCategoryHomeModel).pipe(
      takeUntil(this.unsubscribed$),
      filter(category => category !== null),
      tap(category => {
        console.log(category);
        this.setBreadcrumb(category);
        this.setMetaTags(category);
      })
    );
  }

  private setBreadcrumb(model: CategoryHomeModel) {
    if (model) {
      this.store.dispatch(
        new SetBreadcrumbPath(
          {
            path: [ model.title ]
          }
        )
      );
    }
  }

  private setMetaTags(model: CategoryHomeModel) {
    if (model) {
      this.meta.setTitle(model.metaTitle);
    }
  }

  //#region

  ngOnDestroy(): void {
    this.unsubscribed$.next();
    this.unsubscribed$.complete();
    this.store.dispatch(
      new LoadCategoryHomeModelSuccess(
      {
        homeModel: null
      })
    );
  }
}

