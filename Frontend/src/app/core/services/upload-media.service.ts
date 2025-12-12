import { isPlatformBrowser } from '@angular/common';
import { inject, Injectable, PLATFORM_ID } from '@angular/core';
import * as imageConversion from 'image-conversion';
import { EImageType } from 'image-conversion';
import { from, Observable, of, switchMap } from 'rxjs';
import { ApiService } from './api.service';
export interface UploadResponse {
  url: string;
}
@Injectable({
  providedIn: 'root',
})
export class UploadMediaService {
  private api = inject(ApiService);
  private endpoint = `media/upload`;
  platformId = inject(PLATFORM_ID);

  upload(file: File): Observable<UploadResponse> {
    if (!isPlatformBrowser(this.platformId)) {

      // Відправляємо оригінальний файл у SSR
      const formData = new FormData();
      formData.append('file', file);
      return this.api.uploadFile(this.endpoint, file);
    }

    // Обробка файлу перед відправкою
    return this.processFile(file).pipe(
      switchMap(processedFile => {
        const formData = new FormData();
        formData.append('file', processedFile);
        return this.api.uploadFile<UploadResponse>(this.endpoint, file);
      })
    );
  }
  private processFile(file: File): Observable<File> {
    const maxSizeMB = 5; // Максимальний розмір файлу в МБ
    const maxDimension = 1500; // Максимальна роздільна здатність по довшій стороні
    const quality = 0.8; // Початкова якість стиснення (0-1)

    if (!file.type.startsWith('image/')) {
      // Якщо файл не є зображенням, повертаємо оригінальний файл
      return of(file);
    }

    // Перевірка розміру файлу
    if (file.size <= maxSizeMB * 1024 * 1024) {
      // Якщо розмір у межах, перевіряємо лише роздільну здатність
      return this.resizeImage(file, maxDimension);
    }

    // Стискаємо файл, якщо він перевищує 5 МБ
    return from(
      imageConversion.compressAccurately(file, {
        size: maxSizeMB * 1024, // Розмір у кілобайтах
        type: file.type as EImageType,
        quality: quality,
      })
    ).pipe(
      switchMap((compressedFile: Blob) => {
        const file = new File([compressedFile], (compressedFile as File).name, {
          lastModified: (compressedFile as File).lastModified,
          type: compressedFile.type,
        });
        return this.resizeImage(file, maxDimension);
      })
    );
  }
  private resizeImage(file: File, maxDimension: number): Observable<File> {
    return new Observable(observer => {
      const img = new Image();
      const reader = new FileReader();

      reader.onload = (event: ProgressEvent<FileReader>) => {
        const reader = event.target as FileReader;
        if (reader.result) {
          img.src = reader.result as string;
        }

        img.onload = () => {
          let width = img.width;
          let height = img.height;

          // Перевірка, чи потрібно змінювати розмір
          if (width <= maxDimension && height <= maxDimension) {
            observer.next(file); // Розмір відповідає, повертаємо оригінальний файл
            observer.complete();
            return;
          }

          // Обчислюємо нові розміри, зберігаючи пропорції
          if (width > height) {
            height = Math.round((height / width) * maxDimension);
            width = maxDimension;
          } else {
            width = Math.round((width / height) * maxDimension);
            height = maxDimension;
          }

          // Створюємо canvas для зміни розміру
          const canvas = document.createElement('canvas');
          canvas.width = width;
          canvas.height = height;
          const ctx = canvas.getContext('2d')!;
          ctx.drawImage(img, 0, 0, width, height);

          // Конвертуємо canvas у Blob
          canvas.toBlob(
            blob => {
              if (blob) {
                const resizedFile = new File([blob], file.name, {
                  type: file.type,
                  lastModified: file.lastModified,
                });
                observer.next(resizedFile);
                observer.complete();
              } else {
                observer.error(new Error('Failed to convert canvas to Blob'));
              }
            },
            file.type,
            0.8 // Якість для JPEG
          );
        };

        img.onerror = () => observer.error(new Error('Failed to load image'));
      };

      reader.onerror = () => observer.error(new Error('Failed to read file'));
      reader.readAsDataURL(file);
    });
  }
}
