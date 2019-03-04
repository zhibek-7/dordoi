import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EditStringProjectFormModalComponent } from './edit-string-project-form-modal.component';

describe('EditStringProjectFormModalComponent', () => {
  let component: EditStringProjectFormModalComponent;
  let fixture: ComponentFixture<EditStringProjectFormModalComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EditStringProjectFormModalComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EditStringProjectFormModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
