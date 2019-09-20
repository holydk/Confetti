import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CategoryComponent } from './pages/category/category.component';
import { CategoryService } from './category.service';
import { CategoryRoutes } from './category.routing';
import { StoreModule } from '@ngrx/store';
import { EffectsModule } from '@ngrx/effects';
import { categoryReducer, categoryStateKey } from './state/reducers/category.reducer';
import { CategoryEffects } from './state/effects/category.effects';
import { CategoryResolveGuard } from './guard/category.resolver';
import { MaterialModule } from '@shared/material/material.module';

@NgModule({
  declarations: [CategoryComponent],
  imports: [
    CommonModule,
    CategoryRoutes,
    StoreModule.forFeature(categoryStateKey, categoryReducer),
    EffectsModule.forFeature([CategoryEffects]),
    MaterialModule
  ],
  providers: [
    CategoryService,
    CategoryResolveGuard
  ]
})
export class CategoryModule { }
