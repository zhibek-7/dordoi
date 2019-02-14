import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SetLanguagesWithNativeComponent } from './set-languages-with-native.component';

describe('SetLanguagesWithNativeComponent', () => {
  let component: SetLanguagesWithNativeComponent;
  let fixture: ComponentFixture<SetLanguagesWithNativeComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SetLanguagesWithNativeComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SetLanguagesWithNativeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
