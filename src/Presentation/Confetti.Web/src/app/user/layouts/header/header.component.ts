import { Component, OnInit, Input } from '@angular/core';
import { LayoutModel } from '@app/user/models/common/layout_model';
import { Store } from '@ngrx/store';
import { getBreadcrumbPath } from '@shared/state/reducers/shared.selectors';
import { AppState } from '@app/state/reducers/app.state';
import { CategorySimpleModel } from '@user/models/catalog/category_simple_model';
import { HeaderModel } from '@user/models/common/header_model';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss']
})
export class HeaderComponent implements OnInit {
  public selectedCategoryId = 0;

  @Input()
  public headerModel: HeaderModel;

  constructor(
    private store: Store<AppState>
  ) { }

  ngOnInit(): void {
    this.store.select(getBreadcrumbPath).subscribe(path => {
      console.log('Current path');
      console.log(path);

      if (this.headerModel && path.length > 0) {
        const categories = this.headerModel.topMenuModel.categories;
        if (categories.length > 0) {
          const category = categories.find(c => c.title === path[0]);
          if (category) {
            this.selectedCategoryId = categories.indexOf(category);
            console.log(this.selectedCategoryId);
          }
        }
      }
    });
    // this.store.select(getTopCategories).pipe(map(categories => {
    //   return this.store.select(getBreadcrumbPath);
    // })).subscribe(path$ => path$);

    // this.store.select(getBreadcrumbPath).subscribe(path => {
    //   console.log('getBreadcrumbPath');
    //   console.log(path);
    //   const categories = this.layoutModel.topMenuModel.categories;
    //   if (categories && categories.length > 0) {
    //     const category = categories.find(c => c.title === path[0]);
    //     if (category) {
    //       this.selectedCategoryId = categories.indexOf(category);
    //       console.log(category);
    //     }
    //   }
    // });
  }
}
