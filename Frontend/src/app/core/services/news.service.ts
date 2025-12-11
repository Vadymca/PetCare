import { Injectable } from '@angular/core';
import { News } from '../models/news';

@Injectable({
  providedIn: 'root',
})
export class NewsService {
  getUkNews() {
    return this.mockUkNews;
  }

  getEnNews() {
    return this.mockEnNews;
  }
  getUkNewsById(id: string) {
    const res = this.mockUkNews.find(news => news.id === id);
    if (!res) {
      return null;
    } else {
      return res;
    }
  }
  getEnNewsById(id: string) {
    const res = this.mockEnNews.find(news => news.id === id);
    if (!res) {
      return null;
    } else {
      return res;
    }
  }
  private mockUkNews: News[] = [
    {
      id: 'animal-festival-2025-12-11',
      date: '2025-12-11T10:00:00Z',
      titleShort: 'Фестиваль тварин',
      title:
        '25 серпня відбулося свято турботи та нових початків — Фестиваль адопції тварин',
      descriptionFirstPart:
        '25 серпня у нашому місті відбувся Фестиваль адопції тварин, організований притулком. Захід зібрав десятки небайдужих людей, які прийшли познайомитися з нашими підопічними та подарувати їм шанс на щасливе майбутнє. Під час фестивалю панувала тепла й дружня атмосфера: для гостей підготували фотозони, дитячі розваги та тематичні майстер-класи.',
      subTitle: 'Результати, якими ми пишаємося:',
      descriptionSecondPart:
        '12 тварин отримали нові родини. 20 сімей залишили заявки на подальшу адопцію після співбесід із волонтерами. Зібрано понад 25 000 UAH пожертв. До команди «Добродій» приєдналися п’ять нових волонтерів.',
      photos: [
        'https://i.pinimg.com/1200x/66/0b/8a/660b8a134ffe1edaef519d2f17cae4ef.jpg',
        'https://i.pinimg.com/1200x/3a/0a/9c/3a0a9cb759c719bb69cd7e58d0fe9a5e.jpg',
        'https://i.pinimg.com/1200x/44/ee/d0/44eed0c5210c0ce7b8268f0c41b2b406.jpg',
        'https://i.pinimg.com/1200x/14/20/b6/1420b6d5794cb11a08fad276e05a752a.jpg',
        'https://i.pinimg.com/736x/47/18/59/47185955d2fd19061c71cd6a93fd34d7.jpg',
      ],
      conclusion:
        'Фестиваль показав, що разом можна робити неймовірні речі: дарувати безпритульним тваринам не лише їжу й дах, а найцінніше — любов і дім. Щиро дякуємо кожному, хто допоміг зробити цей день святом добра.',
    },
    {
      id: 'saving-little-tails-2025-11-01',
      date: '2025-11-01T09:00:00Z',
      titleShort: 'Порятунок хвостиків',
      title:
        'Історія порятунку Мурчика: як команда «Добродія» повернула віру в життя маленькому котику',
      descriptionFirstPart:
        'Наприкінці жовтня до притулку потрапив сильно виснажений кіт Мурчик. Його знайшли біля зупинки — холодного, наляканого та з травмованою лапою. Волонтери одразу доправили тварину в клініку, де лікарі провели обстеження й призначили курс лікування.',
      subTitle: 'Як ми допомогли Мурчику:',
      descriptionSecondPart:
        'Протягом двох тижнів кіт отримував терапію, збалансоване харчування та багато уваги. Завдяки спільним зусиллям ветеринарів і волонтерів Мурчик одужав і знову став активним і лагідним. Уже 30 жовтня він знайшов нову родину, яка приїжджала в притулок щодня, щоб підтримувати його.',
      photos: [
        'https://i.pinimg.com/736x/b5/28/c4/b528c4c2fc0d17d33df5a58312f1bf8a.jpg',
        'https://i.pinimg.com/1200x/83/b6/a5/83b6a5eb4101cb4caca5fc1da7991d88.jpg',
        'https://i.pinimg.com/1200x/fc/b3/9d/fcb39d83501415f648a3eb0309add000.jpg',
        'https://i.pinimg.com/1200x/74/f8/1c/74f81cefa019027f058c83e8f95a5ba0.jpg',
        'https://i.pinimg.com/736x/5b/12/82/5b1282d3bc63febc98db9922a77ef1d0.jpg',
      ],
      conclusion:
        'Кожна врятована тварина — це історія боротьби, любові та надії. Ми вдячні всім, хто підтримує притулок і дає шанс таким хвостикам, як Мурчик.',
    },
    {
      id: 'clean-paws-2025-10-18',
      date: '2025-10-18T10:00:00Z',
      titleShort: 'Чисті лапки',
      title:
        'День гігієни в притулку: як ми піклуємося про здоровʼя наших підопічних',
      descriptionFirstPart:
        'У середині жовтня ми провели черговий «День чистих лапок» — комплексний догляд за тваринами, що включав миття лап, огляд кігтів, обробку від паразитів та перевірку загального стану здоров’я.',
      subTitle: 'Що вдалося зробити:',
      descriptionSecondPart:
        'Протягом дня наші волонтери попіклувалися про 53 тварини: 27 собак та 26 котів. Було проведено профілактичні огляди, обробку від бліх і кліщів, а також оновлено облік стану здоровʼя кожного хвостика.',
      photos: [
        'https://i.pinimg.com/1200x/82/fb/de/82fbdec3839b14c309cb703200f415c3.jpg',
        'https://i.pinimg.com/1200x/ba/8f/1a/ba8f1af30a6b10c29397698eef872418.jpg',
        'https://i.pinimg.com/1200x/b6/dc/fc/b6dcfc4afe9c42e763f685b9da5c4e3e.jpg',
        'https://i.pinimg.com/1200x/60/52/63/60526394d38611e19902eb0f7a8e20d1.jpg',
        'https://i.pinimg.com/1200x/3b/cc/3b/3bcc3b7ad4ba598cded06c7e545acdc9.jpg',
        'https://i.pinimg.com/736x/57/2c/79/572c79485298355c82fa16fe8242a62b.jpg',
        'https://i.pinimg.com/736x/ed/28/8c/ed288c538dfa98c67d1bd204f2541032.jpg',
      ],

      conclusion:
        'Регулярний догляд — це ключ до довгого й здорового життя тварин у притулку. Ми пишаємося нашою командою та всіма, хто підтримує такі ініціативи.',
    },
    {
      id: '20-animals-are-home-2025-08-01',
      date: '2025-08-01T10:00:00Z',
      titleShort: '20 тварин уже вдома',
      title:
        'Рекордний липень: 20 наших підопічних знайшли нові родини всього за два тижні',
      descriptionFirstPart:
        'У другій половині липня в притулку відбувся справжній сплеск адопцій. Багато сімей, надихнувшись нашими історіями, вирішили подарувати дім тим, хто давно чекав цього шансу.',
      subTitle: 'Досягнення місяця:',
      descriptionSecondPart:
        'Нові родини знайшли 12 собак та 8 котів. Для частини тварин це були роки очікування. Волонтери допомогли адаптувати хвостиків та підготували родини до відповідального утримання.',
      photos: [
        'https://i.pinimg.com/736x/11/6f/18/116f185c018e5af9e6af6758cff7e8f1.jpg',
        'https://i.pinimg.com/736x/31/90/ee/3190eece74200416b2f6530f639136d3.jpg',
        'https://i.pinimg.com/1200x/17/01/e1/1701e155907432d974711eeaaba7139c.jpg',
        'https://i.pinimg.com/736x/c2/bd/a5/c2bda55736d515cc282d7cc7defd675b.jpg',
        'https://i.pinimg.com/1200x/af/6e/d8/af6ed89528568b1743a0fc05525022b1.jpg',
        'https://i.pinimg.com/1200x/1d/24/58/1d2458429cd118ff7e3804dc3fdaf1cc.jpg',
        'https://i.pinimg.com/736x/da/bc/c4/dabcc494fc5f8ca48df0bb43d1eb19e4.jpg',
        'https://i.pinimg.com/474x/db/1a/e7/db1ae79009f2bf3b22a3071d24e74e20.jpg',
        'https://i.pinimg.com/736x/d4/00/0e/d4000e9a1f7dc46701b5cdd292d8fe05.jpg',
      ],
      conclusion:
        'Кожна успішна адопція — це маленьке диво. Дякуємо всім, хто обирає турботу й відкриває двері свого дому для наших хвостиків.',
    },
    {
      id: 'kids-draw-animals-2025-07-05',
      date: '2025-07-05T10:00:00Z',
      titleShort: 'Діти малюють тварин',
      title:
        'Маленькі художники — великі серця: у притулку відбувся конкурс дитячих малюнків',
      descriptionFirstPart:
        'На початку липня ми провели творчий конкурс для дітей, присвячений нашим підопічним. У заході взяли участь понад 40 юних художників віком від 5 до 14 років.',
      subTitle: 'Найяскравіші моменти:',
      descriptionSecondPart:
        'Діти малювали улюблених тварин, створювали плакати в підтримку адопції та передавали свої роботи притулку. Переможці отримали подарунки, а найкращі малюнки стануть частиною виставки в нашому центрі.',
      photos: [
        'https://i.pinimg.com/736x/eb/e2/75/ebe275d7d17b1af2d9b6c455f8c540ad.jpg',
        'https://i.pinimg.com/736x/59/fb/c3/59fbc3a8bfcb0712aeb0833c7d1a882e.jpg',
        'https://i.pinimg.com/1200x/15/55/43/1555433f2d0f12644f657309acb59140.jpg',
        'https://i.pinimg.com/1200x/42/6b/c4/426bc4f433ba5c74b3441a4ad28948fb.jpg',
        'https://i.pinimg.com/1200x/28/05/ea/2805ea9852bb3f12b6d177e79f512730.jpg',
        'https://i.pinimg.com/1200x/f0/3a/e8/f03ae89ca61c01bcc928e43234b9ff33.jpg',
        'https://i.pinimg.com/1200x/67/b3/d8/67b3d88ac2143086f262413e174beb54.jpg',
        'https://i.pinimg.com/736x/a5/2b/18/a52b187e72ebc290ab11d400caa750bb.jpg',
        'https://i.pinimg.com/1200x/b0/82/d2/b082d290064939266c7e9f34b7aecb91.jpg',
        'https://i.pinimg.com/1200x/a0/64/d3/a064d32309395453e8ab68654e454e23.jpg',
        'https://i.pinimg.com/1200x/3d/05/04/3d05049616c0d1dc1ff85db530fac71b.jpg',
        'https://i.pinimg.com/1200x/1d/53/97/1d539764a2596eef0f947661baf9037b.jpg',
        'https://i.pinimg.com/1200x/ac/a1/46/aca146ccbc0b555ce69b10dcfef36be7.jpg',
        'https://i.pinimg.com/1200x/8d/8a/c1/8d8ac1508038509a2a154d7ca0504c5f.jpg',
        '',
      ],
      conclusion:
        'Конкурс став нагадуванням, що любов до тварин виховується з дитинства. Ми вдячні всім батькам та учасникам за щирість і тепло, які вони подарували.',
    },
    {
      id: 'a-parcel-of-kindness-from-abroad-2025-07-01',
      date: '2025-07-01T10:00:00Z',
      titleShort: 'Посилка добра з-за кордону',
      title:
        'Від щирого серця: притулок отримав гуманітарну допомогу від благодійників із Нідерландів',
      descriptionFirstPart:
        'На початку липня до нас надійшла велика посилка з кормом, ліками та речами для догляду за тваринами. Її надіслали наші давні друзі — благодійна організація з Нідерландів, яка підтримує притулки по всьому світу.',
      subTitle: 'Що ми отримали:',
      descriptionSecondPart:
        'У вантажі було понад 120 кг якісного сухого та вологого корму, медикаменти для котів і собак, засоби гігієни, пледи та амуніція. Ця допомога суттєво зменшить витрати притулку протягом найближчих місяців.',
      photos: [
        'https://sylach.com.ua/images/movers%202.jpg',
        'https://move-team.com.ua/assets/images/blog/gruzchik-popularnaya-profesiya.jpg',
        'https://www.gruzchiki-kiev.net/wp-content/uploads/2022/11/gruzchiki-obolon.webp',
        'https://www.volynnews.com/files/news/2023/09-22/368916/1.jpg',
        'https://i.pinimg.com/1200x/d4/34/e9/d434e9f48ff21498aba58ab5fcc8c601.jpg',
        'https://i.pinimg.com/1200x/cd/82/dc/cd82dcec7c75e32977e1e69ccc95ab5f.jpg',
        'https://i.pinimg.com/736x/2a/f0/d0/2af0d05e7584f226e807eb23f1434cbc.jpg',
      ],
      conclusion:
        'Ми щиро вдячні міжнародним друзям за турботу та підтримку. Кожен подарунок — це внесок у щасливе життя наших хвостатих підопічних.',
    },
  ];
  private mockEnNews: News[] = [
    {
      id: 'animal-festival-2025-12-11',
      date: '2025-12-11T10:00:00Z',
      titleShort: 'Animal Festival',
      title:
        'On August 25, we celebrated a day of care and new beginnings — the Animal Adoption Festival',
      descriptionFirstPart:
        'On August 25, our city hosted the Animal Adoption Festival organized by the shelter. The event brought together dozens of caring people who came to meet our animals and give them a chance for a happy future. The atmosphere was warm and friendly: guests enjoyed photo zones, children’s activities, and themed workshops.',
      subTitle: 'Results we are proud of:',
      descriptionSecondPart:
        '12 animals found new families. 20 families submitted adoption applications after interviews with volunteers. Over 25,000 UAH in donations were collected. Five new volunteers joined the “Dobrodiy” team.',
      photos: [
        'https://i.pinimg.com/1200x/66/0b/8a/660b8a134ffe1edaef519d2f17cae4ef.jpg',
        'https://i.pinimg.com/1200x/3a/0a/9c/3a0a9cb759c719bb69cd7e58d0fe9a5e.jpg',
        'https://i.pinimg.com/1200x/44/ee/d0/44eed0c5210c0ce7b8268f0c41b2b406.jpg',
        'https://i.pinimg.com/1200x/14/20/b6/1420b6d5794cb11a08fad276e05a752a.jpg',
        'https://i.pinimg.com/736x/47/18/59/47185955d2fd19061c71cd6a93fd34d7.jpg',
      ],
      conclusion:
        'The festival showed that together we can do incredible things: giving homeless animals not only food and shelter but the most precious gift — love and a home. We sincerely thank everyone who supported this event and made it a true celebration of kindness.',
    },
    {
      id: 'saving-little-tails-2025-11-01',
      date: '2025-11-01T09:00:00Z',
      titleShort: 'Saving Little Tails',
      title:
        'The story of Murchyk: how the “Dobrodiy” team restored hope to a little cat',
      descriptionFirstPart:
        'At the end of October, a severely weakened cat named Murchyk arrived at the shelter. He was found near a bus stop — cold, frightened, and with an injured paw. Volunteers immediately took him to a clinic, where veterinarians conducted examinations and prescribed treatment.',
      subTitle: 'How we helped Murchyk:',
      descriptionSecondPart:
        'For two weeks, the cat received therapy, proper nutrition, and lots of attention. Thanks to the combined efforts of veterinarians and volunteers, Murchyk recovered and became active and affectionate again. On October 30, he found his new family, who had visited him daily during his treatment.',
      photos: [
        'https://i.pinimg.com/736x/b5/28/c4/b528c4c2fc0d17d33df5a58312f1bf8a.jpg',
        'https://i.pinimg.com/1200x/83/b6/a5/83b6a5eb4101cb4caca5fc1da7991d88.jpg',
        'https://i.pinimg.com/1200x/fc/b3/9d/fcb39d83501415f648a3eb0309add000.jpg',
        'https://i.pinimg.com/1200x/74/f8/1c/74f81cefa019027f058c83e8f95a5ba0.jpg',
        'https://i.pinimg.com/736x/5b/12/82/5b1282d3bc63febc98db9922a77ef1d0.jpg',
      ],
      conclusion:
        'Every rescued animal is a story of struggle, love, and hope. We are grateful to everyone who supports the shelter and gives a chance to animals like Murchyk.',
    },
    {
      id: 'clean-paws-2025-10-18',
      date: '2025-10-18T10:00:00Z',
      titleShort: 'Clean Paws',
      title:
        'Hygiene Day at the shelter: how we care for the health of our animals',
      descriptionFirstPart:
        'In mid-October, we held our regular “Clean Paws Day” — a comprehensive care event that included paw washing, claw check-ups, parasite treatments, and general health assessments.',
      subTitle: 'What we accomplished:',
      descriptionSecondPart:
        'Throughout the day, our volunteers cared for 53 animals: 27 dogs and 26 cats. Preventive check-ups were performed, animals were treated for fleas and ticks, and health records were updated for each one.',
      photos: [
        'https://i.pinimg.com/1200x/82/fb/de/82fbdec3839b14c309cb703200f415c3.jpg',
        'https://i.pinimg.com/1200x/ba/8f/1a/ba8f1af30a6b10c29397698eef872418.jpg',
        'https://i.pinimg.com/1200x/b6/dc/fc/b6dcfc4afe9c42e763f685b9da5c4e3e.jpg',
        'https://i.pinimg.com/1200x/60/52/63/60526394d38611e19902eb0f7a8e20d1.jpg',
        'https://i.pinimg.com/1200x/3b/cc/3b/3bcc3b7ad4ba598cded06c7e545acdc9.jpg',
        'https://i.pinimg.com/736x/57/2c/79/572c79485298355c82fa16fe8242a62b.jpg',
        'https://i.pinimg.com/736x/ed/28/8c/ed288c538dfa98c67d1bd204f2541032.jpg',
      ],
      conclusion:
        'Regular care is the key to long and healthy lives for shelter animals. We are proud of our team and everyone who supports these initiatives.',
    },
    {
      id: '20-animals-are-home-2025-08-01',
      date: '2025-08-01T10:00:00Z',
      titleShort: '20 Animals Are Home',
      title:
        'A record-breaking July: 20 of our animals found homes in just two weeks',
      descriptionFirstPart:
        'In the second half of July, the shelter experienced a surge in adoptions. Many families, inspired by our stories, decided to give a long-awaited home to those who needed it most.',
      subTitle: 'This month’s achievements:',
      descriptionSecondPart:
        '12 dogs and 8 cats found new families. For some animals, it meant the end of years of waiting. Volunteers helped with adaptation and prepared families for responsible pet ownership.',
      photos: [
        'https://i.pinimg.com/736x/11/6f/18/116f185c018e5af9e6af6758cff7e8f1.jpg',
        'https://i.pinimg.com/736x/31/90/ee/3190eece74200416b2f6530f639136d3.jpg',
        'https://i.pinimg.com/1200x/17/01/e1/1701e155907432d974711eeaaba7139c.jpg',
        'https://i.pinimg.com/736x/c2/bd/a5/c2bda55736d515cc282d7cc7defd675b.jpg',
        'https://i.pinimg.com/1200x/af/6e/d8/af6ed89528568b1743a0fc05525022b1.jpg',
        'https://i.pinimg.com/1200x/1d/24/58/1d2458429cd118ff7e3804dc3fdaf1cc.jpg',
        'https://i.pinimg.com/736x/da/bc/c4/dabcc494fc5f8ca48df0bb43d1eb19e4.jpg',
        'https://i.pinimg.com/474x/db/1a/e7/db1ae79009f2bf3b22a3071d24e74e20.jpg',
        'https://i.pinimg.com/736x/d4/00/0e/d4000e9a1f7dc46701b5cdd292d8fe05.jpg',
      ],
      conclusion:
        'Every successful adoption is a small miracle. We thank all families who choose compassion and open their homes to our shelter animals.',
    },
    {
      id: 'kids-draw-animals-2025-07-05',
      date: '2025-07-05T10:00:00Z',
      titleShort: 'Kids Draw Animals',
      title:
        'Little artists with big hearts: a children’s drawing contest took place at the shelter',
      descriptionFirstPart:
        'In early July, we organized a creative contest for children dedicated to our shelter pets. More than 40 young artists aged 5 to 14 took part in the event.',
      subTitle: 'Highlights of the day:',
      descriptionSecondPart:
        'Children drew their favourite animals, created posters promoting adoption, and gifted their artworks to the shelter. Winners received prizes, and the best drawings will be displayed in our center.',
      photos: [
        'https://i.pinimg.com/736x/eb/e2/75/ebe275d7d17b1af2d9b6c455f8c540ad.jpg',
        'https://i.pinimg.com/736x/59/fb/c3/59fbc3a8bfcb0712aeb0833c7d1a882e.jpg',
        'https://i.pinimg.com/1200x/15/55/43/1555433f2d0f12644f657309acb59140.jpg',
        'https://i.pinimg.com/1200x/42/6b/c4/426bc4f433ba5c74b3441a4ad28948fb.jpg',
        'https://i.pinimg.com/1200x/28/05/ea/2805ea9852bb3f12b6d177e79f512730.jpg',
        'https://i.pinimg.com/1200x/f0/3a/e8/f03ae89ca61c01bcc928e43234b9ff33.jpg',
        'https://i.pinimg.com/1200x/67/b3/d8/67b3d88ac2143086f262413e174beb54.jpg',
        'https://i.pinimg.com/736x/a5/2b/18/a52b187e72ebc290ab11d400caa750bb.jpg',
        'https://i.pinimg.com/1200x/b0/82/d2/b082d290064939266c7e9f34b7aecb91.jpg',
        'https://i.pinimg.com/1200x/a0/64/d3/a064d32309395453e8ab68654e454e23.jpg',
        'https://i.pinimg.com/1200x/3d/05/04/3d05049616c0d1dc1ff85db530fac71b.jpg',
        'https://i.pinimg.com/1200x/1d/53/97/1d539764a2596eef0f947661baf9037b.jpg',
        'https://i.pinimg.com/1200x/ac/a1/46/aca146ccbc0b555ce69b10dcfef36be7.jpg',
        'https://i.pinimg.com/1200x/8d/8a/c1/8d8ac1508038509a2a154d7ca0504c5f.jpg',
        '',
      ],
      conclusion:
        'The contest reminded us that love for animals starts in childhood. We are thankful to all participants and parents for the sincerity and warmth they shared.',
    },
    {
      id: 'a-parcel-of-kindness-from-abroad-2025-07-01',
      date: '2025-07-01T10:00:00Z',
      titleShort: 'A Parcel of Kindness from Abroad',
      title:
        'From the heart: the shelter received humanitarian aid from donors in the Netherlands',
      descriptionFirstPart:
        'At the beginning of July, we received a large package filled with pet food, medicine, and care supplies. It was sent by our long-term friends — a charity organization from the Netherlands that supports shelters worldwide.',
      subTitle: 'What we received:',
      descriptionSecondPart:
        'The shipment included over 120 kg of premium dry and wet food, medication for cats and dogs, hygiene products, blankets, and pet gear. This aid will significantly reduce shelter expenses for the coming months.',
      photos: [
        'https://sylach.com.ua/images/movers%202.jpg',
        'https://move-team.com.ua/assets/images/blog/gruzchik-popularnaya-profesiya.jpg',
        'https://www.gruzchiki-kiev.net/wp-content/uploads/2022/11/gruzchiki-obolon.webp',
        'https://www.volynnews.com/files/news/2023/09-22/368916/1.jpg',
        'https://i.pinimg.com/1200x/d4/34/e9/d434e9f48ff21498aba58ab5fcc8c601.jpg',
        'https://i.pinimg.com/1200x/cd/82/dc/cd82dcec7c75e32977e1e69ccc95ab5f.jpg',
        'https://i.pinimg.com/736x/2a/f0/d0/2af0d05e7584f226e807eb23f1434cbc.jpg',
      ],
      conclusion:
        'We are sincerely grateful to our international friends for their kindness and support. Every gift is a contribution to the well-being of our animals.',
    },
  ];
}
