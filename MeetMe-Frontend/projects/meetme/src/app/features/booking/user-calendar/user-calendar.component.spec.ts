import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UserCalendarComponent } from './user-calendar.component';

describe('UserEventTypesComponent', () => {
  let component: UserCalendarComponent;
  let fixture: ComponentFixture<UserCalendarComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ UserCalendarComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(UserCalendarComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
