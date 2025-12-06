import { Injectable, inject } from '@angular/core';
import {
  Observable,
  catchError,
  forkJoin,
  from,
  map,
  mergeMap,
  of,
  toArray,
} from 'rxjs';

import { AnimalAidRequest } from '../models/animalAidRequest';
import { ApiService } from './api.service';
import { ShelterService } from './shelter.service';

@Injectable({
  providedIn: 'root',
})
export class AnimalAidRequestService {
  private api = inject(ApiService);
  private endpoint = `animal-aid-requests`;

  private shelterService = inject(ShelterService);

  // getAnimalAidRequests(): Observable<AnimalAidRequest[]> {
  //   return this.api.get<AnimalAidRequest[]>(this.endpoint).pipe(
  //     mergeMap(animalAidRequests => from(animalAidRequests)),
  //     mergeMap(animalAidRequest => {
  //       const shelter$ = this.shelterService
  //         .getShelterById(animalAidRequest.shelterId)
  //         .pipe(catchError(() => of(undefined)));
  //       const user$ = this.userService
  //         .getUserById(animalAidRequest.userId)
  //         .pipe(catchError(() => of(undefined)));

  //       return forkJoin({ shelter: shelter$, user: user$ }).pipe(
  //         map(
  //           ({ shelter, user }) =>
  //             ({
  //               ...animalAidRequest,
  //               shelter,
  //               user,
  //             }) as AnimalAidRequest
  //         )
  //       );
  //     }),
  //     toArray()
  //   );
  // }

  // getAnimalAidRequestById(
  //   id: string
  // ): Observable<AnimalAidRequest | undefined> {
  //   return this.api.getById<AnimalAidRequest>(this.endpoint, id).pipe(
  //     mergeMap(animalAidRequest => {
  //       if (!animalAidRequest) return of(undefined);
  //       const shelter$ = this.shelterService
  //         .getShelterById(animalAidRequest.shelterId)
  //         .pipe(catchError(() => of(undefined)));
  //       const user$ = this.userService
  //         .getUserById(animalAidRequest.userId)
  //         .pipe(catchError(() => of(undefined)));

  //       return forkJoin({ shelter: shelter$, user: user$ }).pipe(
  //         map(
  //           ({ shelter, user }) =>
  //             ({
  //               ...animalAidRequest,
  //               shelter,
  //               user,
  //             }) as AnimalAidRequest
  //         )
  //       );
  //     })
  //   );
  // }

  //несправжні

  // createAnimalAidRequest(animalAidRequest: Partial<AnimalAidRequest>) {
  //   return this.api.post<AnimalAidRequest>(this.endpoint, animalAidRequest);
  // }
  // updateAnimalAidRequest(
  //   id: string,
  //   animalAidRequest: Partial<AnimalAidRequest>
  // ) {
  //   return this.api.put<AnimalAidRequest>(this.endpoint, id, animalAidRequest);
  // }
  // deleteAnimalAidRequest(id: string): Observable<void> {
  //   return this.api.delete<void>(this.endpoint, id);
  // }

  getAnimalAidRequests(): Observable<AnimalAidRequest[]> {
    return from(this.mockAnimalAidRequests).pipe(
      mergeMap(request => {
        const shelter$ = this.shelterService
          .getShelterById(request.shelterId)
          .pipe(catchError(() => of(undefined)));

        return forkJoin({ shelter: shelter$ }).pipe(
          map(({ shelter }) => ({
            ...request,
            shelter,
          }))
        );
      }),
      toArray()
    );
  }
  getAnimalAidRequestById(
    id: string
  ): Observable<AnimalAidRequest | undefined> {
    const found = this.mockAnimalAidRequests.find(x => x.id === id);
    if (!found) return of(undefined);

    const shelter$ = this.shelterService
      .getShelterById(found.shelterId)
      .pipe(catchError(() => of(undefined)));

    return forkJoin({ shelter: shelter$ }).pipe(
      map(({ shelter }) => ({
        ...found,
        shelter,
      }))
    );
  }
  createAnimalAidRequest(animalAidRequest: Partial<AnimalAidRequest>) {
    const newItem: AnimalAidRequest = {
      ...animalAidRequest,
      id: crypto.randomUUID(),
      createdAt: new Date().toISOString(),
      updatedAt: new Date().toISOString(),
    } as AnimalAidRequest;

    this.mockAnimalAidRequests.push(newItem);
    return of(newItem);
  }

  updateAnimalAidRequest(id: string, req: Partial<AnimalAidRequest>) {
    const index = this.mockAnimalAidRequests.findIndex(x => x.id === id);
    if (index === -1) return of(undefined);

    const updated = {
      ...this.mockAnimalAidRequests[index],
      ...req,
      updatedAt: new Date().toISOString(),
    };

    this.mockAnimalAidRequests[index] = updated;

    return of(updated);
  }

  deleteAnimalAidRequest(id: string): Observable<void> {
    this.mockAnimalAidRequests = this.mockAnimalAidRequests.filter(
      x => x.id !== id
    );
    return of(void 0);
  }

  private mockAnimalAidRequests: AnimalAidRequest[] = [
    {
      id: '57a3ef20-9796-4081-92d9-4d229f275a84',

      shelterId: 'aa22ba89-681a-4a7e-b588-bf20c6825cd6',
      title: 'Вигодуй малятко',
      description:
        'Закуплено спеціальне харчування для новонароджених тварин. Забезпечено приладами для догляду та обігріву малюків. Під опікою: 54 цуценят та кошенят.',
      category: 'Food',
      status: 'InProgress',
      estimatedCost: 30000,
      allreadyDonated: 21000,
      photos: [
        'https://i.pinimg.com/1200x/16/25/73/16257348c5559a5ad67a8a48d0cd1471.jpg',
        'https://i.pinimg.com/1200x/6d/0a/e2/6d0ae270db6516f769c2105f27673162.jpg',
        'https://i.pinimg.com/1200x/ed/53/39/ed533945e3460fb2c901e2288adcdaa7.jpg',
        'https://i.pinimg.com/736x/03/37/09/0337098b7b602958298e3376004226a9.jpg',
        'https://i.pinimg.com/736x/06/02/48/060248da2e468fb4a2236dcce9498e58.jpg',
      ],
      createdAt: '2025-07-01T10:00:00Z',
      updatedAt: '2025-07-01T10:00:00Z',
    },
    {
      id: 'ccd793bb-f942-4461-999a-639cd4ffaf25',

      shelterId: '1567a749-225b-4df1-bca1-39760e5da7ff',
      title: 'Відбудуй-поремонтуй',
      description:
        'Відновлено вольєри, утеплено приміщення та створено мийно - сушильну зону для дезінфекції. Площа робіт: 250 м²',
      category: 'Equipment',
      status: 'InProgress',
      estimatedCost: 200000,
      allreadyDonated: 111200,
      photos: [
        'https://i.pinimg.com/1200x/67/a6/fe/67a6fefee2854a03d7b00f59ca5d5508.jpg',
        'https://i.pinimg.com/736x/05/f5/63/05f5632ff9becf9e3bea5d5203fba8d7.jpg',
        'https://i.pinimg.com/1200x/da/53/4d/da534d9d207158cb9dec24320cb71473.jpg',
        'https://i.pinimg.com/1200x/9c/e5/2d/9ce52da1b34ecb96f76010241f9cfab8.jpg',
        'https://i.pinimg.com/1200x/8d/db/02/8ddb0296477afa8578d8186668f672ad.jpg',
      ],
      createdAt: '2025-07-01T10:00:00Z',
      updatedAt: '2025-07-01T10:00:00Z',
    },
    {
      id: '8e62ba1a-a613-47b4-8533-15edd23182b0',

      shelterId: 'aa22ba89-681a-4a7e-b588-bf20c6825cd6',
      title: 'Візочок для безлапки',
      description:
        'Придбано та виготовлено спеціальні інвалідні візочки для тварин із травмами лап чи хребта, щоб вони могли пересуватись. Кількість візочків: 20 одиниць.',
      category: 'Equipment',
      status: 'InProgress',
      estimatedCost: 80000,
      allreadyDonated: 36000,
      photos: [
        'https://ireland.apollo.olxcdn.com/v1/files/bvgahtsb4o952-UA/image;s=1200x1600',
        'https://images.shafastatic.net/-191690529',
        'https://img.kwcdn.com/product/open/c3a224a516e24a1d858f65814723342f-goods.jpeg?imageView2/2/w/800/q/70/format/webp',
        'https://img.kwcdn.com/product/open/ac39ec106276452a84fc483c9ebec926-goods.jpeg?imageView2/2/w/800/q/70/format/webp',
        'https://img.kwcdn.com/product/open/3f829af48e5545d29e7d71cf19913bf5-goods.jpeg?imageView2/2/w/800/q/70/format/webp',
        'https://img.kwcdn.com/product/open/a7ec7237ec694b078d5b4b07a9b45f49-goods.jpeg?imageView2/2/w/800/q/70/format/webp',
      ],
      createdAt: '2025-07-01T10:00:00Z',
      updatedAt: '2025-07-01T10:00:00Z',
    },
    {
      id: '4832b5d9-9949-4d07-86cb-cdbc4c8b0b39',

      shelterId: 'aa22ba89-681a-4a7e-b588-bf20c6825cd6',
      title: 'Сім’ю під опіку',
      description:
        'Забезпечено корм, ліки та безпечні умови для матусь із малюками. Частину родин вже вдалося залишити разом і знайти їм тимчасових опікунів. Сімей під опікою: 8 родин.',
      category: 'Equipment',
      status: 'InProgress',
      estimatedCost: 814,
      photos: [
        'https://i.pinimg.com/1200x/e3/87/ff/e387ff74a56ffc9a1e02c754ed810604.jpg',
        'https://i.pinimg.com/1200x/fe/5c/19/fe5c197f57a1017cdeb3baa4a8531e7e.jpg',
        'https://i.pinimg.com/736x/cf/0c/fe/cf0cfef003001e8246181cf838211ae1.jpg',
        'https://i.pinimg.com/736x/16/9c/e9/169ce9d58c70aff36c92827654578aca.jpg',
      ],
      createdAt: '2025-07-01T10:00:00Z',
      updatedAt: '2025-07-01T10:00:00Z',
    },
  ];
}
