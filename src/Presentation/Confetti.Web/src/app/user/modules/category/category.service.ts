import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ApiResponse } from '@app/models/api_response';
import { CategoryPublicModel } from '@user/models/catalog/category_public_model';
import { environment } from '@env/environment';

@Injectable()
export class CategoryService {

  constructor(
    private http: HttpClient
  ) { }

  /**
   * Get public category by id
   */
  public getCategoryPublicModel(categoryId: number): Observable<ApiResponse<CategoryPublicModel>> {
    return this.http.get<ApiResponse<CategoryPublicModel>>(`${environment.apiUrl}/Category/${categoryId}`);
  }
}
