import { CommonModule, isPlatformBrowser } from '@angular/common';
import {
  Component,
  DestroyRef,
  effect,
  inject,
  PLATFORM_ID,
  Renderer2,
  RendererFactory2,
  signal,
} from '@angular/core';
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
  private articleService = inject(ArticleService);
  private platformId = inject(PLATFORM_ID);
  private renderer: Renderer2 = inject(RendererFactory2).createRenderer(
    null,
    null
  );
  private destroyRef = inject(DestroyRef);

  mapUrl = signal<SafeResourceUrl | null>(null);

  slug = toSignal(
    this.route.paramMap.pipe(
      switchMap(params => [params.get('slug')]),
      filter((slug): slug is string => slug !== null && slug !== undefined)
    )
  );

  article = signal<Article | undefined>(undefined);

  constructor() {
    effect(() => {
      const slugValue = this.slug();
      if (!slugValue) return;

      const subscription = this.articleService
        .getArticleBySlug(slugValue)
        .subscribe(article => {
          if (!article) {
            this.router.navigate(['/not-found']);
            return;
          }

          this.article.set(article);
          this.title.setTitle(article.title);
          this.meta.updateTag({
            name: 'description',
            content: article.content?.slice(0, 150) || '',
          });
          this.meta.updateTag({ property: 'og:title', content: article.title });
          this.meta.updateTag({
            property: 'og:description',
            content: article.content || '',
          });
          this.meta.updateTag({ property: 'og:url', content: this.router.url });
          this.meta.updateTag({ property: 'og:type', content: 'article' });
          this.meta.updateTag({
            name: 'twitter:card',
            content: 'summary_large_image',
          });
          this.meta.updateTag({
            name: 'twitter:title',
            content: article.title,
          });
          this.meta.updateTag({
            name: 'twitter:description',
            content: article.content || '',
          });

          // Add JSON-LD only in the browser
          if (isPlatformBrowser(this.platformId)) {
            this.addJsonLd(article);
          }
        });

      // Clean up subscription on component destruction
      this.destroyRef.onDestroy(() => {
        subscription.unsubscribe();
      });
    });
  }

  private addJsonLd(article: Article) {
    const script = this.renderer.createElement('script');
    this.renderer.setAttribute(script, 'type', 'application/ld+json');
    const jsonLd = {
      '@context': 'https://schema.org',
      '@type': 'Article', // Changed from 'Pet' to 'Article' for correct schema
      headline: article.title,
      description: article.content?.slice(0, 150) || '',
      datePublished: article.createdAt,
      author: {
        '@type': 'Person',
        name: article.author?.firstName
          ? `${article.author.firstName} ${article.author.lastName}`
          : 'Admin',
      },
    };
    this.renderer.setProperty(script, 'text', JSON.stringify(jsonLd));
    this.renderer.appendChild(document.head, script);
  }
}
