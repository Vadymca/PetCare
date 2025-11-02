import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { SuccessStory } from '../models/successStory';
import { ApiService } from './api.service';

@Injectable({
  providedIn: 'root',
})
export class SuccessStoryService {
  private api = inject(ApiService);
  private endpoint = `success-stories`;

  getSuccessStories(): Observable<SuccessStory[]> {
    return this.api.get<SuccessStory[]>(this.endpoint);
  }

  getSuccessStoryById(id: string): Observable<SuccessStory | undefined> {
    return this.api.getById<SuccessStory>(this.endpoint, id);
  }
  getSuccessStoryBySlug(slug: string): Observable<SuccessStory | undefined> {
    return this.api.getBySlug<SuccessStory>(this.endpoint, slug);
  }

  createSuccessStory(
    succesStory: Partial<SuccessStory>
  ): Observable<SuccessStory> {
    return this.api.post<SuccessStory>(this.endpoint, succesStory);
  }
  updateSuccessStory(
    id: string,
    article: Partial<SuccessStory>
  ): Observable<SuccessStory> {
    return this.api.put<SuccessStory>(this.endpoint, id, article);
  }
  deleteSuccessStory(id: string): Observable<void> {
    return this.api.delete<void>(this.endpoint, id);
  }
}
