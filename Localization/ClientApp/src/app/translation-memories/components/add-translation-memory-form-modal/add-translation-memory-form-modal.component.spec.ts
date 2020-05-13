import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AddTranslationMemoryFormModalComponent } from './add-translation-memory-form-modal.component';

describe('AddTranslationMemoryFormModalComponent', () => {
  let component: AddTranslationMemoryFormModalComponent;
  let fixture: ComponentFixture<AddTranslationMemoryFormModalComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AddTranslationMemoryFormModalComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AddTranslationMemoryFormModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
