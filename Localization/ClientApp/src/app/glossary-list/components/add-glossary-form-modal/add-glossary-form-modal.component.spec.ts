import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AddGlossaryFormModalComponent } from './add-glossary-form-modal.component';

describe('AddGlossaryFormModalComponent', () => {
  let component: AddGlossaryFormModalComponent;
  let fixture: ComponentFixture<AddGlossaryFormModalComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AddGlossaryFormModalComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AddGlossaryFormModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
