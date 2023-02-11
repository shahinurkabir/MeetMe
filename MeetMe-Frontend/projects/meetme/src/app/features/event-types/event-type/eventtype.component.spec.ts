import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EventTypeComponent } from './eventtype.component';

describe('EventtypeComponent', () => {
  let component: EventTypeComponent;
  let fixture: ComponentFixture<EventTypeComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ EventTypeComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(EventTypeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
