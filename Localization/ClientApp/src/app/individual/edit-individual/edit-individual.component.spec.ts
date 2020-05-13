import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EditIndividualComponent } from './edit-individual.component';

describe('EditIndividualComponent', () => {
  let component: EditIndividualComponent;
  let fixture: ComponentFixture<EditIndividualComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EditIndividualComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EditIndividualComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
