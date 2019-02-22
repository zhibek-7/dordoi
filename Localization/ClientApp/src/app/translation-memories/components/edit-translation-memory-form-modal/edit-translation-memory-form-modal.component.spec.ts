import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EditTranslationMemoryFormModalComponent } from './edit-translation-memory-form-modal.component';

describe('EditTranslationMemoryFormModalComponent', () => {
  let component: EditTranslationMemoryFormModalComponent;
  let fixture: ComponentFixture<EditTranslationMemoryFormModalComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EditTranslationMemoryFormModalComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EditTranslationMemoryFormModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
