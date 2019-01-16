import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ConfirmDeleteGlossaryComponent } from './confirm-delete-glossary.component';

describe('ConfirmDeleteGlossaryComponent', () => {
  let component: ConfirmDeleteGlossaryComponent;
  let fixture: ComponentFixture<ConfirmDeleteGlossaryComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ConfirmDeleteGlossaryComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ConfirmDeleteGlossaryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
