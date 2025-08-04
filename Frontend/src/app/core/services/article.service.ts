import { inject, Injectable } from '@angular/core';
import { forkJoin, map, Observable, of, switchMap } from 'rxjs';
import { Article } from '../models/article';
import { ApiService } from './api.service';
import { CategoryService } from './category.service';
import { UserService } from './user.service';

@Injectable({
  providedIn: 'root',
})
export class ArticleService {
  private api = inject(ApiService);
  private endpoint = `articles`;
  private categoryService = inject(CategoryService);
  private userService = inject(UserService);

  getArticles(): Observable<Article[]> {
    return this.api.get(this.endpoint);
  }

  getArticleById(id: string): Observable<Article | undefined> {
    return this.api.getById<Article>(this.endpoint, id).pipe(
      switchMap(article => {
        if (!article) return of(undefined);

        const category$ = article.categoryId
          ? this.categoryService.getCategoryById(article.categoryId)
          : of(undefined);

        const author$ = article.authorId
          ? this.userService.getUserById(article.authorId)
          : of(undefined);

        return forkJoin({ category: category$, author: author$ }).pipe(
          map(({ category, author }) => {
            if (!category || !author) return undefined;

            return {
              ...article,
              category,
              author,
            } as Article;
          })
        );
      })
    );
  }
  getArticleBySlug(slug: string): Observable<Article | undefined> {
    return this.api
      .getBySlug<Article>(this.endpoint, slug)
      .pipe(map(articles => articles[0]))
      .pipe(
        switchMap(article => {
          if (!article) return of(undefined);

          const category$ = article.categoryId
            ? this.categoryService.getCategoryById(article.categoryId)
            : of(undefined);

          const author$ = article.authorId
            ? this.userService.getUserById(article.authorId)
            : of(undefined);

          return forkJoin({ category: category$, author: author$ }).pipe(
            map(({ category, author }) => {
              if (!category || !author) return undefined;

              return {
                ...article,
                category,
                author,
              } as Article;
            })
          );
        })
      );
  }
  getArticlesByAuthorId(authorId: string): Observable<Article[]> {
    return this.getArticles().pipe(
      map(articles => articles.filter(article => article.authorId === authorId))
    );
  }
  createArticle(article: Partial<Article>): Observable<Article> {
    return this.api.post<Article>(this.endpoint, article);
  }
  updateArticle(id: string, article: Partial<Article>): Observable<Article> {
    return this.api.put<Article>(this.endpoint, id, article);
  }
  deleteArticle(id: string): Observable<void> {
    return this.api.delete<void>(this.endpoint, id);
  }
}
