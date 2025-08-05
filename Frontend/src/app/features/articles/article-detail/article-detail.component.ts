import { Component, effect, inject, signal } from '@angular/core';

import { CommonModule } from '@angular/common';
import { toSignal } from '@angular/core/rxjs-interop';
import { Meta, SafeResourceUrl, Title } from '@angular/platform-browser';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { TranslateModule, TranslateService } from '@ngx-translate/core';
import { filter, switchMap } from 'rxjs/operators';

import { Article } from '../../../core/models/article';
import { ArticleService } from '../../../core/services/article.service';

@Component({
  selector: 'app-article-detail',
  standalone: true,
  imports: [CommonModule, RouterModule, TranslateModule],
  templateUrl: './article-detail.component.html',
  styleUrl: './article-detail.component.css',
})
export class ArticleDetailComponent {
  private route = inject(ActivatedRoute);
  public router = inject(Router);
  private title = inject(Title);
  private meta = inject(Meta);
  private translate = inject(TranslateService);

  // private authService = inject(AuthService);
  private articleService = inject(ArticleService);

  mapUrl = signal<SafeResourceUrl | null>(null); // оголошено з дефолтним значенням

  slug = toSignal(
    this.route.paramMap.pipe(
      switchMap(params => [params.get('slug')]),
      filter((slug): slug is string => slug !== null && slug !== undefined)
    )
  );

  article = signal<Article | undefined>(undefined);
  //public isAuthenticated: Signal<boolean> = this.authService.isLoggedIn;
  //user: Signal<User | null> = signal(this.authService.currentUser());

  constructor() {
    effect(() => {
      const slugValue = this.slug();
      if (!slugValue) return;

      this.articleService.getArticleBySlug(slugValue).subscribe(article => {
        if (!article) {
          this.router.navigate(['/not-found']);
          return;
        }

        this.article.set(article);
        this.addJsonLd(article);
        this.title.setTitle(article.title);
        this.meta.updateTag({
          name: 'description',
          content: article.content?.slice(0, 150),
        });
        this.meta.updateTag({ property: 'og:title', content: article.title });
        this.meta.updateTag({
          property: 'og:description',
          content: article.content,
        });
        this.meta.updateTag({ property: 'og:url', content: this.router.url });
        this.meta.updateTag({ property: 'og:type', content: 'article' });
        this.meta.updateTag({
          name: 'twitter:card',
          content: 'summary_large_image',
        });
        this.meta.updateTag({ name: 'twitter:title', content: article.title });
        this.meta.updateTag({
          name: 'twitter:description',
          content: article.content,
        });

        console.log('this.article', this.article());
      });
    });
  }
  addJsonLd(article: Article) {
    const script = document.createElement('script');
    script.type = 'application/ld+json';
    script.text = JSON.stringify({
      '@context': 'https://schema.org',
      '@type': 'Pet',
      headline: article.title,
      description: article.content?.slice(0, 150),
      datePublished: article.createdAt,
      author: {
        '@type': 'Person',
        name:
          article.author?.firstName + ' ' + article.author?.lastName || 'Admin',
      },
    });
    document.head.appendChild(script);
  }
}
