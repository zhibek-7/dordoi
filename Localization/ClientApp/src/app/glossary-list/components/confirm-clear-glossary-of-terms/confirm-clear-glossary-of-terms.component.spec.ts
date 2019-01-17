import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ConfirmClearGlossaryOfTermsComponent } from './confirm-clear-glossary-of-terms.component';

describe('ConfirmClearGlossaryOfTermsComponent', () => {
  let component: ConfirmClearGlossaryOfTermsComponent;
  let fixture: ComponentFixture<ConfirmClearGlossaryOfTermsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ConfirmClearGlossaryOfTermsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ConfirmClearGlossaryOfTermsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
