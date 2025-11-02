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
import { filter, switchMap } from 'rxjs';
import { SuccessStory } from '../../../core/models/successStory';
import { SuccessStoryService } from '../../../core/services/success-story.service';

@Component({
  selector: 'app-succes-story-detail',
  standalone: true,
  imports: [TranslateModule, CommonModule, RouterModule],
  templateUrl: './succes-story-detail.component.html',
  styleUrl: './succes-story-detail.component.css',
})
export class SuccesStoryDetailComponent {
  private route = inject(ActivatedRoute);
  public router = inject(Router);
  private title = inject(Title);
  private meta = inject(Meta);
  private translate = inject(TranslateService);
  private successStoryService = inject(SuccessStoryService);
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

  successStory = signal<SuccessStory | undefined>(undefined);

  constructor() {
    effect(() => {
      const slugValue = this.slug();
      if (!slugValue) return;

      const subscription = this.successStoryService
        .getSuccessStoryBySlug(slugValue)
        .subscribe(successStory => {
          if (!successStory) {
            this.router.navigate(['/not-found']);
            return;
          }

          this.successStory.set(successStory);
          this.title.setTitle(successStory.title);
          this.meta.updateTag({
            name: 'description',
            content: successStory.shortDescription?.slice(0, 150) || '',
          });
          this.meta.updateTag({
            property: 'og:title',
            content: successStory.title,
          });
          this.meta.updateTag({
            property: 'og:description',
            content: successStory.description || '',
          });
          this.meta.updateTag({ property: 'og:url', content: this.router.url });
          this.meta.updateTag({ property: 'og:type', content: 'article' });
          this.meta.updateTag({
            name: 'twitter:card',
            content: 'summary_large_image',
          });
          this.meta.updateTag({
            name: 'twitter:title',
            content: successStory.title,
          });
          this.meta.updateTag({
            name: 'twitter:description',
            content: successStory.description || '',
          });

          // Add JSON-LD only in the browser
          if (isPlatformBrowser(this.platformId)) {
            this.addJsonLd(successStory);
          }
        });

      // Clean up subscription on component destruction
      this.destroyRef.onDestroy(() => {
        subscription.unsubscribe();
      });
    });
  }

  private addJsonLd(successStory: SuccessStory) {
    const script = this.renderer.createElement('script');
    this.renderer.setAttribute(script, 'type', 'application/ld+json');
    const jsonLd = {
      '@context': 'https://schema.org',
      '@type': 'Article', // Changed from 'Pet' to 'Article' for correct schema
      headline: successStory.title,
      description: successStory.shortDescription?.slice(0, 150) || '',
      datePublished: successStory.createdAt,
      author: {
        '@type': 'Person',
        name: successStory.adoptionApplication?.user?.firstName
          ? `${successStory.adoptionApplication?.user?.firstName} ${successStory.adoptionApplication?.user?.lastName}`
          : 'Admin',
      },
    };
    this.renderer.setProperty(script, 'text', JSON.stringify(jsonLd));
    this.renderer.appendChild(document.head, script);
  }
}
