const { v4: uuidv4 } = require('uuid');
const fs = require('fs');

// Генерація 10 видів тварин (species)
const species = [
  { id: uuidv4(), name: 'Dog' },
  { id: uuidv4(), name: 'Cat' },
  { id: uuidv4(), name: 'Bird' },
  { id: uuidv4(), name: 'Rabbit' },
  { id: uuidv4(), name: 'Hamster' },
  { id: uuidv4(), name: 'GuineaPig' },
  { id: uuidv4(), name: 'Fish' },
  { id: uuidv4(), name: 'Turtle' },
  { id: uuidv4(), name: 'Ferret' },
  { id: uuidv4(), name: 'Hedgehog' },
];

// Генерація порід
const breeds = [];
const breedNamesBySpecies = {
  Dog: [
    'Labrador Retriever',
    'German Shepherd',
    'Golden Retriever',
    'Bulldog',
    'Poodle',
    'Beagle',
    'Rottweiler',
    'Dachshund',
    'Siberian Husky',
    'Boxer',
    'Yorkshire Terrier',
    'Chihuahua',
    'Doberman Pinscher',
    'Shih Tzu',
    'Great Dane',
  ],
  Cat: [
    'Persian',
    'Maine Coon',
    'Siamese',
    'Ragdoll',
    'British Shorthair',
    'Abyssinian',
    'Sphynx',
    'Bengal',
    'Scottish Fold',
    'Birman',
  ],
  Bird: ['Parrot', 'Canary', 'Budgerigar', 'Cockatiel', 'Lovebird'],
  Rabbit: [
    'Holland Lop',
    'Netherland Dwarf',
    'Flemish Giant',
    'Mini Rex',
    'Lionhead',
  ],
  Hamster: [
    'Syrian',
    'Dwarf Campbell',
    'Roborovski',
    'Chinese',
    'Winter White',
  ],
  GuineaPig: ['American', 'Abyssinian', 'Peruvian', 'Silkie', 'Texel'],
  Fish: ['Goldfish', 'Betta', 'Guppy', 'Neon Tetra', 'Angelfish'],
  Turtle: ['Red-Eared Slider', 'Box Turtle', 'Painted Turtle'],
  Ferret: ['Standard Ferret', 'Black-Footed Ferret'],
  Hedgehog: ['African Pygmy', 'Algerian'],
};

species.forEach(s => {
  const breedNames = breedNamesBySpecies[s.name] || ['Generic Breed'];
  breedNames.forEach(name => {
    if (breeds.length < 50) {
      breeds.push({
        id: uuidv4(),
        speciesId: s.id,
        name,
        description: `${name} is a ${s.name.toLowerCase()} breed known for its unique traits.`,
      });
    }
  });
});
while (breeds.length < 50) {
  const speciesIndex = Math.floor(Math.random() * species.length);
  breeds.push({
    id: uuidv4(),
    speciesId: species[speciesIndex].id,
    name: `Custom Breed ${breeds.length + 1}`,
    description: `A custom breed for ${species[speciesIndex].name}.`,
  });
}

// Користувачі
const users = [];
const firstNames = [
  'John',
  'Anna',
  'Oleh',
  'Maria',
  'Ivan',
  'Sofia',
  'Dmytro',
  'Yulia',
];
const lastNames = [
  'Doe',
  'Smith',
  'Kovalenko',
  'Shevchenko',
  'Bondarenko',
  'Petrenko',
];
for (let i = 0; i < 50; i++) {
  const firstName = firstNames[Math.floor(Math.random() * firstNames.length)];
  const lastName = lastNames[Math.floor(Math.random() * lastNames.length)];
  users.push({
    id: uuidv4(),
    email: `user${i + 1}@example.com`,
    passwordHash: 'hashed',
    firstName,
    lastName,
    phone: `+38067${Math.floor(1000000 + Math.random() * 9000000)}`,
    role: i === 0 ? 'Admin' : 'User',

    points: Math.floor(Math.random() * 100),
    lastLogin: null,
    profilePhoto: null,
    language: Math.random() > 0.5 ? 'uk' : 'en',
    createdAt: '2025-07-01T10:00:00Z',
    updatedAt: '2025-07-01T10:00:00Z',
  });
}

// Притулки
const shelters = [];
const cities = ['Kyiv', 'Lviv', 'Odesa', 'Kharkiv', 'Dnipro'];
for (let i = 0; i < 20; i++) {
  const city = cities[Math.floor(Math.random() * cities.length)];
  const slug = `${city.toLowerCase()}-shelter-${i + 1}`;
  shelters.push({
    id: uuidv4(),
    slug,
    name: `${city} Shelter ${i + 1}`,
    address: `${city}, vul. Example ${i + 1}`,
    coordinates: {
      lat: 50.4 + Math.random() * 0.1,
      lng: 30.4 + Math.random() * 0.2,
    },
    contactPhone: `+38067${Math.floor(1000000 + Math.random() * 9000000)}`,
    contactEmail: `${slug}@shelter.org`,
    description: `A shelter in ${city} for homeless animals.`,
    capacity: Math.floor(50 + Math.random() * 100),
    currentOccupancy: Math.floor(20 + Math.random() * 80),
    photos: [],
    virtualTourUrl: null,
    workingHours: '9:00-18:00',
    socialMedia: {},
    managerId: users[Math.floor(Math.random() * users.length)].id,
    createdAt: '2025-07-01T10:00:00Z',
    updatedAt: '2025-07-01T10:00:00Z',
  });
}

// Тварини
const animals = [];
const animalNames = [
  'Buddy',
  'Luna',
  'Max',
  'Bella',
  'Charlie',
  'Lucy',
  'Milo',
  'Sophie',
];
const colors = ['Brown', 'Black', 'White', 'Gray', 'Golden'];
const genders = ['Male', 'Female'];
const statuses = ['Available', 'Adopted', 'Pending'];
for (let i = 0; i < 200; i++) {
  const name = animalNames[Math.floor(Math.random() * animalNames.length)];
  const breed = breeds[Math.floor(Math.random() * breeds.length)];
  const shelter = shelters[Math.floor(Math.random() * shelters.length)];
  const user = users[Math.floor(Math.random() * users.length)];
  animals.push({
    id: uuidv4(),
    slug: `${name.toLowerCase()}-${uuidv4().slice(0, 8)}`,
    userId: user.id,
    name,
    breedId: breed.id,
    birthday: `202${Math.floor(Math.random() * 4 + 1)}-0${Math.floor(Math.random() * 9 + 1)}-01`,
    gender: genders[Math.floor(Math.random() * genders.length)],
    description: `A ${breed.name.toLowerCase()} looking for a loving home.`,
    healthStatus: Math.random() > 0.1 ? 'Healthy' : 'Needs Care',
    photos: [],
    videos: [],
    shelterId: shelter.id,
    status: statuses[Math.floor(Math.random() * statuses.length)],
    adoptionRequirements: 'Active family with time for care.',
    microchipId: `${Math.floor(100000000 + Math.random() * 900000000)}`,
    idNumber: i + 1,
    weight: 5 + Math.random() * 30,
    height: 20 + Math.random() * 50,
    color: colors[Math.floor(Math.random() * colors.length)],
    isSterilized: Math.random() > 0.3,
    haveDocuments: Math.random() > 0.2,
    createdAt: '2025-07-01T10:00:00Z',
    updatedAt: '2025-07-01T10:00:00Z',
  });
}

// Категорії
const categories = [
  'Health',
  'Adoption',
  'Training',
  'Nutrition',
  'Rescue',
].map(name => ({
  id: uuidv4(),
  name,
  description: `${name} related articles`,
  createdAt: '2025-07-01T10:00:00Z',
}));

// Статті
const articles = [];
for (let i = 0; i < 20; i++) {
  const author = users[Math.floor(Math.random() * users.length)];
  const category = categories[Math.floor(Math.random() * categories.length)];
  const title = `Article ${i + 1}`;
  articles.push({
    id: uuidv4(),
    slug: `${title.toLowerCase()}-${uuidv4().slice(0, 8)}`,
    title,
    content: `Content of ${title}.`,
    categoryId: category.id,
    authorId: author.id,
    status: Math.random() > 0.2 ? 'published' : 'draft',

    createdAt: '2025-07-01T10:00:00Z',
    updatedAt: '2025-07-01T10:00:00Z',
  });
}

// LostPets
const lostPets = [];
for (let i = 0; i < 30; i++) {
  const breed = breeds[Math.floor(Math.random() * breeds.length)];
  const user = users[Math.floor(Math.random() * users.length)];
  const name = animalNames[Math.floor(Math.random() * animalNames.length)];
  const status = ['Lost', 'Found', 'Reunited'][Math.floor(Math.random() * 3)];
  lostPets.push({
    id: uuidv4(),
    slug: `${name.toLowerCase()}-${uuidv4().slice(0, 8)}`,
    userId: user.id,
    breedId: breed.id,
    name: name,

    description: `Lost ${breed.name.toLowerCase()} near the city.`,
    lastSeenLocation: 'Some district, city',
    lastSeenDate: `2025-06-${10 + i}T10:00:00Z`,
    photos: [],
    status,
    adminNotes: '',
    reward: Math.random() > 0.7 ? 500 : 0,
    contactAlternative: user.phone,
    microchipId: `${Math.floor(100000000 + Math.random() * 900000000)}`,
    createdAt: '2025-07-01T10:00:00Z',
    updatedAt: '2025-07-01T10:00:00Z',
  });
}

startUser = users[0];
const animal1 = animals[Math.floor(Math.random() * animals.length)];
const animal2 = animals[Math.floor(Math.random() * animals.length)];

// Підписки на тварин
const animalSubscriptions = [];
animalSubscriptions.push({
  id: uuidv4(),
  userId: startUser.id,
  animalId: animal1.id,
  subscribedAt: '2025-07-10T12:00:00Z',
});
animalSubscriptions.push({
  id: uuidv4(),
  userId: startUser.id,
  animalId: animal2.id,
  subscribedAt: '2025-07-10T12:00:00Z',
});

for (let i = 0; i < 50; i++) {
  const user = users[Math.floor(Math.random() * users.length)];
  const animal = animals[Math.floor(Math.random() * animals.length)];
  animalSubscriptions.push({
    id: uuidv4(),
    userId: user.id,
    animalId: animal.id,
    subscribedAt: '2025-07-10T12:00:00Z',
  });
}
// Підписки на притулки
const shelterSubscriptions = [];
const shelter1 = shelters[Math.floor(Math.random() * shelters.length)];
const shelter2 = shelters[Math.floor(Math.random() * shelters.length)];
shelterSubscriptions.push({
  id: uuidv4(),
  userId: startUser.id,
  shelterId: shelter1.id,
  subscribedAt: '2025-07-10T12:00:00Z',
});
shelterSubscriptions.push({
  id: uuidv4(),
  userId: startUser.id,
  shelterId: shelter2.id,
  subscribedAt: '2025-07-10T12:00:00Z',
});
for (let i = 0; i < 50; i++) {
  const user = users[Math.floor(Math.random() * users.length)];
  const shelter = shelters[Math.floor(Math.random() * shelters.length)];
  shelterSubscriptions.push({
    id: uuidv4(),
    userId: user.id,
    shelterlId: shelter.id,
    subscribedAt: '2025-07-10T12:00:00Z',
  });
}
const aidCategories = ['Food', 'Medical', 'Equipment', 'Other'];
const aidStatuses = ['Open', 'InProgress', 'Fulfilled', 'Cancelled'];

const animalAidRequests = [];
for (let i = 0; i < 30; i++) {
  const user = users[Math.floor(Math.random() * users.length)];
  const shelter = shelters[Math.floor(Math.random() * shelters.length)];
  animalAidRequests.push({
    id: uuidv4(),
    userId: user ? user.id : undefined,
    shelterId: shelter ? shelter.id : undefined,
    title: `Aid Request ${i + 1}`,
    description: `This is a ${aidCategories[Math.floor(Math.random() * aidCategories.length)].toLowerCase()} aid request for animals in need.`,
    category: aidCategories[Math.floor(Math.random() * aidCategories.length)],
    status: aidStatuses[Math.floor(Math.random() * aidStatuses.length)],
    estimatedCost: Math.floor(100 + Math.random() * 5000),
    photos: [],
    createdAt: '2025-07-01T10:00:00Z',
    updatedAt: '2025-07-01T10:00:00Z',
  });
}

// Запис у файл
const db = {
  species,
  breeds,
  shelters,
  animals,
  users,
  categories,
  articles,
  'lost-pets': lostPets,
  'animal-subscriptions': animalSubscriptions,
  'shelter-subscriptions': shelterSubscriptions,
  'animal-aid-requests': animalAidRequests, // ✅ нова сутність
};

fs.writeFileSync('mock-data/db.json', JSON.stringify(db, null, 2));
