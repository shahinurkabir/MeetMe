import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TimeAvailabilityComponent } from './time-availability.component';

describe('TimeAvailabilityComponent', () => {
  let component: TimeAvailabilityComponent;
  let fixture: ComponentFixture<TimeAvailabilityComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ TimeAvailabilityComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TimeAvailabilityComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
