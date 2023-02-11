import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EventAvailabilityComponent } from './event-availability.component';

describe('EventAvailabilityComponent', () => {
  let component: EventAvailabilityComponent;
  let fixture: ComponentFixture<EventAvailabilityComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ EventAvailabilityComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(EventAvailabilityComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
