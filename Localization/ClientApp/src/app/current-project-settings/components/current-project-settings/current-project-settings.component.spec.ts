import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CurrentProjectSettingsComponent } from './current-project-settings.component';

describe('CurrentProjectComponent', () => {
  let component: CurrentProjectSettingsComponent;
  let fixture: ComponentFixture<CurrentProjectSettingsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CurrentProjectSettingsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CurrentProjectSettingsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
