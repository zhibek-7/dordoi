import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ConfirmDeleteTranslationMemoryComponent } from './confirm-delete-translation-memory.component';

describe('ConfirmDeleteTranslationMemoryComponent', () => {
  let component: ConfirmDeleteTranslationMemoryComponent;
  let fixture: ComponentFixture<ConfirmDeleteTranslationMemoryComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ConfirmDeleteTranslationMemoryComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ConfirmDeleteTranslationMemoryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
