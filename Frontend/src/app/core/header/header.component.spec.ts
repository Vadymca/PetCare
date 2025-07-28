import { ComponentFixture, TestBed } from '@angular/core/testing';
import { RouterTestingModule } from '@angular/router/testing';
import { TranslateModule, TranslateService } from '@ngx-translate/core';
import { HeaderComponent } from './header.component';

describe('HeaderComponent', () => {
  let component: HeaderComponent;
  let fixture: ComponentFixture<HeaderComponent>;
  let translateService: TranslateService;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [
        HeaderComponent,
        RouterTestingModule,
        TranslateModule.forRoot(), // Підключаємо реальний TranslateModule
      ],
    }).compileComponents();

    fixture = TestBed.createComponent(HeaderComponent);
    component = fixture.componentInstance;
    translateService = TestBed.inject(TranslateService);
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should call translate.use when changeLanguage is called', () => {
    spyOn(translateService, 'use');
    component.changeLanguage('uk');
    expect(translateService.use).toHaveBeenCalledWith('uk');
  });

  it('should return currentLang from translate service', () => {
    translateService.currentLang = 'en';
    translateService.defaultLang = 'uk';
    expect(component.currentLang).toBe('en');

    translateService.currentLang = '';
    expect(component.currentLang).toBe('uk');
  });
});
