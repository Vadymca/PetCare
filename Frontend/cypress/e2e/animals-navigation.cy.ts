
describe('E2E тест навігації з мок даними', () => {
  const animalsMock = [
    {
      id: "f636c0ff-1424-47c0-8a51-049df797a2b2",
      slug: "milo-e778deb9",
      name: "Milo",
      breedId: "0fc41828-de17-4271-ae74-9677c313b84b",
      birthday: "2021-03-01",
      gender: "Female",
      description: "A standard ferret looking for a loving home.",
      healthStatus: "Healthy",
      photos: [],
      videos: [],
      shelterId: "bb604cb7-6394-4110-9837-25d36029661d",
      status: "Available",
      adoptionRequirements: "Active family with time for care.",
      microchipId: "616322127",
      idNumber: 1,
      weight: 28.85,
      height: 23.0,
      color: "White",
      isSterilized: true,
      haveDocuments: true,
      createdAt: "2025-07-01T10:00:00Z",
      updatedAt: "2025-07-01T10:00:00Z"
    }
  ];

  beforeEach(() => {
  cy.intercept('GET', '/api/animals', { body: animalsMock }).as('getAnimals');
  cy.intercept('GET', `/api/animals/${animalsMock[0].slug}`, { body: animalsMock[0] }).as('getAnimalDetails');
});


  it('Відображає список тварин при переході на /animals', () => {
    cy.visit('/animals');
    cy.wait('@getAnimals');
    cy.get('.animal-list').should('exist');
    cy.contains('.animal-list', 'Milo').should('be.visible');
  });

  it('Відображає деталі тварини при переході на /animals/:slug', () => {
    cy.visit(`/animals/${animalsMock[0].slug}`);
    cy.wait('@getAnimalDetails');
    cy.contains('Milo').should('be.visible');
    cy.contains('A standard ferret looking for a loving home.').should('be.visible');
  });
});
