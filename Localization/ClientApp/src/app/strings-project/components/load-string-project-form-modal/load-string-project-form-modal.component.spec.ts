import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { LoadStringProjectFormModalComponent } from './load-string-project-form-modal.component';

describe('LoadStringProjectFormModalComponent', () => {
  let component: LoadStringProjectFormModalComponent;
  let fixture: ComponentFixture<LoadStringProjectFormModalComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ LoadStringProjectFormModalComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(LoadStringProjectFormModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
