import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EventTypeCalendarComponent } from './eventtype-calendar.component';

describe('CalendarComponent', () => {
  let component: EventTypeCalendarComponent;
  let fixture: ComponentFixture<EventTypeCalendarComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ EventTypeCalendarComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(EventTypeCalendarComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
