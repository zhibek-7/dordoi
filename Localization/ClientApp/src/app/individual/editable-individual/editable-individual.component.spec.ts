import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EditableIndividualComponent } from './editable-individual.component';

describe('EditableIndividualComponent', () => {
  let component: EditableIndividualComponent;
  let fixture: ComponentFixture<EditableIndividualComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EditableIndividualComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EditableIndividualComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
