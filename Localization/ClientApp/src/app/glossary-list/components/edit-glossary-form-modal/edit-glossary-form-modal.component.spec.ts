import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EditGlossaryFormModalComponent } from './edit-glossary-form-modal.component';

describe('EditGlossaryFormModalComponent', () => {
  let component: EditGlossaryFormModalComponent;
  let fixture: ComponentFixture<EditGlossaryFormModalComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EditGlossaryFormModalComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EditGlossaryFormModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
