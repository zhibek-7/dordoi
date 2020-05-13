import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { StringsProjectComponent } from './strings-project.component';

describe('StringsProjectComponent', () => {
  let component: StringsProjectComponent;
  let fixture: ComponentFixture<StringsProjectComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ StringsProjectComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(StringsProjectComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
