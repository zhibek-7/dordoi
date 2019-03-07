import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CurrentProjectTranslationsComponent } from './current-project-translations.component';

describe('CurrentProjectTranslationsComponent', () => {
  let component: CurrentProjectTranslationsComponent;
  let fixture: ComponentFixture<CurrentProjectTranslationsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [CurrentProjectTranslationsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CurrentProjectTranslationsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
