import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { environment } from '@env/environment';
import { LayoutModel } from '@app/user/models/common/layout_model';
import { ApiResponse } from '@app/models/api_response';

@Injectable({
  providedIn: 'root'
})
export class LayoutService {

  constructor(
    private http: HttpClient
  ) { }

  /**
   * Get layout model
   */
  public getLayoutModel(): Observable<ApiResponse<LayoutModel>> {
    return this.http.get<ApiResponse<LayoutModel>>(`${environment.apiUrl}/home`);
  }
}
