import { Injectable } from '@angular/core';
import { ApiResponse } from '@app/models/api_response';
import { CategoryHomeModel } from './models/home_model';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { environment } from '@env/environment';

@Injectable()
export class HomeService {
    constructor(
        private http: HttpClient
    ) {}
    /**
     * Get CategoryHomeModel
     */
    public getCategoryHomeModel(rootCategoryId: number): Observable<ApiResponse<CategoryHomeModel>> {
        return this.http.get<ApiResponse<CategoryHomeModel>>(`${environment.apiUrl}/Category/Home/${rootCategoryId}`);
    }
}
