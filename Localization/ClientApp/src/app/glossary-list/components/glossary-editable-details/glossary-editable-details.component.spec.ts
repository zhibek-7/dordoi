import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { GlossaryEditableDetailsComponent } from './glossary-editable-details.component';

describe('GlossaryEditableDetailsComponent', () => {
  let component: GlossaryEditableDetailsComponent;
  let fixture: ComponentFixture<GlossaryEditableDetailsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ GlossaryEditableDetailsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(GlossaryEditableDetailsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
