import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TimezoneControlComponent } from './timezone-control.component';

describe('TimezoneControlComponent', () => {
  let component: TimezoneControlComponent;
  let fixture: ComponentFixture<TimezoneControlComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ TimezoneControlComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TimezoneControlComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
