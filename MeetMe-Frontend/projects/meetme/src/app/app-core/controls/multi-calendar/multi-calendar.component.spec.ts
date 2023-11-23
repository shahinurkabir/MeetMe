import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MultiCalendarComponent } from './multi-calendar.component';

describe('MultiCalendarComponent', () => {
  let component: MultiCalendarComponent;
  let fixture: ComponentFixture<MultiCalendarComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ MultiCalendarComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(MultiCalendarComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
