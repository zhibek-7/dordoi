import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ConfirmDeleteStringProjectFormModalComponent } from './confirm-delete-string-project-form-modal.component';

describe('ConfirmDeleteStringProjectFormModalComponent', () => {
  let component: ConfirmDeleteStringProjectFormModalComponent;
  let fixture: ComponentFixture<ConfirmDeleteStringProjectFormModalComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ConfirmDeleteStringProjectFormModalComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ConfirmDeleteStringProjectFormModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
