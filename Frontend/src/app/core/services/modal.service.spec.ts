import { TestBed } from '@angular/core/testing';
import { ModalService, ModalState } from './modal.service';

describe('ModalService', () => {
  let service: ModalService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ModalService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should open modal with correct component and data', () => {
    const testComponent: ModalState['component'] = 'login';
    const testData = { returnUrl: '/dashboard' };
    service.openModal(testComponent, testData);
    expect(service.modalStateReadonly()).toEqual({
      isOpen: true,
      component: testComponent,
      data: testData,
    });
  });

  it('should close modal and reset state', () => {
    service.openModal('welcome', { returnUrl: '/home' });
    service.closeModal();
    expect(service.modalStateReadonly()).toEqual({
      isOpen: false,
      component: null,
      data: undefined,
    });
  });
});
