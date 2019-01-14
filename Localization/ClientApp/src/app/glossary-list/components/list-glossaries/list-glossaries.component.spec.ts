import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ListGlossariesComponent } from './list-glossaries.component';

describe('ListGlossariesComponent', () => {
  let component: ListGlossariesComponent;
  let fixture: ComponentFixture<ListGlossariesComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ListGlossariesComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ListGlossariesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
