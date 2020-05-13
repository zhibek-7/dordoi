import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { LoadTranslationMemoryFormModalComponent } from './load-translation-memory-form-modal.component';

describe('LoadTranslationMemoryFormModalComponent', () => {
  let component: LoadTranslationMemoryFormModalComponent;
  let fixture: ComponentFixture<LoadTranslationMemoryFormModalComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ LoadTranslationMemoryFormModalComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(LoadTranslationMemoryFormModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
