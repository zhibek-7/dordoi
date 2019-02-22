import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TranslationMemoryEditableDetailsComponent } from './translation-memory-editable-details.component';

describe('TranslationMemoryEditableDetailsComponent', () => {
  let component: TranslationMemoryEditableDetailsComponent;
  let fixture: ComponentFixture<TranslationMemoryEditableDetailsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TranslationMemoryEditableDetailsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TranslationMemoryEditableDetailsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
