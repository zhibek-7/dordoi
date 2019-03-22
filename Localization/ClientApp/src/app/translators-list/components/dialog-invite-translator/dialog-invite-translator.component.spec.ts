import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DialogInviteTranslatorComponent } from './dialog-invite-translator.component';

describe('DialogInviteTranslatorComponent', () => {
  let component: DialogInviteTranslatorComponent;
  let fixture: ComponentFixture<DialogInviteTranslatorComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DialogInviteTranslatorComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DialogInviteTranslatorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
