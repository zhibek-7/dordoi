import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ForSeniorsComponent } from './for-seniors.component';

describe('ForSeniorsComponent', () => {
  let component: ForSeniorsComponent;
  let fixture: ComponentFixture<ForSeniorsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ForSeniorsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ForSeniorsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
