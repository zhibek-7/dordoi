import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ConfirmDeleteProjectComponent } from './confirm-delete-project.component';

describe('ConfirmDeleteProjectComponent', () => {
  let component: ConfirmDeleteProjectComponent;
  let fixture: ComponentFixture<ConfirmDeleteProjectComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ConfirmDeleteProjectComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ConfirmDeleteProjectComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
