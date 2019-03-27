import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { InviteTranslatorComponent } from './invite-translator.component';

describe('InviteTranslatorComponent', () => {
  let component: InviteTranslatorComponent;
  let fixture: ComponentFixture<InviteTranslatorComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ InviteTranslatorComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(InviteTranslatorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
