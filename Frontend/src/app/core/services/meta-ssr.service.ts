// src/app/services/meta-ssr.service.ts
import { isPlatformServer } from '@angular/common';
import { inject, Injectable, PLATFORM_ID } from '@angular/core';
import { Meta, MetaDefinition, Title } from '@angular/platform-browser';

@Injectable()
export class MetaSsrService {
  private readonly meta = inject(Meta);
  private readonly title = inject(Title);
  private readonly platformId = inject(PLATFORM_ID);

  update(
    ogTitle: string,
    ogDescription: string,
    ogImage = '/assets/images/background1.png',
    ogUrl = 'https://dobrodii.onrender.com'
  ) {
    if (isPlatformServer(this.platformId)) {
      this.title.setTitle(ogTitle);

      const tags: MetaDefinition[] = [
        { property: 'og:title', content: ogTitle },
        { property: 'og:description', content: ogDescription },
        { property: 'og:image', content: ogImage },
        { property: 'og:url', content: ogUrl },
        { property: 'og:type', content: 'website' },
        { name: 'twitter:card', content: 'summary_large_image' },
        { name: 'twitter:title', content: ogTitle },
        { name: 'twitter:description', content: ogDescription },
        { name: 'twitter:image', content: ogImage },
      ];

      this.meta.addTags(tags, true); // true = replace existing
    }
  }
}
