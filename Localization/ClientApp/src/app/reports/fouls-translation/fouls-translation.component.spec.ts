import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FoulsTranslationComponent } from './fouls-translation.component';

describe('FoulsTranslationComponent', () => {
  let component: FoulsTranslationComponent;
  let fixture: ComponentFixture<FoulsTranslationComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FoulsTranslationComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FoulsTranslationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
