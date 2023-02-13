import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DailyTimeIntervalsComponent } from './daily-time-intervals.component';

describe('DailyTimeIntervalsComponent', () => {
  let component: DailyTimeIntervalsComponent;
  let fixture: ComponentFixture<DailyTimeIntervalsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ DailyTimeIntervalsComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DailyTimeIntervalsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
