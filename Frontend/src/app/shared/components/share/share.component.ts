import { isPlatformBrowser, UpperCasePipe } from '@angular/common';
import { Component, inject, input, PLATFORM_ID, signal } from '@angular/core';
import { TranslateModule, TranslateService } from '@ngx-translate/core';
import { SmallShareButtonComponent } from '../buttons/small-share-button/small-share-button.component';
import { IconComponent } from '../icon.component';
type IconName = 'shareInsta' | 'shareFacebook' | 'shareX' | 'shareTiktok';
@Component({
  selector: 'app-share',
  standalone: true,
  imports: [
    TranslateModule,
    SmallShareButtonComponent,
    UpperCasePipe,
    IconComponent,
  ],
  templateUrl: './share.component.html',
  styleUrl: './share.component.css',
})
export class ShareComponent {
  text = input.required<string>();
  shareInsta = signal<IconName>('shareInsta');
  shareFacebook = signal<IconName>('shareFacebook');
  shareX = signal<IconName>('shareX');
  shareTiktok = signal<IconName>('shareTiktok');
  platformId = inject(PLATFORM_ID);
  translate = inject(TranslateService);
  onShareFacebookClick() {
    if (!isPlatformBrowser(this.platformId)) return;

    const url = encodeURIComponent(window.location.href);
    const text = this.translate.instant(this.text());
    const shareUrl = `https://www.facebook.com/sharer/sharer.php?u=${url}&quote=${text}`;

    window.open(shareUrl, '_blank', 'width=600,height=400');
  }

  onShareInstaClick() {
    if (!isPlatformBrowser(this.platformId)) return;

    const url = encodeURIComponent(window.location.href);
    navigator.clipboard.writeText(url).then(() => {
      alert(this.translate.instant('LINK_COPIED'));
    });
  }
  onShareXClick() {
    if (!isPlatformBrowser(this.platformId)) return;

    const url = encodeURIComponent(window.location.href);
    const text = encodeURIComponent(this.translate.instant(this.text()));
    const shareUrl = `https://x.com/intent/tweet?text=${text}&url=${url}`;

    window.open(shareUrl, '_blank', 'width=600,height=400');
  }
  onShareTikTokClick() {
    if (!isPlatformBrowser(this.platformId)) return;

    const url = encodeURIComponent(window.location.href);
    navigator.clipboard.writeText(url).then(() => {
      alert(this.translate.instant('LINK_COPIED'));
    });
  }
}
