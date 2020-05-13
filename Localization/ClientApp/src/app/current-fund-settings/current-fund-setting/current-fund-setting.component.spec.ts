import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CurrentFundSettingComponent } from './current-fund-setting.component';

describe('CurrentFundSettingComponent', () => {
  let component: CurrentFundSettingComponent;
  let fixture: ComponentFixture<CurrentFundSettingComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CurrentFundSettingComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CurrentFundSettingComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
