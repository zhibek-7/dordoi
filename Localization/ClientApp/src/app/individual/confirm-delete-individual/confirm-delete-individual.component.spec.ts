import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ConfirmDeleteIndividualComponent } from './confirm-delete-individual.component';

describe('ConfirmDeleteIndividualComponent', () => {
  let component: ConfirmDeleteIndividualComponent;
  let fixture: ComponentFixture<ConfirmDeleteIndividualComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ConfirmDeleteIndividualComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ConfirmDeleteIndividualComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
