import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FundPageComponent } from './fund-page.component';

describe('FundPageComponent', () => {
  let component: FundPageComponent;
  let fixture: ComponentFixture<FundPageComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FundPageComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FundPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
