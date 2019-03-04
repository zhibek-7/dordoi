import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AppointTranslationMemoriesProjectFormModalComponent } from './appoint-translation-memories-project-form-modal.component';

describe('AppointTranslationMemoriesProjectFormModalComponent', () => {
  let component: AppointTranslationMemoriesProjectFormModalComponent;
  let fixture: ComponentFixture<AppointTranslationMemoriesProjectFormModalComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AppointTranslationMemoriesProjectFormModalComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AppointTranslationMemoriesProjectFormModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
