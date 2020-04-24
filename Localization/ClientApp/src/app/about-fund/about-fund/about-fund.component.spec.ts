import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AboutFundComponent } from './about-fund.component';

describe('AboutFundComponent', () => {
  let component: AboutFundComponent;
  let fixture: ComponentFixture<AboutFundComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AboutFundComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AboutFundComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
