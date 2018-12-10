import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CostsReportComponent } from './costs-report.component';

describe('CostsReportComponent', () => {
  let component: CostsReportComponent;
  let fixture: ComponentFixture<CostsReportComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CostsReportComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CostsReportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
