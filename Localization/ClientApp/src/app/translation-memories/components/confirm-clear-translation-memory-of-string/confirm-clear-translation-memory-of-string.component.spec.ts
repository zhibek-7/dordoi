import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ConfirmClearTranslationMemoryOfStringComponent } from './confirm-clear-translation-memory-of-string.component';

describe('ConfirmClearTranslationMemoryOfStringComponent', () => {
  let component: ConfirmClearTranslationMemoryOfStringComponent;
  let fixture: ComponentFixture<ConfirmClearTranslationMemoryOfStringComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ConfirmClearTranslationMemoryOfStringComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ConfirmClearTranslationMemoryOfStringComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
