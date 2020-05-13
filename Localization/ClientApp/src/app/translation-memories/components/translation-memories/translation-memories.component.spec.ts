import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TranslationMemoriesComponent } from './translation-memories.component';

describe('TranslationMemoriesComponent', () => {
  let component: TranslationMemoriesComponent;
  let fixture: ComponentFixture<TranslationMemoriesComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TranslationMemoriesComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TranslationMemoriesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
