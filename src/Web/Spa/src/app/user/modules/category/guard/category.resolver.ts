import { Injectable } from '@angular/core';
import { Resolve, ActivatedRouteSnapshot, RouterStateSnapshot, Router } from '@angular/router';
import { Observable, throwError, of } from 'rxjs';
import { CategoryPublicModel } from '@user/models/catalog/category_public_model';
import { CategoryService } from '../category.service';
import { map, catchError } from 'rxjs/operators';

@Injectable()
export class CategoryResolveGuard implements Resolve<CategoryPublicModel> {
  constructor(
    private categoryService: CategoryService,
    private router: Router
  ) {}

  resolve(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ): Observable<CategoryPublicModel> {
    const categoryId = route.params['slug'];
    return this.categoryService.getCategoryPublicModel(categoryId).pipe(
      map(model => {
        if (!model.successful) {
          throwError(model.errors.join(','));
        }

        return model.response;
      }),
      catchError(_ => {
        this.router.navigate(['/']);
        return of(new CategoryPublicModel());
      })
    );
  }
}
