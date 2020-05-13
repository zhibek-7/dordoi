import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EditableFundComponent } from './editable-fund.component';

describe('EditableFundComponent', () => {
  let component: EditableFundComponent;
  let fixture: ComponentFixture<EditableFundComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EditableFundComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EditableFundComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
