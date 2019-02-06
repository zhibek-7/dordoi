import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SetTimeZoneComponent } from './set-time-zone.component';

describe('SetTimeZoneComponent', () => {
  let component: SetTimeZoneComponent;
  let fixture: ComponentFixture<SetTimeZoneComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SetTimeZoneComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SetTimeZoneComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
