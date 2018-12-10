import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TranslationsCostComponent } from './translations-cost.component';

describe('TranslationsCostComponent', () => {
  let component: TranslationsCostComponent;
  let fixture: ComponentFixture<TranslationsCostComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TranslationsCostComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TranslationsCostComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
