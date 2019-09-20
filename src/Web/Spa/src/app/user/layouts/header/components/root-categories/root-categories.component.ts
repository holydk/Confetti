import { Component, OnInit, OnDestroy, Input } from '@angular/core';
import { Subject } from 'rxjs';
import { CategorySimpleModel } from '@user/models/catalog/category_simple_model';

@Component({
  selector: 'app-root-categories',
  templateUrl: './root-categories.component.html',
  styleUrls: ['./root-categories.component.scss']
})
export class RootCategoriesComponent implements OnInit, OnDestroy {
  //#region Fields

  private unsubscribed$ = new Subject();

  //#endregion

  //#region Properties

  @Input()
  public categories: CategorySimpleModel[];
  @Input()
  public breadcrumbPath: string[];

  //#endregion

  constructor() { }

  ngOnInit(): void {
  }

  ngOnDestroy(): void {
    this.unsubscribed$.next();
    this.unsubscribed$.complete();
  }
}
