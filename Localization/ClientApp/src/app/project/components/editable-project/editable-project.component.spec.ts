import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EditableProjectComponent } from './editable-project.component';

describe('EditableProjectComponent', () => {
  let component: EditableProjectComponent;
  let fixture: ComponentFixture<EditableProjectComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EditableProjectComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EditableProjectComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
