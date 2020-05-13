import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ConfirmDeleteFundComponent } from './confirm-delete-fund.component';

describe('ConfirmDeleteFundComponent', () => {
  let component: ConfirmDeleteFundComponent;
  let fixture: ComponentFixture<ConfirmDeleteFundComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ConfirmDeleteFundComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ConfirmDeleteFundComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
