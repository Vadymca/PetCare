import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { Category } from '../models/category';
import { ApiService } from './api.service';

@Injectable({
  providedIn: 'root',
})
export class CategoryService {
  private api = inject(ApiService);
  private endpoint = `categories`;

  getCategories(): Observable<Category[]> {
    return this.api.get<Category[]>(this.endpoint);
  }
  getCategoryById(id: string): Observable<Category> {
    return this.api.getById<Category>(this.endpoint, id);
  }
  createCategory(category: Partial<Category>): Observable<Category> {
    return this.api.post<Category>(this.endpoint, category);
  }
  updateCategory(
    id: string,
    category: Partial<Category>
  ): Observable<Category> {
    return this.api.put<Category>(this.endpoint, id, category);
  }
  deleteCategory(id: string): Observable<void> {
    return this.api.delete<void>(this.endpoint, id);
  }
}
